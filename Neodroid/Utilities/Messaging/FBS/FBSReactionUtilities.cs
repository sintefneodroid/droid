using UnityEngine;

namespace Neodroid.Utilities.Messaging.FBS {
  /// <summary>
  ///
  /// </summary>
  public static class FbsReactionUtilities {
    #region PublicMethods

    /// <summary>
    ///
    /// </summary>
    /// <param name="reactions"></param>
    /// <returns></returns>
    public static System.Tuple<Messages.Reaction[],bool,string,Messages.SimulatorConfiguration> deserialise_reactions(Neodroid.FBS.Reaction.FReactions? reactions) {
      var out_reactions = new System.Collections.Generic.List<Messages.Reaction>();

      var close = false;
      var api_version = "";
      var simulator_configuration = new Messages.SimulatorConfiguration();

      if (reactions.HasValue) {
        var rs = reactions.Value;
        for(var i = 0; i < rs.ReactionsLength; i++) {
          out_reactions.Add(deserialise_reaction(rs.Reactions(i)));
        }

        close = rs.Close;
        api_version = rs.ApiVersion;
        if(rs.SimulatorConfiguration.HasValue) {
          simulator_configuration.FbsParse(rs.SimulatorConfiguration.Value);
        }
      }

      if(out_reactions.Count==0) {
        Debug.LogWarning("Empty reactions received");
      }

      return new System.Tuple<Messages.Reaction[], bool, System.String, Messages.SimulatorConfiguration>(out_reactions.ToArray(),close,api_version,simulator_configuration);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static Messages.Reaction deserialise_reaction(Neodroid.FBS.Reaction.FReaction? reaction) {
      if (reaction.HasValue) {
        var r = reaction.Value;
        var motions = deserialise_motions(r);
        var configurations = deserialise_configurations(r);
        var displayables = deserialise_displayables(r);
        var unobservables = deserialise_unobservables(r);
        var parameters = deserialise_parameters(r);
        var serialised_message = deserialise_serialised_message(r);

        return new Messages.Reaction(
            parameters,
            motions,
            configurations,
            unobservables,
            displayables,
            serialised_message,
            recipient_environment:r.EnvironmentName);
      }

      Debug.LogWarning("Empty reaction received");
      return new Messages.Reaction(null, null, null, null, null, "");
    }

    #endregion

    #region PrivateMethods
    static System.String deserialise_simulator_configuration(Neodroid.FBS.Reaction.FReaction reaction_value) {
      return reaction_value.SerialisedMessage;
    }

    static System.String deserialise_serialised_message(Neodroid.FBS.Reaction.FReaction reaction_value) {
      return reaction_value.SerialisedMessage;
    }

    static Messages.Unobservables deserialise_unobservables(Neodroid.FBS.Reaction.FReaction reaction) {
      if (reaction.Unobservables.HasValue) {
        var bodies = deserialise_bodies(reaction.Unobservables.Value);

        var poses = deserialise_poses(reaction.Unobservables.Value);

        return new Messages.Unobservables(bodies, poses);
      }

      return new Messages.Unobservables();
    }

    static Messages.ReactionParameters deserialise_parameters(Neodroid.FBS.Reaction.FReaction reaction) {
      if (reaction.Parameters.HasValue) {
        return new Messages.ReactionParameters(
            reaction.Parameters.Value.Terminable,
            reaction.Parameters.Value.Step,
            reaction.Parameters.Value.Reset,
            reaction.Parameters.Value.Configure,
            reaction.Parameters.Value.Describe,
            reaction.Parameters.Value.EpisodeCount);
      }

      return new Messages.ReactionParameters();
    }

    static Messages.Configuration[] deserialise_configurations(Neodroid.FBS.Reaction.FReaction reaction) {
      var l = reaction.ConfigurationsLength;
      var configurations = new Messages.Configuration[l];
      for (var i = 0; i < l; i++) {
        configurations[i] = deserialise_configuration(reaction.Configurations(i));
      }

      return configurations;
    }

    static Messages.Displayables.Displayable[] deserialise_displayables(Neodroid.FBS.Reaction.FReaction reaction) {
      var l = reaction.DisplayablesLength;
      var configurations = new Messages.Displayables.Displayable[l];
      for (var i = 0; i < l; i++) {
        configurations[i] = deserialise_displayable(reaction.Displayables(i));
      }

      return configurations;
    }

    static Messages.Displayables.Displayable deserialise_displayable(Neodroid.FBS.Reaction.FDisplayable? displayable) {
      if (displayable.HasValue) {
        var d = displayable.Value;

        switch (d.DisplayableValueType) {
          case Neodroid.FBS.Reaction.FDisplayableValue.NONE: break;

          case Neodroid.FBS.Reaction.FDisplayableValue.FValue:
            return new Messages.Displayables.DisplayableFloat(
                d.DisplayableName,
                d.DisplayableValue<Neodroid.FBS.FValue>()?.Val);

          case Neodroid.FBS.Reaction.FDisplayableValue.FValues:
            var v3 = d.DisplayableValue<Neodroid.FBS.FValues>().GetValueOrDefault();
            var a1 = new System.Collections.Generic.List<float>();
            for (var i = 0; i < v3.ValsLength; i++) {
              a1.Add((float)v3.Vals(i));
            }

            return new Messages.Displayables.DisplayableValues(d.DisplayableName, a1.ToArray());

          case Neodroid.FBS.Reaction.FDisplayableValue.FVector3s:
            var v2 = d.DisplayableValue<Neodroid.FBS.FVector3s>().GetValueOrDefault();
            var a = new System.Collections.Generic.List<Vector3>();
            for (var i = 0; i < v2.PointsLength; i++) {
              var p = v2.Points(i).GetValueOrDefault();
              var v = new Vector3((float)p.X, (float)p.Y, (float)p.Z);
              a.Add(v);
            }

            return new Messages.Displayables.DisplayableVector3S(d.DisplayableName, a.ToArray());

          case Neodroid.FBS.Reaction.FDisplayableValue.FValuedVector3s:
            var flat_fvec3 = d.DisplayableValue<Neodroid.FBS.FValuedVector3s>().GetValueOrDefault();
            var output = new System.Collections.Generic.List<Structs.Points.ValuePoint>();

            for (var i = 0; i < flat_fvec3.PointsLength; i++) {
              var val = (float)flat_fvec3.Vals(i);
              var p = flat_fvec3.Points(i).GetValueOrDefault();
              var v = new Structs.Points.ValuePoint(
                  new Vector3((float)p.X, (float)p.Y, (float)p.Z),
                  val,
                  1);
              output.Add(v);
            }

            return new Messages.Displayables.DisplayableValuedVector3S(d.DisplayableName, output.ToArray());

          case Neodroid.FBS.Reaction.FDisplayableValue.FString:
            return new Messages.Displayables.DisplayableString(
                d.DisplayableName,
                d.DisplayableValue<Neodroid.FBS.FString>()?.Str);

          case Neodroid.FBS.Reaction.FDisplayableValue.FByteArray: break;
          default: throw new System.ArgumentOutOfRangeException();
        }
      }

      return null;
    }

    static Messages.MotorMotion[] deserialise_motions(Neodroid.FBS.Reaction.FReaction reaction) {
      var l = reaction.MotionsLength;
      var motions = new Messages.MotorMotion[l];
      for (var i = 0; i < l; i++) {
        motions[i] = deserialise_motion(reaction.Motions(i));
      }

      return motions;
    }

    static Messages.Configuration deserialise_configuration(Neodroid.FBS.Reaction.FConfiguration? configuration) {
      if (configuration.HasValue) {
        var c = configuration.Value;
        return new Messages.Configuration(
c.ConfigurableName,
            (float)c.ConfigurableValue);
      }

      return null;
    }

    static Messages.MotorMotion deserialise_motion(Neodroid.FBS.Reaction.FMotion? motion) {
      if (motion.HasValue) {
        return new Messages.MotorMotion(
            motion.Value.ActorName,
            motion.Value.MotorName,
            (float)motion.Value.Strength);
      }

      return null;
    }

    static Pose[] deserialise_poses(Neodroid.FBS.FUnobservables unobservables) {
      var l = unobservables.PosesLength;
      var poses = new Pose[l];
      for (var i = 0; i < l; i++) {
        poses[i] = deserialise_pose(unobservables.Poses(i));
      }

      return poses;
    }

    static Messages.Body[] deserialise_bodies(Neodroid.FBS.FUnobservables unobservables) {
      var l = unobservables.BodiesLength;
      var bodies = new Messages.Body[l];
      for (var i = 0; i < l; i++) {
        bodies[i] = deserialise_body(unobservables.Bodies(i));
      }

      return bodies;
    }

    static Pose deserialise_pose(Neodroid.FBS.FQuaternionTransform? trans) {
      if (trans.HasValue) {
        var position = trans.Value.Position;
        var rotation = trans.Value.Rotation;
        var vec3_pos = new Vector3((float)position.X, (float)position.Y, (float)position.Z);
        var quat_rot = new Quaternion(
            (float)rotation.X,
            (float)rotation.Y,
            (float)rotation.Z,
            (float)rotation.W);
        return new Pose(vec3_pos, quat_rot);
      }

      return new Pose();
    }

    static Messages.Body deserialise_body(Neodroid.FBS.FBody? body) {
      if (body.HasValue) {
        var vel = body.Value.Velocity;
        var ang = body.Value.AngularVelocity;
        var vec3_vel = new Vector3((float)vel.X, (float)vel.Y, (float)vel.Z);
        var vec3_ang = new Vector3((float)ang.X, (float)ang.Y, (float)ang.Z);
        return new Messages.Body(vec3_vel, vec3_ang);
      }

      return null;
    }

    #endregion
  }
}
