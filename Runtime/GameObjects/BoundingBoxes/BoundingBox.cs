using System;
using System.Collections.Generic;
using droid.Runtime.Enums.BoundingBox;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects.BoundingBoxes.Experimental;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace droid.Runtime.GameObjects.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class BoundingBox : MonoBehaviour {
    /// <summary>
    /// </summary>
    Transform _bb_transform = null;

    /// <summary>
    /// </summary>
    protected Bounds _Bounds = new Bounds();

    /// <summary>
    /// </summary>
    protected Vector3 _Bounds_Offset;

    /// <summary>
    /// </summary>
    Collider[] _children_colliders = null;

    /// <summary>
    /// </summary>
    MeshFilter[] _children_meshes = null;

    GameObject _empty_go = null;

    /// <summary>
    /// </summary>
    Vector3[,] _lines = null;

    List<Vector3[]> _lines_list = new List<Vector3[]>();

    Collider _local_collider;

    MeshFilter _local_mesh;

    Vector3 _point_bbl;
    Vector3 _point_bbr;
    Vector3 _point_bfl;
    Vector3 _point_bfr;
    Vector3 _point_tbl;
    Vector3 _point_tbr;
    Vector3 _point_tfl;
    Vector3 _point_tfr;

    /// <summary>
    /// </summary>
    Vector3[] _points = null;

    #region fields

    /// <summary>
    /// </summary>
    [SerializeField]
    public bool _use_bb_transform = false;

    [SerializeField] bool _use_shared_mesh = false;

    /// <summary>
    /// </summary>
    [SearchableEnum]
    public BasedOn basedOn = BasedOn.Geometry_;

    /// <summary>
    /// </summary>
    [SerializeField]
    float bb_margin = 0;

    /// <summary>
    /// </summary>
    [SerializeField]
    BoundingBoxOrientation BbAligning = BoundingBoxOrientation.Axis_aligned_;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool cacheChildren = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    Color editorPreviewLineColor = new Color(1f,
                                             0.36f,
                                             0.38f,
                                             0.74f);

    /// <summary>
    /// </summary>
    [SerializeField]
    ISpatialPrototypingEnvironment environment = null;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool freezeAfterFirstCalculation = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool includeChildren = false;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool _only_active_children = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool includeSelf = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool OnAwakeSetup = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool RunInEditModeSetup = false;

    #endregion

    #region Properties

    /// <summary>
    /// </summary>
    public Vector3[] BoundingBoxCoordinates {
      get {
        return new[] {
                         this._point_tfl,
                         this._point_tfr,
                         this._point_tbl,
                         this._point_tbr,
                         this._point_bfl,
                         this._point_bfr,
                         this._point_bbl,
                         this._point_bbr
                     };
      }
    }

    /// <summary>
    /// </summary>
    public Bounds Bounds { get { return this._Bounds; } }

    public Vector3 Max { get { return this._Bounds.max; } }

    public Vector3 Min { get { return this._Bounds.min; } }

    /// <summary>
    /// </summary>
    public string BoundingBoxCoordinatesAsString {
      get {
        var str_rep = "";
        str_rep += $"\"_top_front_left\":{this.BoundingBoxCoordinates[0]}, ";
        str_rep += $"\"_top_front_right\":{this.BoundingBoxCoordinates[1]}, ";
        str_rep += $"\"_top_back_left\":{this.BoundingBoxCoordinates[2]}, ";
        str_rep += $"\"_top_back_right\":{this.BoundingBoxCoordinates[3]}, ";
        str_rep += $"\"_bottom_front_left\":{this.BoundingBoxCoordinates[4]}, ";
        str_rep += $"\"_bottom_front_right\":{this.BoundingBoxCoordinates[5]}, ";
        str_rep += $"\"_bottom_back_left\":{this.BoundingBoxCoordinates[6]}, ";
        str_rep += $"\"_bottom_back_right\":{this.BoundingBoxCoordinates[7]}";
        return str_rep;
      }
    }

    /// <summary>
    /// </summary>
    public string BoundingBoxCoordinatesWorldSpaceAsJson {
      get {
        var str_rep = "{";
        var transform1 = this.transform;
        if (this._use_bb_transform) {
          transform1 = this._bb_transform;
        }

        var rotation = transform1.rotation;
        var position = transform1.position;
        if (this.environment != null) {
          str_rep +=
              $"\"top_front_left\":{JsonifyVec3(this.environment.TransformPoint(rotation * this._point_tfl + position))}, ";
          str_rep +=
              $"\"bottom_back_right\":{JsonifyVec3(this.environment.TransformPoint(rotation * this._point_bbr + position))}";
        }

        str_rep += "}";
        return str_rep;
      }
    }

    /// <summary>
    /// </summary>
    public Vector3[,] Lines { get { return this._lines; } }

    /// <summary>
    /// </summary>
    public Vector3[] Points { get { return this._points; } }

    /// <summary>
    ///
    /// </summary>
    public Color EditorPreviewLineColor {
      get { return this.editorPreviewLineColor; }
      set { this.editorPreviewLineColor = value; }
    }

    #endregion

    /// <summary>
    /// </summary>
    /// <param name="a_camera"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public Rect ScreenSpaceBoundingRect(Camera a_camera, float margin = 0f) {
      if (this.basedOn == BasedOn.Collider_) {
        var a = this._local_collider as MeshCollider;
        if (a) {
          return a.sharedMesh.GetCameraMinMaxRect(this.transform, a_camera, this.bb_margin - margin);
        }
      }

      if (this._local_mesh) {
        if (this._use_shared_mesh || !Application.isPlaying) {
          var a = this._local_mesh.sharedMesh.GetCameraMinMaxPoints(this.transform, a_camera);
          if (this.includeChildren) {
            foreach (var children_mesh in this._children_meshes) {
              a = children_mesh.sharedMesh.GetCameraMinMaxPoints(children_mesh.transform,
                                                                 a_camera,
                                                                 a[0],
                                                                 a[1]);
            }

            return BoundingBoxUtilities.GetMinMaxRect(a[0], a[1], this.bb_margin - margin);
          }
        } else {
          var a = this._local_mesh.mesh.GetCameraMinMaxPoints(this.transform, a_camera);
          if (this.includeChildren) {
            foreach (var children_mesh in this._children_meshes) {
              a = children_mesh.mesh.GetCameraMinMaxPoints(children_mesh.transform,
                                                           a_camera,
                                                           a[0],
                                                           a[1]);
            }

            return BoundingBoxUtilities.GetMinMaxRect(a[0], a[1], this.bb_margin - margin);
          }
        }
      } else {
        if (this._use_shared_mesh || !Application.isPlaying) {
          if (this._children_meshes != null && this._children_meshes.Length > 0) {
            var a = this._children_meshes[0].sharedMesh
                        .GetCameraMinMaxPoints(this._children_meshes[0].transform, a_camera);
            if (this.includeChildren) {
              for (var index = 1; index < this._children_meshes.Length; index++) {
                var children_mesh = this._children_meshes[index];
                a = children_mesh.sharedMesh.GetCameraMinMaxPoints(children_mesh.transform,
                                                                   a_camera,
                                                                   a[0],
                                                                   a[1]);
              }

              return BoundingBoxUtilities.GetMinMaxRect(a[0], a[1], this.bb_margin - margin);
            }
          }
        } else {
          if (this._children_meshes != null && this._children_meshes.Length > 0) {
            var a = this._children_meshes[0].mesh
                        .GetCameraMinMaxPoints(this._children_meshes[0].transform, a_camera);
            if (this.includeChildren) {
              for (var index = 1; index < this._children_meshes.Length; index++) {
                var children_mesh = this._children_meshes[index];
                a = children_mesh.mesh.GetCameraMinMaxPoints(children_mesh.transform,
                                                             a_camera,
                                                             a[0],
                                                             a[1]);
              }

              return BoundingBoxUtilities.GetMinMaxRect(a[0], a[1], this.bb_margin - margin);
            }
          }
        }
      }

      return new Rect();
    }

    /// <summary>
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    static string JsonifyVec3(Vector3 vec) { return $"[{vec.x},{vec.y},{vec.z}]"; }

    /// <summary>
    /// </summary>
    void BoundingBoxReset() {
      this.Awake();
      this.Start();
    }

    /// <summary>
    /// </summary>
    void Start() {
      if (!this.OnAwakeSetup) {
        this.Setup();
      }
    }

    /// <summary>
    /// </summary>
    void Awake() {
      if (!this.enabled) {
        return;
      }

      if (this.environment == null) {
        this.environment = FindObjectOfType<AbstractSpatialPrototypingEnvironment>();
      }

      if (!this._bb_transform) {
        this._empty_go = new GameObject {hideFlags = HideFlags.HideAndDontSave};
        this._bb_transform = this._empty_go.transform;
      }

      if (this.OnAwakeSetup) {
        this.Setup();
      }
    }

    /// <summary>
    /// </summary>
    void Setup() {
      if (!this.RunInEditModeSetup && !Application.isPlaying) {
        return;
      }

      if (!this._bb_transform) {
        this._empty_go = new GameObject {hideFlags = HideFlags.HideAndDontSave};
        this._bb_transform = this._empty_go.transform;
      }

      if (this.includeSelf) {
        this._local_collider = this.GetComponent<BoxCollider>();
        this._local_mesh = this.GetComponent<MeshFilter>();
      }

      if (this.includeChildren) {
        this._children_meshes = this.GetComponentsInChildren<MeshFilter>();
        this._children_colliders = this.GetComponentsInChildren<Collider>();
      }

      this.CalculateBoundingBox();
    }

    /// <summary>
    /// </summary>
    void LateUpdate() {
      if (this.freezeAfterFirstCalculation) {
        return;
      }

      if (this.includeChildren && !this.cacheChildren) {
        if (this._children_meshes != this.GetComponentsInChildren<MeshFilter>()) {
          this.BoundingBoxReset();
        }

        if (this._children_colliders != this.GetComponentsInChildren<Collider>()) {
          this.BoundingBoxReset();
        }
      } else {
        this.CalculateBoundingBox();
      }
    }

    /// <summary>
    /// </summary>
    void FitCollidersAabb() {
      var transform1 = this.transform;
      this._bb_transform.rotation = transform1.rotation;
      this._bb_transform.position = transform1.position;

      var bounds = new Bounds(this._bb_transform.position, Vector3.zero);

      if (this.includeSelf && this._local_collider) {
        this._bb_transform.position = this._local_collider.bounds.center;
        bounds = this._local_collider.bounds;
      }

      if (this.includeChildren && this._children_colliders != null) {
        foreach (var a_collider in this._children_colliders) {
          if (a_collider && a_collider != this._local_collider) {
            if (this._only_active_children) {
              if (a_collider.gameObject.activeInHierarchy
                  && a_collider.gameObject.activeSelf
                  && a_collider.enabled) {
                if (bounds.size == Vector3.zero) {
                  this._bb_transform.rotation = a_collider.transform.rotation;
                  this._bb_transform.position = a_collider.bounds.center;
                  bounds = a_collider.bounds;
                } else {
                  bounds.Encapsulate(a_collider.bounds);
                }
              }
            } else {
              if (bounds.size == Vector3.zero) {
                this._bb_transform.rotation = a_collider.transform.rotation;
                this._bb_transform.position = a_collider.bounds.center;
                bounds = a_collider.bounds;
              } else {
                bounds.Encapsulate(a_collider.bounds);
              }
            }
          }
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = this._Bounds.center - this._bb_transform.position;
    }

    /// <summary>
    /// </summary>
    void FitRenderersAabb() {
      var transform1 = this.transform;
      var position = transform1.position;
      this._bb_transform.position = position;
      this._bb_transform.rotation = transform1.rotation;

      var bounds = new Bounds(position, Vector3.zero);

      if (this.includeSelf && this._local_mesh) {
        Mesh a_mesh;

        if (this._use_shared_mesh) {
          a_mesh = this._local_mesh.sharedMesh;
        } else {
          a_mesh = this._local_mesh.mesh;
        }

        if (a_mesh.isReadable) {
          var vc = a_mesh.vertexCount;
          for (var i = 0; i < vc; i++) {
            bounds.Encapsulate(this._local_mesh.transform.TransformPoint(a_mesh.vertices[i]));
          }
        } else {
          Debug.LogWarning("Make sure mesh is marked as readable when imported!");
        }
      }

      if (this.includeChildren && this._children_meshes != null) {
        foreach (var t in this._children_meshes) {
          if (t) {
            if (this._only_active_children) {
              if (t.gameObject.activeInHierarchy && t.gameObject.activeSelf) {
                if (bounds.size == Vector3.zero) {
                  var transform2 = t.transform;
                  position = transform2.position;
                  this._bb_transform.position = position;
                  this._bb_transform.rotation = transform2.rotation;
                  bounds = new Bounds(position, Vector3.zero);
                }

                Mesh a_mesh;

                if (this._use_shared_mesh) {
                  a_mesh = t.sharedMesh;
                } else {
                  a_mesh = t.mesh;
                }

                if (a_mesh) {
                  if (a_mesh.isReadable) {
                    var vc = a_mesh.vertexCount;
                    for (var j = 0; j < vc; j++) {
                      bounds.Encapsulate(t.transform.TransformPoint(a_mesh.vertices[j]));
                    }
                  } else {
                    Debug.LogWarning("Make sure mesh is marked as readable when imported!");
                  }
                }
              }
            } else {
              if (bounds.size == Vector3.zero) {
                bounds = new Bounds(t.transform.position, Vector3.zero);
              }

              Mesh a_mesh;

              if (this._use_shared_mesh) {
                a_mesh = t.sharedMesh;
              } else {
                a_mesh = t.mesh;
              }

              if (a_mesh) {
                var vc = a_mesh.vertexCount;
                for (var j = 0; j < vc; j++) {
                  bounds.Encapsulate(t.transform.TransformPoint(a_mesh.vertices[j]));
                }
              }
            }
          }
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = this._Bounds.center - position;
    }

    /// <summary>
    /// </summary>
    void CalculateBoundingBox() {
      if (!this.RunInEditModeSetup && !Application.isPlaying || this._bb_transform == null) {
        return;
      }

      if (this.basedOn == BasedOn.Collider_) {
        switch (this.BbAligning) {
          case BoundingBoxOrientation.Axis_aligned_:
            this.FitCollidersAabb();
            this.RecalculatePoints();
            this.RecalculateLines();
            break;
          case BoundingBoxOrientation.Object_oriented_:
            this.FitCollidersOobb();
            break;
          case BoundingBoxOrientation.Camera_oriented_:
            this.FitRenderersCabb();
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      } else {
        switch (this.BbAligning) {
          case BoundingBoxOrientation.Axis_aligned_:
            this.FitRenderersAabb();
            this.RecalculatePoints();
            this.RecalculateLines();
            break;
          case BoundingBoxOrientation.Object_oriented_:
            this.FitRenderersOobb();
            break;
          case BoundingBoxOrientation.Camera_oriented_:
            this.FitRenderersCabb();

            break;
          default: throw new ArgumentOutOfRangeException();
        }
      }
    }

    void FitRenderersCabb() {
      throw new NotImplementedException();

      /*
      var transform1 = this.transform;
      var position = transform1.position;
      this._bb_transform.position = position;
      this._bb_transform.rotation = transform1.rotation;

      var a = this._local_mesh.sharedMesh.GetCameraMinMaxPoints(this._bb_transform,
                                                                this._camera,
                                                                this.use_view_port);
      var min = a[0];
      var max = a[1];
      var extent = a[2];

      if (this.use_view_port) {
        min = this._camera.ViewportToWorldPoint(min);
        max = this._camera.ViewportToWorldPoint(max);
      } else {
        min = this._camera.ScreenToWorldPoint(min);
        max = this._camera.ScreenToWorldPoint(max);
        extent = max - min;
      }

      var cobb_extent = extent;
      var cobb_center =
          new Vector3(min.x + extent.x / 2.0f, min.y + extent.y / 2.0f, min.z + extent.z / 2.0f);

      this._point_tfr = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Top_Front_Right);
      this._point_tfl = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Top_Front_Left);
      this._point_tbl = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Top_Back_Left);
      this._point_tbr = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Top_Back_Right);
      this._point_bfr = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Bottom_Front_Right);
      this._point_bfl = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Bottom_Front_Left);
      this._point_bbl = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Bottom_Back_Left);
      this._point_bbr = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Bottom_Back_Right);

      this._Bounds.center = cobb_center;
      this._Bounds.extents = cobb_extent;

      this._points = new[] {
                               this._point_tfr,
                               this._point_tfl,
                               this._point_tbl,
                               this._point_tbr,
                               this._point_bfr,
                               this._point_bfl,
                               this._point_bbl,
                               this._point_bbr
                           };

      var rot = Quaternion.identity;
      var pos = Vector3.zero;

      //rot = transform1.rotation;
      //pos = transform1.position;

      this._lines_list.Clear();

      for (var i = 0; i < 4; i++) {
        //width
        var line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 1] + pos};
        this._lines_list.Add(line);

        //height
        line = new[] {rot * this.Points[i] + pos, rot * this.Points[i + 4] + pos};
        this._lines_list.Add(line);

        //depth
        line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 3 - 4 * (i % 2)] + pos};
        this._lines_list.Add(line);
      }

      this._lines = new Vector3[BoundingBoxUtilities._Num_Lines, BoundingBoxUtilities._Num_Points_Per_Line];
      for (var j = 0; j < BoundingBoxUtilities._Num_Lines; j++) {
        this.Lines[j, 0] = this._lines_list[j][0];
        this.Lines[j, 1] = this._lines_list[j][1];
      }
      */
    }

    void FitRenderersOobb() { throw new NotImplementedException(); }

    void FitCollidersOobb() { throw new NotImplementedException(); }

    /// <summary>
    /// </summary>
    void RecalculatePoints() {
      this._point_tfr = this._Bounds_Offset
                        + Vector3.Scale(this._Bounds.extents, BoundingBoxUtilities._Top_Front_Right);
      this._point_tfl = this._Bounds_Offset
                        + Vector3.Scale(this._Bounds.extents, BoundingBoxUtilities._Top_Front_Left);
      this._point_tbl = this._Bounds_Offset
                        + Vector3.Scale(this._Bounds.extents, BoundingBoxUtilities._Top_Back_Left);
      this._point_tbr = this._Bounds_Offset
                        + Vector3.Scale(this._Bounds.extents, BoundingBoxUtilities._Top_Back_Right);
      this._point_bfr = this._Bounds_Offset
                        + Vector3.Scale(this._Bounds.extents, BoundingBoxUtilities._Bottom_Front_Right);
      this._point_bfl = this._Bounds_Offset
                        + Vector3.Scale(this._Bounds.extents, BoundingBoxUtilities._Bottom_Front_Left);
      this._point_bbl = this._Bounds_Offset
                        + Vector3.Scale(this._Bounds.extents, BoundingBoxUtilities._Bottom_Back_Left);
      this._point_bbr = this._Bounds_Offset
                        + Vector3.Scale(this._Bounds.extents, BoundingBoxUtilities._Bottom_Back_Right);

      this._points = new[] {
                               this._point_tfr,
                               this._point_tfl,
                               this._point_tbl,
                               this._point_tbr,
                               this._point_bfr,
                               this._point_bfl,
                               this._point_bbl,
                               this._point_bbr
                           };
    }

    /// <summary>
    /// </summary>
    void RecalculateLines() {
      var transform1 = this.transform;
      if (this._bb_transform) {
        transform1 = this._bb_transform;
      }

      var rot = Quaternion.identity;
      var pos = Vector3.zero;
      if (this._use_bb_transform) {
        rot = transform1.rotation;
      }

      pos = transform1.position;

      this._lines_list.Clear();

      for (var i = 0; i < 4; i++) {
        //width
        var line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 1] + pos};
        this._lines_list.Add(line);

        //height
        line = new[] {rot * this.Points[i] + pos, rot * this.Points[i + 4] + pos};
        this._lines_list.Add(line);

        //depth
        line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 3 - 4 * (i % 2)] + pos};
        this._lines_list.Add(line);
      }

      this._lines = new Vector3[BoundingBoxUtilities._Num_Lines, BoundingBoxUtilities._Num_Points_Per_Line];
      for (var j = 0; j < BoundingBoxUtilities._Num_Lines; j++) {
        this.Lines[j, 0] = this._lines_list[j][0];
        this.Lines[j, 1] = this._lines_list[j][1];
      }
    }

    /// <summary>
    /// </summary>
    void OnMouseDown() {
      //if (_permanent)
      //  return;
      //this.enabled = !this.enabled;
    }

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    void OnValidate() {
      if (!this.enabled) {
        return;
      }

      if (EditorApplication.isPlaying) {
        return;
      }

      this.CalculateBoundingBox();
    }

    /// <summary>
    /// </summary>
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        Gizmos.color = this.editorPreviewLineColor;

        if (this.Lines != null) {
          for (var i = 0; i < this.Lines.GetLength(0); i++) {
            Gizmos.DrawLine(this.Lines[i, 0], this.Lines[i, 1]);
          }
        } else {
          Gizmos.DrawWireCube(this.Bounds.center, this.Bounds.size);
        }

        if (this._bb_transform) {
          Handles.Label(this._bb_transform.position, this.name);
        } else {
          Handles.Label(this.transform.position, this.name);
        }
      }
    }
    #endif
  }
}
