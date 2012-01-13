using EnvDTE;

namespace DataDynamics.PageFX.VisualStudio
{
    internal interface IAddinCommand
    {
        CommandAttribute Attribute { get; set; }

        vsCommandStatus QueryStatus(VSAddin addin);

        void Exec(VSAddin addin);
    }
}