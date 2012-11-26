using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public static class LanguageInfrastructure
    {
        private static readonly List<ILanguageInfrastructure> _lis = new List<ILanguageInfrastructure>();

        public static void Register(ILanguageInfrastructure li)
        {
            _lis.Add(li);
        }

        public static ILanguageInfrastructure Find(string name)
        {
            return _lis.Find(li => string.Compare(li.Name, name, true) == 0);
        }

        public static ILanguageInfrastructure CLI
        {
            get { return _cli ?? (_cli = Find("CLI")); }
        }
        private static ILanguageInfrastructure _cli;

        public static ILanguageInfrastructure FLI
        {
            get { return _fli ?? (_fli = Find("FLI")); }
        }
        private static ILanguageInfrastructure _fli;
    }
}