using NUnit.Framework;
using UnityEngine;

namespace NeodroidTests {
	[TestFixture]
	public class SimulatorTests {
		[Test]
		public void Sanity(){
			Assert.That(true, Is.True);
		}
		
		[Test]
		public void RegistrationNameCheck()
		{
			var go = new GameObject("MyGameObject");
			Assert.AreEqual("MyGameObject", go.name);
		}
	}
}
