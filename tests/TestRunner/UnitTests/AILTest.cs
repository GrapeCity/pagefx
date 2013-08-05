using DataDynamics.PageFX.Flash.IL;
using NUnit.Framework;

namespace DataDynamics.PageFX.TestRunner.UnitTests
{
    [TestFixture]
    public class AILTest
    {
        private static void AssertStackPush(int n, Instruction instr)
        {
            Assert.AreEqual(n, instr.StackPush, instr.ToString());
        }

        private static void AssertStackPush(int n, InstructionCode code)
        {
            AssertStackPush(n, new Instruction(code));
        }

        [Test]
        public void TestStackPush()
        {
            AssertStackPush(1, InstructionCode.Pushtrue);
            AssertStackPush(1, InstructionCode.Pushfalse);
        }
    }
}