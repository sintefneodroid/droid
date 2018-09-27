using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.Misc.Grasping;
using Neodroid.Runtime.Utilities.Misc.Grid;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;
using NeodroidUtilities = Neodroid.Runtime.Utilities.Misc.NeodroidUtilities;

namespace Neodroid.Runtime.Prototyping.Observers.Grid {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "GoalCell" + ObserverComponentMenuPath._Postfix)]
  public class GoalCellObserver : Observer,
                                  IHasTriple {
    [SerializeField] EmptyCell _current_goal;
    [SerializeField] Vector3 _current_goal_position;

    [SerializeField] bool _draw_names = true;

    [SerializeField] int _order_index;

    /// <summary>
    /// 
    /// </summary>
    public int OrderIndex { get { return this._order_index; } set { this._order_index = value; } }

    /// <summary>
    /// 
    /// </summary>
    public bool DrawNames { get { return this._draw_names; } set { this._draw_names = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "GoalObserver"; } }

    /// <summary>
    /// 
    /// </summary>
    public EmptyCell CurrentGoal {
      get {
        this.UpdateObservation();
        return this._current_goal;
      }
      set { this._current_goal = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._current_goal_position; }
      private set { this._current_goal_position = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Space3 TripleSpace { get; } = new Space3();

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateObservation() {
      if (this._current_goal) {
        this._current_goal_position = this._current_goal.transform.position;
      }

      this.FloatEnumerable = new[] {
          this._current_goal_position.x, this._current_goal_position.y, this._current_goal_position.z
      };
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.DrawNames) {
        if (this._current_goal) {
          NeodroidDrawingUtilities.DrawString(
              this._current_goal.name,
              this._current_goal.transform.position,
              Color.green);
        }
      }
    }
    #endif
  }
}
