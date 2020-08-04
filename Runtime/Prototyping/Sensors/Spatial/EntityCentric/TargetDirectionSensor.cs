using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "Compass"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class TargetDirectionSensor : Sensor,
                                       IHasDouble {
    /// <summary>
    /// </summary>
    [SerializeField]
    Vector2 _2_d_position = Vector2.zero;

    /// <summary>
    /// </summary>
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position = Vector3.zero;

    /// <summary>
    /// </summary>
    [SerializeField]
    Space3 _position_space = new Space3 {DecimalGranularity = 1, Max = Vector3.one, Min = -Vector3.one};

    /// <summary>
    /// </summary>
    [Header("Specific", order = 102)]
    [SerializeField]
    UnityEngine.Transform _target = null;

    /// <summary>
    /// </summary>
    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this._position_space.Project(v : value);
        this._2_d_position = new Vector2(x : this._position.x, y : this._position.z);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space2 DoubleSpace {
      get {
        return new Space2 {
                              Max = new Vector2(x : this._position_space.Max.x,
                                                y : this._position_space.Max.y),
                              Min = new Vector2(x : this._position_space.Min.x,
                                                y : this._position_space.Min.y),
                              DecimalGranularity = this._position_space.DecimalGranularity
                          };
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector2 ObservationValue { get { return this._2_d_position; } set { this._2_d_position = value; } }

    public override IEnumerable<Single> FloatEnumerable {
      get {
        yield return this.Position.x;
        yield return this.Position.z;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.Position =
          this.transform.InverseTransformVector(vector : this.transform.position - this._target.position)
              .normalized;
    }
  }
}
