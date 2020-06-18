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
    [TestCase(0,
              0,
              0,
              ExpectedResult = 0f)]
    [TestCase(1,
              0,
              0,
              ExpectedResult = 0f)]
    [TestCase(0,
              0,
              10,
              ExpectedResult = 0f)]
    [TestCase(0.5f,
              0,
              1,
              ExpectedResult = 0.5f)]
    [TestCase(0.1f,
              0,
              10,
              ExpectedResult = 1f)]
    [TestCase(1.0f,
              0,
              10,
              ExpectedResult = 10f)]
    [TestCase(0.5f,
              5,
              10,
              ExpectedResult = 7.5f)]
    [TestCase(0.5f,
              -10,
              10,
              ExpectedResult = 0f)]
    [TestCase(0.75f,
              -10,
              10,
              ExpectedResult = 5f)]
    [TestCase(0.5f,
              -10,
              -5,
              ExpectedResult = -7.5f)]
    [TestCase(0.5f,
              -10,
              0,
              ExpectedResult = -5f)]
    public float TestDenormalise01(float v, float min_value, float max_value) {
      var space = new Space1 {
                                 Min = min_value,
                                 Max = max_value,
                                 Normalised = ProjectionEnum.Zero_one_,
                                 DecimalGranularity = 2
                             };

      return space.Reproject(v : v);
    }

    /// <summary>
    ///
    /// </summary>
    [TestCase(5,
              0,
              10,
              ExpectedResult = 0.5f)]
    [TestCase(0.2f,
              0,
              1,
              ExpectedResult = 0.2f)]
    [TestCase(0.5f,
              -1,
              1,
              ExpectedResult = 0.75f)]
    [TestCase(-0.8f,
              -1,
              0,
              ExpectedResult = 0.2f)]
    public float TestNormalise01(float v, float min_value, float max_value) {
      var space = new Space1 {
                                 Min = min_value,
                                 Max = max_value,
                                 DecimalGranularity = 2,
                                 Normalised = ProjectionEnum.Zero_one_
                             };

      //Assert.AreEqual(expected, result, tolerance);
      return space.Project(v : v);
    }

    [TestCase(-1)]
    [TestCase(11)]
    public void TestDenormalise010Throws(float v) {
      var space = new Space1 {Min = 0, Max = 10, Normalised = ProjectionEnum.Zero_one_};

      Assert.That(() => space.Reproject(v : v), expr : Throws.TypeOf<ArgumentException>());
    }

    [TestCase(-1)]
    [TestCase(10.1f)]
    public void TestNormalise010Throws(float v) {
      var space = new Space1 {Min = 0, Max = 10, Normalised = ProjectionEnum.Zero_one_};

      Assert.That(() => space.Project(v : v), expr : Throws.TypeOf<ArgumentException>());
    }

    [TestCase(-2)]
    [TestCase(2)]
    public void TestDenormaliseMinusOneOne010Throws(float v) {
      var space = new Space1 {Min = 0, Max = 10, Normalised = ProjectionEnum.Minus_one_one_};

      Assert.That(() => space.Reproject(v : v), expr : Throws.TypeOf<ArgumentException>());
    }

    [TestCase(-1)]
    [TestCase(10.1f)]
    public void TestNormaliseMinusOneOne010Throws(float v) {
      var space = new Space1 {Min = 0, Max = 10, Normalised = ProjectionEnum.Minus_one_one_};

      Assert.That(() => space.Project(v : v), expr : Throws.TypeOf<ArgumentException>());
    }


    /// <summary>
    ///
    /// </summary>
    [TestCase(0,
              0,
              0,
              ExpectedResult = 0f)]
    [TestCase(1,
              0,
              0,
              ExpectedResult = 0f)]
    [TestCase(-1.0f,
              0,
              10,
              ExpectedResult = 0)]
    [TestCase(0.5f,
              0,
              1,
              ExpectedResult = 0.75f)]
    [TestCase(0.1f,
              0,
              10,
              ExpectedResult = 5.5f)]
    [TestCase(1.0f,
              0,
              10,
              ExpectedResult = 10f)]
    [TestCase(0f,
              5,
              10,
              ExpectedResult = 7.5f)]
    [TestCase(0.5f,
              -10,
              10,
              ExpectedResult = 5.0f)]
    [TestCase(0.75f,
              -10,
              10,
              ExpectedResult = 7.5f)]
    [TestCase(0.5f,
              -10,
              -5,
              ExpectedResult = -6.25f)]
    [TestCase(0.5f,
              -10,
              0,
              ExpectedResult = -2.5f)]
    public float TestDenormaliseMinusOneOne(float v, float min_value, float max_value) {
      var space = new Space1 {
                                 Min = min_value,
                                 Max = max_value,
                                 Normalised = ProjectionEnum.Minus_one_one_,
                                 DecimalGranularity = 2
                             };

      return space.Reproject(v : v);
    }

    /// <summary>
    ///
    /// </summary>
    [TestCase(5,
              0,
              10,
              ExpectedResult = 0.0f)]
    [TestCase(0f,
              0,
              1,
              ExpectedResult = -1.0f)]
    [TestCase(1f,
              -1,
              1,
              ExpectedResult = 1.0f)]
    [TestCase(-0.5f,
              -1,
              0,
              ExpectedResult = 0f)]
    public float TestNormaliseMinusOneOne(float v, float min_value, float max_value) {
      var space = new Space1 {
                                 Min = min_value,
                                 Max = max_value,
                                 DecimalGranularity = 2,
                                 Normalised = ProjectionEnum.Minus_one_one_
                             };

      //Assert.AreEqual(expected, result, tolerance);
      return space.Project(v : v);
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestNormaliseMinus11() {
      var space = new Space1 {
                                 Min = -1,
                                 Max = 1,
                                 Normalised = ProjectionEnum.Zero_one_,
                                 DecimalGranularity = 2
                             };

      Assert.That(Math.Abs(space.Project(0.5f) - 0.75f), Is.LessThanOrEqualTo(expected : float.Epsilon));
    }

    [Test]
    public void TestClipDenormalise01RoundClip() {
      var space = new Space1 {Min = -1, Max = 1, Normalised = ProjectionEnum.Zero_one_};

      Assert.That(Math.Abs(space.Reproject(0.5f) - 0.0f), Is.LessThanOrEqualTo(expected : float.Epsilon));
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestDenormaliseMinus11() {
      var space = new Space1 {
                                 Min = -1f,
                                 Max = 1f,
                                 Normalised = ProjectionEnum.Zero_one_,
                                 DecimalGranularity = 1
                             };

      var d = space.Reproject(0.75f);
      Assert.That(Math.Abs(d - 0.5f), Is.LessThanOrEqualTo(expected : float.Epsilon));
    }
  }
}
