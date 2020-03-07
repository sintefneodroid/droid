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
      for (var index = 0; index < this._mesh_filter_objects.Length; index++) {
        var mesh_filter_object = this._mesh_filter_objects[index];
        if (mesh_filter_object.gameObject.CompareTag("Target")) {
          GameObject liner;
          if (!this._lines.ContainsKey(key : mesh_filter_object.gameObject)) {
            liner = Instantiate(original : this._Line_Object, parent : this._Line_Object.transform);
            this._lines.Add(key : mesh_filter_object.gameObject, value : liner);
          } else {
            Debug.Log("found Target");
            liner = this._lines[key : mesh_filter_object.gameObject];
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

          var corners = Corners.ExtractCorners(v3_center : v3_center,
                                               v3_extents : v3_extents,
                                               reference_transform : mesh_filter_object.transform);

          liner.GetComponent<LineRenderer>().SetPosition(0, position : corners[4]);
          liner.GetComponent<LineRenderer>().SetPosition(1, position : corners[5]);

          Corners.DrawBox(v3_front_top_left : corners[0],
                          v3_front_top_right : corners[1],
                          v3_front_bottom_left : corners[2],
                          v3_front_bottom_right : corners[3],
                          v3_back_top_left : corners[4],
                          v3_back_top_right : corners[5],
                          v3_back_bottom_left : corners[6],
                          v3_back_bottom_right : corners[7],
                          color : this._Color);
        }
      }
    }
  }
}
