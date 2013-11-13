using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.Flash.Core.SwfGeneration
{
	// http://stackoverflow.com/questions/9537076/chart-of-swf-versions-to-flash-versions
	// http://blogs.adobe.com/airodynamics/2011/08/16/versioning-in-flash-runtime-swf-version/
	// http://sleepydesign.blogspot.ru/2012/04/flash-swf-version-meaning.html

	internal static class SwfVersion
	{
		private static readonly IDictionary<float, int> FlashToSwfVersion = new Dictionary<float, int>
		{
			{9.0f, 9},
			{10.0f, 10},
			{10.1f, 10},
			{10.2f, 11},
			{10.3f, 12},
			{11.0f, 13},
			{11.1f, 14},
			{11.2f, 15},
			{11.3f, 16},
			{11.4f, 17},
			{11.5f, 18},
			{11.6f, 19},
			{11.7f, 20},
			{11.8f, 21},
			{11.9f, 22},
		};

		public static int FromFlashVersion(float version)
		{
			int result;
			if (!FlashToSwfVersion.TryGetValue((float)Math.Round(version, 1), out result))
				throw new NotSupportedException(string.Format("FlashPlayer version {0} is not supported yet!", version));
			return result;
		}
	}
}