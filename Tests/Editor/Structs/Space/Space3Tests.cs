using System;
using droid.Runtime.Structs.Space;
using NUnit.Framework;
using UnityEngine;

namespace droid.Tests.Editor.Structs.Space {
  /// <summary>
  ///
  /// </summary>
  [TestFixture]
  public class Space3Tests {
    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestDenormalise010() {
      var space = new Space3 {MinValues = Vector3.zero,MaxValues =Vector3.one*10};

      Assert.That(Vector3.Distance(space.Denormalise01(Vector3.one*0.5f) ,Vector3.one*5f) <= float.Epsilon, Is
                      .True);
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestNormalise010() {
      var space = new Space3 {MinValues = Vector3.zero,MaxValues =Vector3.one*10};

      Assert.That(Vector3.Distance(space.Normalise01(6*Vector3.one),Vector3.one*0.6f) <= float.Epsilon, Is.True);

    }


    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestNormaliseMinus11() {

      var space = new Space3 {MinValues = Vector3.one*-1,MaxValues =Vector3.one*1};

      Assert.That(Vector3.Distance(space.Normalise01(0.5f*Vector3.one),Vector3.one*0.75f) <= float.Epsilon,
                  Is.True);

    }



    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestDenormaliseMinus11() {


      var space = new Space3 {MinValues = Vector3.one*-1,MaxValues =Vector3.one*1};

      Assert.That(Vector3.Distance(space.Denormalise01(0.75f*Vector3.one),Vector3.one*0.5f) <= float.Epsilon,
                  Is.True);

    }
  }
}
