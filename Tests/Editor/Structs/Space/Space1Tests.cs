using System;
using droid.Runtime.Enums;
using droid.Runtime.Structs.Space;
using NUnit.Framework;

namespace droid.Tests.Editor.Structs.Space {
  /// <summary>
  ///
  /// </summary>
  [TestFixture]
  public class Space1Tests {
    /// <summary>
    ///
    /// </summary>
    [TestCase(0, 0, 0, ExpectedResult = 0f)]
    [TestCase(1, 0, 0, ExpectedResult = 0f)]
    [TestCase(0, 0, 10, ExpectedResult = 0f)]
    [TestCase(0.5f, 0, 1, ExpectedResult = 0.5f)]
    [TestCase(0.1f, 0, 10, ExpectedResult = 1f)]
    [TestCase(1.0f, 0, 10, ExpectedResult = 10f)]
    [TestCase(0.5f, 5, 10, ExpectedResult = 7.5f)]
    [TestCase(0.5f, -10, 10, ExpectedResult = 0f)]
    [TestCase(0.75f, -10, 10, ExpectedResult = 5f)]
    [TestCase(0.5f, -10, -5, ExpectedResult = -7.5f)]
    [TestCase(0.5f, -10, 0, ExpectedResult = -5f)]
    public float TestDenormalise010(float v, float min_value, float max_value) {
      var space = new Space1 {Min = min_value, Max = max_value, Normalised = Normalisation.Zero_one_, DecimalGranularity = 1};

      return space.Reproject(v);
    }

    [TestCase(-1)]
    [TestCase(11)]
    public void TestDenormalise010Throws(float v) {
      var space = new Space1 {Min = 0, Max = 10, Normalised = Normalisation.Zero_one_};

      Assert.That(() => space.Reproject(v), Throws.TypeOf<ArgumentException>());
    }

    [TestCase(-1)]
    [TestCase(10.1f)]
    public void TestNormalise010Throws(float v) {
      var space = new Space1 {Min = 0, Max = 10, Normalised = Normalisation.Zero_one_};

      Assert.That(() => space.Project(v), Throws.TypeOf<ArgumentException>());
    }

    /// <summary>
    ///
    /// </summary>
    [TestCase(5, 0, 10, ExpectedResult = 0.5f)]
    [TestCase(0.2f, 0, 1, ExpectedResult = 0.2f)]
    [TestCase(0.5f, -1, 1, ExpectedResult = 0.75f)]
    [TestCase(-0.8f, -1, 0, ExpectedResult = 0.2f)]
    public float TestNormalise010(float v, float min_value, float max_value) {
      var space = new Space1 {Min = min_value, Max = max_value,DecimalGranularity = 1, Normalised = Normalisation.Zero_one_};

      //Assert.AreEqual(expected, result, tolerance);
      return  space.Round(space.Project(v));
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestNormaliseMinus11() {
      var space = new Space1 {Min = -1, Max = 1, Normalised = Normalisation.Zero_one_};

      Assert.That(Math.Abs(space.Project(0.5f) - 0.75f) <= float.Epsilon, Is.True);
    }

    [Test]
    public void TestClipDenormalise01RoundClip() {
      var space = new Space1 {Min = -1, Max = 1, Normalised = Normalisation.Zero_one_};

      Assert.That(Math.Abs(space.Reproject(0.5f) - 0.0f) <= float.Epsilon, Is.True);
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestDenormaliseMinus11() {
      var space = new Space1 {Min = -1f, Max = 1f, Normalised = Normalisation.Zero_one_,DecimalGranularity = 1};

      var d = space.Reproject(0.75f);
      Assert.That(Math.Abs(d - 0.5f) <= float.Epsilon, Is.True);
    }
  }
}
