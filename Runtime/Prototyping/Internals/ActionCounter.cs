using Neodroid.Runtime.Interfaces;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Internals {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ActionCounter : EnvironmentListener {
    [SerializeField] IPrototypingEnvironment _environment;

    /// <summary>
    /// </summary>
    public IPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "ActionCounter"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      /*if (!this._environment) {
        this._environment = this.GetComponent<PrototypingEnvironment>();
      }*/
    }

    /// <summary>
    /// </summary>
    void HandleStep() {
      var reaction = this._environment.LastReaction;
      var motions = reaction.Motions;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreStep() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Step() { this.HandleStep(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() { }
  }
}