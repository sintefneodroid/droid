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

    /// <summary>
    /// 
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

    protected override void PreStep() { }
    protected override void Step() { this.HandleStep(); }
    protected override void PostStep() { }

    public override string PrototypingTypeName { get { return "ActionCounter"; } }
  }
}
