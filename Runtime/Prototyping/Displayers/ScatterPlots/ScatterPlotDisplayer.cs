using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Structs;
using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Prototyping.Displayers.ScatterPlots {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu(DisplayerComponentMenuPath._ComponentMenuPath
                    + "ScatterPlot"
                    + DisplayerComponentMenuPath._Postfix)]
  [RequireComponent(typeof(ParticleSystem))]
  public class ScatterPlotDisplayer : Displayer {
    [SerializeField] Gradient _gradient;
    ParticleSystem _particle_system;

    ParticleSystem.MainModule _particle_system_main_module;
    ParticleSystemRenderer _particle_system_renderer;

    [SerializeField]
    ParticleSystemSimulationSpace _particle_system_simulation_space = ParticleSystemSimulationSpace.World;

    ParticleSystem.Particle[] _particles;
    [SerializeField] float _default_start_size = 0.6f;

    List<float> _vs = new List<float>();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
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
      this._particle_system_main_module.startSize = this._default_start_size;

      this._particle_system_renderer = this.GetComponent<ParticleSystemRenderer>();
      //this._particle_system_renderer.renderMode = ParticleSystemRenderMode.Mesh;
      this._particle_system_renderer.alignment = ParticleSystemRenderSpace.World;

      if (this._gradient == null) {
        this._gradient = new Gradient {
                                          colorKeys = new[] {
                                                                new GradientColorKey(new Color(1, 0, 0), 0f),
                                                                new GradientColorKey(new Color(0, 1, 0), 1f)
                                                            }
                                      };
      }
    }

    public override void Display(Double value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying the double " + value + " To " + this.name);
      }
      #endif

      this._Values = new[] {(float)value};
      this.PlotSeries(this._Values);
    }

    public override void Display(float[] values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        var s = "";
        foreach (var value in values) {
          s += $"{value},";
        }

        Debug.Log("Applying the float array " + s + " To " + this.name);
      }
      #endif
      this._Values = values;
      this.PlotSeries(values);
    }

    public override void Display(String values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying the float array " + values + " To " + this.name);
      }
      #endif

      this._vs.Clear();
      foreach (var value in values.Split(',')) {
        this._vs.Add(float.Parse(value));
      }

      this._Values = this._vs.ToArray();
      this.PlotSeries(this._Values);
    }

    public override void Display(Vector3 value) { throw new NotImplementedException(); }
    public override void Display(Vector3[] value) { this.ScatterPlot(value); }

    public override void Display(Points.ValuePoint points) { this.PlotSeries(new[] {points}); }

    public override void Display(Points.ValuePoint[] points) {
      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        var points_str = points.Aggregate("",
                                          (current, point) =>
                                              current
                                              + $"({point._Pos.ToString()}, {point._Val},{point._Size})"
                                              + ", ");
        Debug.Log("Applying the points " + points_str + " to " + this.name);
      }
      #endif

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        var clamped = Math.Min(Math.Max(0.0f, point._Val), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(clamped);
        this._particles[i].startSize = point._Size;
        this._particles[i].startSize3D = this._default_start_size.BroadcastVector3();
        i++;
      }

      this._particle_system.SetParticles(this._particles, points.Length);
    }

    public override void Display(Points.StringPoint point) { throw new NotImplementedException(); }
    public override void Display(Points.StringPoint[] points) { throw new NotImplementedException(); }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    public override void Display(float values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying the float " + values + " To " + this.name);
      }
      #endif

      this._Values = new[] {values};
      this.PlotSeries(this._Values);
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public void ScatterPlot(Vector3[] points) {
      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        var points_str = points.Aggregate("", (current, point) => current + point.ToString() + ", ");
        Debug.Log("Applying the points " + points_str + " To " + this.name);
      }
      #endif

      var i = 0;
      var l = (float)points.Length;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point;
        var clamped = Math.Min(Math.Max(0.0f, i / l), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(clamped);
        this._particles[i].startSize = this._default_start_size;
        this._particles[i].startSize3D = this._default_start_size.BroadcastVector3();
        i++;
      }

      this._particle_system.SetParticles(this._particles, points.Length);
    }

    public void PlotSeries(float[] points) {
      if (!this._particle_system) {
        return;
      }

      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying the series " + points + " To " + this.name);
      }
      #endif

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = Vector3.one * i;
        var clamped = Math.Min(Math.Max(0.0f, point), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(clamped);
        this._particles[i].startSize = this._default_start_size;
        this._particles[i].startSize3D = this._default_start_size.BroadcastVector3();
        i++;
      }

      this._particle_system.SetParticles(this._particles, points.Length);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void Clean() {
      if (!this._RetainLastPlot) {
        if (this._particle_system) {
          this._particle_system.Clear(true);
        }
      }

      base.Clean();
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public override void PlotSeries(Points.ValuePoint[] points) {
      if (!this._particle_system) {
        return;
      }

      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new ParticleSystem.Particle[points.Length];
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying the series " + points + " To " + this.name);
      }
      #endif

      var alive = this._particle_system.GetParticles(this._particles);
      if (alive < points.Length) {
        this._particles = new ParticleSystem.Particle[points.Length];
      }

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        this._particles[i].startColor = this._gradient.Evaluate(point._Val);
        this._particles[i].startSize = point._Size;
        this._particles[i].startSize3D = point._Size.BroadcastVector3();
        i++;
      }

      this._particle_system.SetParticles(this._particles, points.Length);
    }
  }
}
