using System;
using droid.Neodroid.Prototyping.Motors;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Evaluation {
	/// <inheritdoc />
	/// <summary>
	/// </summary>
	[AddComponentMenu(
	EvaluationComponentMenuPath._ComponentMenuPath + "PoseDeviance" + EvaluationComponentMenuPath._Postfix)]
	public class MultiArmedBanditEvaluation : ObjectiveFunction {
		[SerializeField] BanditArmMotor[] _arms;
		
		/// <summary>
		/// 
		/// </summary>
		protected override void PostSetup() {
			if (this._arms == null) {
				this._arms = FindObjectsOfType<BanditArmMotor>();
			}
		}

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <exception cref="T:System.NotImplementedException"></exception>
		public override Single InternalEvaluate() {
			return 1;
		}
	}
}
