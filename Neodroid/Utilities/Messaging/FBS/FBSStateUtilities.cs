using droid.Neodroid.Prototyping.Actors;
using droid.Neodroid.Prototyping.Configurables;
using droid.Neodroid.Prototyping.Motors;
using droid.Neodroid.Prototyping.Observers;
using droid.Neodroid.Utilities.Interfaces;
using droid.Neodroid.Utilities.Messaging.Messages;
using FlatBuffers;
using UnityEngine;
using SimulatorConfiguration = droid.Neodroid.Utilities.ScriptableObjects.SimulatorConfiguration;

namespace droid.Neodroid.Utilities.Messaging.FBS {
  /// <summary>
  ///
  /// </summary>
  public static class FbsStateUtilities {
    #region PublicMethods

    ///  <summary>
    ///
    ///  </summary>
    ///  <param name="states"></param>
    /// <param name="simulator_configuration"></param>
    /// <param name="api_version"></param>
    /// <returns></returns>
    public static byte[] serialise_states(EnvironmentState[] states,  SimulatorConfiguration simulator_configuration=null, string api_version="") {
      var b = new FlatBufferBuilder(1);
      var state_offsets = new Offset<FState>[states.Length];
      var i = 0;
      foreach (var state in states) {
          state_offsets[i++] = serialise_state(b, state);
      }
      var states_vector_offset = FStates.CreateStatesVector(b,state_offsets);

      var api_version_offset = b.CreateString(api_version);

      FStates.StartFStates(b);
      FStates.AddStates(b, states_vector_offset);
      FStates.AddApiVersion(b, api_version_offset);
      //TODO: droid.Neodroid.FBS.State.FStates.AddSimulatorConfiguration();
      var states_offset = FStates.EndFStates(b);

      FStates.FinishFStatesBuffer(b, states_offset);

      return b.SizedByteArray();
    }

