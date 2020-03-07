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
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "Position2D"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class Position2DSensor : Sensor,
                                  IHasDouble {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector2 _2_d_position;

    [SerializeField]
    [SearchableEnum]
    Dimension2DCombinationEnum _dim_combinationEnum = Dimension2DCombinationEnum.Xz_;

    [SerializeField] Space2 _position_space = Space2.ZeroOne;

    [Header("Specific", order = 102)]
    [SerializeField]
    CoordinateSpaceEnum _coordinate_spaceEnum = CoordinateSpaceEnum.Environment_;

    [SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Vector2 ObservationValue { get { return this._2_d_position; } set { this._2_d_position = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Space2 DoubleSpace { get { return this._position_space; } }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetPosition(Vector3 position) {
      Vector2 vector2_pos;
      switch (this._dim_combinationEnum) {
        case Dimension2DCombinationEnum.Xy_:
          vector2_pos = new Vector2(x : position.x, y : position.y);
          break;
        case Dimension2DCombinationEnum.Xz_:
          vector2_pos = new Vector2(x : position.x, y : position.z);
          break;
        case Dimension2DCombinationEnum.Yz_:
          vector2_pos = new Vector2(x : position.y, y : position.z);
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      this._2_d_position = this._position_space.Project(v : vector2_pos);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this._2_d_position.x, this._2_d_position.y}; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null && this._coordinate_spaceEnum == CoordinateSpaceEnum.Environment_) {
        this.SetPosition(position : this.ParentEnvironment.TransformPoint(point : this.transform.position));
      } else if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Local_) {
        this.SetPosition(position : this.transform.localPosition);
      } else {
        this.SetPosition(position : this.transform.position);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      base.RemotePostSetup();
      if (this.normalised_overwrite_space_if_env_bounds) {
        if (this.ParentEnvironment && this.ParentEnvironment.PlayableArea != null) {
          var ex = this.ParentEnvironment.PlayableArea.Bounds.extents;
          switch (this._dim_combinationEnum) {
            case Dimension2DCombinationEnum.Xy_:
              this._position_space =
                  Space2.FromCenterExtents(bounds_extents : new Vector2(x : ex.x, y : ex.y));
              break;
            case Dimension2DCombinationEnum.Xz_:
              this._position_space =
                  Space2.FromCenterExtents(bounds_extents : new Vector2(x : ex.x, y : ex.z));
              break;
            case Dimension2DCombinationEnum.Yz_:
              this._position_space =
                  Space2.FromCenterExtents(bounds_extents : new Vector2(x : ex.y, y : ex.z));
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
        switch (this._dim_combinationEnum) {
          case Dimension2DCombinationEnum.Xy_:
            Debug.DrawLine(start : position, end : position + Vector3.right * 2, color : Color.green);
            Debug.DrawLine(start : position, end : position + Vector3.up * 2, color : Color.red);
            break;
          case Dimension2DCombinationEnum.Xz_:
            Debug.DrawLine(start : position, end : position + Vector3.right * 2, color : Color.green);
            Debug.DrawLine(start : position, end : position + Vector3.forward * 2, color : Color.red);
            break;
          case Dimension2DCombinationEnum.Yz_:

            Debug.DrawLine(start : position, end : position + Vector3.up * 2, color : Color.green);
            Debug.DrawLine(start : position, end : position + Vector3.forward * 2, color : Color.red);
            break;
          default: //TODO add the Direction cases
            Gizmos.DrawIcon(center : position, "console.warnicon", true);
            break;
        }
      }
    }
    #endif
  }
}
