using System;
using System.Collections;
using droid.Runtime.GameObjects.BoundingBoxes;
using droid.Runtime.GameObjects.BoundingBoxes.Experimental.Unused;
using UnityEngine;

namespace droid.Runtime.Prototyping.EnvironmentListener {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  [RequireComponent(typeof(Camera))]
  public class CameraFitter : EnvironmentListener {

    /// <summary>
    ///
    /// </summary>
    enum FitModeEnum {
      Zoom_,
      Move_
    }

    /// <summary>
    ///
    /// </summary>
    enum SourceEnum {
      BB,
      Collider
    }

    [SerializeField] BoundingBox bb;
    [SerializeField] Collider collider;
    [SerializeField] SourceEnum _source_enum = SourceEnum.Collider;
    [SerializeField] float margin = 0f;
    [SerializeField] FitModeEnum _fit_mode_enum = FitModeEnum.Zoom_;
    Camera _camera;

    public override void PreSetup() { base.PreSetup();
      if (!this.bb) {
        this.bb = FindObjectOfType<BoundingBox>();
      }

      if (!this.collider) {
        this.collider = FindObjectOfType<Collider>();
      }

      if (!this._camera) {
        this._camera = this.GetComponent<Camera>();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void PostTick() {
      base.PostTick();
      switch (this._source_enum) {
        case SourceEnum.BB:
          if (this.bb) {
            this._camera.transform.LookAt(this.bb.transform);
            var radius = this.bb.Bounds.extents.MaxDim();
            switch (this._fit_mode_enum) {
              case FitModeEnum.Zoom_:
                this._camera.LookAtZoomToInstant(radius, this.bb.transform.position, this.margin);
                break;
              case FitModeEnum.Move_:
                this._camera.MoveToInstant(radius, this.bb.transform.position, this.margin);
                break;
              default: throw new ArgumentOutOfRangeException();
            }

          }

          break;
        case SourceEnum.Collider:
          if (this.collider) {
            this._camera.transform.LookAt(this.bb.transform);
            var radius = this.collider.bounds.extents.MaxDim();
            switch (this._fit_mode_enum) {
              case FitModeEnum.Zoom_:
                this._camera.LookAtZoomToInstant(radius, this.collider.transform.position, this.margin);
                break;
              case FitModeEnum.Move_:
                this._camera.MoveToInstant(radius, this.collider.transform.position, this.margin);
                break;
              default: throw new ArgumentOutOfRangeException();
            }

          }

          break;
        default: throw new ArgumentOutOfRangeException();
      }
    }

  }

  /// <summary>
  ///
  /// </summary>
  public static class CameraFittingUtilities {

    // cam - camera to use
    // center - screen pixel center
    // pixelHeight - height of the rectangle in pixels
    // time - time to take zooming
    static IEnumerator ZoomToLerp(this Camera cam, Vector3 center, float pixel_height, float time) {
      var cam_tran = cam.transform;
      var ray = cam.ScreenPointToRay(center);
      var end_rotation = Quaternion.LookRotation(ray.direction);
      var position = cam_tran.position;
      var end_position = ProjectPointOnPlane(cam_tran.forward, position, ray.origin);
      var opp = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
      opp *= pixel_height / Screen.height;
      var end_fov = Mathf.Atan(opp) * 2.0f * Mathf.Rad2Deg;

      var timer = 0.0f;
      var start_rotation = cam_tran.rotation;
      var start_fov = cam.fieldOfView;
      var start_position = position;

      while (timer <= 1.0f) {
        var t = Mathf.Sin(timer * Mathf.PI * 0.5f);
        cam_tran.rotation = Quaternion.Slerp(start_rotation, end_rotation, t);
        cam_tran.position = Vector3.Lerp(start_position, end_position, t);
        cam.fieldOfView = Mathf.Lerp(start_fov, end_fov, t);
        timer += Time.deltaTime / time;
        yield return null;
      }

      cam_tran.rotation = end_rotation;
      cam_tran.position = end_position;
      cam.fieldOfView = end_fov;
    }

    // cam - camera to use
    // center - screen pixel center
    // pixelHeight - height of the rectangle in pixels
    public static void ZoomToInstant(this Camera cam, Vector2 center, float pixel_height) {
      var cam_tran = cam.transform;
      var ray = cam.ScreenPointToRay(center);
      var end_rotation = Quaternion.LookRotation(ray.direction);
      var end_position = ProjectPointOnPlane(cam_tran.forward, cam_tran.position, ray.origin);

      var opp = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
      opp *= pixel_height / Screen.height;
      var end_fov = Mathf.Atan(opp) * 2.0f * Mathf.Rad2Deg;

      cam_tran.rotation = end_rotation;
      cam_tran.position = end_position;
      cam.fieldOfView = end_fov;
    }

    public static void LookAtZoomToInstant(this Camera cam,
                                           float radius,
                                           Vector3 center,
                                           float margin = 1f) {

      cam.transform.LookAt(center,Vector3.up);
      var bound_sphere_radius = radius + margin;
      var distance = Vector3.Distance(center,cam.transform.position);
      var end_fov = Mathf.Atan((bound_sphere_radius * 0.5f)/distance) * 2.0f * Mathf.Rad2Deg;

      cam.fieldOfView = end_fov;
    }

    // cam - camera to use
    // center - screen pixel center
    // pixelHeight - height of the rectangle in pixels
    /// <summary>
    ///
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="rect"></param>
    /// <param name="bb_position"></param>
    /// <param name="margin"></param>
    public static void MoveToInstant(this Camera cam,
                                            float radius,
                                            Vector3 bb_position,
                                            float margin = 1f) {
      var bound_sphere_radius = radius + margin;
      var fov = Mathf.Deg2Rad * cam.fieldOfView;
      var cam_distance = bound_sphere_radius * .5f / Mathf.Tan(fov * .5f);

      var transform = cam.transform;
      transform.position = bb_position - transform.forward * cam_distance;
    }

    public static Vector3 ProjectPointOnPlane(Vector3 plane_normal, Vector3 plane_point, Vector3 point) {
      plane_normal.Normalize();
      var distance = -Vector3.Dot(plane_normal.normalized, point - plane_point);
      return point + plane_normal * distance;
    }
  }
}
