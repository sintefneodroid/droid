using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Grid {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "GridPosition"
                    + SensorComponentMenuPath._Postfix)]
  public class GridPositionSensor : Sensor,
                                    IHasSingle {
    /// <summary>
    /// </summary>
    int[,] _grid = null;

    /// <summary>
    /// </summary>
    [SerializeField]
    int _height = 0;

    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    [SerializeField] Space1 _observation_value_space;
    [SerializeField] int _width = 0;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Value"; } }

    /// <summary>
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = this.SingleSpace.Project(value); }
    }

    public Space1 SingleSpace { get { return this._observation_value_space; } }

    public override void PreSetup() {
      this._grid = new int[this._width, this._height];

      var k = 0;
      for (var i = 0; i < this._width; i++) {
        for (var j = 0; j < this._height; j++) {
          this._grid[i, j] = k++;
        }
      }
    }

    public override IEnumerable<float> FloatEnumerable { get { return new[] {this.ObservationValue}; } }

    public override void UpdateObservation() {
      var position = this.transform.position;
      var x = position.x + this._width;
      var z = position.z + this._height;

      this.ObservationValue = this._grid[(int)x, (int)z];
    }
  }
}
