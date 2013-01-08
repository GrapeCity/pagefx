using System;
using DataDynamics.PageFX.Common.Tools;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class GlobalOptions
	{
		public const string KeyBaseDir = "QA.BaseDir";
		public const string KeyOptimizeCode = "CG.Optimize";
		public const string KeyEmitDebugInfo = "CG.Debug";
		public const string KeyUseCommonDir = "UseCommonDir";
		public const string KeyNightlyBuild = "NightlyBuild";
		public static string BaseDir = "C:\\QA";

		internal sealed class Option<T>
		{
			private T _value;

			public Option()
			{
			}

			public Option(T value)
			{
				_value = value;
			}

			public T Value
			{
				get { return _value; }
				set
				{
					if (Equals(value, _value)) return;
					_value = value;
					Save();
				}
			}

			public static implicit operator T(Option<T> o)
			{
				return o.Value;
			}

			public static implicit operator Option<T>(T value)
			{
				return new Option<T>(value);
			}
		}

		public static Option<bool> OptimizeCode = true;
		public static Option<bool> EmitDebugInfo = true;
		
		public static bool LoadNUnitTests = true;
		public static bool TestDebugSupport;
		public static bool ProtectNUnitTest = true;
		public static bool RunSuiteAsOneTest = true;
		public static bool IsNUnitSession { get; set; }

		public static TestRunnerGeneratorOptions TestRunnerOptions
		{
			get
			{
				return new TestRunnerGeneratorOptions
					{
						Protect = ProtectNUnitTest,
						EndMarker = FlashPlayer.MarkerEnd,
						FailString = FlashPlayer.MarkerFail,
						SuccessString = FlashPlayer.MarkerSuccess
					};
			}
		}

		public static bool UseCommonDirectory
		{
			get
			{
				if (IsNUnitSession)
					return true;
				return _useCommonDir;
			}
			set
			{
				if (value != _useCommonDir)
				{
					_useCommonDir = value;
					Save();
				}
			}
		}

		private static bool _useCommonDir;

		public static void Load()
		{
			BaseDir = Storage.GetValue(KeyBaseDir, "C:\\QA");
			if (String.IsNullOrEmpty(BaseDir))
				BaseDir = "C:\\QA";

			OptimizeCode = Storage.GetValue(KeyOptimizeCode, true);
			EmitDebugInfo = Storage.GetValue(KeyEmitDebugInfo, true);
			_useCommonDir = Storage.GetValue(KeyUseCommonDir, false);
		}

		public static void Save()
		{
			Storage.SetValue(KeyBaseDir, BaseDir);
			Storage.SetValue(KeyOptimizeCode, OptimizeCode.Value);
			Storage.SetValue(KeyEmitDebugInfo, EmitDebugInfo.Value);
			Storage.SetValue(KeyUseCommonDir, _useCommonDir);
		}

		private static bool ToBool(object value)
		{
			if (value is bool)
				return (bool)value;
			var c = value as IConvertible;
			if (c != null)
				return c.ToBoolean(null);
			throw new InvalidCastException();
		}

		public static string GetOptionKey(GlobalOptionName name)
		{
			switch (name)
			{
				case GlobalOptionName.OptimizeCode:
					return KeyOptimizeCode;

				case GlobalOptionName.EmitDebugInfo:
					return KeyEmitDebugInfo;

				case GlobalOptionName.UseCommonDirectory:
					return KeyUseCommonDir;

				default:
					throw new ArgumentOutOfRangeException("name");
			}
		}

		public static void SetOption(GlobalOptionName name, object value)
		{
			switch (name)
			{
				case GlobalOptionName.OptimizeCode:
					OptimizeCode = ToBool(value);
					break;

				case GlobalOptionName.EmitDebugInfo:
					EmitDebugInfo = ToBool(value);
					break;

				case GlobalOptionName.UseCommonDirectory:
					UseCommonDirectory = ToBool(value);
					break;

				default:
					throw new ArgumentOutOfRangeException("name");
			}
		}

		public static object GetOption(GlobalOptionName name)
		{
			switch (name)
			{
				case GlobalOptionName.OptimizeCode:
					return OptimizeCode.Value;

				case GlobalOptionName.EmitDebugInfo:
					return EmitDebugInfo.Value;

				case GlobalOptionName.UseCommonDirectory:
					return UseCommonDirectory;

				default:
					throw new ArgumentOutOfRangeException("name");
			}
		}

		public static bool GetBoolOption(GlobalOptionName name)
		{
			return ToBool(GetOption(name));
		}
	}

	public enum GlobalOptionName
	{
		OptimizeCode,
		EmitDebugInfo,
		UseCommonDirectory
	}
}
