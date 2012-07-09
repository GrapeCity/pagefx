using System;

namespace DataDynamics
{
    public class Angle
    {
    	public const double PI2 = Math.PI * 2;

        internal static double RadPerDeg = Math.PI / 180.0;
        internal static double DegPerRad = 180.0 / Math.PI;

        public static float Clamp(float value)
        {
            return ((value % 360) + 360) % 360;
        }

        public static double Clamp(double value)
        {
            return ((value % 360) + 360) % 360;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees value.</param>
        /// <returns>Returns the value in radians.</returns>
        public static double ToRadians(double degrees)
        {
            return degrees * RadPerDeg;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees value.</param>
        /// <returns>Returns the value in radians.</returns>
        public static float ToRadians(float degrees)
        {
            return degrees * (float)RadPerDeg;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">The radians value.</param>
        /// <returns>Returns the value in degrees.</returns>
        public static double ToDegrees(double radians)
        {
            return radians * DegPerRad;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">The radians value.</param>
        /// <returns>Returns the value in degrees.</returns>
        public static float ToDegrees(float radians)
        {
            return radians * (float)DegPerRad;
        }


        /// <summary>
        /// Calculates the unit value of the given angle (in radians).
        /// </summary>
        /// <param name="radians">The angle.</param>
        /// <returns>Returns a unit value for the given angle (in radians).</returns>
        public static double UnitRadian(double radians)
        {
            if (radians <= -Math.PI)
            {
                return radians + PI2 * Math.Floor(-(radians - Math.PI) / PI2);
            }

            if (radians > Math.PI)
            {
                return radians - PI2 * (Math.Ceiling((radians + Math.PI) / PI2) - 1);
            }

            return radians;
        }

        /// <summary>
        /// Calculates the unit value of the given angle (in degrees).
        /// </summary>
        /// <param name="degrees">The angle.</param>
        /// <returns>Returns a unit value for the given angle (in degrees).</returns>
        public static double UnitDeg(double degrees)
        {
            return degrees % 360;
        }
    }
}