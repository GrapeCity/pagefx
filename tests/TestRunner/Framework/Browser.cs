using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class Browser
	{
		public static void Show(string title, string path)
		{
			var form = Create(title, path);
			form.Show();
		}

		public static void ShowDialog(string title, string path)
		{
			using (var form = Create(title, path))
				form.ShowDialog();
		}

		private static Form Create(string title, string path)
		{
			return new Form
				{
					Text = title,
					StartPosition = FormStartPosition.CenterScreen,
					Size = new Size(800, 600),
					Controls =
						{
							new WebBrowser
								{
									Dock = DockStyle.Fill,
									Url = new Uri(path)
								}
						}
				};
		}
	}
}
