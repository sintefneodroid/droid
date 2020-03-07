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
    [Test]
    public void TestDenormalise010() {
      var space = new Space2 {
                                 Min = Vector2.zero,
                                 Max = Vector2.one * 10,
                                 Normalised = NormalisationEnum.Zero_one_
                             };

      Assert.That(Vector2.Distance(a : space.Reproject(v : Vector2.one * 0.5f), b : Vector2.one * 5f)
                  <= float.Epsilon,
                  Is.True);
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestNormalise010() {
      var space = new Space2 {
                                 Min = Vector2.zero,
                                 Max = Vector2.one * 10,
                                 Normalised = NormalisationEnum.Zero_one_
                             };

      Assert.That(Vector2.Distance(a : space.Project(v : 6 * Vector2.one), b : Vector2.one * 0.6f)
                  <= float.Epsilon,
                  Is.True);
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestNormaliseMinus11() {
      var space = new Space2 {
                                 Min = Vector2.one * -1,
                                 Max = Vector2.one * 1,
                                 Normalised = NormalisationEnum.Zero_one_
                             };

      Assert.That(Vector2.Distance(a : space.Project(v : 0.5f * Vector2.one), b : Vector2.one * 0.75f)
                  <= float.Epsilon,
                  Is.True);
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestDenormaliseMinus11() {
      var space = new Space2 {
                                 Min = Vector2.one * -1,
                                 Max = Vector2.one * 1,
                                 Normalised = NormalisationEnum.Zero_one_
                             };

      Assert.That(Vector2.Distance(a : space.Reproject(v : 0.75f * Vector2.one), b : Vector2.one * 0.5f)
                  <= float.Epsilon,
                  Is.True);
    }
  }
}
