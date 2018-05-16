using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "NearestByTag" + ObserverComponentMenuPath._Postfix)]
  public class NearestByTagObserver : Observer,
                                      IHasEulerTransform {
    [SerializeField] string _tag = "";
    [SerializeField] Vector3 _direction;
    [SerializeField] Space3 _direction_space = new Space3(10);

    [Header("Specific", order = 102)]
    [SerializeField]
    GameObject _nearest_object;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3(10);
    [SerializeField] Vector3 _rotation;
    [SerializeField] Space3 _rotation_space = new Space3(10);

    public override string PrototypingType { get { return "Nearest" + this._tag; } }

    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this.NormaliseObservationUsingSpace
                             ? this._position_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    public Vector3 Rotation {
      get { return this._rotation; }
      set {
        this._rotation = this.NormaliseObservationUsingSpace
                             ? this._rotation_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    public Space3 PositionSpace { get; }
    public Space3 DirectionSpace { get; }
    public Space3 RotationSpace { get; }

    public Vector3 Direction {
      get { return this._direction; }
      set {
        this._direction = this.NormaliseObservationUsingSpace
                              ? this._direction_space.ClipNormaliseRound(value)
                              : value;
      }
    }

    public override void UpdateObservation() {
      this._nearest_object = this.FindNearest();

      if (this.ParentEnvironment) {
        this.Position = this.ParentEnvironment.TransformPosition(this._nearest_object.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(this._nearest_object.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(this._nearest_object.transform.up);
      } else {
        this.Position = this._nearest_object.transform.position;
        this.Direction = this._nearest_object.transform.forward;
        this.Rotation = this._nearest_object.transform.up;
      }

      this.FloatEnumerable = new[] {
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

    protected override void PreSetup() {
      this.FloatEnumerable = new[] {
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
