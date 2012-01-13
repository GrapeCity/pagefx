using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using DataDynamics.PageFX.VisualStudio;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

namespace DataDynamics.PageFX
{
    /// <summary>Represents PageFX Visual Studio Addin.</summary>
	/// <seealso class='IDTExtensibility2' />
    [ComVisible(true)]
    [Guid("53F6888A-5BFF-48e0-87A8-8AF8BFA094BC")]
    public class VSAddin : IDTExtensibility2, IDTCommandTarget
    {
        public VSAddin()
        {
        }

        #region Properties
        internal DTE2 DTE
        {
            get { return _dte; }
        }
        DTE2 _dte;

        internal AddIn Instance
        {
            get { return _addInInstance; }
        }
        AddIn _addInInstance;

        internal VSDebugger Debugger
        {
            get { return _debugger; }
        }
        VSDebugger _debugger;
        #endregion

        #region IDTExtensibility2 Members
        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public virtual void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _dte = (DTE2)application;
            _addInInstance = (AddIn)addInInst;
            _debugger = new VSDebugger(_dte);
            _menuBar = null;
            
            LoadCommands();

            SetupUI();

            //if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            //{
            //}
        }

        readonly Hashtable _commandsCache = new Hashtable();
        readonly List<IAddinCommand> _commands = new List<IAddinCommand>();

        void SetupUI()
        {
            var rootMenu = CreateRootMenu();

            RegisterCommands(rootMenu);
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }
        #endregion

        #region IDTCommandTarget Members
        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
        /// <param term='commandName'>The name of the command to determine state for.</param>
        /// <param term='neededText'>Text that is needed for the command.</param>
        /// <param term='status'>The state of the command in the user interface.</param>
        /// <param term='commandText'>Text requested by the neededText parameter.</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                var cmd = _commandsCache[commandName] as IAddinCommand;
                if (cmd != null)
                {
                    status = cmd.QueryStatus(this);
                }
            }
        }

        /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
        /// <param term='commandName'>The name of the command to execute.</param>
        /// <param term='executeOption'>Describes how the command should be run.</param>
        /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
        /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
        /// <param term='handled'>Informs the caller if the command was handled or not.</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                var cmd = _commandsCache[commandName] as IAddinCommand;
                if (cmd != null)
                {
                    handled = true;
                    cmd.Exec(this);
                }
            }
        }
        #endregion

        string GetCommandKey(string name)
        {
            return _addInInstance.ProgID + "." + name;
        }

        #region LoadCommands
        void LoadCommands()
        {
            _commandsCache.Clear();
            _commands.Clear();
            var asm = GetType().Assembly;
            foreach (var type in asm.GetTypes())
            {
                if (type.IsAbstract) continue;
                if (!typeof(IAddinCommand).IsAssignableFrom(type)) continue;
                var attr = Utils.GetAttribute<CommandAttribute>(type);
                if (attr == null) continue;

                try
                {
                    var cmd = Activator.CreateInstance(type) as IAddinCommand;
                    if (cmd == null) continue;
                    cmd.Attribute = attr;
                    _commandsCache[GetCommandKey(attr.Name)] = cmd;
                    _commands.Add(cmd);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.ToString());
                }
            }
            _commands.Sort((x, y) => x.Attribute.Position - y.Attribute.Position);
        }
        #endregion

        #region RegisterCommands
        void RegisterCommands(CommandBar rootMenu)
        {
            foreach (var cmd in _commands)
            {
                RegisterCommand(rootMenu, cmd);
            }
        }

        void RegisterCommand(CommandBar root, IAddinCommand cmd)
        {
            try
            {
                var attr = cmd.Attribute;
                RegisterCommand(attr, root);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        Command RegisterCommand(CommandAttribute attr, CommandBar rootMenu)
        {
            UIUtils.RemoveControls(rootMenu, attr.Caption);

            var command = RegisterCommand(attr);
            if (command == null) return null;

            //Add a control for the command to the root menu:
            if (rootMenu != null)
            {
                try
                {
                    var control = (CommandBarButton)command.AddControl(rootMenu, attr.Position);
                    IconUtils.SetPicture(control, attr.Icon);
                }
                catch (Exception exc)
                {
                    Trace.WriteLine(string.Format("PFX: Unable to add menu item for command {0}.", attr.Name));
                    Trace.WriteLine("PFX: Exception: " + exc);
                }
            }

            return command;
        }

        Command RegisterCommand(CommandAttribute attr)
        {
            var commands = (Commands2)_dte.Commands;

            string fullName = GetCommandKey(attr.Name);

            try
            {
                var command = commands.Item(fullName, 0);
                if (command != null)
                    return command;
            }
            catch
            {
            }

            //This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Add-in,
            //just make sure you also update the QueryStatus/Exec method to include the new command names.
            try
            {
                object[] contextGUIDS = null;

                //Add a command to the Commands collection:
                var command = commands.AddNamedCommand2(
                    _addInInstance, attr.Name,
                    attr.Caption, attr.Tooltip, true,
                    null, ref contextGUIDS,
                    (int)vsCommandStatus.vsCommandStatusSupported
                    + (int)vsCommandStatus.vsCommandStatusEnabled,
                    (int)vsCommandStyle.vsCommandStylePictAndText,
                    vsCommandControlType.vsCommandControlTypeButton);

                BindHotKey(command, attr.HotKey);

                return command;
            }
            catch (ArgumentException exc)
            {
                //If we are here, then the exception is probably because a command with that name
                //  already exists. If so there is no need to recreate the command and we can 
                //  safely ignore the exception.
            }
            catch (SecurityException sex)
            {
                Utils.ErrorBox("Security Problems! Details:\n{0}", sex.ToString());
            }
            return null;
        }

        static void BindHotKey(Command command, string hotkey)
        {
            try
            {
                if (!string.IsNullOrEmpty(hotkey))
                {
                    command.Bindings = new object[] { hotkey };
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
        }
        #endregion

        #region UI Utils
        CommandBar MenuBar
        {
            get
            {
                if (_menuBar == null)
                    _menuBar = ((CommandBars)_dte.CommandBars)["MenuBar"];
                return _menuBar;
            }
        }
        CommandBar _menuBar;

        int GetRootMenuPosition()
        {
            try
            {
                var c = MenuBar.Controls["Tools"];
                return c.Index + 2;
            }
            catch
            {
                return 7;
            }
        }

        CommandBar CreateRootMenu()
        {
            return CreateMenu("PageFX", GetRootMenuPosition());
        }

        CommandBar CreateMenu(string name, int pos)
        {
            return CreateMenu(MenuBar, name, pos);
        }

        CommandBar CreateMenu(CommandBar parent, string name, int pos)
        {
            //var commands = _dte.Commands;
            //try
            //{
            //    return (CommandBar)commands.AddCommandBar(name, vsCommandBarType.vsCommandBarTypeMenu, parent, pos);
            //}
            //catch
            //{
            //}
            //return null;

            return UIUtils.CreatePopup(parent, name, pos).CommandBar;
        }

        CommandBar AddCommandBar(string name, MsoBarPosition position)
        {
            var bars = ((CommandBars)_dte.CommandBars);
            CommandBar bar = null;

            try
            {
                try
                {
                    bar = bars.Add(name, position, false, false);
                }
                catch (ArgumentException)
                {
                    // Try to find an existing CommandBar
                    bar = bars[name];
                }
            }
            catch
            {
            }
            return bar;
        }
        #endregion
    }
}