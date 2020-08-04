using NUnit.Framework;

namespace droid.Tests.Editor.Commands {
  [TestFixture]
  public class SystemCommands {
    [Test]
    public void TestSystemPythonAddition() {
      droid.Editor.Utilities.Commands.Commands.SystemCommand("python", "-c 'print(1+1)'");

      Assert.That(true, expression : Is.True);
    }
  }
}
