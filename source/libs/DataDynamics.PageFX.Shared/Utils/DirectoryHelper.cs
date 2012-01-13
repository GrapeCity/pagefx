using System;
using System.IO;

namespace DataDynamics
{
    public static class DirectoryHelper
    {
        private class Changer : IDisposable
        {
            private readonly string olddir;

            public Changer(string newdir)
            {
                olddir = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(newdir);
            }

            #region IDisposable Members
            public void Dispose()
            {
                Directory.SetCurrentDirectory(olddir);
            }
            #endregion
        }

        public static IDisposable Change(string newdir)
        {
            return new Changer(newdir);
        }
    }
}