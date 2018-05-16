using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.BoundingBoxes {
  [ExecuteInEditMode]
  public class ShowBoundingBoxes : MonoBehaviour {
    public GameObject _Line_Object;
    Dictionary<GameObject, GameObject> _lines;

    MeshFilter[] _mesh_filter_objects;
    public Color _Color = Color.green;

    void ReallocateLineRenderers() {
      this._mesh_filter_objects = FindObjectsOfType<MeshFilter>();
      this._lines = new Dictionary<GameObject, GameObject>();
    }

    void Update() {
      if (this._lines == null || this._mesh_filter_objects == null) {
        this.ReallocateLineRenderers();
      }

      this.CalcPositonsAndDrawBoxes();
    }

    void CalcPositonsAndDrawBoxes() {
      foreach (var mesh_filter_object in this._mesh_filter_objects) {
        if (mesh_filter_object.gameObject.CompareTag("Target")) {
          GameObject liner;
          if (!this._lines.ContainsKey(mesh_filter_object.gameObject)) {
            liner = Instantiate(this._Line_Object, this._Line_Object.transform);
            this._lines.Add(mesh_filter_object.gameObject, liner);
          } else {
            Debug.Log("found Target");
            liner = this._lines[mesh_filter_object.gameObject];
          }

          var bounds = mesh_filter_object.mesh.bounds;

          //Bounds bounds;
          //BoxCollider bc = GetComponent<BoxCollider>();
          //if (bc != null)
          //    bounds = bc.bounds;
          //else
          //return;

          var v3_center = bounds.center;
          var v3_extents = bounds.extents;

          var corners = Corners.ExtractCorners(v3_center, v3_extents, mesh_filter_object.transform);

          liner.GetComponent<LineRenderer>().SetPosition(0, corners[4]);
          liner.GetComponent<LineRenderer>().SetPosition(1, corners[5]);

          Corners.DrawBox(
              corners[0],
              corners[1],
              corners[2],
              corners[3],
              corners[4],
              corners[5],
              corners[6],
              corners[7],
              this._Color);
        }
      }
    }
  }
}
