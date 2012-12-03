#if CONSOLE_READ
using System.IO;
using flash.display;
using flash.events;
using flash.text;
using flash.ui;

namespace System
{
	public static class RootStage
	{
		public static Stage Value;
	}

	internal sealed class ConsoleReader : TextReader
	{
		private readonly TextField _console;
		private int _lines;

		public ConsoleReader()
		{
			var stage = RootStage.Value;

			_console = new TextField
			           	{
			           		type = TextFieldType.INPUT,
							alpha = 1,
			           		multiline = true,
							textColor = 0xffffff,
							background = true,
			           		backgroundColor = 0x000000,
							x = 0,
							y = 0,
			           		width = 500,
			           		height = 500,
			           	};
			_console.keyUp += OnKeyUp;

			stage.resize += e =>
			                	{
			                		_console.x = 0;
			                		_console.y = 0;
			                		_console.width = stage.width;
			                		_console.height = stage.height;
			                	};

			stage.addChild(_console);
		}

		void OnKeyUp(KeyboardEvent e)
		{
			if (e.keyCode == Keyboard.ENTER)
			{
				++_lines;
				Console.FireCancelKeyPress();
			}
		}

		public override int Read()
		{
			return 0;
		}

		public override string ReadLine()
		{
			if (_lines == 0) return "";
			return _console.getLineText(_lines - 1);
		}

		public void WriteLine(string line)
		{
			_console.text += line + "\n";
		}

		public void Clear()
		{
			_console.text = "";
			_lines = 0;
		}
	}
}
#endif