#if ECS_EXISTS
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Displayers.ECS {
  [ExecuteInEditMode]
  [AddComponentMenu("Neodroid/Displayers/ECSScatterPlot")]
  [RequireComponent(typeof(ParticleSystem))]
  public class EcsScatterPlotDisplayer : EcsDisplayer {
    ParticleSystem _particle_system;
    ParticleSystemRenderer _particle_system_renderer;
    [SerializeField]ParticleSystemSimulationSpace _particle_system_simulation_space =
 ParticleSystemSimulationSpace.World;
    ParticleSystem.MainModule _particle_system_main_module;
    ParticleSystem.Particle[] _particles;
    [SerializeField] float[] _values;
    [SerializeField] Gradient _gradient;
    [SerializeField] float _size = 0.6f;
    [SerializeField] bool _plot_random_series = false;

    protected override void Setup() {
      this._particle_system = this.GetComponent<ParticleSystem>();
      var em = this._particle_system.emission;
      em.enabled = false;
      em.rateOverTime = 0;
      var sh = this._particle_system.shape;
      sh.enabled = false;

      this._particle_system_main_module = this._particle_system.main;
      this._particle_system_main_module.loop = false;
      this._particle_system_main_module.playOnAwake = false;
      this._particle_system_main_module.simulationSpace = this._particle_system_simulation_space;
      this._particle_system_main_module.simulationSpeed = 0;
      this._particle_system_main_module.startSize = this._size;

      this._particle_system_renderer = this.GetComponent<ParticleSystemRenderer>();
      //this._particle_system_renderer.renderMode = ParticleSystemRenderMode.Mesh;
      this._particle_system_renderer.alignment = ParticleSystemRenderSpace.World;



      this._gradient = new Gradient {
          colorKeys = new[] {
              new GradientColorKey(new Color(1, 0, 0), 0f),
              new GradientColorKey(new Color(0, 1, 0), 1f)
          }
      };
    }

    protected override void Awake() {
      base.Awake();
      this.Setup();
    }

    public struct ValuePoint {
      public  Vector3 _Pos;
      public  float _Size;
      public  float _Val;

      public ValuePoint(Vector3 pos, float val, float size) {
        this._Pos = pos;
        this._Val = val;
        this._Size = size;
      }
    }

    public override void Display(Double value) {
        #if NEODROID_DEBUG
      if (this.Debugging)
        print("Applying the double " + value + " To " + this.name);

  #endif
      this._values = new[] {(float)value};
      this.ScatterPlot(this._values);
    }

    public override void Display(float[] values) {
      #if NEODROID_DEBUG
 if (this.Debugging) {
        var s = "";
        foreach (var value in values) {
          s += $"{value},";
        }


        print("Applying the float array " + s + " To " + this.name);
      }
  #endif

      this._values = values;
      this.ScatterPlot(values);
    }

    public override void Display(String values) {
      #if NEODROID_DEBUG
 if (this.Debugging) {
        Debug.Log("Applying the float array " + values + " To " + this.name);
      }
  #endif

      var vs = new List<float>();
      foreach (var value in values.Split(',')) {
          vs.Add(float.Parse(value));
      }

      this._values = vs.ToArray();
      this.ScatterPlot(this._values);
    }

    public override void Display(float values) {
          #if NEODROID_DEBUG
      if (this.Debugging)
        print("Applying the float " + values + " To " + this.name);
#endif
      this._values = new[] {values};
      this.ScatterPlot(this._values);
    }

    public void ScatterPlot(float[] points) {
      if(this._particles == null || this._particles.Length != points.Length)
        this._particles = new ParticleSystem.Particle[points.Length];
      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = Vector3.one * i;
        var clamped = Math.Min(Math.Max(0.0f, point), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(clamped);
        this._particles[i].startSize = 1f;
        i++;
      }

      this._particle_system.SetParticles(this._particles, points.Length);
    }

    #if UNITY_EDITOR
    void OnDrawGizmos() { if (this.enabled) {
        if (this.enabled) {
      if(this._plot_random_series){
        this.ScatterPlot(PlotFunctions.SampleRandomSeries(1));
      }
  }
    }
    #endif

    public void ScatterPlot(ValuePoint[] points) {
      var alive = this._particle_system.GetParticles(this._particles);
      if(alive < points.Length)
        this._particles = new ParticleSystem.Particle[points.Length];
      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        this._particles[i].startColor = this._gradient.Evaluate(point._Val);
        this._particles[i].startSize = point._Size;
        i++;
      }

      this._particle_system.SetParticles(this._particles, points.Length);
    }
  }
}
#endif
