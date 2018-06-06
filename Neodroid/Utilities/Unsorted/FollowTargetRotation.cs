using UnityEngine;

namespace droid.Neodroid.Utilities.Unsorted {
  [ExecuteInEditMode]
  public class FollowTargetRotation : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    public Transform _TargetPose;

    public Quaternion _Rot;

    [SerializeField] Vector3 _forward;

    void LateUpdate() {
      if (this._TargetPose) {
        this._Rot = this._TargetPose.rotation;

        var projection_on_plane = Vector3.ProjectOnPlane(this._TargetPose.up, Vector3.up);

        var rot = this.transform.rotation;
        var normalised_proj = projection_on_plane.normalized;
        var view = Quaternion.Euler(0, -90, 0) * normalised_proj;
        rot.SetLookRotation(view, Vector3.down);
        this.transform.rotation = rot;
      }
    }
  }
}
