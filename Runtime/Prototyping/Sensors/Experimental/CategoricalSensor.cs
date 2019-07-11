using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Structs;
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

    //void OneHotEncoding() { }

    /*public override string PrototypingTypeName {
      get { return "CategoricalSensor"; }
    }*/

    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._categoryProvider is ICategoryProvider provider) {
        this.ObservationValue = provider.CurrentCategoryValue;
      }
    }

    public float ObservationValue {
      get { return this._observationValue; }
      private set { this._observationValue = value; }
    }

    public Space1 SingleSpace { get; }
  }
}
