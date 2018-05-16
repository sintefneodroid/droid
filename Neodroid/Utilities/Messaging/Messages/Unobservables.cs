using System.Collections.Generic;
using UnityEngine;

namespace Neodroid.Utilities.Messaging.Messages {
  /// <summary>
  /// 
  /// </summary>
  public class Unobservables {
    public Unobservables(IList<Rigidbody> rigidbodies, IList<Transform> transforms) {
      if (rigidbodies != null) {
        this.Bodies = new Body[rigidbodies.Count];
        for (var i = 0; i < this.Bodies.Length; i++) {
          if (rigidbodies[i] != null) {
            this.Bodies[i] = new Body(rigidbodies[i].velocity, rigidbodies[i].angularVelocity);
          }
        }
      }

      if (transforms != null) {
        this.Poses = new Pose[transforms.Count];
        for (var i = 0; i < this.Poses.Length; i++) {
          if (transforms[i] != null) {
            this.Poses[i] = new Pose(transforms[i].position, transforms[i].rotation);
          }
        }
      }
    }

    public Unobservables(Body[] bodies, Pose[] poses) {
      this.Bodies = bodies;
      this.Poses = poses;
    }

    public Unobservables() { }

    /// <summary>
    /// 
    /// </summary>
    public Body[] Bodies { get; } = { };

    /// <summary>
    /// 
    /// </summary>
    public Pose[] Poses { get; } = { };

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var poses_str = "";
      if (this.Poses != null) {
        foreach (var pose in this.Poses) {
          poses_str += pose + "\n";
        }
      }

      var bodies_str = "";
      if (this.Bodies != null) {
        foreach (var body in this.Bodies) {
          bodies_str += body + "\n";
        }
      }

      return $"<Unobservables>\n {poses_str},{bodies_str}\n</Unobservables>\n";
    }
  }
}