    public static Offset<FSimulatorConfiguration> serialise_simulator_configuration(
        FlatBufferBuilder b,
        SimulatorConfiguration configuration) {



      return FSimulatorConfiguration.CreateFSimulatorConfiguration(b,configuration.Width,configuration.Height,configuration.FullScreen,configuration.QualityLevel,configuration.TimeScale,configuration.TargetFrameRate,(int)configuration.SimulationType,configuration.FrameSkips,configuration.ResetIterations,configuration.NumOfEnvironments);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public static Offset<FState> serialise_state(FlatBufferBuilder b, EnvironmentState state) {
        var n = b.CreateString(state.EnvironmentName);

        var observables_vector = FState.CreateObservablesVector(b, state.Observables);

        var observers = new Offset<FOBS>[state.Observations.Values.Count];
        var k = 0;
        foreach (var observer in state.Observations.Values) {
          observers[k++] = serialise_observer(b, observer);
        }

        var observers_vector = FState.CreateObservationsVector(b, observers);

        FUnobservables.StartBodiesVector(b, state.Unobservables.Bodies.Length);
        foreach (var rig in state.Unobservables.Bodies) {
          var vel = rig.Velocity;
          var ang = rig.AngularVelocity;
          FBody.CreateFBody(b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z);
        }

        var bodies_vector = b.EndVector();

        FUnobservables.StartPosesVector(b, state.Unobservables.Poses.Length);
        foreach (var tra in state.Unobservables.Poses) {
          var pos = tra.position;
          var rot = tra.rotation;
          FQuaternionTransform.CreateFQuaternionTransform(b, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w);
        }

        var poses_vector = b.EndVector();

        FUnobservables.StartFUnobservables(b);
        FUnobservables.AddPoses(b, poses_vector);
        FUnobservables.AddBodies(b, bodies_vector);
        var unobservables = FUnobservables.EndFUnobservables(b);

        var description_offset = new Offset<FEnvironmentDescription>();
        if (state.Description != null) {
          description_offset = serialise_description(b, state);
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
        FState.AddUnobservables(b, unobservables);

        FState.AddTotalEnergySpent(b, state.TotalEnergySpentSinceReset);
        FState.AddSignal(b, state.Signal);

        FState.AddTerminated(b, state.Terminated);
        FState.AddTerminationReason(b, t);

        FState.AddObservations(b, observers_vector);
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

    static Offset<FMotor> serialise_motor(FlatBufferBuilder b, Motor motor, string identifier) {
      var n = b.CreateString(identifier);
      FMotor.StartFMotor(b);
      FMotor.AddMotorName(b, n);
      FMotor.AddValidInput(
          b,
          FRange.CreateFRange(
              b,
              motor.MotionValueSpace._Decimal_Granularity,
              motor.MotionValueSpace._Max_Value,
              motor.MotionValueSpace._Min_Value));
      FMotor.AddEnergySpentSinceReset(b, motor.GetEnergySpend());
      return FMotor.EndFMotor(b);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    static Offset<FEulerTransform> serialise_euler_transform(FlatBufferBuilder b, IHasEulerTransform observer) {
      Vector3 pos = observer.Position, rot = observer.Rotation, dir = observer.Direction;
      return FEulerTransform.CreateFEulerTransform(
          b,
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
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    static Offset<FQT> serialise_quaternion_transform(FlatBufferBuilder b, IHasQuaternionTransform observer) {
      var pos = observer.Position;
      var rot = observer.Rotation;
      FQT.StartFQT(b);
      FQT.AddTransform(
          b,
          FQuaternionTransform.CreateFQuaternionTransform(
              b,
              pos.x,
              pos.y,
              pos.z,
              rot.x,
              rot.y,
              rot.z,
              rot.w));
      return FQT.EndFQT(b);
    }

    static Offset<FByteArray> serialise_byte_array(FlatBufferBuilder b, IHasByteArray camera) {
      //var v_offset = FByteArray.CreateBytesVector(b, camera.Bytes);
      var v_offset = CustomFlatBufferImplementation.CreateByteVector(b, camera.Bytes);
      FByteArray.StartFByteArray(b);
      FByteArray.AddType(b, FByteDataType.PNG);
      FByteArray.AddBytes(b, v_offset);
      return FByteArray.EndFByteArray(b);
    }

    static Offset<FArray> serialise_array(FlatBufferBuilder b, IHasArray float_a) {
      //var v_offset = FArray.CreateArrayVector(b, camera.ObservationArray);
      var v_offset = CustomFlatBufferImplementation.CreateFloatVector(b, float_a.ObservationArray);
      //FArray.StartRangesVector(b,);
      FArray.StartFArray(b);
      FArray.AddArray(b, v_offset);
      //FArray.AddRanges(b,);
      return FArray.EndFArray(b);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="vel"></param>
    /// <param name="ang"></param>
    /// <returns></returns>
    static Offset<FRB> serialise_body_observation(FlatBufferBuilder b, Vector3 vel, Vector3 ang) {
      FRB.StartFRB(b);
      FRB.AddBody(b, FBody.CreateFBody(b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z));
      return FRB.EndFRB(b);
    }

    static Offset<FSingle> serialise_single(FlatBufferBuilder b, IHasSingle numeral) {
      FSingle.StartFSingle(b);
      FSingle.AddValue(b, numeral.ObservationValue);

      var range_offset = FRange.CreateFRange(
          b,
          numeral.SingleSpace._Decimal_Granularity,
          numeral.SingleSpace._Max_Value,
          numeral.SingleSpace._Min_Value);
      FSingle.AddRange(b, range_offset);
      return FSingle.EndFSingle(b);
    }

    static Offset<FDouble> serialise_double(FlatBufferBuilder b, IHasDouble numeral) {
      FDouble.StartFDouble(b);
      var vec2 = numeral.ObservationValue;
      FDouble.AddVec2(b, FVector2.CreateFVector2(b, vec2.x, vec2.y));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return FDouble.EndFDouble(b);
    }

    static Offset<FTriple> serialise_triple(FlatBufferBuilder b, IHasTriple numeral) {
      FTriple.StartFTriple(b);
      var vec3 = numeral.ObservationValue;
      FTriple.AddVec3(b, FVector3.CreateFVector3(b, vec3.x, vec3.y, vec3.z));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return FTriple.EndFTriple(b);
    }

    static Offset<FQuadruple> serialise_quadruple(FlatBufferBuilder b, IHasQuadruple numeral) {
      FQuadruple.StartFQuadruple(b);
      var quad = numeral.ObservationValue;
      FQuadruple.AddQuat(b, FQuaternion.CreateFQuaternion(b, quad.x, quad.y, quad.z, quad.z));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return FQuadruple.EndFQuadruple(b);
    }

    static Offset<FActor> serialise_actor(
        FlatBufferBuilder b,
        Offset<FMotor>[] motors,
        Actor actor,
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

    static Offset<FOBS> serialise_observer(FlatBufferBuilder b, Observer observer) {
      var n = b.CreateString(observer.Identifier);

      int observation_offset;
      FObservation observation_type;

      if (observer is IHasArray) {
        observation_offset = serialise_array(b, (IHasArray)observer).Value;
        observation_type = FObservation.FArray;
      } else if (observer is IHasSingle) {
        observation_offset = serialise_single(b, (IHasSingle)observer).Value;
        observation_type = FObservation.FSingle;
      } else if (observer is IHasDouble) {
        observation_offset = serialise_double(b, (IHasDouble)observer).Value;
        observation_type = FObservation.FDouble;
      } else if (observer is IHasTriple) {
        observation_offset = serialise_triple(b, (IHasTriple)observer).Value;
        observation_type = FObservation.FTriple;
      } else if (observer is IHasQuadruple) {
        observation_offset = serialise_quadruple(b, (IHasQuadruple)observer).Value;
        observation_type = FObservation.FQuadruple;
      } else if (observer is IHasEulerTransform) {
        observation_offset = serialise_euler_transform(b, (IHasEulerTransform)observer).Value;
        observation_type = FObservation.FET;
      } else if (observer is IHasQuaternionTransform) {
        observation_offset = serialise_quaternion_transform(b, (IHasQuaternionTransform)observer).Value;
        observation_type = FObservation.FQT;
      } else if (observer is IHasRigidbody) {
        observation_offset = serialise_body_observation(
            b,
            ((IHasRigidbody)observer).Velocity,
            ((IHasRigidbody)observer).AngularVelocity).Value;
        observation_type = FObservation.FRB;
      } else if (observer is IHasByteArray) {
        observation_offset = serialise_byte_array(b, (IHasByteArray)observer).Value;
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

    static Offset<FEnvironmentDescription> serialise_description(
        FlatBufferBuilder b,
        EnvironmentState state) {
      var actors_offsets = new Offset<FActor>[state.Description.Actors.Values.Count];
      var j = 0;
      foreach (var actor in state.Description.Actors) {
        var motors_offsets = new Offset<FMotor>[actor.Value.Motors.Values.Count];
        var i = 0;
        foreach (var motor in actor.Value.Motors) {
          motors_offsets[i++] = serialise_motor(b, motor.Value, motor.Key);
        }

        actors_offsets[j++] = serialise_actor(b, motors_offsets, actor.Value, actor.Key);
      }

      var actors_vector_offset = FEnvironmentDescription.CreateActorsVector(b, actors_offsets);

      var configurables_offsets = new Offset<FConfigurable>[state.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in state.Description.Configurables) {
        configurables_offsets[k++] = serialise_configurable(b, configurable.Value, configurable.Key);
      }

      var configurables_vector_offset = FEnvironmentDescription.CreateConfigurablesVector(b, configurables_offsets);


      var objective_offset = serialise_objective(b, state.Description);

      FEnvironmentDescription.StartFEnvironmentDescription(b);
      FEnvironmentDescription.AddObjective(b, objective_offset);

      FEnvironmentDescription.AddActors(b, actors_vector_offset);
      FEnvironmentDescription.AddConfigurables(b, configurables_vector_offset);

      return FEnvironmentDescription.EndFEnvironmentDescription(b);
    }

    static Offset<FObjective> serialise_objective(
        FlatBufferBuilder b,
        EnvironmentDescription description) {
      var objective_name_offset = b.CreateString("Default objective");
      FObjective.StartFObjective(b);
      FObjective.AddMaxEpisodeLength(b, description.MaxSteps);
      FObjective.AddSolvedThreshold(b, description.SolvedThreshold);
      FObjective.AddObjectiveName(b, objective_name_offset);
      return FObjective.EndFObjective(b);
    }

    static Offset<FTriple> serialise_position(FlatBufferBuilder b, PositionConfigurable observer) {
      var pos = observer.ObservationValue;
      FTriple.StartFTriple(b);
      FTriple.AddVec3(b, FVector3.CreateFVector3(b, pos.x, pos.y, pos.z));
      return FTriple.EndFTriple(b);
    }

    static Offset<FConfigurable> serialise_configurable(
        FlatBufferBuilder b,
        ConfigurableGameObject configurable,
        string identifier) {
      var n = b.CreateString(identifier);

      int observation_offset;
      FObservation observation_type;

      if (configurable is IHasQuaternionTransform) {
        observation_offset = serialise_quaternion_transform(b, (IHasQuaternionTransform)configurable).Value;
        observation_type = FObservation.FQT;
      } else if (configurable is PositionConfigurable) {
        observation_offset = serialise_position(b, (PositionConfigurable)configurable).Value;
        observation_type = FObservation.FTriple;
      } else if (configurable is IHasSingle) {
        observation_offset = serialise_single(b, (IHasSingle)configurable).Value;
        observation_type = FObservation.FSingle;
        // ReSharper disable once SuspiciousTypeConversion.Global
      } else if (configurable is IHasDouble) {
        // ReSharper disable once SuspiciousTypeConversion.Global
        observation_offset = serialise_double(b, (IHasDouble)configurable).Value;
        observation_type = FObservation.FDouble;
      } else if (configurable is EulerTransformConfigurable) {
        observation_offset = serialise_euler_transform(b, (IHasEulerTransform)configurable).Value;
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
