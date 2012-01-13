namespace DataDynamics.PageFX.FLI.IL
{
    [Template("fli.InstructionCode.txt", "..\\..\\..\\..\\libs\\DataDynamics.PageFX.FLI\\IL\\InstructionCode.cs")]
    public class InstructionCodeTemplate : ITextTemplateContext
    {
        private readonly Instruction[] _instructions;
        private int _index;

        public InstructionCodeTemplate()
        {
            _instructions = Instructions.GetInstructions();
            _index = -1;
        }

        #region ITextTemplateContext Members
        static string Cap(string s)
        {
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public string Eval(string statement, string var)
        {
            if (statement == "for")
            {
                if (var == "i")
                {
                    ++_index;
                    if (_index < _instructions.Length)
                        return "1";
                    return null;
                }
                return null;
            }
            var i = _instructions[_index];
            if (var == "desc")
                return i.Description;
            if (var == "name")
                return Cap(i.Name);
            if (var == "value")
                return string.Format("0x{0:X2}", (int)i.Code);
            return null;
        }
        #endregion
    }
}