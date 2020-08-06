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
          _out_reactions.Add(item : deserialise_reaction(reaction : rs.Reactions(j : i)));
        }

        close = rs.Close;
        api_version = rs.ApiVersion;
        if (rs.SimulatorConfiguration.HasValue) {
          simulator_configuration.FbsParse(flat_simulator_configuration : rs.SimulatorConfiguration.Value);
        }
      }

      if (_out_reactions.Count == 0) {
        Debug.LogWarning("Empty reactions received");
      }

      return new
          Tuple<Reaction[], bool, string, SimulatorConfigurationMessage>(item1 : _out_reactions.ToArray(),
                                                                         item2 : close,
                                                                         item3 : api_version,
                                                                         item4 : simulator_configuration);
    }

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static Reaction deserialise_reaction(FReaction? reaction) {
      if (reaction.HasValue) {
        var r = reaction.Value;
        var motions = deserialise_motions(reaction : r);
        var configurations = deserialise_configurations(reaction : r);
        var displayables = deserialise_displayables(reaction : r);
        var unobservables = deserialise_unobservables(reaction : r);
        var parameters = deserialise_parameters(reaction : r);
        var serialised_message = deserialise_serialised_message(reaction_value : r);

        return new Reaction(parameters : parameters,
                            motions : motions,
                            configurations : configurations,
                            unobservables : unobservables,
                            displayables : displayables,
                            serialised_message : serialised_message,
                            recipient_environment : r.EnvironmentName);
      }

      Debug.LogWarning("Empty reaction received");
      return _null_reaction;
    }

    #endregion

    #region PrivateMethods

    static string deserialise_simulator_configuration(FReaction reaction_value) {
      return reaction_value.SerialisedMessage;
    }

    static string deserialise_serialised_message(FReaction reaction_value) {
      return reaction_value.SerialisedMessage;
    }

    static Unobservables deserialise_unobservables(FReaction reaction) {
      if (reaction.Unobservables.HasValue) {
        var bodies = deserialise_bodies(unobservables : reaction.Unobservables.Value);

        var poses = deserialise_poses(unobservables : reaction.Unobservables.Value);

        return new Unobservables(bodies : ref bodies, poses : ref poses);
      }

      return new Unobservables();
    }

    static ReactionParameters deserialise_parameters(FReaction reaction) {
      if (reaction.Parameters.HasValue) {
        var s = ReactionTypeEnum.Observe_;
        if (reaction.Parameters.Value.Reset) {
          s = ReactionTypeEnum.Reset_;
        } else if (reaction.Parameters.Value.Step) {
          s = ReactionTypeEnum.Step_;
        }

        return new ReactionParameters(reaction_type : s,
                                      terminable : reaction.Parameters.Value.Terminable,
                                      configure : reaction.Parameters.Value.Configure,
                                      episode_count : reaction.Parameters.Value.EpisodeCount,
                                      describe : reaction.Parameters.Value.Describe);
      }

      Debug.LogError("NULL PARAMETERS");
      return null;
    }

    static Configuration[] deserialise_configurations(FReaction reaction) {
      var l = reaction.ConfigurationsLength;
      var configurations = new Configuration[l];
      for (var i = 0; i < l; i++) {
        configurations[i] = deserialise_configuration(configuration : reaction.Configurations(j : i));
      }

      return configurations;
    }

    static Displayable[] deserialise_displayables(FReaction reaction) {
      var l = reaction.DisplayablesLength;
      var configurations = new Displayable[l];
      for (var i = 0; i < l; i++) {
        configurations[i] = deserialise_displayable(displayable : reaction.Displayables(j : i));
      }

      return configurations;
    }

    static Displayable deserialise_displayable(FDisplayable? displayable) {
      if (displayable.HasValue) {
        var d = displayable.Value;

        switch (d.DisplayableValueType) {
          case FDisplayableValue.NONE: break;

          case FDisplayableValue.FValue:
            return new DisplayableFloat(displayable_name : d.DisplayableName,
                                        displayable_value : d.DisplayableValue<FValue>()?.Val);

          case FDisplayableValue.FValues:
            var v3 = d.DisplayableValue<FValues>().GetValueOrDefault();
            _float_out.Clear();
            for (var i = 0; i < v3.ValsLength; i++) {
              _float_out.Add(item : (float)v3.Vals(j : i));
            }

            return new DisplayableValues(displayable_name : d.DisplayableName,
                                         displayable_value : _float_out.ToArray());

          case FDisplayableValue.FVector3s:
            var v2 = d.DisplayableValue<FVector3s>().GetValueOrDefault();
            _vector_out.Clear();
            for (var i = 0; i < v2.PointsLength; i++) {
              var p = v2.Points(j : i).GetValueOrDefault();
              var v = new Vector3(x : (float)p.X, y : (float)p.Y, z : (float)p.Z);
              _vector_out.Add(item : v);
            }

            return new DisplayableVector3S(displayable_name : d.DisplayableName,
                                           displayable_value : _vector_out.ToArray());

          case FDisplayableValue.FValuedVector3s:
            var flat_fvec3 = d.DisplayableValue<FValuedVector3s>().GetValueOrDefault();
            _output.Clear();

            for (var i = 0; i < flat_fvec3.PointsLength; i++) {
              var val = (float)flat_fvec3.Vals(j : i);
              var p = flat_fvec3.Points(j : i).GetValueOrDefault();
              var v = new Points.ValuePoint(pos : new Vector3(x : (float)p.X, y : (float)p.Y, z : (float)p.Z),
                                            val : val,
                                            1);
              _output.Add(item : v);
            }

            return new DisplayableValuedVector3S(displayable_name : d.DisplayableName,
                                                 displayable_value : _output.ToArray());

          case FDisplayableValue.FString:
            return new DisplayableString(displayable_name : d.DisplayableName,
                                         displayable_value : d.DisplayableValue<FString>()?.Str);

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
        motions[i] = deserialise_motion(motion : reaction.Motions(j : i));
      }

      return motions;
    }

    static Configuration deserialise_configuration(FConfiguration? configuration) {
      if (configuration.HasValue) {
        var c = configuration.Value;
        var sample_random = false; //TODO: c.SampleRandom;
        return new Configuration(configurable_name : c.ConfigurableName,
                                 configurable_value : (float)c.ConfigurableValue,
                                 sample_random : sample_random);
      }

      return null;
    }

    static ActuatorMotion deserialise_motion(FMotion? motion) {
      if (motion.HasValue) {
        return new ActuatorMotion(actor_name : motion.Value.ActorName,
                                  actuator_name : motion.Value.ActuatorName,
                                  strength : (float)motion.Value.Strength);
      }

      return null;
    }

    static Pose[] deserialise_poses(FUnobservables unobservables) {
      var l = unobservables.PosesLength;
      var poses = new Pose[l];
      for (var i = 0; i < l; i++) {
        poses[i] = deserialise_pose(trans : unobservables.Poses(j : i));
      }

      return poses;
    }

    static Body[] deserialise_bodies(FUnobservables unobservables) {
      var l = unobservables.BodiesLength;
      var bodies = new Body[l];
      for (var i = 0; i < l; i++) {
        bodies[i] = deserialise_body(body : unobservables.Bodies(j : i));
      }

      return bodies;
    }

    static Pose deserialise_pose(FQuaternionTransform? trans) {
      if (trans.HasValue) {
        var position = trans.Value.Position;
        var rotation = trans.Value.Rotation;
        var vec3_pos = new Vector3(x : (float)position.X, y : (float)position.Y, z : (float)position.Z);
        var quat_rot = new Quaternion(x : (float)rotation.X,
                                      y : (float)rotation.Y,
                                      z : (float)rotation.Z,
                                      w : (float)rotation.W);
        return new Pose(position : vec3_pos, rotation : quat_rot);
      }

      return new Pose();
    }

    static Body deserialise_body(FBody? body) {
      if (body.HasValue) {
        var vel = body.Value.Velocity;
        var ang = body.Value.AngularVelocity;
        var vec3_vel = new Vector3(x : (float)vel.X, y : (float)vel.Y, z : (float)vel.Z);
        var vec3_ang = new Vector3(x : (float)ang.X, y : (float)ang.Y, z : (float)ang.Z);
        return new Body(vel : vec3_vel, ang : vec3_ang);
      }

      return null;
    }

    #endregion
  }
}
