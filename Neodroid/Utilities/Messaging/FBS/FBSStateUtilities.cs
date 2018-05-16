using FlatBuffers;
using Neodroid.Prototyping.Actors;
using Neodroid.Prototyping.Configurables;
using Neodroid.Prototyping.Motors;
using Neodroid.Prototyping.Observers;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Utilities.Messaging.FBS {
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
    public static byte[] serialise_states(Messages.EnvironmentState[] states,  ScriptableObjects.SimulatorConfiguration simulator_configuration=null, string api_version="") {
      var b = new FlatBufferBuilder(1);
      var state_offsets = new Offset<Neodroid.FBS.State.FState>[states.Length];
      var i = 0;
      foreach (var state in states) {
          state_offsets[i++] = serialise_state(b, state);
      }
      var states_vector_offset = Neodroid.FBS.State.FStates.CreateStatesVector(b,state_offsets);

      var api_version_offset = b.CreateString(api_version);

      Neodroid.FBS.State.FStates.StartFStates(b);
      Neodroid.FBS.State.FStates.AddStates(b, states_vector_offset);
      Neodroid.FBS.State.FStates.AddApiVersion(b, api_version_offset);
      //TODO: Neodroid.FBS.State.FStates.AddSimulatorConfiguration();
      var states_offset = Neodroid.FBS.State.FStates.EndFStates(b);

      Neodroid.FBS.State.FStates.FinishFStatesBuffer(b, states_offset);

      return b.SizedByteArray();
    }

    public static Offset<Neodroid.FBS.FSimulatorConfiguration> serialise_simulator_configuration(
        FlatBufferBuilder b,
        ScriptableObjects.SimulatorConfiguration configuration) {



      return Neodroid.FBS.FSimulatorConfiguration.CreateFSimulatorConfiguration(b,configuration.Width,configuration.Height,configuration.FullScreen,configuration.QualityLevel,configuration.TimeScale,configuration.TargetFrameRate,(int)configuration.SimulationType,configuration.FrameSkips,configuration.ResetIterations,configuration.NumOfEnvironments);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public static Offset<Neodroid.FBS.State.FState> serialise_state(FlatBufferBuilder b, Messages.EnvironmentState state) {
        var n = b.CreateString(state.EnvironmentName);

        var observables_vector = Neodroid.FBS.State.FState.CreateObservablesVector(b, state.Observables);

        var observers = new Offset<Neodroid.FBS.State.FOBS>[state.Observations.Values.Count];
        var k = 0;
        foreach (var observer in state.Observations.Values) {
          observers[k++] = serialise_observer(b, observer);
        }

        var observers_vector = Neodroid.FBS.State.FState.CreateObservationsVector(b, observers);

        Neodroid.FBS.FUnobservables.StartBodiesVector(b, state.Unobservables.Bodies.Length);
        foreach (var rig in state.Unobservables.Bodies) {
          var vel = rig.Velocity;
          var ang = rig.AngularVelocity;
          Neodroid.FBS.FBody.CreateFBody(b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z);
        }

        var bodies_vector = b.EndVector();

        Neodroid.FBS.FUnobservables.StartPosesVector(b, state.Unobservables.Poses.Length);
        foreach (var tra in state.Unobservables.Poses) {
          var pos = tra.position;
          var rot = tra.rotation;
          Neodroid.FBS.FQuaternionTransform.CreateFQuaternionTransform(b, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w);
        }

        var poses_vector = b.EndVector();

        Neodroid.FBS.FUnobservables.StartFUnobservables(b);
        Neodroid.FBS.FUnobservables.AddPoses(b, poses_vector);
        Neodroid.FBS.FUnobservables.AddBodies(b, bodies_vector);
        var unobservables = Neodroid.FBS.FUnobservables.EndFUnobservables(b);

        var description_offset = new Offset<Neodroid.FBS.State.FEnvironmentDescription>();
        if (state.Description != null) {
          description_offset = serialise_description(b, state);
        }

        var d = new StringOffset();
        if (state.DebugMessage != "") {
          d = b.CreateString(state.DebugMessage);
        }

        var t = b.CreateString(state.TerminationReason);

        Neodroid.FBS.State.FState.StartFState(b);
        Neodroid.FBS.State.FState.AddEnvironmentName(b, n);

        Neodroid.FBS.State.FState.AddFrameNumber(b, state.FrameNumber);
        Neodroid.FBS.State.FState.AddObservables(b, observables_vector);
        Neodroid.FBS.State.FState.AddUnobservables(b, unobservables);

        Neodroid.FBS.State.FState.AddTotalEnergySpent(b, state.TotalEnergySpentSinceReset);
        Neodroid.FBS.State.FState.AddSignal(b, state.Signal);

        Neodroid.FBS.State.FState.AddTerminated(b, state.Terminated);
        Neodroid.FBS.State.FState.AddTerminationReason(b, t);

        Neodroid.FBS.State.FState.AddObservations(b, observers_vector);
        if (state.Description != null) {
          Neodroid.FBS.State.FState.AddEnvironmentDescription(b, description_offset);
        }

        if (state.DebugMessage != "") {
          Neodroid.FBS.State.FState.AddSerialisedMessage(b, d);
        }

        return Neodroid.FBS.State.FState.EndFState(b);
    }


    #endregion

    #region PrivateMethods

    static Offset<Neodroid.FBS.State.FMotor> serialise_motor(FlatBufferBuilder b, Motor motor, string identifier) {
      var n = b.CreateString(identifier);
      Neodroid.FBS.State.FMotor.StartFMotor(b);
      Neodroid.FBS.State.FMotor.AddMotorName(b, n);
      Neodroid.FBS.State.FMotor.AddValidInput(
          b,
          Neodroid.FBS.FRange.CreateFRange(
              b,
              motor.MotionValueSpace._Decimal_Granularity,
              motor.MotionValueSpace._Max_Value,
              motor.MotionValueSpace._Min_Value));
      Neodroid.FBS.State.FMotor.AddEnergySpentSinceReset(b, motor.GetEnergySpend());
      return Neodroid.FBS.State.FMotor.EndFMotor(b);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="observer"></param>
    /// <returns></returns>
    static Offset<Neodroid.FBS.FEulerTransform> serialise_euler_transform(FlatBufferBuilder b, IHasEulerTransform observer) {
      Vector3 pos = observer.Position, rot = observer.Rotation, dir = observer.Direction;
      return Neodroid.FBS.FEulerTransform.CreateFEulerTransform(
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
    static Offset<Neodroid.FBS.FQT> serialise_quaternion_transform(FlatBufferBuilder b, IHasQuaternionTransform observer) {
      var pos = observer.Position;
      var rot = observer.Rotation;
      Neodroid.FBS.FQT.StartFQT(b);
      Neodroid.FBS.FQT.AddTransform(
          b,
          Neodroid.FBS.FQuaternionTransform.CreateFQuaternionTransform(
              b,
              pos.x,
              pos.y,
              pos.z,
              rot.x,
              rot.y,
              rot.z,
              rot.w));
      return Neodroid.FBS.FQT.EndFQT(b);
    }

    static Offset<Neodroid.FBS.FByteArray> serialise_byte_array(FlatBufferBuilder b, IHasByteArray camera) {
      //var v_offset = FByteArray.CreateBytesVector(b, camera.Bytes);
      var v_offset = CustomFlatBufferImplementation.CreateByteVector(b, camera.Bytes);
      Neodroid.FBS.FByteArray.StartFByteArray(b);
      Neodroid.FBS.FByteArray.AddType(b, Neodroid.FBS.FByteDataType.PNG);
      Neodroid.FBS.FByteArray.AddBytes(b, v_offset);
      return Neodroid.FBS.FByteArray.EndFByteArray(b);
    }

    static Offset<Neodroid.FBS.FArray> serialise_array(FlatBufferBuilder b, IHasArray float_a) {
      //var v_offset = FArray.CreateArrayVector(b, camera.ObservationArray);
      var v_offset = CustomFlatBufferImplementation.CreateFloatVector(b, float_a.ObservationArray);
      //FArray.StartRangesVector(b,);
      Neodroid.FBS.FArray.StartFArray(b);
      Neodroid.FBS.FArray.AddArray(b, v_offset);
      //FArray.AddRanges(b,);
      return Neodroid.FBS.FArray.EndFArray(b);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="b"></param>
    /// <param name="vel"></param>
    /// <param name="ang"></param>
    /// <returns></returns>
    static Offset<Neodroid.FBS.FRB> serialise_body_observation(FlatBufferBuilder b, Vector3 vel, Vector3 ang) {
      Neodroid.FBS.FRB.StartFRB(b);
      Neodroid.FBS.FRB.AddBody(b, Neodroid.FBS.FBody.CreateFBody(b, vel.x, vel.y, vel.z, ang.x, ang.y, ang.z));
      return Neodroid.FBS.FRB.EndFRB(b);
    }

    static Offset<Neodroid.FBS.FSingle> serialise_single(FlatBufferBuilder b, IHasSingle numeral) {
      Neodroid.FBS.FSingle.StartFSingle(b);
      Neodroid.FBS.FSingle.AddValue(b, numeral.ObservationValue);

      var range_offset = Neodroid.FBS.FRange.CreateFRange(
          b,
          numeral.SingleSpace._Decimal_Granularity,
          numeral.SingleSpace._Max_Value,
          numeral.SingleSpace._Min_Value);
      Neodroid.FBS.FSingle.AddRange(b, range_offset);
      return Neodroid.FBS.FSingle.EndFSingle(b);
    }

    static Offset<Neodroid.FBS.FDouble> serialise_double(FlatBufferBuilder b, IHasDouble numeral) {
      Neodroid.FBS.FDouble.StartFDouble(b);
      var vec2 = numeral.ObservationValue;
      Neodroid.FBS.FDouble.AddVec2(b, Neodroid.FBS.FVector2.CreateFVector2(b, vec2.x, vec2.y));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return Neodroid.FBS.FDouble.EndFDouble(b);
    }

    static Offset<Neodroid.FBS.FTriple> serialise_triple(FlatBufferBuilder b, IHasTriple numeral) {
      Neodroid.FBS.FTriple.StartFTriple(b);
      var vec3 = numeral.ObservationValue;
      Neodroid.FBS.FTriple.AddVec3(b, Neodroid.FBS.FVector3.CreateFVector3(b, vec3.x, vec3.y, vec3.z));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return Neodroid.FBS.FTriple.EndFTriple(b);
    }

    static Offset<Neodroid.FBS.FQuadruple> serialise_quadruple(FlatBufferBuilder b, IHasQuadruple numeral) {
      Neodroid.FBS.FQuadruple.StartFQuadruple(b);
      var quad = numeral.ObservationValue;
      Neodroid.FBS.FQuadruple.AddQuat(b, Neodroid.FBS.FQuaternion.CreateFQuaternion(b, quad.x, quad.y, quad.z, quad.z));
      //FSingle.AddRange(b, numeral.ObservationValue);
      return Neodroid.FBS.FQuadruple.EndFQuadruple(b);
    }

    static Offset<Neodroid.FBS.State.FActor> serialise_actor(
        FlatBufferBuilder b,
        Offset<Neodroid.FBS.State.FMotor>[] motors,
        Actor actor,
        string identifier) {
      var n = b.CreateString(identifier);
      var motor_vector = Neodroid.FBS.State.FActor.CreateMotorsVector(b, motors);
      Neodroid.FBS.State.FActor.StartFActor(b);
      if (actor is KillableActor) {
        Neodroid.FBS.State.FActor.AddAlive(b, ((KillableActor)actor).IsAlive);
      } else {
        Neodroid.FBS.State.FActor.AddAlive(b, true);
      }

      Neodroid.FBS.State.FActor.AddActorName(b, n);
      Neodroid.FBS.State.FActor.AddMotors(b, motor_vector);
      return Neodroid.FBS.State.FActor.EndFActor(b);
    }

    static Offset<Neodroid.FBS.State.FOBS> serialise_observer(FlatBufferBuilder b, Observer observer) {
      var n = b.CreateString(observer.Identifier);

      int observation_offset;
      Neodroid.FBS.State.FObservation observation_type;

      if (observer is IHasArray) {
        observation_offset = serialise_array(b, (IHasArray)observer).Value;
        observation_type = Neodroid.FBS.State.FObservation.FArray;
      } else if (observer is IHasSingle) {
        observation_offset = serialise_single(b, (IHasSingle)observer).Value;
        observation_type = Neodroid.FBS.State.FObservation.FSingle;
      } else if (observer is IHasDouble) {
        observation_offset = serialise_double(b, (IHasDouble)observer).Value;
        observation_type = Neodroid.FBS.State.FObservation.FDouble;
      } else if (observer is IHasTriple) {
        observation_offset = serialise_triple(b, (IHasTriple)observer).Value;
        observation_type = Neodroid.FBS.State.FObservation.FTriple;
      } else if (observer is IHasQuadruple) {
        observation_offset = serialise_quadruple(b, (IHasQuadruple)observer).Value;
        observation_type = Neodroid.FBS.State.FObservation.FQuadruple;
      } else if (observer is IHasEulerTransform) {
        observation_offset = serialise_euler_transform(b, (IHasEulerTransform)observer).Value;
        observation_type = Neodroid.FBS.State.FObservation.FET;
      } else if (observer is IHasQuaternionTransform) {
        observation_offset = serialise_quaternion_transform(b, (IHasQuaternionTransform)observer).Value;
        observation_type = Neodroid.FBS.State.FObservation.FQT;
      } else if (observer is IHasRigidbody) {
        observation_offset = serialise_body_observation(
            b,
            ((IHasRigidbody)observer).Velocity,
            ((IHasRigidbody)observer).AngularVelocity).Value;
        observation_type = Neodroid.FBS.State.FObservation.FRB;
      } else if (observer is IHasByteArray) {
        observation_offset = serialise_byte_array(b, (IHasByteArray)observer).Value;
        observation_type = Neodroid.FBS.State.FObservation.FByteArray;
      } else {
        return Neodroid.FBS.State.FOBS.CreateFOBS(b, n);
      }

      Neodroid.FBS.State.FOBS.StartFOBS(b);
      Neodroid.FBS.State.FOBS.AddObservationName(b, n);
      Neodroid.FBS.State.FOBS.AddObservationType(b, observation_type);
      Neodroid.FBS.State.FOBS.AddObservation(b, observation_offset);
      return Neodroid.FBS.State.FOBS.EndFOBS(b);
    }

    static Offset<Neodroid.FBS.State.FEnvironmentDescription> serialise_description(
        FlatBufferBuilder b,
        Messages.EnvironmentState state) {
      var actors_offsets = new Offset<Neodroid.FBS.State.FActor>[state.Description.Actors.Values.Count];
      var j = 0;
      foreach (var actor in state.Description.Actors) {
        var motors_offsets = new Offset<Neodroid.FBS.State.FMotor>[actor.Value.Motors.Values.Count];
        var i = 0;
        foreach (var motor in actor.Value.Motors) {
          motors_offsets[i++] = serialise_motor(b, motor.Value, motor.Key);
        }

        actors_offsets[j++] = serialise_actor(b, motors_offsets, actor.Value, actor.Key);
      }

      var actors_vector_offset = Neodroid.FBS.State.FEnvironmentDescription.CreateActorsVector(b, actors_offsets);

      var configurables_offsets = new Offset<Neodroid.FBS.State.FConfigurable>[state.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in state.Description.Configurables) {
        configurables_offsets[k++] = serialise_configurable(b, configurable.Value, configurable.Key);
      }

      var configurables_vector_offset = Neodroid.FBS.State.FEnvironmentDescription.CreateConfigurablesVector(b, configurables_offsets);


      var objective_offset = serialise_objective(b, state.Description);

      Neodroid.FBS.State.FEnvironmentDescription.StartFEnvironmentDescription(b);
      Neodroid.FBS.State.FEnvironmentDescription.AddObjective(b, objective_offset);

      Neodroid.FBS.State.FEnvironmentDescription.AddActors(b, actors_vector_offset);
      Neodroid.FBS.State.FEnvironmentDescription.AddConfigurables(b, configurables_vector_offset);

      return Neodroid.FBS.State.FEnvironmentDescription.EndFEnvironmentDescription(b);
    }

    static Offset<Neodroid.FBS.State.FObjective> serialise_objective(
        FlatBufferBuilder b,
        Messages.EnvironmentDescription description) {
      var objective_name_offset = b.CreateString("Default objective");
      Neodroid.FBS.State.FObjective.StartFObjective(b);
      Neodroid.FBS.State.FObjective.AddMaxEpisodeLength(b, description.MaxSteps);
      Neodroid.FBS.State.FObjective.AddSolvedThreshold(b, description.SolvedThreshold);
      Neodroid.FBS.State.FObjective.AddObjectiveName(b, objective_name_offset);
      return Neodroid.FBS.State.FObjective.EndFObjective(b);
    }

    static Offset<Neodroid.FBS.FTriple> serialise_position(FlatBufferBuilder b, PositionConfigurable observer) {
      var pos = observer.ObservationValue;
      Neodroid.FBS.FTriple.StartFTriple(b);
      Neodroid.FBS.FTriple.AddVec3(b, Neodroid.FBS.FVector3.CreateFVector3(b, pos.x, pos.y, pos.z));
      return Neodroid.FBS.FTriple.EndFTriple(b);
    }

    static Offset<Neodroid.FBS.State.FConfigurable> serialise_configurable(
        FlatBufferBuilder b,
        ConfigurableGameObject configurable,
        string identifier) {
      var n = b.CreateString(identifier);

      int observation_offset;
      Neodroid.FBS.State.FObservation observation_type;

      if (configurable is IHasQuaternionTransform) {
        observation_offset = serialise_quaternion_transform(b, (IHasQuaternionTransform)configurable).Value;
        observation_type = Neodroid.FBS.State.FObservation.FQT;
      } else if (configurable is PositionConfigurable) {
        observation_offset = serialise_position(b, (PositionConfigurable)configurable).Value;
        observation_type = Neodroid.FBS.State.FObservation.FTriple;
      } else if (configurable is IHasSingle) {
        observation_offset = serialise_single(b, (IHasSingle)configurable).Value;
        observation_type = Neodroid.FBS.State.FObservation.FSingle;
        // ReSharper disable once SuspiciousTypeConversion.Global
      } else if (configurable is IHasDouble) {
        // ReSharper disable once SuspiciousTypeConversion.Global
        observation_offset = serialise_double(b, (IHasDouble)configurable).Value;
        observation_type = Neodroid.FBS.State.FObservation.FDouble;
      } else if (configurable is EulerTransformConfigurable) {
        observation_offset = serialise_euler_transform(b, (IHasEulerTransform)configurable).Value;
        observation_type = Neodroid.FBS.State.FObservation.FET;
      } else {
        Neodroid.FBS.State.FConfigurable.StartFConfigurable(b);
        Neodroid.FBS.State.FConfigurable.AddConfigurableName(b, n);
        return Neodroid.FBS.State.FConfigurable.EndFConfigurable(b);
      }

      Neodroid.FBS.State.FConfigurable.StartFConfigurable(b);
      Neodroid.FBS.State.FConfigurable.AddConfigurableName(b, n);
      Neodroid.FBS.State.FConfigurable.AddObservation(b, observation_offset);
      Neodroid.FBS.State.FConfigurable.AddObservationType(b, observation_type);
      return Neodroid.FBS.State.FConfigurable.EndFConfigurable(b);
    }

    #endregion
  }
}
