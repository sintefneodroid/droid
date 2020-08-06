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
      const string go_name = "MyGameObject";
      var go = new GameObject(name : go_name);
      Assert.AreEqual(expected : go_name, actual : go.name);
    }
  }
}
