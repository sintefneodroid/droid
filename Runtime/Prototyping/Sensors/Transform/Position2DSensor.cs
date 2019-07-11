using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.Misc.SearchableEnum;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "PositionObserver2D"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class Position2DSensor : Sensor,
                                  IHasDouble {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector2 _2_d_position;

    [SerializeField] [SearchableEnum] Dimension2DCombination _dim_combination = Dimension2DCombination.Xz_;

    [SerializeField] Space2 _position_space = Space2.ZeroOne;

    [Header("Specific", order = 102)]
    [SerializeField]
    ObservationSpace observation_space = ObservationSpace.Environment_;

    /// <summary>
    ///
    /// </summary>
    public Vector2 ObservationValue { get { return this._2_d_position; } set { this._2_d_position = value; } }

    /// <summary>
    ///
    /// </summary>
    public Space2 DoubleSpace { get { return this._position_space; } }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetPosition(Vector3 position) {
      Vector2 vector2_pos;
      switch (this._dim_combination) {
        case Dimension2DCombination.Xy_:
          vector2_pos = new Vector2(position.x, position.y);
          break;
        case Dimension2DCombination.Xz_:
          vector2_pos = new Vector2(position.x, position.z);
          break;
        case Dimension2DCombination.Yz_:
          vector2_pos = new Vector2(position.y, position.z);
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      this._2_d_position = this.DoubleSpace.Normalised
                               ? this._position_space.ClipNormaliseRound(vector2_pos)
                               : vector2_pos;
    }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this._2_d_position.x, this._2_d_position.y}; }
    }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null && this.observation_space == ObservationSpace.Environment_) {
        this.SetPosition(this.ParentEnvironment.TransformPoint(this.transform.position));
      } else if (this.observation_space == ObservationSpace.Local_) {
        this.SetPosition(this.transform.localPosition);
      } else {
        this.SetPosition(this.transform.position);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() { }

    void OnDrawGizmos() {
      if (this.enabled) {
        var position = this.transform.position;
        switch (this._dim_combination) {
          case Dimension2DCombination.Xy_:
            Debug.DrawLine(position, position + Vector3.right * 2, Color.green);
            Debug.DrawLine(position, position + Vector3.up * 2, Color.red);
            break;
          case Dimension2DCombination.Xz_:
            Debug.DrawLine(position, position + Vector3.right * 2, Color.green);
            Debug.DrawLine(position, position + Vector3.forward * 2, Color.red);
            break;
          case Dimension2DCombination.Yz_:

            Debug.DrawLine(position, position + Vector3.up * 2, Color.green);
            Debug.DrawLine(position, position + Vector3.forward * 2, Color.red);
            break;
          default: //TODO add the Direction cases
            Gizmos.DrawIcon(position, "console.warnicon", true);
            break;
        }
      }
    }
  }
}
