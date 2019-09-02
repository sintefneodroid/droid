using System;
using droid.Runtime.Structs.Space;
using NUnit.Framework;
using UnityEngine;

namespace droid.Tests.Editor.Structs.Space {
  /// <summary>
  ///
  /// </summary>
  [TestFixture]
  public class Space4Tests {
    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestDenormalise010() {
      var space = new Space4 {Min = Vector4.zero,Max =Vector4.one*10};

      Assert.That(Vector4.Distance(space.Denormalise01(Vector4.one*0.5f) ,Vector4.one*5f) <= float.Epsilon, Is
                      .True);
    }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestNormalise010() {
      var space = new Space4 {Min = Vector4.zero,Max =Vector4.one*10};

      Assert.That(Vector4.Distance(space.Normalise01(6*Vector4.one),Vector4.one*0.6f) <= float.Epsilon, Is.True);

    }


    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestNormaliseMinus11() {

      var space = new Space4 {Min = Vector4.one*-1,Max =Vector4.one*1};

      Assert.That(Vector4.Distance(space.Normalise01(0.5f*Vector4.one),Vector4.one*0.75f) <= float.Epsilon,
                  Is.True);

    }



    /// <summary>
    ///
    /// </summary>
    [Test]
    public void TestDenormaliseMinus11() {


      var space = new Space4 {Min = Vector4.one*-1,Max =Vector4.one*1};

      Assert.That(Vector4.Distance(space.Denormalise01(0.75f*Vector4.one),Vector4.one*0.5f) <= float.Epsilon,
                  Is.True);

    }
  }
}
