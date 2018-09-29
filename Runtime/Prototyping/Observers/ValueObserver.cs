using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Value" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class ValueObserver : Observer,
                               IHasSingle {
    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    [SerializeField] ValueSpace _observation_value_space;

    /// <summary>
    ///
    /// </summary>
    public ValueSpace SingleSpace { get { return this._observation_value_space; } }

    /// <summary>
    ///
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      set {
        this._observation_value = this.NormaliseObservation
                                      ? this._observation_value_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    protected override void PreSetup() { this.FloatEnumerable = new[] {this.ObservationValue}; }

    public override void UpdateObservation() { this.FloatEnumerable = new[] {this.ObservationValue}; }
  }
}
