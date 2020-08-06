using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using droid.Runtime.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Displayers.ScatterPlots {
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                               + "ScatterPlot"
                               + DisplayerComponentMenuPath._Postfix)]
  [RequireComponent(requiredComponent : typeof(ParticleSystem))]
  public class TextScatterPlotDisplayer : Displayer {
    [SerializeField] Gradient _gradient;
    ParticleSystem _particle_system;

    ParticleSystem.MainModule _particle_system_main_module;
    ParticleSystemRenderer _particle_system_renderer;

    [SerializeField]
    ParticleSystemSimulationSpace _particle_system_simulation_space = ParticleSystemSimulationSpace.World;

    ParticleSystem.Particle[] _particles;
    [SerializeField] float _size = 0.6f;

    List<string> _vs = new List<string>();

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
      this._particle_system_main_module.startSize = this._size;

      this._particle_system_renderer = this.GetComponent<ParticleSystemRenderer>();
      //this._particle_system_renderer.renderMode = ParticleSystemRenderMode.Mesh;
      this._particle_system_renderer.alignment = ParticleSystemRenderSpace.World;

      if (this._gradient == null) {
        this._gradient = new Gradient {
                                          colorKeys = new[] {
                                                                new GradientColorKey(col : new Color(1, 0, 0),
                                                                                     time : 0f),
                                                                new GradientColorKey(col : new Color(0, 1, 0),
                                                                                     time : 1f)
                                                            }
                                      };
      }
    }

    public override void Display(double value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : "Applying the double " + value + " To " + this.name);
      }
      #endif

      this._Values = new[] {value.ToString(provider : CultureInfo.InvariantCulture)};
      this.PlotSeries(this._Values);
    }

    public override void Display(float[] values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        var s = "";
        foreach (var value in values) {
          s += $"{value},";
        }

        Debug.Log(message : "Applying the float array " + s + " To " + this.name);
      }
      #endif
      this._Values = values.Select(v => v.ToString(provider : CultureInfo.InvariantCulture)).ToArray();
      this.PlotSeries(points : values);
    }

    public override void Display(string values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : "Applying the float array " + values + " To " + this.name);
      }
      #endif

      this._vs.Clear();
      foreach (var value in values.Split(',')) {
        this._vs.Add(item : value);
      }

      this._Values = this._vs.ToArray();
      this.PlotSeries(this._Values);
    }

    public override void Display(Vector3 value) { throw new NotImplementedException(); }
    public override void Display(Vector3[] value) { this.ScatterPlot(points : value); }

    public override void Display(Points.ValuePoint points) { this.PlotSeries(points : new[] {points}); }

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
        Debug.Log(message : "Applying the points " + points_str + " to " + this.name);
      }
      #endif

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        var clamped = Math.Min(val1 : Math.Max(0.0f, val2 : point._Val), val2 : 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = point._Size;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    public override void Display(Points.StringPoint point) { throw new NotImplementedException(); }
    public override void Display(Points.StringPoint[] points) { throw new NotImplementedException(); }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    public override void Display(float values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : "Applying the float " + values + " To " + this.name);
      }
      #endif

      this._Values = new[] {values.ToString(provider : CultureInfo.InvariantCulture)};
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
        var points_str = points.Aggregate("", (current, point) => current + point + ", ");
        Debug.Log(message : "Applying the points " + points_str + " To " + this.name);
      }
      #endif

      var i = 0;
      var l = (float)points.Length;
      for (var index = 0; index < l; index++) {
        var point = points[index];
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point;
        var clamped = Math.Min(val1 : Math.Max(0.0f, val2 : i / l), val2 : 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = 1f;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="points"></param>
    public void PlotSeries(float[] points) {
      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : "Applying the series " + points + " To " + this.name);
      }
      #endif

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = Vector3.one * i;
        var clamped = Math.Min(val1 : Math.Max(0.0f, val2 : point), val2 : 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = 1f;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    public void PlotSeries(string[] points) {
      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : "Applying the series " + points + " To " + this.name);
      }
      #endif

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = Vector3.one * i;
        this._particles[i].startSize = 1f;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public override void PlotSeries(Points.ValuePoint[] points) {
      var alive = this._particle_system.GetParticles(particles : this._particles);
      if (alive < points.Length) {
        this._particles = new ParticleSystem.Particle[points.Length];
      }

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        this._particles[i].startColor = this._gradient.Evaluate(time : point._Val);
        this._particles[i].startSize = point._Size;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }
  }
}
