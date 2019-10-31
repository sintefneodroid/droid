using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "NearestByTag"
                    + SensorComponentMenuPath._Postfix)]
  public class NearestByTagSensor : Sensor,
                                    IHasEulerTransform {
    [SerializeField] Vector3 _direction;
    [SerializeField] Space3 _direction_space = Space3.ZeroOne;

    [Header("Specific", order = 102)]
    [SerializeField]
    GameObject _nearest_object;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = Space3.ZeroOne;
    [SerializeField] Vector3 _rotation;
    [SerializeField] Space3 _rotation_space = Space3.ZeroOne;
    [SerializeField] string _tag = "";

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName { get { return "Nearest" + this._tag; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this._position_space.Project(value);
      }
    }

    public Vector3 Rotation {
      get { return this._rotation; }
      set { this._rotation = this._rotation_space.Project(value); }
    }

    public Space3 PositionSpace { get { return this._position_space; } }
    public Space3 DirectionSpace { get { return this._direction_space; } }
    public Space3 RotationSpace { get { return this._rotation_space; } }

    public Vector3 Direction {
      get { return this._direction; }
      set {
        this._direction = this._direction_space.Project(value);
      }
    }

    public override IEnumerable<float> FloatEnumerable {
      get {
        return new[] {
                         this.Position.x,
                         this.Position.y,
                         this.Position.z,
                         this.Direction.x,
                         this.Direction.y,
                         this.Direction.z,
                         this.Rotation.x,
                         this.Rotation.y,
                         this.Rotation.z
                     };
      }
    }

    public override void UpdateObservation() {
      this._nearest_object = this.FindNearest();

      if (this.ParentEnvironment != null) {
        this.Position = this.ParentEnvironment.TransformPoint(this._nearest_object.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(this._nearest_object.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(this._nearest_object.transform.up);
      } else {
        this.Position = this._nearest_object.transform.position;
        this.Direction = this._nearest_object.transform.forward;
        this.Rotation = this._nearest_object.transform.up;
      }
    }


    GameObject FindNearest() {
      var candidates = FindObjectsOfType<GameObject>();
      var nearest_object = this.gameObject;
      var nearest_distance = -1.0;
      foreach (var candidate in candidates) {
        if (candidate.CompareTag(this._tag)) {
          var dist = Vector3.Distance(this.transform.position, candidate.transform.position);
          if (nearest_distance > dist || nearest_distance < 0) {
            nearest_distance = dist;
            nearest_object = candidate;
          }
        }
      }

      return nearest_object;
    }
  }
}
