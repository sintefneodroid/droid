using Neodroid.Environments;
using Neodroid.Prototyping.Internals;
using UnityEngine;

namespace Neodroid.Utilities.Unsorted {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  public class ActionCounter : EnvironmentListener {
    [SerializeField] PrototypingEnvironment _environment;

    /// <summary>
    /// 
    /// </summary>
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      if (!this._environment) {
        this._environment = this.GetComponent<PrototypingEnvironment>();
      }
    }

    /// <summary>
    ///
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "ActionCounter"; } }
  }
}
