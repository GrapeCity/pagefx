using DataDynamics.PageFX.FlashLand.IL;
using NUnit.Framework;

namespace DataDynamics.PageFX.FLI.Tests
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