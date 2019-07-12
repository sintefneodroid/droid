using System;
using NUnit.Framework;
using UnityEngine;

namespace droid.Tests.Runtime.Structs.Space {
  /// <summary>
  ///
  /// </summary>
  [TestFixture]
  public class SanityTests {
    /// <summary>
    /// 
    /// </summary>
    [Test] public void Sanity() { Assert.That(true, Is.True); }

    /// <summary>
    ///
    /// </summary>
    [Test]
    public void RegistrationNameCheck() {
      const String go_name = "MyGameObject";
      var go = new GameObject(go_name);
      Assert.AreEqual(go_name, go.name);
    }
  }
}
