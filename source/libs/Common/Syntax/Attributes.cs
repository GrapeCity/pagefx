using System;

namespace DataDynamics.PageFX.Common.Syntax
{
    /// <summary>
    /// Defines some format string for some language. The format string is used to format specified item from Code Model.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LanguageAttribute : Attribute
    {
        #region Constructors
        /// <summary>
        /// Initialized new instance of <see cref="LanguageAttribute"/> class.
        /// </summary>
        /// <param name="lang">name of language</param>
        /// <param name="format">format string</param>
        public LanguageAttribute(string lang, string format)
        {
            _lang = lang;
            _value = format;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name of language.
        /// </summary>
        public string Language
        {
            get { return _lang; }
        }
        private readonly string _lang;

        /// <summary>
        /// Gets the format string.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }
        private readonly string _value;
        #endregion
    }

    /// <summary>
    /// <see cref="LanguageAttribute"/> for C#.
    /// </summary>
    public sealed class CSharpAttribute : LanguageAttribute
    {
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="CSharpAttribute"/> class.
        /// </summary>
        /// <param name="format">format string</param>
        public CSharpAttribute(string format) 
            : base("c#", format)
        {
        }
        #endregion
    }

    /// <summary>
    /// <see cref="LanguageAttribute"/> for VB.NET.
    /// </summary>
    public sealed class VBAttribute : LanguageAttribute
    {
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="VBAttribute"/> class.
        /// </summary>
        /// <param name="format">format string</param>
        public VBAttribute(string format)
            : base("vb", format)
        {
        }
        #endregion
    }

    /// <summary>
    /// <see cref="LanguageAttribute"/> for ActionScript.
    /// </summary>
    public sealed class ActionScriptAttribute : LanguageAttribute
    {
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="ActionScriptAttribute"/> class.
        /// </summary>
        /// <param name="format">format string</param>
        public ActionScriptAttribute(string format)
            : base("as", format)
        {
        }
        #endregion
    }
}