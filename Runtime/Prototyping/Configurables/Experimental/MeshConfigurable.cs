using System;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Debugging;
using Neodroid.Runtime.Utilities.Misc;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Mesh" + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(MeshFilter))]
  public class MeshConfigurable : Configurable {
    string _mesh_str;

    Mesh _deforming_mesh;
    Vector3[] _original_vertices, _displaced_vertices;
    Perlin _noise;
    float _scale = 1.0f;
    float _speed = 1.0f;

    [SerializeField] Mesh[ ] _meshes;
    [SerializeField] MeshFilter _mesh_filter;
    [SerializeField] bool _displace_mesh;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._mesh_str = this.Identifier + "Mesh";
      this._mesh_filter = this.GetComponent<MeshFilter>();
      if (Application.isPlaying){
        this._deforming_mesh = _mesh_filter.mesh;
        this._original_vertices = this._deforming_mesh.vertices;
        this._displaced_vertices = new Vector3[this._original_vertices.Length];
        for (var i = 0; i < this._original_vertices.Length; i++)
        {
          this._displaced_vertices[i] = this._original_vertices[i];
        }
      }

      this._noise = new Perlin ();
    }


    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._mesh_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>n
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(this, this._mesh_str);
    }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      #endif

      if (configuration.ConfigurableName == this._mesh_str) {
        if (this._displace_mesh){
          if (this._deforming_mesh){
            var time_x = Time.time * this._speed + 0.1365143f;
            var time_y = Time.time * this._speed + 1.21688f;
            var time_z = Time.time * this._speed + 2.5564f;

            for (var i = 0; i < this._displaced_vertices.Length; i++){
              var orig = this._original_vertices[i];
              //orig.y = orig.y * (1+(float)Math.Cos(Time.deltaTime))*(configuration.ConfigurableValue);
              //orig.x = orig.x * (1+(float)Math.Sin(Time.deltaTime))*(configuration.ConfigurableValue);

              orig.x += this._noise.Noise(time_x + orig.x, time_x + orig.y, time_x + orig.z) * this._scale;
              orig.y += this._noise.Noise(time_y + orig.x, time_y + orig.y, time_y + orig.z) * this._scale;
              orig.z += this._noise.Noise(time_z + orig.x, time_z + orig.y, time_z + orig.z) * this._scale;

              this._displaced_vertices[i] = orig;
            }

            this._deforming_mesh.vertices = this._displaced_vertices;

            this._deforming_mesh.RecalculateNormals();
          }
        }else if (this._meshes.Length > 0) {
          var idx = (int)(configuration.ConfigurableValue * this._meshes.Length);
          this._mesh_filter.mesh = this._meshes[idx];
        }

      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      return new Configuration(this._mesh_str, (float)random_generator.NextDouble());
    }


  }
}
