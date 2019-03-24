using System.Collections;
using System.Collections.Generic;
using droid.Runtime.Utilities.BoundingBoxes;
using droid.Runtime.Utilities.BoundingBoxes.Experimental;
using UnityEngine;

namespace droid.Runtime.Utilities.NeodroidCamera
{
    [RequireComponent(typeof(Camera))]

    public class ZoomToRect : MonoBehaviour
    {

        [SerializeField] BoundingBox bb;
        [SerializeField] float margin = 1f;
        Camera _came;



        void Start()
        {
            if(!this.bb)
            {
                this.bb = FindObjectOfType<BoundingBox>();
            }

            if (!this._came)
            {
                this._came = this.GetComponent<Camera>();
            }

        }

        void Update()
        {
            if (this.bb)
            {
                this._came.transform.LookAt(this.bb.transform);
                var screenSpaceBoundingRect = this.bb.ScreenSpaceBoundingRect(this._came, this.margin);
                this._came.MoveToDisplayInstant(screenSpaceBoundingRect, this.bb.transform.position, this.margin);
            }
        }




    }

    public static class Utils{


        // cam - camera to use
        // center - screen pixel center
        // pixelHeight - height of the rectangle in pixels
        // time - time to take zooming
        static IEnumerator ZoomToDisplay(this Camera cam, Vector3 center, float pixelHeight, float time) {
            var camTran = cam.transform;
            var ray = cam.ScreenPointToRay(center);
            var endRotation = Quaternion.LookRotation(ray.direction);
            var position = camTran.position;
            var endPosition = ProjectPointOnPlane(camTran.forward, position, ray.origin);
            var opp = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            opp *= pixelHeight / Screen.height;
            var endFov = Mathf.Atan(opp) * 2.0f * Mathf.Rad2Deg;

            var timer = 0.0f;
            var startRotation = camTran.rotation;
            var startFov = cam.fieldOfView;
            var startPosition = position;

            while (timer <= 1.0f) {
                var t = Mathf.Sin(timer * Mathf.PI * 0.5f);
                camTran.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                camTran.position = Vector3.Lerp(startPosition, endPosition, t);
                cam.fieldOfView = Mathf.Lerp(startFov, endFov, t);
                timer += Time.deltaTime / time;
                yield return null;
            }

            camTran.rotation = endRotation;
            camTran.position = endPosition;
            cam.fieldOfView = endFov;
        }

        // cam - camera to use
        // center - screen pixel center
        // pixelHeight - height of the rectangle in pixels
        public static void ZoomToDisplayInstant(this Camera cam, Vector2 center, float pixelHeight) {
            var camTran = cam.transform;
            var ray = cam.ScreenPointToRay(center);
            var endRotation = Quaternion.LookRotation(ray.direction);
            var endPosition = ProjectPointOnPlane(camTran.forward, camTran.position, ray.origin);

            var opp = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            opp *= pixelHeight / Screen.height;
            var endFov = Mathf.Atan(opp) * 2.0f * Mathf.Rad2Deg;

            camTran.rotation = endRotation;
            camTran.position = endPosition;
            cam.fieldOfView = endFov;
        }

        // cam - camera to use
        // center - screen pixel center
        // pixelHeight - height of the rectangle in pixels
        public static void MoveToDisplayInstant(this Camera cam, Rect rect, Vector3 transformPosition,
            float margin = 0f)
        {

            var boundSphereRadius = Mathf.Max(rect.size.x, rect.size.y)+margin;
            var fov = Mathf.Deg2Rad * cam.fieldOfView;

            var camDistance = boundSphereRadius *.5f / Mathf.Tan(fov *.5f);

            var position = cam.transform.position;
            position = transformPosition - cam.transform.forward * camDistance;
            cam.transform.position = position;
        }

        public static Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point) {
            planeNormal.Normalize();
            var distance = -Vector3.Dot(planeNormal.normalized, (point - planePoint));
            return point + planeNormal * distance;
        }


    }
}