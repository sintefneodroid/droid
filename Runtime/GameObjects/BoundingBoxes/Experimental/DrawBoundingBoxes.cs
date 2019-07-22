using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.GameObjects.BoundingBoxes.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ShowBoundingBoxes : MonoBehaviour {
    public Color _Color = Color.green;
    public GameObject _Line_Object;
    Dictionary<GameObject, GameObject> _lines = new Dictionary<GameObject, GameObject>();

    MeshFilter[] _mesh_filter_objects;

    void ReallocateLineRenderers() {
      this._mesh_filter_objects = FindObjectsOfType<MeshFilter>();
      this._lines.Clear();
    }

    // void OnWillRenderObject() { throw new System.NotImplementedException(); }

    void Update() {
      if (this._lines == null || this._mesh_filter_objects == null) {
        this.ReallocateLineRenderers();
      }

      this.CalcPositionsAndDrawBoxes();
    }

    void CalcPositionsAndDrawBoxes() {
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

          Corners.DrawBox(corners[0],
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
