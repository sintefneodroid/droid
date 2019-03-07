using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Prototyping.Configurables;
using FlatBuffers;
using UnityEngine;

namespace droid.Runtime.Messaging.FBS {
  /// <summary>
  /// </summary>
  public static class FbsStateUtilities {
    static VectorOffset _null_vector_offset = new VectorOffset();
    static Offset<FUnobservables> _null_unobservables_offset = new Offset<FUnobservables>();

    #region PublicMethods

    /// <summary>
    /// </summary>
    /// <param name="states"></param>
    /// <param name="simulator_configuration"></param>
    /// <param name="serialise_individual_observables"></param>
    /// <param name="do_serialise_unobservables"></param>
    /// <param name="api_version"></param>
    /// <returns></returns>
    public static byte[] Serialise(EnvironmentState[] states,
                                   SimulatorConfigurationMessage simulator_configuration = null,
                                   bool serialise_individual_observables = false,
                                   bool do_serialise_unobservables = false,
                                   string api_version = "") {
      var b = new FlatBufferBuilder(1);
      var state_offsets = new Offset<FState>[states.Length];
      var i = 0;
      foreach (var state in states) {
        state_offsets[i++] =
            serialise_state(b, state, serialise_individual_observables, do_serialise_unobservables);
      }

      var states_vector_offset = FStates.CreateStatesVector(b, state_offsets);

      var api_version_offset = b.CreateString(api_version);

      FStates.StartFStates(b);
      FStates.AddStates(b, states_vector_offset);
      FStates.AddApiVersion(b, api_version_offset);
      FStates.AddSimulatorConfiguration(b, Serialise(b, simulator_configuration));
      var states_offset = FStates.EndFStates(b);

      FStates.FinishFStatesBuffer(b, states_offset);

      return b.SizedByteArray();
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static Offset<FSimulatorConfiguration>
        Serialise(FlatBufferBuilder b, SimulatorConfigurationMessage configuration) {
      return FSimulatorConfiguration.CreateFSimulatorConfiguration(b,
                                                                   configuration.Width,
                                                                   configuration.Height,
                                                                   configuration.FullScreen,
                                                                   configuration.QualityLevel,
                                                                   configuration.TimeScale,
                                                                   configuration.TargetFrameRate,
                                                                   configuration.SimulationType,
                                                                   configuration.FrameSkips,
                                                                   configuration.ResetIterations,
                                                                   configuration.NumOfEnvironments,
                                                                   configuration
                                                                       .DoSerialiseIndividualObservables,
                                                                   configuration.DoSerialiseUnobservables);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="state"></param>
    /// <param name="serialise_individual_observables"></param>
    /// <param name="do_serialise_unobservables"></param>
    /// <returns></returns>
    public static Offset<FState> serialise_state(FlatBufferBuilder b,
                                                 EnvironmentState state,
                                                 bool serialise_individual_observables = false,
                                                 bool do_serialise_unobservables = false) {
      var n = b.CreateString(state.EnvironmentName);

      var observables_vector = FState.CreateObservablesVector(b, state.Observables);

      var observers_vector = _null_vector_offset;
      if (serialise_individual_observables) {
        var observations = state.Observers;

        var observers = new Offset<FOBS>[observations.Length];
        var k = 0;
        foreach (var observer in observations) {
          observers[k++] = Serialise(b, observer);
        }

        observers_vector = FState.CreateObservationsVector(b, observers);
      }

      var unobservables = _null_unobservables_offset;
      if (do_serialise_unobservables) {
        var state_unobservables = state.Unobservables;
        var bodies = state_unobservables.Bodies;

        FUnobservables.StartBodiesVector(b, bodies.Length);
        foreach (var rig in bodies) {
          var vel = rig.Velocity;
          var ang = rig.AngularVelocity;
          FBody.CreateFBody(b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z);
        }

        var bodies_vector = b.EndVector();

        var poses = state_unobservables.Poses;

        FUnobservables.StartPosesVector(b, poses.Length);
        foreach (var tra in poses) {
          var pos = tra.position;
          var rot = tra.rotation;
          FQuaternionTransform.CreateFQuaternionTransform(b, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w);
        }

        var poses_vector = b.EndVector();

        FUnobservables.StartFUnobservables(b);
        FUnobservables.AddPoses(b, poses_vector);
        FUnobservables.AddBodies(b, bodies_vector);
        unobservables = FUnobservables.EndFUnobservables(b);
      }

      var description_offset = new Offset<FEnvironmentDescription>();
      if (state.Description != null) {
        description_offset = Serialise(b, state);
      }

      var d = new StringOffset();
      if (state.DebugMessage != "") {
        d = b.CreateString(state.DebugMessage);
      }

      var t = b.CreateString(state.TerminationReason);

      FState.StartFState(b);
      FState.AddEnvironmentName(b, n);

      FState.AddFrameNumber(b, state.FrameNumber);
      FState.AddObservables(b, observables_vector);

      if (do_serialise_unobservables) {
        FState.AddUnobservables(b, unobservables);
      }

      FState.AddTotalEnergySpent(b, state.TotalEnergySpentSinceReset);
      FState.AddSignal(b, state.Signal);

      FState.AddTerminated(b, state.Terminated);
      FState.AddTerminationReason(b, t);

      if (serialise_individual_observables) {
        FState.AddObservations(b, observers_vector);
      }

      if (state.Description != null) {
        FState.AddEnvironmentDescription(b, description_offset);
      }

      if (state.DebugMessage != "") {
        FState.AddSerialisedMessage(b, d);
      }

      return FState.EndFState(b);
    }

    #endregion

    #region PrivateMethods

    static Offset<FMotor> Serialise(FlatBufferBuilder b, IMotor motor, string identifier) {
      var n = b.CreateString(identifier);
      FMotor.StartFMotor(b);
      FMotor.AddMotorName(b, n);
      FMotor.AddValidInput(b,
                           FRange.CreateFRange(b,
                                               motor.MotionSpace1._Decimal_Granularity,
                                               motor.MotionSpace1._Max_Value,
                                               motor.MotionSpace1._Min_Value));
      FMotor.AddEnergySpentSinceReset(b, motor.GetEnergySpend());
      return FMotor.EndFMotor(b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    static Offset<FEulerTransform> Serialise(FlatBufferBuilder b, IHasEulerTransform observer) {
      Vector3 pos = observer.Position, rot = observer.Rotation, dir = observer.Direction;
      return FEulerTransform.CreateFEulerTransform(b,
                                                   pos.x,
                                                   pos.y,
                                                   pos.z,
                                                   rot.x,
                                                   rot.y,
                                                   rot.z,
                                                   dir.x,
                                                   dir.y,
                                                   dir.z);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    static Offset<FQT> Serialise(FlatBufferBuilder b, IHasQuaternionTransform observer) {
      var pos = observer.Position;
      var rot = observer.Rotation;
      FQT.StartFQT(b);
      FQT.AddTransform(b,
                       FQuaternionTransform.CreateFQuaternionTransform(b,
                                                                       pos.x,
                                                                       pos.y,
                                                                       pos.z,
                                                                       rot.x,
                                                                       rot.y,
                                                                       rot.z,
                                                                       rot.w));
      return FQT.EndFQT(b);
    }

    static Offset<FByteArray> Serialise(FlatBufferBuilder b, IHasByteArray camera) {
      //var v_offset = FByteArray.CreateBytesVector(b, camera.Bytes);
      var v_offset = CustomFlatBufferImplementation.CreateByteVector(b, camera.Bytes);
      FByteArray.StartFByteArray(b);
      FByteArray.AddType(b, FByteDataType.PNG);
      FByteArray.AddBytes(b, v_offset);
      return FByteArray.EndFByteArray(b);
    }

    static Offset<FArray> Serialise(FlatBufferBuilder b, IHasArray float_a) {
      //var v_offset = FArray.CreateArrayVector(b, camera.ObservationArray);
      var v_offset = CustomFlatBufferImplementation.CreateFloatVector(b, float_a.ObservationArray);

      //  TODO:     var space_offset = CustomFlatBufferImplementation.CreateSpaceVector(b, float_a.ObservationSpace);
      //FArray.StartRangesVector(b,);
      FArray.StartFArray(b);
      FArray.AddArray(b, v_offset);
      // TODO:       FArray.AddSpace(b, space_offset);
      //FArray.AddRanges(b,);
      return FArray.EndFArray(b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="vel"></param>
    /// <param name="ang"></param>
    /// <returns></returns>
    static Offset<FRB> Serialise(FlatBufferBuilder b, Vector3 vel, Vector3 ang) {
      FRB.StartFRB(b);
      FRB.AddBody(b, FBody.CreateFBody(b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z));
      return FRB.EndFRB(b);
    }

    static Offset<FSingle> Serialise(FlatBufferBuilder b, IHasSingle numeral) {
      FSingle.StartFSingle(b);
      FSingle.AddValue(b, numeral.ObservationValue);

      var range_offset = FRange.CreateFRange(b,
                                             numeral.SingleSpace._Decimal_Granularity,
                                             numeral.SingleSpace._Max_Value,
                                             numeral.SingleSpace._Min_Value);
      FSingle.AddRange(b, range_offset);
      return FSingle.EndFSingle(b);
    }

    static Offset<FDouble> Serialise(FlatBufferBuilder b, IHasDouble numeral) {
      FDouble.StartFDouble(b);
      var vec2 = numeral.ObservationValue;
      FDouble.AddVec2(b, FVector2.CreateFVector2(b, vec2.x, vec2.y));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return FDouble.EndFDouble(b);
    }

    static Offset<FTriple> Serialise(FlatBufferBuilder b, IHasTriple numeral) {
      FTriple.StartFTriple(b);
      var vec3 = numeral.ObservationValue;
      FTriple.AddVec3(b, FVector3.CreateFVector3(b, vec3.x, vec3.y, vec3.z));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return FTriple.EndFTriple(b);
    }

    static Offset<FQuadruple> Serialise(FlatBufferBuilder b, IHasQuadruple numeral) {
      FQuadruple.StartFQuadruple(b);
      var quad = numeral.ObservationValue;
      FQuadruple.AddQuat(b, FQuaternion.CreateFQuaternion(b, quad.x, quad.y, quad.z, quad.z));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return FQuadruple.EndFQuadruple(b);
    }

    static Offset<FString> Serialise(FlatBufferBuilder b, IHasString numeral) {
      var string_offset = b.CreateString(numeral.ObservationValue);
      FString.StartFString(b);
      FString.AddStr(b, string_offset);

      return FString.EndFString(b);
    }

    static Offset<FActor> Serialise(FlatBufferBuilder b,
                                    Offset<FMotor>[] motors,
                                    IActor actor,
                                    string identifier) {
      var n = b.CreateString(identifier);
      var motor_vector = FActor.CreateMotorsVector(b, motors);
      FActor.StartFActor(b);
      if (actor is KillableActor) {
        FActor.AddAlive(b, ((KillableActor)actor).IsAlive);
      } else {
        FActor.AddAlive(b, true);
      }

      FActor.AddActorName(b, n);
      FActor.AddMotors(b, motor_vector);
      return FActor.EndFActor(b);
    }

    static Offset<FOBS> Serialise(FlatBufferBuilder b, IObserver observer) {
      var n = b.CreateString(observer.Identifier);

      int observation_offset;
      FObservation observation_type;
      if (observer is IHasString) {
        observation_offset = Serialise(b, (IHasString)observer).Value;
        observation_type = FObservation.FString;
      } else if (observer is IHasArray) {
        observation_offset = Serialise(b, (IHasArray)observer).Value;
        observation_type = FObservation.FArray;
      } else if (observer is IHasSingle) {
        observation_offset = Serialise(b, (IHasSingle)observer).Value;
        observation_type = FObservation.FSingle;
      } else if (observer is IHasDouble) {
        observation_offset = Serialise(b, (IHasDouble)observer).Value;
        observation_type = FObservation.FDouble;
      } else if (observer is IHasTriple) {
        observation_offset = Serialise(b, (IHasTriple)observer).Value;
        observation_type = FObservation.FTriple;
      } else if (observer is IHasQuadruple) {
        observation_offset = Serialise(b, (IHasQuadruple)observer).Value;
        observation_type = FObservation.FQuadruple;
      } else if (observer is IHasEulerTransform) {
        observation_offset = Serialise(b, (IHasEulerTransform)observer).Value;
        observation_type = FObservation.FET;
      } else if (observer is IHasQuaternionTransform) {
        observation_offset = Serialise(b, (IHasQuaternionTransform)observer).Value;
        observation_type = FObservation.FQT;
      } else if (observer is IHasRigidbody) {
        observation_offset = Serialise(b,
                                       ((IHasRigidbody)observer).Velocity,
                                       ((IHasRigidbody)observer).AngularVelocity).Value;
        observation_type = FObservation.FRB;
      } else if (observer is IHasByteArray) {
        observation_offset = Serialise(b, (IHasByteArray)observer).Value;
        observation_type = FObservation.FByteArray;
      } else {
        return FOBS.CreateFOBS(b, n);
      }

      FOBS.StartFOBS(b);
      FOBS.AddObservationName(b, n);
      FOBS.AddObservationType(b, observation_type);
      FOBS.AddObservation(b, observation_offset);
      return FOBS.EndFOBS(b);
    }

    static Offset<FEnvironmentDescription> Serialise(FlatBufferBuilder b, EnvironmentState state) {
      var actors_offsets = new Offset<FActor>[state.Description.Actors.Values.Count];
      var j = 0;
      foreach (var actor in state.Description.Actors) {
        var motors_offsets = new Offset<FMotor>[actor.Value.Motors.Values.Count];
        var i = 0;
        foreach (var motor in actor.Value.Motors) {
          motors_offsets[i++] = Serialise(b, motor.Value, motor.Key);
        }

        actors_offsets[j++] = Serialise(b, motors_offsets, actor.Value, actor.Key);
      }

      var actors_vector_offset = FEnvironmentDescription.CreateActorsVector(b, actors_offsets);

      var configurables_offsets = new Offset<FConfigurable>[state.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in state.Description.Configurables) {
        configurables_offsets[k++] = Serialise(b, configurable.Value, configurable.Key);
      }

      var configurables_vector_offset =
          FEnvironmentDescription.CreateConfigurablesVector(b, configurables_offsets);

      var objective_offset = Serialise(b, state.Description);

      FEnvironmentDescription.StartFEnvironmentDescription(b);
      FEnvironmentDescription.AddObjective(b, objective_offset);

      FEnvironmentDescription.AddActors(b, actors_vector_offset);
      FEnvironmentDescription.AddConfigurables(b, configurables_vector_offset);

      return FEnvironmentDescription.EndFEnvironmentDescription(b);
    }

    static Offset<FObjective> Serialise(FlatBufferBuilder b, EnvironmentDescription description) {
      var objective_name_offset = b.CreateString("Default objective");
      FObjective.StartFObjective(b);
      FObjective.AddMaxEpisodeLength(b, description.MaxSteps);
      FObjective.AddSolvedThreshold(b, description.SolvedThreshold);
      FObjective.AddObjectiveName(b, objective_name_offset);
      return FObjective.EndFObjective(b);
    }

    static Offset<FTriple> Serialise(FlatBufferBuilder b, PositionConfigurable observer) {
      var pos = observer.ObservationValue;
      FTriple.StartFTriple(b);
      FTriple.AddVec3(b, FVector3.CreateFVector3(b, pos.x, pos.y, pos.z));
      return FTriple.EndFTriple(b);
    }

    static Offset<FConfigurable> Serialise(
        FlatBufferBuilder b,
        IConfigurable configurable,
        string identifier) {
      var n = b.CreateString(identifier);

      int observation_offset;
      FObservation observation_type;

      if (configurable is IHasQuaternionTransform) {
        observation_offset = Serialise(b, (IHasQuaternionTransform)configurable).Value;
        observation_type = FObservation.FQT;
      } else if (configurable is PositionConfigurable) {
        observation_offset = Serialise(b, (PositionConfigurable)configurable).Value;
        observation_type = FObservation.FTriple;
      } else if (configurable is IHasSingle) {
        observation_offset = Serialise(b, (IHasSingle)configurable).Value;
        observation_type = FObservation.FSingle;
        // ReSharper disable once SuspiciousTypeConversion.Global
      } else if (configurable is IHasDouble) {
        // ReSharper disable once SuspiciousTypeConversion.Global
        observation_offset = Serialise(b, (IHasDouble)configurable).Value;
        observation_type = FObservation.FDouble;
      } else if (configurable is EulerTransformConfigurable) {
        observation_offset = Serialise(b, (IHasEulerTransform)configurable).Value;
        observation_type = FObservation.FET;
      } else {
        FConfigurable.StartFConfigurable(b);
        FConfigurable.AddConfigurableName(b, n);
        return FConfigurable.EndFConfigurable(b);
      }

      FConfigurable.StartFConfigurable(b);
      FConfigurable.AddConfigurableName(b, n);
      FConfigurable.AddObservation(b, observation_offset);
      FConfigurable.AddObservationType(b, observation_type);
      return FConfigurable.EndFConfigurable(b);
    }

    #endregion
  }
}
