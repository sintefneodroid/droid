using System;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath
      + "QuaternionTransform"
      + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class QuaternionTransformObserver : Observer,
                                             IHasQuaternionTransform {
    [Header("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment_;

    [SerializeField] bool _use_environments_coordinates = true;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    public ObservationSpace Space { get { return this._space; } }

    public override string PrototypingType { get { return "QuaternionTransform"; } }

    public Vector3 Position { get { return this._position; } }

    public Quaternion Rotation { get { return this._rotation; } }

    public override void UpdateObservation() {
      if (this.ParentEnvironment && this._use_environments_coordinates) {
        this._position = this.ParentEnvironment.TransformPosition(this.transform.position);
        this._rotation = Quaternion.Euler(this.ParentEnvironment.TransformDirection(this.transform.forward));
      } else {
        this._position = this.transform.position;
        this._rotation = this.transform.rotation;
      }

      this.FloatEnumerable = new[] {
          this._position.x,
          this._position.y,
          this._position.z,
          this._rotation.x,
          this._rotation.y,
          this._rotation.z,
          this._rotation.w
      };
    }

    protected override void PreSetup() {
      this.FloatEnumerable = new[] {
          this._position.x,
          this._position.y,
          this._position.z,
          this._rotation.x,
          this._rotation.y,
          this._rotation.z,
          this._rotation.w
      };
    }
  }
}
