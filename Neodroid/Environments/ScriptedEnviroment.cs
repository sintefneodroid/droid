using System;
using System.Collections.Generic;
using Neodroid.Managers;
using Neodroid.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Neodroid.Environments {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/ScriptedEnvironment")]
  public class ScriptedEnviroment : NeodroidEnvironment {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Renderer _actor_renderer;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _actor_x;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _actor_y;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Renderer _goal_renderer;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _goal_x;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _goal_y;

    /// <summary>
    ///
    /// </summary>
    int[,] _grid;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _height;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    NeodroidManager _time_simulation_manager;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _width;

    /// <inheritdoc />
    /// <summary>
    ///
    /// </summary>
    public override string PrototypingType { get { return "ScriptedEnvironment"; } }

    /// <summary>
    ///
    /// </summary>
    public Int32 ActorX {
      get { return this._actor_x; }
      set { this._actor_x = Mathf.Max(0, Mathf.Min(this._width - 1, value)); }
    }

    /// <summary>
    ///
    /// </summary>
    public Int32 ActorY {
      get { return this._actor_y; }
      set { this._actor_y = Mathf.Max(0, Mathf.Min(this._height - 1, value)); }
    }

    /// <summary>
    ///
    /// </summary>
    public Int32 GoalX {
      get { return this._goal_x; }
      set { this._goal_x = Mathf.Max(0, Mathf.Min(this._width - 1, value)); }
    }

    /// <summary>
    ///
    /// </summary>
    public Int32 GoalY {
      get { return this._goal_y; }
      set { this._goal_y = Mathf.Max(0, Mathf.Min(this._height - 1, value)); }
    }

    /// <inheritdoc />
    /// <summary>
    ///
    /// </summary>
    protected override void Setup() {
      this._grid = new Int32[this._width, this._height];

      var k = 0;
      for (var i = 0; i < this._width; i++) {
        for (var j = 0; j < this._height; j++) {
          this._grid[i, j] = k++;
        }
      }

      this._time_simulation_manager = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(
          this._time_simulation_manager,
          (NeodroidEnvironment)this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() {
      if (this._goal_renderer) {
        this._goal_renderer.transform.position = new Vector3(this.GoalX, 0, this.GoalY);
      }

      if (this._actor_renderer) {
        this._actor_renderer.transform.position = new Vector3(this.ActorX, 0, this.ActorY);
      }
    }

    /// <inheritdoc />
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override Utilities.Messaging.Messages.Reaction SampleReaction() {
      var motions = new List<Utilities.Messaging.Messages.MotorMotion>();

      var strength = Random.Range(0, 4);
      motions.Add(new Utilities.Messaging.Messages.MotorMotion("", "", strength));

      var rp = new Utilities.Messaging.Messages.ReactionParameters(true, true, episode_count : true) {
          IsExternal = false
      };
      return new Utilities.Messaging.Messages.Reaction(rp, motions.ToArray(), null, null, null, "");
    }

    /// <inheritdoc />
    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override Utilities.Messaging.Messages.EnvironmentState ReactAndCollectState(
        Utilities.Messaging.Messages.Reaction reaction) {
      foreach (var motion in reaction.Motions) {
        switch ((int)motion.Strength) {
          case 0:
            this.ActorY += 1;
            break;
          case 1:
            this.ActorX += 1;
            break;
          case 2:
            this.ActorY -= 1;
            break;
          case 3:
            this.ActorX -= 1;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      var actor_idx = this._grid[this.ActorX, this.ActorY];
      var goal_idx = this._grid[this.GoalX, this.GoalY];

      var terminated = actor_idx == goal_idx;
      var signal = terminated ? 1 : 0;

      var time = Time.time - this._Lastest_Reset_Time;

      return new Utilities.Messaging.Messages.EnvironmentState(
          this.Identifier,
          0,
          time,
          signal,
          terminated,
          new Single[] {actor_idx},
          new Rigidbody[] { },
          new Transform[] { });
    }

    public override void React(Utilities.Messaging.Messages.Reaction reaction) {
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override void Tick() { throw new NotImplementedException(); }

    public override Utilities.Messaging.Messages.EnvironmentState CollectState() {
      throw new NotImplementedException();
    }
  }
}
