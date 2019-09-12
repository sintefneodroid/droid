using System;
using NUnit.Framework;
using UnityEngine;

namespace droid.Tests.Runtime {
  /// <summary>
  ///
  /// </summary>
  [TestFixture]
  public class RegistrationTests {
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
