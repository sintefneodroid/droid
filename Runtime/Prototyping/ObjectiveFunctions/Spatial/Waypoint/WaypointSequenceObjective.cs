using System;
using System.Collections;
using UnityEngine;

namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial.Waypoint {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(EvaluationComponentMenuPath._ComponentMenuPath
                    + "WaypointSequence"
                    + EvaluationComponentMenuPath._Postfix)]
  class WaypointSequenceObjective : SpatialObjective {
    [SerializeField] Waypoint[] waypoints;
    [SerializeField] Waypoint last_waypoint;
    [SerializeField] Waypoint current_waypoint;
    [SerializeField] Waypoint next_waypoint;
    [SerializeField] Transform tracking_point;
    [SerializeField] IEnumerator waypoint_enumerator;

    [SerializeField] float margin = 0.01f;
    [SerializeField] bool inverse = false;
    [SerializeField] bool reward_for_accounting_for_next;
    [SerializeField] bool reward_for_smoothing_with_last;

    public override void InternalReset() {
      if (this.waypoint_enumerator == null)
        this.waypoint_enumerator = this.waypoints.GetEnumerator();

      this.waypoint_enumerator.Reset();

      this.last_waypoint = null;
      this.current_waypoint = this.waypoint_enumerator.Current as Waypoint;

      this.waypoint_enumerator.MoveNext();
      this.next_waypoint = this.waypoint_enumerator.Current as Waypoint;
    }

    public void MoveToNext() {
      this.waypoint_enumerator.MoveNext();
      var nex = this.waypoint_enumerator.Current as Waypoint;

      if (!nex) {
        this.ParentEnvironment.Terminate("Last waypoint reached");
      } else {
        this.last_waypoint = this.current_waypoint;
        this.current_waypoint = this.next_waypoint;
        this.next_waypoint = nex;
      }
    }

    public override float InternalEvaluate() {
      var signal = 0.0f;
      var distance = Vector3.Distance(a : this.tracking_point.position, b : this.current_waypoint.transform
      .position);

      if (distance <= this.margin) {
        this.MoveToNext();
        signal += this.SolvedSignal;
      } else {
        if (this.inverse) {
          signal += distance;
        } else {
          signal -= distance;
        }
      }

      if (this.reward_for_accounting_for_next) {
        signal += 1/Vector3.Distance(a : this.tracking_point.position, b : this.current_waypoint.transform.position);
        signal += Vector3.Dot(this.tracking_point.forward, this.next_waypoint.transform.forward);
      }

      if (this.reward_for_smoothing_with_last) {
        signal += Vector3.Dot(this.tracking_point.forward, this.next_waypoint.transform.forward);
      }

      return signal;
    }

    public override void RemotePostSetup() { }
  }
}
