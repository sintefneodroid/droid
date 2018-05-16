using System;
using System.Collections.Generic;
using Neodroid.Prototyping.Observers;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation.Tasks {
  //[ExecuteInEditMode]
  /// <summary>
  /// 
  /// </summary>
  public class TaskSequence : NeodroidTask {
    [SerializeField] GoalCellObserver _current_goal_cell;

    [SerializeField] Stack<GoalCellObserver> _goal_stack;

    [SerializeField] GoalCellObserver[] _sequence;

    public GoalCellObserver CurrentGoalCell {
      get { return this._current_goal_cell; }
      private set { this._current_goal_cell = value; }
    }

    void Start() {
      if (this._sequence == null || this._sequence.Length == 0) {
        this._sequence = FindObjectsOfType<GoalCellObserver>();
        Array.Sort(this._sequence, (g1, g2) => g1.OrderIndex.CompareTo(g2.OrderIndex));
      }

      Array.Reverse(this._sequence);
      this._goal_stack = new Stack<GoalCellObserver>(this._sequence);
      this.CurrentGoalCell = this.PopGoal();
    }

    public GoalCellObserver[] GetSequence() { return this._sequence; }

    public GoalCellObserver PopGoal() {
      this.CurrentGoalCell = this._goal_stack.Pop();
      return this.CurrentGoalCell;
    }
  }
}
