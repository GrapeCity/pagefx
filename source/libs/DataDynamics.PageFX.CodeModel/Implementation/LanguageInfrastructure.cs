using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public static class LanguageInfrastructure
    {
        static readonly List<ILanguageInfrastructure> _lis = new List<ILanguageInfrastructure>();

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
            get
            {
                if (_cli == null)
                    _cli = Find("CLI");
                return _cli;
            }
        }
        static ILanguageInfrastructure _cli;

        public static ILanguageInfrastructure FLI
        {
            get
            {
                if (_fli == null)
                    _fli = Find("FLI");
                return _fli;
            }
        }
        static ILanguageInfrastructure _fli;
    }
}