using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Position2D"
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
    CoordinateSpace _coordinate_space = CoordinateSpace.Environment_;

    [SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

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

      this._2_d_position = this._position_space.Project(vector2_pos);

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
      if (this.ParentEnvironment != null && this._coordinate_space == CoordinateSpace.Environment_) {
        this.SetPosition(this.ParentEnvironment.TransformPoint(this.transform.position));
      } else if (this._coordinate_space == CoordinateSpace.Local_) {
        this.SetPosition(this.transform.localPosition);
      } else {
        this.SetPosition(this.transform.position);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        if (this.ParentEnvironment && this.ParentEnvironment.PlayableArea != null) {
          var ex = this.ParentEnvironment.PlayableArea.Bounds.extents;
          switch (this._dim_combination) {
            case Dimension2DCombination.Xy_:
              this._position_space = Space2.FromCenterExtents(new Vector2(ex.x, ex.y));
              break;
            case Dimension2DCombination.Xz_:
              this._position_space = Space2.FromCenterExtents(new Vector2(ex.x, ex.z));
              break;
            case Dimension2DCombination.Yz_:
              this._position_space = Space2.FromCenterExtents(new Vector2(ex.y, ex.z));
              break;
            default: throw new ArgumentOutOfRangeException();
          }
        }
      }
    }

    #if UNITY_EDITOR

    void OnDrawGizmosSelected() {
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
    #endif
  }
}
