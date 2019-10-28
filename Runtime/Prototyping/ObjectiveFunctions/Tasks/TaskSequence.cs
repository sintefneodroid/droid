using System;
using System.Collections.Generic;
using droid.Runtime.Prototyping.Sensors.Spatial.Grid;
using droid.Runtime.ScriptableObjects.Deprecated;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Tasks {
  //[ExecuteInEditMode]
  /// <summary>
  /// </summary>
  public class TaskSequence : NeodroidTask {
    [SerializeField] GoalCellSensor _current_goal_cell;

    [SerializeField] Stack<GoalCellSensor> _goal_stack;

    [SerializeField] GoalCellSensor[] _sequence;

    public GoalCellSensor CurrentGoalCell {
      get { return this._current_goal_cell; }
      private set { this._current_goal_cell = value; }
    }

    void Start() {
      if (this._sequence == null || this._sequence.Length == 0) {
        this._sequence = FindObjectsOfType<GoalCellSensor>();
        Array.Sort(this._sequence, (g1, g2) => g1.OrderIndex.CompareTo(g2.OrderIndex));
      }

      Array.Reverse(this._sequence);
      this._goal_stack = new Stack<GoalCellSensor>(this._sequence);
      this.CurrentGoalCell = this.PopGoal();
    }

    public GoalCellSensor[] GetSequence() { return this._sequence; }

    public GoalCellSensor PopGoal() {
      this.CurrentGoalCell = this._goal_stack.Pop();
      return this.CurrentGoalCell;
    }
  }
}
