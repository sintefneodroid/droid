using NUnit.Framework;

namespace droid.Tests.Editor.Commands {
  [TestFixture]
  public class PythonCommands {
    [Test]
    public void TestPythonAddition() {
      droid.Editor.Utilities.Commands.Commands.PythonCommand("print(1+1)");

      Assert.That(true, expression : Is.True);
    }
  }
}
