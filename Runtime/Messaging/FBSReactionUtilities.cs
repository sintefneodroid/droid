using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.FBS;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Messaging.Messages.Displayables;
using droid.Runtime.Structs;
using UnityEngine;

namespace droid.Runtime.Messaging {
  /// <summary>
  /// </summary>
  public static class FbsReactionUtilities {
    static List<Vector3> _vector_out = new List<Vector3>();
    static List<float> _float_out = new List<float>();
    static List<Points.ValuePoint> _output = new List<Points.ValuePoint>();

    #region PublicMethods

    static Reaction _null_reaction = new Reaction(null,
                                                  null,
                                                  null,
                                                  null,
                                                  null,
                                                  "");

    static ReactionParameters _null_reaction_parameters = new ReactionParameters();
    static List<Reaction> _out_reactions = new List<Reaction>();

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    /// <returns></returns>
    public static Tuple<Reaction[], bool, string, SimulatorConfigurationMessage> deserialise_reactions(
        FReactions? reactions) {
      _out_reactions.Clear();

      var close = false;
      var api_version = "";
      var simulator_configuration = new SimulatorConfigurationMessage();

      if (reactions.HasValue) {
        var rs = reactions.Value;
        for (var i = 0; i < rs.ReactionsLength; i++) {
          _out_reactions.Add(deserialise_reaction(rs.Reactions(i)));
        }

        close = rs.Close;
        api_version = rs.ApiVersion;
        if (rs.SimulatorConfiguration.HasValue) {
          simulator_configuration.FbsParse(rs.SimulatorConfiguration.Value);
        }
      }

      if (_out_reactions.Count == 0) {
        Debug.LogWarning("Empty reactions received");
      }

      return new Tuple<Reaction[], bool, String, SimulatorConfigurationMessage>(_out_reactions.ToArray(),
                                                                                close,
                                                                                api_version,
                                                                                simulator_configuration);
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static Reaction deserialise_reaction(FReaction? reaction) {
      if (reaction.HasValue) {
        var r = reaction.Value;
        var motions = deserialise_motions(r);
        var configurations = deserialise_configurations(r);
        var displayables = deserialise_displayables(r);
        var unobservables = deserialise_unobservables(r);
        var parameters = deserialise_parameters(r);
        var serialised_message = deserialise_serialised_message(r);

        return new Reaction(parameters,
                            motions,
                            configurations,
                            unobservables,
                            displayables,
                            serialised_message,
                            r.EnvironmentName);
      }

      Debug.LogWarning("Empty reaction received");
      return _null_reaction;
    }

    #endregion

    #region PrivateMethods

    static String deserialise_simulator_configuration(FReaction reaction_value) {
      return reaction_value.SerialisedMessage;
    }

    static String deserialise_serialised_message(FReaction reaction_value) {
      return reaction_value.SerialisedMessage;
    }

    static Unobservables deserialise_unobservables(FReaction reaction) {
      if (reaction.Unobservables.HasValue) {
        var bodies = deserialise_bodies(reaction.Unobservables.Value);

        var poses = deserialise_poses(reaction.Unobservables.Value);

        return new Unobservables(ref bodies, ref poses);
      }

      return new Unobservables();
    }

    static ReactionParameters deserialise_parameters(FReaction reaction) {
      if (reaction.Parameters.HasValue) {
        var s = StepResetObserve.Observe_;
        if (reaction.Parameters.Value.Reset) {
          s = StepResetObserve.Reset_;
        }else if (reaction.Parameters.Value.Step) {
          s = StepResetObserve.Step_;
        }

        return new ReactionParameters(s,
                                      reaction.Parameters.Value.Terminable,
                                      reaction.Parameters.Value.Configure,
                                      reaction.Parameters.Value.Describe,
                                      reaction.Parameters.Value.EpisodeCount);
      }


      Debug.LogWarning("NULL PARAMETERS");
      return _null_reaction_parameters;
    }

    static Configuration[] deserialise_configurations(FReaction reaction) {
      var l = reaction.ConfigurationsLength;
      var configurations = new Configuration[l];
      for (var i = 0; i < l; i++) {
        configurations[i] = deserialise_configuration(reaction.Configurations(i));
      }

      return configurations;
    }

    static Displayable[] deserialise_displayables(FReaction reaction) {
      var l = reaction.DisplayablesLength;
      var configurations = new Displayable[l];
      for (var i = 0; i < l; i++) {
        configurations[i] = deserialise_displayable(reaction.Displayables(i));
      }

      return configurations;
    }

    static Displayable deserialise_displayable(FDisplayable? displayable) {
      if (displayable.HasValue) {
        var d = displayable.Value;

        switch (d.DisplayableValueType) {
          case FDisplayableValue.NONE: break;

          case FDisplayableValue.FValue:
            return new DisplayableFloat(d.DisplayableName, d.DisplayableValue<FValue>()?.Val);

          case FDisplayableValue.FValues:
            var v3 = d.DisplayableValue<FValues>().GetValueOrDefault();
            _float_out.Clear();
            for (var i = 0; i < v3.ValsLength; i++) {
              _float_out.Add((float)v3.Vals(i));
            }

            return new DisplayableValues(d.DisplayableName, _float_out.ToArray());

          case FDisplayableValue.FVector3s:
            var v2 = d.DisplayableValue<FVector3s>().GetValueOrDefault();
            _vector_out.Clear();
            for (var i = 0; i < v2.PointsLength; i++) {
              var p = v2.Points(i).GetValueOrDefault();
              var v = new Vector3((float)p.X, (float)p.Y, (float)p.Z);
              _vector_out.Add(v);
            }

            return new DisplayableVector3S(d.DisplayableName, _vector_out.ToArray());

          case FDisplayableValue.FValuedVector3s:
            var flat_fvec3 = d.DisplayableValue<FValuedVector3s>().GetValueOrDefault();
            _output.Clear();

            for (var i = 0; i < flat_fvec3.PointsLength; i++) {
              var val = (float)flat_fvec3.Vals(i);
              var p = flat_fvec3.Points(i).GetValueOrDefault();
              var v = new Points.ValuePoint(new Vector3((float)p.X, (float)p.Y, (float)p.Z), val, 1);
              _output.Add(v);
            }

            return new DisplayableValuedVector3S(d.DisplayableName, _output.ToArray());

          case FDisplayableValue.FString:
            return new DisplayableString(d.DisplayableName, d.DisplayableValue<FString>()?.Str);

          case FDisplayableValue.FByteArray: break;
          default: throw new ArgumentOutOfRangeException();
        }
      }

      return null;
    }

    static IMotion[] deserialise_motions(FReaction reaction) {
      var l = reaction.MotionsLength;
      var motions = new IMotion[l];
      for (var i = 0; i < l; i++) {
        motions[i] = deserialise_motion(reaction.Motions(i));
      }

      return motions;
    }

    static Configuration deserialise_configuration(FConfiguration? configuration) {
      if (configuration.HasValue) {
        var c = configuration.Value;
        var sample_random = false; //TODO: c.SampleRandom;
        return new Configuration(c.ConfigurableName, (float)c.ConfigurableValue, sample_random);
      }

      return null;
    }

    static ActuatorMotion deserialise_motion(FMotion? motion) {
      if (motion.HasValue) {
        return new ActuatorMotion(motion.Value.ActorName,
                                  motion.Value.ActuatorName,
                                  (float)motion.Value.Strength);
      }

      return null;
    }

    static Pose[] deserialise_poses(FUnobservables unobservables) {
      var l = unobservables.PosesLength;
      var poses = new Pose[l];
      for (var i = 0; i < l; i++) {
        poses[i] = deserialise_pose(unobservables.Poses(i));
      }

      return poses;
    }

    static Body[] deserialise_bodies(FUnobservables unobservables) {
      var l = unobservables.BodiesLength;
      var bodies = new Body[l];
      for (var i = 0; i < l; i++) {
        bodies[i] = deserialise_body(unobservables.Bodies(i));
      }

      return bodies;
    }

    static Pose deserialise_pose(FQuaternionTransform? trans) {
      if (trans.HasValue) {
        var position = trans.Value.Position;
        var rotation = trans.Value.Rotation;
        var vec3_pos = new Vector3((float)position.X, (float)position.Y, (float)position.Z);
        var quat_rot = new Quaternion((float)rotation.X,
                                      (float)rotation.Y,
                                      (float)rotation.Z,
                                      (float)rotation.W);
        return new Pose(vec3_pos, quat_rot);
      }

      return new Pose();
    }

    static Body deserialise_body(FBody? body) {
      if (body.HasValue) {
        var vel = body.Value.Velocity;
        var ang = body.Value.AngularVelocity;
        var vec3_vel = new Vector3((float)vel.X, (float)vel.Y, (float)vel.Z);
        var vec3_ang = new Vector3((float)ang.X, (float)ang.Y, (float)ang.Z);
        return new Body(vec3_vel, vec3_ang);
      }

      return null;
    }

    #endregion
  }
}
