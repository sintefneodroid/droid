using System.Collections.Generic;
using droid.Runtime.GameObjects;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Categorical"
                    + SensorComponentMenuPath._Postfix)]
  public class CategoricalSensor : Sensor,
                                   IHasSingle {
    [SerializeField] PrototypingGameObject _categoryProvider = null;
    [SerializeField] float _observationValue = 0;

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._categoryProvider is ICategoryProvider provider) {
        this.ObservationValue = provider.CurrentCategoryValue;
      } else {
        Debug.LogWarning($"{this._categoryProvider} does not implement ICategoryProvider, and will not provide at categorical value");
      }
    }

    public override void RemotePostSetup() {
      base.RemotePostSetup();
      if (this._categoryProvider is ICategoryProvider provider) {
        SingleSpace = provider.Space1;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public float ObservationValue {
      get { return this._observationValue; }
      private set { this._observationValue = value; }
    }

    /// <summary>
    ///
    /// </summary>
    [field : SerializeField]
    public Space1 SingleSpace { get; set; }
  }
}
