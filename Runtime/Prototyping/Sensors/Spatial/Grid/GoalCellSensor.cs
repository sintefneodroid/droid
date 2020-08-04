using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Drawing;
using droid.Runtime.Utilities.Grid;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Grid {
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "GoalCell"
                               + SensorComponentMenuPath._Postfix)]
  public class GoalCellSensor : Sensor,
                                IHasTriple {
    [SerializeField] EmptyCell _current_goal;
    [SerializeField] Vector3 _current_goal_position;

    [SerializeField] bool _draw_names = true;

    [SerializeField] int _order_index;

    /// <summary>
    /// </summary>
    public int OrderIndex { get { return this._order_index; } set { this._order_index = value; } }

    /// <summary>
    /// </summary>
    public bool DrawNames { get { return this._draw_names; } set { this._draw_names = value; } }

    /// <summary>
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
    /// <summary>
    /// </summary>
    public Space3 TripleSpace { get; } = new Space3();

    public override IEnumerable<Single> FloatEnumerable {
      get {
        yield return this._current_goal_position.x;
        yield return this._current_goal_position.y;
        yield return this._current_goal_position.z;
      }
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this._current_goal) {
        this._current_goal_position = this._current_goal.transform.position;
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.DrawNames) {
        if (this._current_goal) {
          NeodroidUtilities.DrawString(text : this._current_goal.name,
                                       world_pos : this._current_goal.transform.position,
                                       color : Color.green);
        }
      }
    }
    #endif
  }
}
