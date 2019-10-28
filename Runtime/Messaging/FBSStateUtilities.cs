using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.FBS;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Prototyping.Configurables.Transforms;
using FlatBuffers;
using UnityEngine;

namespace droid.Runtime.Messaging {
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
    /// <param name="do_serialise_observables"></param>
    /// <param name="api_version"></param>
    /// <returns></returns>
    public static byte[] Serialise(EnvironmentSnapshot[] states,
                                   SimulatorConfigurationMessage simulator_configuration = null,
                                   bool do_serialise_unobservables = false,
                                   bool serialise_individual_observables = false,
                                   bool do_serialise_observables = false,
                                   string api_version = "N/A") {
      var b = new FlatBufferBuilder(1);
      var state_offsets = new Offset<FState>[states.Length];
      var i = 0;
      foreach (var state in states) {
        state_offsets[i++] = SerialiseState(b,
                                            state,
                                            do_serialise_unobservables,
                                            do_serialise_observables,
                                            serialise_individual_observables :
                                            serialise_individual_observables);
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

    #endregion

    #region PrivateMethods

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    static Offset<FSimulatorConfiguration>
        Serialise(FlatBufferBuilder b, SimulatorConfigurationMessage configuration) {
      return FSimulatorConfiguration.CreateFSimulatorConfiguration(b,
                                                                   configuration.Width,
                                                                   configuration.Height,
                                                                   configuration.FullScreen,
                                                                   configuration.QualityLevel,
                                                                   configuration.TimeScale,
                                                                   configuration.TargetFrameRate,
                                                                   (FSimulationType)configuration
                                                                       .SimulationType,
                                                                   configuration.FrameSkips,
                                                                   0, //TODO: Remove
                                                                   configuration.NumOfEnvironments,
                                                                   configuration.DoSerialiseIndividualSensors,
                                                                   configuration.DoSerialiseUnobservables
                                                                   //TODO: ,configuration.DoSerialiseAggregatedFloatArray
                                                                  );
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="snapshot"></param>
    /// <param name="serialise_individual_observables"></param>
    /// <param name="do_serialise_unobservables"></param>
    /// <param name="do_serialise_aggregated_float_array"></param>
    /// <returns></returns>
    static Offset<FState> SerialiseState(FlatBufferBuilder b,
                                         EnvironmentSnapshot snapshot,
                                         bool do_serialise_unobservables = false,
                                         bool do_serialise_aggregated_float_array = false,
                                         bool serialise_individual_observables = false) {
      var n = b.CreateString(snapshot.EnvironmentName);

      var observables_vector = _null_vector_offset;
      if (do_serialise_aggregated_float_array) {
        observables_vector = FState.CreateObservablesVector(b, snapshot.Observables);
      }

      var unobservables = _null_unobservables_offset;
      if (do_serialise_unobservables) {
        var state_unobservables = snapshot.Unobservables;
        if (state_unobservables != null) {
          var bodies = state_unobservables.Bodies;

          FUnobservables.StartBodiesVector(b, bodies.Length);
          foreach (var rig in bodies) {
            var vel = rig.Velocity;
            var ang = rig.AngularVelocity;
            FBody.CreateFBody(b,
                              vel.x,
                              vel.y,
                              vel.z,
                              ang.x,
                              ang.y,
                              ang.z);
          }

          var bodies_vector = b.EndVector();

          var poses = state_unobservables.Poses;

          FUnobservables.StartPosesVector(b, poses.Length);
          foreach (var tra in poses) {
            var pos = tra.position;
            var rot = tra.rotation;
            FQuaternionTransform.CreateFQuaternionTransform(b,
                                                            pos.x,
                                                            pos.y,
                                                            pos.z,
                                                            rot.x,
                                                            rot.y,
                                                            rot.z,
                                                            rot.w);
          }

          var poses_vector = b.EndVector();

          FUnobservables.StartFUnobservables(b);
          FUnobservables.AddPoses(b, poses_vector);
          FUnobservables.AddBodies(b, bodies_vector);
          unobservables = FUnobservables.EndFUnobservables(b);
        }
      }

      var description_offset = new Offset<FEnvironmentDescription>();
      if (snapshot.Description != null) {
        description_offset = Serialise(b, snapshot);
      }

      var d = new StringOffset();
      if (snapshot.DebugMessage != "") {
        d = b.CreateString(snapshot.DebugMessage);
      }

      var t = b.CreateString(snapshot.TerminationReason);

      FState.StartFState(b);
      FState.AddEnvironmentName(b, n);

      FState.AddFrameNumber(b, snapshot.FrameNumber);
      if (do_serialise_aggregated_float_array) {
        FState.AddObservables(b, observables_vector);
      }

      if (do_serialise_unobservables) {
        FState.AddUnobservables(b, unobservables);
      }

      FState.AddSignal(b, snapshot.Signal);

      FState.AddTerminated(b, snapshot.Terminated);
      FState.AddTerminationReason(b, t);

      if (snapshot.Description != null) {
        FState.AddEnvironmentDescription(b, description_offset);
      }

      if (snapshot.DebugMessage != "") {
        FState.AddExtraSerialisedMessage(b, d);
      }

      return FState.EndFState(b);
    }

    static Offset<FActuator> Serialise(FlatBufferBuilder b, IActuator actuator, string identifier) {
      var n = b.CreateString(identifier);
      FActuator.StartFActuator(b);
      FActuator.AddActuatorName(b, n);
      FActuator.AddActuatorRange(b,
                                 FRange.CreateFRange(b,
                                                     actuator.MotionSpace.DecimalGranularity,
                                                     actuator.MotionSpace.Max,
                                                     actuator.MotionSpace.Min,
                                                     actuator.MotionSpace.NormalisedBool));
      return FActuator.EndFActuator(b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="sensor"></param>
    /// <returns></returns>
    static Offset<FETObs> Serialise(FlatBufferBuilder b, IHasEulerTransform sensor) {
      FETObs.StartFETObs(b);
      Vector3 pos = sensor.Position, rot = sensor.Rotation, dir = sensor.Direction;
      FETObs.AddTransform(b,
                          FEulerTransform.CreateFEulerTransform(b,
                                                                pos.x,
                                                                pos.y,
                                                                pos.z,
                                                                rot.x,
                                                                rot.y,
                                                                rot.z,
                                                                dir.x,
                                                                dir.y,
                                                                dir.z));

      return FETObs.EndFETObs(b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="sensor"></param>
    /// <returns></returns>
    static Offset<FQTObs> Serialise(FlatBufferBuilder b, IHasQuaternionTransform sensor) {
      var pos = sensor.Position;
      var rot = sensor.Rotation;
      var pos_range = sensor.PositionSpace;
      var rot_range = sensor.RotationSpace;
      FQTObs.StartFQTObs(b);
      FQTObs.AddPosRange(b,
                         FRange.CreateFRange(b,
                                             pos_range.DecimalGranularity,
                                             pos_range.Max,
                                             pos_range.Min,
                                             pos_range.NormalisedBool));
      FQTObs.AddRotRange(b,
                         FRange.CreateFRange(b,
                                             rot_range.DecimalGranularity,
                                             rot_range.Max,
                                             rot_range.Min,
                                             rot_range.NormalisedBool));
      FQTObs.AddTransform(b,
                          FQuaternionTransform.CreateFQuaternionTransform(b,
                                                                          pos.x,
                                                                          pos.y,
                                                                          pos.z,
                                                                          rot.x,
                                                                          rot.y,
                                                                          rot.z,
                                                                          rot.w));

      return FQTObs.EndFQTObs(b);
    }

    static Offset<FByteArray> Serialise(FlatBufferBuilder b, IHasByteArray sensor) {
      var v_offset = FByteArray.CreateBytesVectorBlock(b, sensor.Bytes);
      //var v_offset = CustomFlatBufferImplementation.CreateByteVector(b, camera.Bytes);
      FByteDataType a;
      switch (sensor.ArrayEncoding) {
        case "UINT8":
          a = FByteDataType.UINT8;
          break;
        case "FLOAT16":
          a = FByteDataType.FLOAT16;
          break;
        case "FLOAT32":
          a = FByteDataType.FLOAT32;
          break;
        case "JPEG":
          a = FByteDataType.JPEG;
          break;
        case "PNG":
          a = FByteDataType.PNG;
          break;
        default:
          a = FByteDataType.Other;
          break;
      }

      var c = FByteArray.CreateShapeVector(b, sensor.Shape);

      FByteArray.StartFByteArray(b);
      FByteArray.AddType(b, a);
      FByteArray.AddShape(b, c);
      FByteArray.AddBytes(b, v_offset);
      return FByteArray.EndFByteArray(b);
    }

    static Offset<FArray> Serialise(FlatBufferBuilder b, IHasFloatArray float_a) {
      var v_offset = FArray.CreateArrayVectorBlock(b, float_a.ObservationArray);
      //var v_offset = CustomFlatBufferImplementation.CreateFloatVector(b, float_a.ObservationArray);

      var ranges_vector = new VectorOffset();

      FArray.StartRangesVector(b, float_a.ObservationSpace.Length);
      foreach (var tra in float_a.ObservationSpace) {
        FRange.CreateFRange(b,
                            tra.DecimalGranularity,
                            tra.Max,
                            tra.Min,
                            tra.NormalisedBool);
      }

      ranges_vector = b.EndVector();

      FArray.StartFArray(b);
      FArray.AddArray(b, v_offset);

      FArray.AddRanges(b, ranges_vector);

      return FArray.EndFArray(b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="vel"></param>
    /// <param name="ang"></param>
    /// <returns></returns>
    static Offset<FRBObs> Serialise(FlatBufferBuilder b, IHasRigidbody rigidbody) {
      FRBObs.StartFRBObs(b);
      var a = rigidbody.Velocity;
      var c = rigidbody.AngularVelocity;

      FRBObs.AddBody(b,
                     FBody.CreateFBody(b,
                                       a.x,
                                       a.y,
                                       a.z,
                                       c.x,
                                       c.y,
                                       c.z));
      return FRBObs.EndFRBObs(b);
    }

    static Offset<FSingle> Serialise(FlatBufferBuilder b, IHasSingle numeral) {
      FSingle.StartFSingle(b);
      FSingle.AddValue(b, numeral.ObservationValue);

      var range_offset = FRange.CreateFRange(b,
                                             numeral.SingleSpace.DecimalGranularity,
                                             numeral.SingleSpace.Max,
                                             numeral.SingleSpace.Min,
                                             numeral.SingleSpace.NormalisedBool);
      FSingle.AddRange(b, range_offset);
      return FSingle.EndFSingle(b);
    }

    static Offset<FDouble> Serialise(FlatBufferBuilder b, IHasDouble numeral) {
      FDouble.StartFDouble(b);
      var vec2 = numeral.ObservationValue;

      var granularity = numeral.DoubleSpace.DecimalGranularity;
      var xs = numeral.DoubleSpace.Xspace;
      var ys = numeral.DoubleSpace.Yspace;

      FDouble.AddXRange(b,
                        FRange.CreateFRange(b,
                                            granularity,
                                            xs.Max,
                                            xs.Min,
                                            xs.NormalisedBool));
      FDouble.AddYRange(b,
                        FRange.CreateFRange(b,
                                            granularity,
                                            ys.Max,
                                            ys.Min,
                                            ys.NormalisedBool));
      FDouble.AddVec2(b, FVector2.CreateFVector2(b, vec2.x, vec2.y));

      return FDouble.EndFDouble(b);
    }

    static Offset<FTriple> Serialise(FlatBufferBuilder b, IHasTriple numeral) {
      FTriple.StartFTriple(b);
      var vec3 = numeral.ObservationValue;

      FTriple.AddVec3(b,
                      FVector3.CreateFVector3(b,
                                              vec3.x,
                                              vec3.y,
                                              vec3.z));
      var granularity = numeral.TripleSpace.DecimalGranularity;
      var xs = numeral.TripleSpace.Xspace;
      var ys = numeral.TripleSpace.Yspace;
      var zs = numeral.TripleSpace.Zspace;
      FTriple.AddXRange(b,
                        FRange.CreateFRange(b,
                                            granularity,
                                            xs.Max,
                                            xs.Min,
                                            xs.NormalisedBool));
      FTriple.AddYRange(b,
                        FRange.CreateFRange(b,
                                            granularity,
                                            ys.Max,
                                            ys.Min,
                                            ys.NormalisedBool));
      FTriple.AddZRange(b,
                        FRange.CreateFRange(b,
                                            granularity,
                                            zs.Max,
                                            zs.Min,
                                            zs.NormalisedBool));
      return FTriple.EndFTriple(b);
    }

    static Offset<FQuadruple> Serialise(FlatBufferBuilder b, IHasQuadruple numeral) {
      FQuadruple.StartFQuadruple(b);
      var quad = numeral.ObservationValue;
      FQuadruple.AddQuat(b,
                         FQuaternion.CreateFQuaternion(b,
                                                       quad.x,
                                                       quad.y,
                                                       quad.z,
                                                       quad.z));
      var granularity = numeral.QuadSpace.DecimalGranularity;
      var xs = numeral.QuadSpace.Xspace;
      var ys = numeral.QuadSpace.Yspace;
      var zs = numeral.QuadSpace.Zspace;
      var ws = numeral.QuadSpace.Wspace;
      FQuadruple.AddXRange(b,
                           FRange.CreateFRange(b,
                                               granularity,
                                               xs.Max,
                                               xs.Min,
                                               xs.NormalisedBool));
      FQuadruple.AddYRange(b,
                           FRange.CreateFRange(b,
                                               granularity,
                                               ys.Max,
                                               ys.Min,
                                               ys.NormalisedBool));
      FQuadruple.AddZRange(b,
                           FRange.CreateFRange(b,
                                               granularity,
                                               zs.Max,
                                               zs.Min,
                                               zs.NormalisedBool));
      FQuadruple.AddWRange(b,
                           FRange.CreateFRange(b,
                                               granularity,
                                               ws.Max,
                                               ws.Min,
                                               ws.NormalisedBool));
      return FQuadruple.EndFQuadruple(b);
    }

    static Offset<FString> Serialise(FlatBufferBuilder b, IHasString numeral) {
      var string_offset = b.CreateString(numeral.ObservationValue);
      FString.StartFString(b);
      FString.AddStr(b, string_offset);

      return FString.EndFString(b);
    }

    static Offset<FActor> Serialise(FlatBufferBuilder b,
                                    Offset<FActuator>[] actuators,
                                    IActor actor,
                                    string identifier) {
      var n = b.CreateString(identifier);
      var actuator_vector = FActor.CreateActuatorsVector(b, actuators);
      FActor.StartFActor(b);
      if (actor is KillableActor) {
        FActor.AddAlive(b, ((KillableActor)actor).IsAlive);
      } else {
        FActor.AddAlive(b, true);
      }

      FActor.AddActorName(b, n);
      FActor.AddActuators(b, actuator_vector);
      return FActor.EndFActor(b);
    }

    static Offset<FSensor> Serialise(FlatBufferBuilder b, string identifier, ISensor sensor) {
      var n = b.CreateString(identifier);

      int observation_offset;
      FObservation observation_type;
      switch (sensor) {
        case IHasString numeral:
          observation_offset = Serialise(b, numeral).Value;
          observation_type = FObservation.FString;
          break;
        case IHasFloatArray a:
          observation_offset = Serialise(b, a).Value;
          observation_type = FObservation.FArray;
          break;
        case IHasSingle single:
          observation_offset = Serialise(b, single).Value;
          observation_type = FObservation.FSingle;
          break;
        case IHasDouble has_double:
          observation_offset = Serialise(b, has_double).Value;
          observation_type = FObservation.FDouble;
          break;
        case IHasTriple triple:
          observation_offset = Serialise(b, triple).Value;
          observation_type = FObservation.FTriple;
          break;
        case IHasQuadruple quadruple:
          observation_offset = Serialise(b, quadruple).Value;
          observation_type = FObservation.FQuadruple;
          break;
        case IHasEulerTransform transform:
          observation_offset = Serialise(b, transform).Value;
          observation_type = FObservation.FETObs;
          break;
        case IHasQuaternionTransform quaternion_transform:
          observation_offset = Serialise(b, quaternion_transform).Value;
          observation_type = FObservation.FQTObs;
          break;
        case IHasRigidbody rigidbody:
          observation_offset = Serialise(b, rigidbody).Value;
          observation_type = FObservation.FRBObs;
          break;
        case IHasByteArray array:
          observation_offset = Serialise(b, array).Value;
          observation_type = FObservation.FByteArray;
          break;
        default:
          return FSensor.CreateFSensor(b, n);
      }

      FSensor.StartFSensor(b);
      FSensor.AddSensorName(b, n);
      FSensor.AddSensorValueType(b, observation_type);
      FSensor.AddSensorValue(b, observation_offset);
      return FSensor.EndFSensor(b);
    }

    static Offset<FEnvironmentDescription> Serialise(FlatBufferBuilder b, EnvironmentSnapshot snapshot) {
      var actors_offsets = new Offset<FActor>[snapshot.Description.Actors.Values.Count];
      var j = 0;
      foreach (var actor in snapshot.Description.Actors) {
        var actuators_offsets = new Offset<FActuator>[actor.Value.Actuators.Values.Count];
        var i = 0;
        foreach (var actuator in actor.Value.Actuators) {
          actuators_offsets[i++] = Serialise(b, actuator.Value, actuator.Key);
        }

        actors_offsets[j++] = Serialise(b,
                                        actuators_offsets,
                                        actor.Value,
                                        actor.Key);
      }

      var actors_vector_offset = FEnvironmentDescription.CreateActorsVector(b, actors_offsets);

      var configurables_offsets = new Offset<FConfigurable>[snapshot.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in snapshot.Description.Configurables) {
        configurables_offsets[k++] = Serialise(b, configurable.Value, configurable.Key);
      }

      var configurables_vector_offset =
          FEnvironmentDescription.CreateConfigurablesVector(b, configurables_offsets);

      var objective_offset = Serialise(b, snapshot.Description);

      var sensors = new Offset<FSensor>[snapshot.Description.Sensors.Values.Count];
      var js = 0;
      foreach (var sensor in snapshot.Description.Sensors) {
        sensors[js++] = Serialise(b, sensor.Key, sensor.Value);
      }

      var sensors_vector = FEnvironmentDescription.CreateSensorsVector(b, sensors);

      FEnvironmentDescription.StartFEnvironmentDescription(b);

      FEnvironmentDescription.AddObjective(b, objective_offset);
      FEnvironmentDescription.AddActors(b, actors_vector_offset);
      FEnvironmentDescription.AddConfigurables(b, configurables_vector_offset);
      FEnvironmentDescription.AddSensors(b, sensors_vector);

      return FEnvironmentDescription.EndFEnvironmentDescription(b);
    }

    static Offset<FObjective> Serialise(FlatBufferBuilder b, EnvironmentDescription description) {
      var ob_name = "None";
      var ep_len = -1;
      var a = 0;
      var f = 0f;
      var c = 0f;
      var d = false;
      if (description.ObjectiveFunction != null) {
        ob_name = description.ObjectiveFunction.Identifier;
        ep_len = description.ObjectiveFunction.EpisodeLength;
        f = description.ObjectiveFunction.SignalSpace.Min;
        c = description.ObjectiveFunction.SignalSpace.Max;
        a = description.ObjectiveFunction.SignalSpace.DecimalGranularity;
        d = description.ObjectiveFunction.SignalSpace.NormalisedBool;
      }

      var objective_name_offset = b.CreateString(ob_name);
      FObjective.StartFObjective(b);
      FObjective.AddMaxEpisodeLength(b, ep_len);
      FObjective.AddSignalSpace(b,
                                FRange.CreateFRange(b,
                                                    a,
                                                    f,
                                                    c,
                                                    d));
      FObjective.AddObjectiveName(b, objective_name_offset);
      return FObjective.EndFObjective(b);
    }

    static Offset<FTriple> Serialise(FlatBufferBuilder b, PositionConfigurable sensor) {
      var pos = sensor.ObservationValue;
      FTriple.StartFTriple(b);
      FTriple.AddVec3(b,
                      FVector3.CreateFVector3(b,
                                              pos.x,
                                              pos.y,
                                              pos.z));
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
        observation_type = FObservation.FQTObs;
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
        observation_type = FObservation.FETObs;
      } else {
        FConfigurable.StartFConfigurable(b);
        FConfigurable.AddConfigurableName(b, n);
        return FConfigurable.EndFConfigurable(b);
      }

      FConfigurable.StartFConfigurable(b);
      FConfigurable.AddConfigurableName(b, n);
      FConfigurable.AddConfigurableValue(b, observation_offset);
      FConfigurable.AddConfigurableValueType(b, observation_type);
      FConfigurable.AddConfigurableRange(b,
                                         FRange.CreateFRange(b,
                                                             0,
                                                             0,
                                                             0,
                                                             false));
      return FConfigurable.EndFConfigurable(b);
    }

    #endregion
  }
}
