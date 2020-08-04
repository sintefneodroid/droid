using System;
using droid.Runtime.Enums;
using droid.Runtime.Structs.Space;
using NUnit.Framework;
using UnityEngine;

namespace droid.Tests.Editor.Structs.Space {
  /// <summary>
  ///
  /// </summary>
  [TestFixture]
  public class Space2Tests {
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
      var space = new Space2 {
                                 Min = Vector2.one * min_value,
                                 Max = Vector2.one * max_value,
                                 Normalised = ProjectionEnum.Zero_one_,
                                 DecimalGranularity = 2
                             };

      return space.Reproject(v : Vector2.one * v)[0];
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
      var space = new Space2 {
                                 Min = Vector2.one * min_value,
                                 Max = Vector2.one * max_value,
                                 DecimalGranularity = 2,
                                 Normalised = ProjectionEnum.Zero_one_
                             };

      return space.Project(v : Vector2.one * v)[0];
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
      var space = new Space2 {
                                 Min = Vector2.one * min_value,
                                 Max = Vector2.one * max_value,
                                 DecimalGranularity = 2,
                                 Normalised = ProjectionEnum.Minus_one_one_
                             };

      return space.Project(v : Vector2.one * v)[0];
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
      var space = new Space2 {
                                 Min = Vector2.one * min_value,
                                 Max = Vector2.one * max_value,
                                 Normalised = ProjectionEnum.Minus_one_one_,
                                 DecimalGranularity = 2
                             };

      return space.Reproject(v : Vector2.one * v)[0];
    }

    [TestCase(-1)]
    [TestCase(11)]
    public void TestDenormalise010Throws(float v) {
      var space = new Space2 {
                                 Min = 0 * Vector2.one,
                                 Max = 10 * Vector2.one,
                                 Normalised = ProjectionEnum.Zero_one_
                             };

      Assert.That(() => space.Reproject(v : v * Vector2.one), expr : Throws.TypeOf<ArgumentException>());
    }

    [TestCase(-1)]
    [TestCase(10.1f)]
    public void TestNormalise010Throws(float v) {
      var space = new Space2 {
                                 Min = 0 * Vector2.one,
                                 Max = 10 * Vector2.one,
                                 Normalised = ProjectionEnum.Zero_one_
                             };

      Assert.That(() => space.Project(v : v * Vector2.one), expr : Throws.TypeOf<ArgumentException>());
    }

    [TestCase(-2)]
    [TestCase(2)]
    public void TestDenormaliseMinusOneOne010Throws(float v) {
      var space = new Space2 {
                                 Min = 0 * Vector2.one,
                                 Max = 10 * Vector2.one,
                                 Normalised = ProjectionEnum.Minus_one_one_
                             };

      Assert.That(() => space.Reproject(v : v * Vector2.one), expr : Throws.TypeOf<ArgumentException>());
    }

    [TestCase(-1)]
    [TestCase(10.1f)]
    public void TestNormaliseMinusOneOne010Throws(float v) {
      var space = new Space2 {
                                 Min = 0 * Vector2.one,
                                 Max = 10 * Vector2.one,
                                 Normalised = ProjectionEnum.Minus_one_one_
                             };

      Assert.That(() => space.Project(v : v * Vector2.one), expr : Throws.TypeOf<ArgumentException>());
    }
  }
}
