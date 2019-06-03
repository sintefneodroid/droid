using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Internals {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ActionCounter : EnvironmentListener {
    [SerializeField] IActorisedPrototypingEnvironment _environment;

    /// <summary>
    /// </summary>
    public IActorisedPrototypingEnvironment ParentEnvironment {
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

    public override void EnvironmentReset() { }

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
