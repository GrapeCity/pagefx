namespace Microsoft.VisualBasic.CompilerServices
{
    using Microsoft.VisualBasic;
    using System;

    public sealed class ProjectData
    {
        internal ErrObject m_Err;
        [ThreadStatic]
        private static ProjectData m_oProject;
        internal int m_rndSeed = 0x50000;

        private ProjectData()
        {
        }

        public static void ClearProjectError()
        {
            try
            {
            }
            finally
            {
                Information.Err().Clear();
            }
        }

        public static Exception CreateProjectError(int hr)
        {
            ErrObject obj2 = Information.Err();
            obj2.Clear();
            int num = obj2.MapErrorNumber(hr);
            return obj2.CreateException(hr, Utils.GetResourceString((vbErrors) num));
        }

        internal static ProjectData GetProjectData()
        {
            ProjectData oProject = m_oProject;
            if (oProject == null)
            {
                oProject = new ProjectData();
                m_oProject = oProject;
            }
            return oProject;
        }

        public static void SetProjectError(Exception ex)
        {
            try
            {
            }
            finally
            {
                Information.Err().CaptureException(ex);
            }
        }

        public static void SetProjectError(Exception ex, int lErl)
        {
            try
            {
            }
            finally
            {
                Information.Err().CaptureException(ex, lErl);
            }
        }
    }
}

