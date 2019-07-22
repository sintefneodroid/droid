using UnityEngine;

namespace droid.Runtime.Utilities.Orientation {
  [ExecuteInEditMode]
  public class FollowTargetRotation : MonoBehaviour {
    [SerializeField] Vector3 _forward;

    public Quaternion rot;

    /// <summary>
    /// </summary>
    public Transform targetPose;

    void LateUpdate() {
      if (this.targetPose) {
        this.rot = this.targetPose.rotation;

        var projection_on_plane = Vector3.ProjectOnPlane(this.targetPose.up, Vector3.up);

        var rot = this.transform.rotation;
        var normalised_proj = projection_on_plane.normalized;
        var view = Quaternion.Euler(0, -90, 0) * normalised_proj;
        if (view != Vector3.zero) {
          rot.SetLookRotation(view, Vector3.down);
        }

        this.transform.rotation = rot;
      }
    }
  }
}
