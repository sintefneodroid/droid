using System.Collections.Generic;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Misc.SearchableEnum;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR

#endif

namespace droid.Runtime.Utilities.BoundingBoxes
{
  public enum BasedOn
  {
    Geometry_,
    Collider_
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class BoundingBox : MonoBehaviour
  {


    /// <summary>
    /// </summary>
    protected Bounds _Bounds;

    /// <summary>
    /// </summary>
    protected Vector3 _Bounds_Offset;

    /// <summary>
    /// </summary>
    Collider[] _childrenColliders;

    /// <summary>
    /// </summary>
    MeshFilter[] _childrenMeshes;

    /// <summary>
    /// </summary>
    [SearchableEnum] public BasedOn basedOn = BasedOn.Geometry_;

    /// <summary>
    /// </summary>
    Vector3[] _points;

    /// <summary>
    /// </summary>
    [SerializeField] bool freezeAfterFirstCalculation = true;

    /// <summary>
    /// </summary>
    [SerializeField] bool includeChildren = false;

    Vector3 _lastPosition;
    Quaternion _lastRotation;

    /// <summary>
    /// </summary>
    Vector3 _lastScale;

    /// <summary>
    /// </summary>
    public Color editorPreviewLineColor = new Color(1f, 0.36f, 0.38f, 0.74f);

    /// <summary>
    /// </summary>
    Vector3[,] _lines;

    List<Vector3[]> _linesList = new List<Vector3[]>();

    /// <summary>
    /// </summary>
    Quaternion _rotation;

    /// <summary>
    /// </summary>
    [SerializeField] bool setupOnAwake;

    [SerializeField] IPrototypingEnvironment environment;


    Vector3 _bottomBackLeftExtend;
    Vector3 _bottomBackRightExtend;
    Vector3 _bottomFrontLeftExtend;
    Vector3 _bottomFrontRightExtend;
    Vector3 _topBackLeftExtend;
    Vector3 _topBackRightExtend;
    Vector3 _topFrontLeftExtend;
    Vector3 _topFrontRightExtend;

    [SerializeField] bool cacheChildren = true;

    public Vector3[] BoundingBoxCoordinates =>
      new[]
      {
        this._topFrontLeftExtend,
        this._topFrontRightExtend,
        this._topBackLeftExtend,
        this._topBackRightExtend,
        this._bottomFrontLeftExtend,
        this._bottomFrontRightExtend,
        this._bottomBackLeftExtend,
        this._bottomBackRightExtend
      };


    public Bounds Bounds => this._Bounds;

    public Vector3 Max => this._Bounds.max;

    public Vector3 Min => this._Bounds.min;

    /// <summary>
    /// </summary>
    public string BoundingBoxCoordinatesAsString
    {
      get
      {
        var str_rep = "";
        str_rep += "\"_top_front_left\":" + this.BoundingBoxCoordinates[0] + ", ";
        str_rep += "\"_top_front_right\":" + this.BoundingBoxCoordinates[1] + ", ";
        str_rep += "\"_top_back_left\":" + this.BoundingBoxCoordinates[2] + ", ";
        str_rep += "\"_top_back_right\":" + this.BoundingBoxCoordinates[3] + ", ";
        str_rep += "\"_bottom_front_left\":" + this.BoundingBoxCoordinates[4] + ", ";
        str_rep += "\"_bottom_front_right\":" + this.BoundingBoxCoordinates[5] + ", ";
        str_rep += "\"_bottom_back_left\":" + this.BoundingBoxCoordinates[6] + ", ";
        str_rep += "\"_bottom_back_right\":" + this.BoundingBoxCoordinates[7];
        return str_rep;
      }
    }

    /// <summary>
    /// </summary>
    public string BoundingBoxCoordinatesWorldSpaceAsJson
    {
      get
      {
        var str_rep = "{";
        var transform1 = this.transform;
        var rotation = transform1.rotation;
        var position = transform1.position;
        if (this.environment != null)
        {
          str_rep +=
            $"\"top_front_left\":{this.JsonifyVec3(this.environment.TransformPoint(rotation * this._topFrontLeftExtend + position))}, ";
          str_rep +=
            $"\"bottom_back_right\":{this.JsonifyVec3(this.environment.TransformPoint(rotation * this._bottomBackRightExtend + position))}";
        }

        str_rep += "}";
        return str_rep;
      }
    }

    /// <summary>
    /// </summary>
    public Vector3[,] Lines
    {
      get => this._lines;
      set => this._lines = value;
    }

    /// <summary>
    /// </summary>
    public Vector3[] Points
    {
      get => this._points;
      set => this._points = value;
    }

    /// <summary>
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    string JsonifyVec3(Vector3 vec)
    {
      return $"[{vec.x},{vec.y},{vec.z}]";
    }

    /// <summary>
    /// </summary>
    void Reset()
    {
      this.Awake();
      this.Start();
    }

    /// <summary>
    /// </summary>
    void Start()
    {
      if (!this.setupOnAwake)
      {
        this.Setup();
      }
    }

    /// <summary>
    /// </summary>
    void Awake()

    {
      if (this.environment==null)
      {
        this.environment = FindObjectOfType<PrototypingEnvironment>();
      }

      if (this.setupOnAwake)
      {
        this.Setup();
      }
    }

    /// <summary>
    /// </summary>
    void Setup()
    {
      var transform1 = this.transform;
      this._lastPosition = transform1.position;
      this._lastRotation = transform1.rotation;
      this._lastScale = transform1.localScale;


      if (this.includeChildren)
      {
        this._childrenMeshes = this.GetComponentsInChildren<MeshFilter>();
        this._childrenColliders = this.GetComponentsInChildren<Collider>();
      }

      this.CalculateBounds();
      this.Initialise();
    }

    /// <summary>
    /// </summary>
    public void Initialise()
    {
      this.RecalculatePoints();
      this.RecalculateLines();
    }

    /// <summary>
    /// </summary>
    void LateUpdate()
    {
      if (this.freezeAfterFirstCalculation)
      {
        return;
      }

      if (this.includeChildren && !this.cacheChildren)
      {
        if (this._childrenMeshes != this.GetComponentsInChildren<MeshFilter>())
        {
          this.Reset();
        }

        if (this._childrenColliders != this.GetComponentsInChildren<Collider>())
        {
          this.Reset();
        }
      }

      if (this.transform.localScale != this._lastScale)
      {
        this.ScaleBounds();
        this.RecalculatePoints();
      }

      if (this.transform.position != this._lastPosition
          || this.transform.rotation != this._lastRotation
          || this.transform.localScale != this._lastScale)
      {
        this.RecalculateLines();
        var transform1 = this.transform;
        this._lastRotation = transform1.rotation;
        this._lastPosition = transform1.position;
        this._lastScale = transform1.localScale;
      }
    }

    /// <summary>
    /// </summary>
    public void ScaleBounds()
    {
      //this._Bounds.size = new Vector3(startingBoundSize.x * transform.localScale.x / startingScale.x, startingBoundSize.y * transform.localScale.y / startingScale.y, startingBoundSize.z * transform.localScale.z / startingScale.z);
      //this._Bounds.center = transform.TransformPoint(startingBoundCenterLocal);
    }

    /// <summary>
    /// </summary>
    void FitBoundingBoxToChildrenColliders()
    {
      var col = this.GetComponent<BoxCollider>();
      var bounds = new Bounds(this.transform.position, Vector3.zero); // position and size

      if (col)
      {
        bounds.Encapsulate(col.bounds);
      }

      if (this.includeChildren)
      {
        foreach (var child_col in this._childrenColliders)
        {
          if (child_col != col)
          {
            bounds.Encapsulate(child_col.bounds);
          }
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = bounds.center - this.transform.position;
    }

    /// <summary>
    /// </summary>
    void FitBoundingBoxToChildrenRenders()
    {
      var bounds = new Bounds(this.transform.position, Vector3.zero);

      var mesh = this.GetComponent<MeshFilter>();
      if (mesh)
      {
        var ms = mesh.sharedMesh;
        var vc = ms.vertexCount;
        for (var i = 0; i < vc; i++)
        {
          bounds.Encapsulate(mesh.transform.TransformPoint(ms.vertices[i]));
        }
      }

      foreach (var t in this._childrenMeshes)
      {
        if (t)
        {
          var ms = t.sharedMesh;
          if (ms)
          {
            var vc = ms.vertexCount;
            for (var j = 0; j < vc; j++)
            {
              bounds.Encapsulate(t.transform.TransformPoint(ms.vertices[j]));
            }
          }
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = this._Bounds.center - this.transform.position;
    }

    /// <summary>
    /// </summary>
    void CalculateBounds()
    {
      this._rotation = this.transform.rotation;
      this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

      if (this.basedOn == BasedOn.Collider_)
      {
        this.FitBoundingBoxToChildrenColliders();
      }
      else
      {
        this.FitBoundingBoxToChildrenRenders();
      }

      this.transform.rotation = this._rotation;
    }

    /// <summary>
    /// </summary>
    void RecalculatePoints()
    {
      var localScale = this.transform.localScale;
      this._Bounds.size = new Vector3(
        this._Bounds.size.x * localScale.x / this._lastScale.x,
        this._Bounds.size.y * localScale.y / this._lastScale.y,
        this._Bounds.size.z * localScale.z / this._lastScale.z);
      this._Bounds_Offset = new Vector3(
        this._Bounds_Offset.x * localScale.x / this._lastScale.x,
        this._Bounds_Offset.y * localScale.y / this._lastScale.y,
        this._Bounds_Offset.z * localScale.z / this._lastScale.z);

      this._topFrontRightExtend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(1, 1, 1));
      this._topFrontLeftExtend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(-1, 1, 1));
      this._topBackLeftExtend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(-1, 1, -1));
      this._topBackRightExtend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(1, 1, -1));
      this._bottomFrontRightExtend =
        this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(1, -1, 1));
      this._bottomFrontLeftExtend =
        this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(-1, -1, 1));
      this._bottomBackLeftExtend =
        this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(-1, -1, -1));
      this._bottomBackRightExtend =
        this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, new Vector3(1, -1, -1));

      this.Points = new[]
      {
        this._topFrontRightExtend,
        this._topFrontLeftExtend,
        this._topBackLeftExtend,
        this._topBackRightExtend,
        this._bottomFrontRightExtend,
        this._bottomFrontLeftExtend,
        this._bottomBackLeftExtend,
        this._bottomBackRightExtend
      };
    }

    /// <summary>
    /// </summary>
    void RecalculateLines()
    {
      var transform1 = this.transform;
      var rot = transform1.rotation;
      var pos = transform1.position;

      this._linesList.Clear();
      //int linesCount = 12;

      for (var i = 0; i < 4; i++){
        //width
        var line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 1] + pos};
        this._linesList.Add(line);

        //height
        line = new[] {rot * this.Points[i] + pos, rot * this.Points[i + 4] + pos};
        this._linesList.Add(line);

        //depth
        line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 3 - 4 * (i % 2)] + pos};
        this._linesList.Add(line);
      }

      this.Lines = new Vector3[this._linesList.Count, 2];
      for (var j = 0; j < this._linesList.Count; j++)
      {
        this.Lines[j, 0] = this._linesList[j][0];
        this.Lines[j, 1] = this._linesList[j][1];
      }
    }

    /// <summary>
    /// </summary>
    void OnMouseDown()
    {
      //if (_permanent)
      //  return;
      this.enabled = !this.enabled;
    }

#if UNITY_EDITOR
    /// <summary>
    /// </summary>
    void OnValidate()
    {
      if (EditorApplication.isPlaying)
      {
        return;
      }

      this.Initialise();
    }

    /// <summary>
    /// </summary>
    void OnDrawGizmos()
    {
      if (this.enabled)
      {
        if (this.enabled)
        {
          Gizmos.color = this.editorPreviewLineColor;
          if (this.Lines != null)
          {
            for (var i = 0; i < this.Lines.GetLength(0); i++)
            {
              Gizmos.DrawLine(this.Lines[i, 0], this.Lines[i, 1]);
            }
          }

          var transform1 = this.transform;
          Handles.Label(transform1.position, transform1.gameObject.name);
        }
      }
    }
#endif
  }
}