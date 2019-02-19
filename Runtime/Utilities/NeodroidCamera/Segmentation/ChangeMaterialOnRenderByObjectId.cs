using System;
using System.Collections.Generic;
using droid.Runtime.Utilities.NeodroidCamera.Synthesis;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace droid.Runtime.Utilities.NeodroidCamera.Segmentation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByObjectId : Segmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block;

    [SerializeField] ColorByInstance[] instanceColorArray;
    [SerializeField] Shader segmentation_shader;
    [SerializeField] Camera _camera;

    /// <summary>
    /// </summary>
    public Dictionary<GameObject, Color> ColorsDictGameObject { get; } = new Dictionary<GameObject, Color>();

    /// <summary>
    /// </summary>
    public override Dictionary<String, Color> ColorsDict {
      get {
        var colors = new Dictionary<String, Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key_val.Key.GetInstanceID().ToString(), key_val.Value);
        }

        return colors;
      }
    }

    /// <summary>
    /// </summary>
    public ColorByInstance[] InstanceColors {
      get {

        if (this.ColorsDictGameObject != null) {
          this.instanceColorArray = new ColorByInstance[this.ColorsDictGameObject.Keys.Count];
          var i = 0;
          foreach (var key in this.ColorsDictGameObject.Keys) {
            var seg = new ColorByInstance {_Obj = key, _Col = this.ColorsDictGameObject[key]};
            this.instanceColorArray[i] = seg;
            i++;
          }

          return this.instanceColorArray;
        }

        return null;
      }
      set {
        foreach (var seg in value) {
          this.ColorsDictGameObject[seg._Obj] = seg._Col;
        }
      }
    }

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() { this.Setup(); }

    /// <summary>
    /// </summary>
    void Awake() {
      this._all_renders = FindObjectsOfType<Renderer>();
      this._block = new MaterialPropertyBlock();
      this.Setup();
    }

    /// <summary>
    /// </summary>
    void Update() {

    }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }
    }

    /// <summary>
    /// </summary>
    void Setup() {
      this._camera = this.GetComponent<Camera>();
      SynthesisUtils.Setup(this._camera);
      SynthesisUtils.SetupOnCameraChangeObjectId(this._camera, this.segmentation_shader);
      this._all_renders = FindObjectsOfType<Renderer>();
      this.CheckBlock();
      foreach (var r in this._all_renders) {
        GameObject game_object;
        GameObject o;
        var id = (game_object = (o = r.gameObject)).GetInstanceID();
        var layer = game_object.layer;
        var go_tag = game_object.tag;

        this.ColorsDictGameObject.Add(o, ColorEncoding.EncodeIdAsColor(id));
        this._block.SetColor("_ObjectColor", ColorEncoding.EncodeIdAsColor(id));
        this._block.SetColor("_CategoryColor", ColorEncoding.EncodeLayerAsColor(layer));
        //this._block.SetColor("_CategoryColor", ColorEncoding.EncodeTagHashCodeAsColor(go_tag));
        r.SetPropertyBlock(this._block);
      }
    }
  }
}
