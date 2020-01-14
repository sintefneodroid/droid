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
      for (var index = 0; index < states.Length; index++) {
        var state = states[index];
        state_offsets[i++] = SerialiseState(b : b,
                                            snapshot : state,
                                            do_serialise_unobservables : do_serialise_unobservables,
                                            do_serialise_aggregated_float_array : do_serialise_observables,
                                            serialise_individual_observables :
                                            serialise_individual_observables);
      }

      var states_vector_offset = FStates.CreateStatesVector(b, data : state_offsets);

      var api_version_offset = b.CreateString(s : api_version);

      FStates.StartFStates(b);
      FStates.AddStates(b, statesOffset : states_vector_offset);
      FStates.AddApiVersion(b, apiVersionOffset : api_version_offset);
      FStates.AddSimulatorConfiguration(b, Serialise(b : b, configuration : simulator_configuration));
      var states_offset = FStates.EndFStates(b);

      FStates.FinishFStatesBuffer(b, offset : states_offset);

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
                                                                   Width : configuration.Width,
                                                                   Height : configuration.Height,
                                                                   FullScreen : configuration.FullScreen,
                                                                   QualityLevel : configuration.QualityLevel,
                                                                   TimeScale : configuration.TimeScale,
                                                                   TargetFrameRate : configuration.TargetFrameRate,
                                                                   (FSimulationType)configuration
                                                                       .SimulationType,
                                                                   FrameSkips : configuration.FrameSkips,
                                                                   0, //TODO: Remove
                                                                   NumOfEnvironments : configuration.NumOfEnvironments,
                                                                   DoSerialiseIndividualSensors : configuration.DoSerialiseIndividualSensors,
                                                                   DoSerialiseUnobservables : configuration.DoSerialiseUnobservables
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
      var n = b.CreateString(s : snapshot.EnvironmentName);

      var observables_vector = _null_vector_offset;
      if (do_serialise_aggregated_float_array) {
        observables_vector = FState.CreateObservablesVector(b, data : snapshot.Observables);
      }

      var unobservables = _null_unobservables_offset;
      if (do_serialise_unobservables) {
        var state_unobservables = snapshot.Unobservables;
        if (state_unobservables != null) {
          var bodies = state_unobservables.Bodies;

          FUnobservables.StartBodiesVector(b, numElems : bodies.Length);
          for (var index = 0; index < bodies.Length; index++) {
            var rig = bodies[index];
            var vel = rig.Velocity;
            var ang = rig.AngularVelocity;
            FBody.CreateFBody(b,
                              velocity_X : vel.x,
                              velocity_Y : vel.y,
                              velocity_Z : vel.z,
                              angular_velocity_X : ang.x,
                              angular_velocity_Y : ang.y,
                              angular_velocity_Z : ang.z);
          }

          var bodies_vector = b.EndVector();

          var poses = state_unobservables.Poses;

          FUnobservables.StartPosesVector(b, numElems : poses.Length);
          for (var index = 0; index < poses.Length; index++) {
            var tra = poses[index];
            var pos = tra.position;
            var rot = tra.rotation;
            FQuaternionTransform.CreateFQuaternionTransform(b,
                                                            position_X : pos.x,
                                                            position_Y : pos.y,
                                                            position_Z : pos.z,
                                                            rotation_X : rot.x,
                                                            rotation_Y : rot.y,
                                                            rotation_Z : rot.z,
                                                            rotation_W : rot.w);
          }

          var poses_vector = b.EndVector();

          FUnobservables.StartFUnobservables(b);
          FUnobservables.AddPoses(b, posesOffset : poses_vector);
          FUnobservables.AddBodies(b, bodiesOffset : bodies_vector);
          unobservables = FUnobservables.EndFUnobservables(b);
        }
      }

      var description_offset = new Offset<FEnvironmentDescription>();
      if (snapshot.Description != null) {
        description_offset = Serialise(b : b, snapshot : snapshot);
      }

      var d = new StringOffset();
      if (snapshot.DebugMessage != "") {
        d = b.CreateString(s : snapshot.DebugMessage);
      }

      var t = b.CreateString(s : snapshot.TerminationReason);

      FState.StartFState(b);
      FState.AddEnvironmentName(b, environmentNameOffset : n);

      FState.AddFrameNumber(b, frameNumber : snapshot.FrameNumber);
      if (do_serialise_aggregated_float_array) {
        FState.AddObservables(b, observablesOffset : observables_vector);
      }

      if (do_serialise_unobservables) {
        FState.AddUnobservables(b, unobservablesOffset : unobservables);
      }

      FState.AddSignal(b, signal : snapshot.Signal);

      FState.AddTerminated(b, terminated : snapshot.Terminated);
      FState.AddTerminationReason(b, terminationReasonOffset : t);

      if (snapshot.Description != null) {
        FState.AddEnvironmentDescription(b, environmentDescriptionOffset : description_offset);
      }

      if (snapshot.DebugMessage != "") {
        FState.AddExtraSerialisedMessage(b, extraSerialisedMessageOffset : d);
      }

      return FState.EndFState(b);
    }

    static Offset<FActuator> Serialise(FlatBufferBuilder b, IActuator actuator, string identifier) {
      var n = b.CreateString(s : identifier);
      FActuator.StartFActuator(b);
      FActuator.AddActuatorName(b, actuatorNameOffset : n);
      FActuator.AddActuatorRange( b,
                                 FRange.CreateFRange(b,
                                                     DecimalGranularity : actuator.MotionSpace.DecimalGranularity,
                                                     MaxValue : actuator.MotionSpace.Max,
                                                     MinValue : actuator.MotionSpace.Min,
                                                     Normalised : actuator.MotionSpace.NormalisedBool));
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
                                                                position_X : pos.x,
                                                                position_Y : pos.y,
                                                                position_Z : pos.z,
                                                                rotation_X : rot.x,
                                                                rotation_Y : rot.y,
                                                                rotation_Z : rot.z,
                                                                direction_X : dir.x,
                                                                direction_Y : dir.y,
                                                                direction_Z : dir.z));

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
                                             DecimalGranularity : pos_range.DecimalGranularity,
                                             MaxValue : pos_range.Max,
                                             MinValue : pos_range.Min,
                                             Normalised : pos_range.NormalisedBool));
      FQTObs.AddRotRange(b,
                         FRange.CreateFRange(b,
                                             DecimalGranularity : rot_range.DecimalGranularity,
                                             MaxValue : rot_range.Max,
                                             MinValue : rot_range.Min,
                                             Normalised : rot_range.NormalisedBool));
      FQTObs.AddTransform(b,
                          FQuaternionTransform.CreateFQuaternionTransform(b,
                                                                          position_X : pos.x,
                                                                          position_Y : pos.y,
                                                                          position_Z : pos.z,
                                                                          rotation_X : rot.x,
                                                                          rotation_Y : rot.y,
                                                                          rotation_Z : rot.z,
                                                                          rotation_W : rot.w));

      return FQTObs.EndFQTObs(b);
    }

    static Offset<FByteArray> Serialise(FlatBufferBuilder b, IHasByteArray sensor) {
      var v_offset = FByteArray.CreateBytesVectorBlock(b, data : sensor.Bytes);
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

      var c = FByteArray.CreateShapeVector(b, data : sensor.Shape);

      FByteArray.StartFByteArray(b);
      FByteArray.AddType(b, type : a);
      FByteArray.AddShape(b, shapeOffset : c);
      FByteArray.AddBytes(b, bytesOffset : v_offset);
      return FByteArray.EndFByteArray(b);
    }

    static Offset<FArray> Serialise(FlatBufferBuilder b, IHasFloatArray float_a) {
      var v_offset = FArray.CreateArrayVectorBlock(b, data : float_a.ObservationArray);
      //var v_offset = CustomFlatBufferImplementation.CreateFloatVector(b, float_a.ObservationArray);

      FArray.StartRangesVector(b, numElems : float_a.ObservationSpace.Length);
      for (var index = 0; index < float_a.ObservationSpace.Length; index++) {
        var tra = float_a.ObservationSpace[index];
        FRange.CreateFRange(b,
                            DecimalGranularity : tra.DecimalGranularity,
                            MaxValue : tra.Max,
                            MinValue : tra.Min,
                            Normalised : tra.NormalisedBool);
      }

      var ranges_vector = b.EndVector();

      FArray.StartFArray(b);
      FArray.AddArray(b, arrayOffset : v_offset);

      FArray.AddRanges(b, rangesOffset : ranges_vector);

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
                                       velocity_X : a.x,
                                       velocity_Y : a.y,
                                       velocity_Z : a.z,
                                       angular_velocity_X : c.x,
                                       angular_velocity_Y : c.y,
                                       angular_velocity_Z : c.z));
      return FRBObs.EndFRBObs(b);
    }

    static Offset<FSingle> Serialise(FlatBufferBuilder b, IHasSingle numeral) {
      FSingle.StartFSingle(b);
      FSingle.AddValue(b, value : numeral.ObservationValue);

      var range_offset = FRange.CreateFRange(b,
                                             DecimalGranularity : numeral.SingleSpace.DecimalGranularity,
                                             MaxValue : numeral.SingleSpace.Max,
                                             MinValue : numeral.SingleSpace.Min,
                                             Normalised : numeral.SingleSpace.NormalisedBool);
      FSingle.AddRange(b, rangeOffset : range_offset);
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
                                            DecimalGranularity : granularity,
                                            MaxValue : xs.Max,
                                            MinValue : xs.Min,
                                            Normalised : xs.NormalisedBool));
      FDouble.AddYRange(b,
                        FRange.CreateFRange(b,
                                            DecimalGranularity : granularity,
                                            MaxValue : ys.Max,
                                            MinValue : ys.Min,
                                            Normalised : ys.NormalisedBool));
      FDouble.AddVec2(b, FVector2.CreateFVector2(b, X : vec2.x, Y : vec2.y));

      return FDouble.EndFDouble(b);
    }

    static Offset<FTriple> Serialise(FlatBufferBuilder b, IHasTriple numeral) {
      FTriple.StartFTriple(b);
      var vec3 = numeral.ObservationValue;

      FTriple.AddVec3(b,
                      FVector3.CreateFVector3(b,
                                              X : vec3.x,
                                              Y : vec3.y,
                                              Z : vec3.z));
      var granularity = numeral.TripleSpace.DecimalGranularity;
      var xs = numeral.TripleSpace.Xspace;
      var ys = numeral.TripleSpace.Yspace;
      var zs = numeral.TripleSpace.Zspace;
      FTriple.AddXRange(b,
                        FRange.CreateFRange(b,
                                            DecimalGranularity : granularity,
                                            MaxValue : xs.Max,
                                            MinValue : xs.Min,
                                            Normalised : xs.NormalisedBool));
      FTriple.AddYRange(b,
                        FRange.CreateFRange(b,
                                            DecimalGranularity : granularity,
                                            MaxValue : ys.Max,
                                            MinValue : ys.Min,
                                            Normalised : ys.NormalisedBool));
      FTriple.AddZRange(b,
                        FRange.CreateFRange(b,
                                            DecimalGranularity : granularity,
                                            MaxValue : zs.Max,
                                            MinValue : zs.Min,
                                            Normalised : zs.NormalisedBool));
      return FTriple.EndFTriple(b);
    }

    static Offset<FQuadruple> Serialise(FlatBufferBuilder b, IHasQuadruple numeral) {
      FQuadruple.StartFQuadruple(b);
      var quad = numeral.ObservationValue;
      FQuadruple.AddQuat(b,
                         FQuaternion.CreateFQuaternion(b,
                                                       X : quad.x,
                                                       Y : quad.y,
                                                       Z : quad.z,
                                                       W : quad.z));
      var granularity = numeral.QuadSpace.DecimalGranularity;
      var xs = numeral.QuadSpace.Xspace;
      var ys = numeral.QuadSpace.Yspace;
      var zs = numeral.QuadSpace.Zspace;
      var ws = numeral.QuadSpace.Wspace;
      FQuadruple.AddXRange(b,
                           FRange.CreateFRange(b,
                                               DecimalGranularity : granularity,
                                               MaxValue : xs.Max,
                                               MinValue : xs.Min,
                                               Normalised : xs.NormalisedBool));
      FQuadruple.AddYRange(b,
                           FRange.CreateFRange(b,
                                               DecimalGranularity : granularity,
                                               MaxValue : ys.Max,
                                               MinValue : ys.Min,
                                               Normalised : ys.NormalisedBool));
      FQuadruple.AddZRange(b,
                           FRange.CreateFRange(b,
                                               DecimalGranularity : granularity,
                                               MaxValue : zs.Max,
                                               MinValue : zs.Min,
                                               Normalised : zs.NormalisedBool));
      FQuadruple.AddWRange(b,
                           FRange.CreateFRange(b,
                                               DecimalGranularity : granularity,
                                               MaxValue : ws.Max,
                                               MinValue : ws.Min,
                                               Normalised : ws.NormalisedBool));
      return FQuadruple.EndFQuadruple(b);
    }

    static Offset<FString> Serialise(FlatBufferBuilder b, IHasString numeral) {
      var string_offset = b.CreateString(s : numeral.ObservationValue);
      FString.StartFString(b);
      FString.AddStr(b, strOffset : string_offset);

      return FString.EndFString(b);
    }

    static Offset<FActor> Serialise(FlatBufferBuilder b,
                                    Offset<FActuator>[] actuators,
                                    IActor actor,
                                    string identifier) {
      var n = b.CreateString(s : identifier);
      var actuator_vector = FActor.CreateActuatorsVector(b, data : actuators);
      FActor.StartFActor(b);
      if (actor is KillableActor) {
        FActor.AddAlive(b, alive : ((KillableActor)actor).IsAlive);
      } else {
        FActor.AddAlive(b, true);
      }

      FActor.AddActorName(b, actorNameOffset : n);
      FActor.AddActuators(b, actuatorsOffset : actuator_vector);
      return FActor.EndFActor(b);
    }

    static Offset<FSensor> Serialise(FlatBufferBuilder b, string identifier, ISensor sensor) {
      var n = b.CreateString(s : identifier);

      int observation_offset;
      FObservation observation_type;
      switch (sensor) {
        case IHasString numeral:
          observation_offset = Serialise(b : b, numeral : numeral).Value;
          observation_type = FObservation.FString;
          break;
        case IHasFloatArray a:
          observation_offset = Serialise(b : b, float_a : a).Value;
          observation_type = FObservation.FArray;
          break;
        case IHasSingle single:
          observation_offset = Serialise(b : b, numeral : single).Value;
          observation_type = FObservation.FSingle;
          break;
        case IHasDouble has_double:
          observation_offset = Serialise(b : b, numeral : has_double).Value;
          observation_type = FObservation.FDouble;
          break;
        case IHasTriple triple:
          observation_offset = Serialise(b : b, numeral : triple).Value;
          observation_type = FObservation.FTriple;
          break;
        case IHasQuadruple quadruple:
          observation_offset = Serialise(b : b, numeral : quadruple).Value;
          observation_type = FObservation.FQuadruple;
          break;
        case IHasEulerTransform transform:
          observation_offset = Serialise(b : b, sensor : transform).Value;
          observation_type = FObservation.FETObs;
          break;
        case IHasQuaternionTransform quaternion_transform:
          observation_offset = Serialise(b : b, sensor : quaternion_transform).Value;
          observation_type = FObservation.FQTObs;
          break;
        case IHasRigidbody rigidbody:
          observation_offset = Serialise(b : b, rigidbody : rigidbody).Value;
          observation_type = FObservation.FRBObs;
          break;
        case IHasByteArray array:
          observation_offset = Serialise(b : b, sensor : array).Value;
          observation_type = FObservation.FByteArray;
          break;
        default:
          return FSensor.CreateFSensor(b, sensor_nameOffset : n);
      }

      FSensor.StartFSensor(b);
      FSensor.AddSensorName(b, sensorNameOffset : n);
      FSensor.AddSensorValueType(b, sensorValueType : observation_type);
      FSensor.AddSensorValue(b, sensorValueOffset : observation_offset);
      return FSensor.EndFSensor(b);
    }

    static Offset<FEnvironmentDescription> Serialise(FlatBufferBuilder b, EnvironmentSnapshot snapshot) {
      var actors_offsets = new Offset<FActor>[snapshot.Description.Actors.Values.Count];
      var j = 0;
      foreach (var actor in snapshot.Description.Actors) {
        var actuators_offsets = new Offset<FActuator>[actor.Value.Actuators.Values.Count];
        var i = 0;
        foreach (var actuator in actor.Value.Actuators) {
          actuators_offsets[i++] = Serialise(b : b, actuator : actuator.Value, identifier : actuator.Key);
        }

        actors_offsets[j++] = Serialise(b : b,
                                        actuators : actuators_offsets,
                                        actor : actor.Value,
                                        identifier : actor.Key);
      }

      var actors_vector_offset = FEnvironmentDescription.CreateActorsVector(b, data : actors_offsets);

      var configurables_offsets = new Offset<FConfigurable>[snapshot.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in snapshot.Description.Configurables) {
        configurables_offsets[k++] = Serialise(b : b, configurable : configurable.Value, identifier : configurable.Key);
      }

      var configurables_vector_offset =
          FEnvironmentDescription.CreateConfigurablesVector(b, data : configurables_offsets);

      var objective_offset = Serialise(b : b, description : snapshot.Description);

      var sensors = new Offset<FSensor>[snapshot.Description.Sensors.Values.Count];
      var js = 0;
      foreach (var sensor in snapshot.Description.Sensors) {
        sensors[js++] = Serialise(b : b, identifier : sensor.Key, sensor : sensor.Value);
      }

      var sensors_vector = FEnvironmentDescription.CreateSensorsVector(b, data : sensors);

      FEnvironmentDescription.StartFEnvironmentDescription(b);

      FEnvironmentDescription.AddObjective(b, objectiveOffset : objective_offset);
      FEnvironmentDescription.AddActors(b, actorsOffset : actors_vector_offset);
      FEnvironmentDescription.AddConfigurables(b, configurablesOffset : configurables_vector_offset);
      FEnvironmentDescription.AddSensors(b, sensorsOffset : sensors_vector);

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

      var objective_name_offset = b.CreateString(s : ob_name);
      FObjective.StartFObjective(b);
      FObjective.AddMaxEpisodeLength(b, maxEpisodeLength : ep_len);
      FObjective.AddSignalSpace(b,
                                FRange.CreateFRange(b,
                                                    DecimalGranularity : a,
                                                    MaxValue : f,
                                                    MinValue : c,
                                                    Normalised : d));
      FObjective.AddObjectiveName(b, objectiveNameOffset : objective_name_offset);
      return FObjective.EndFObjective(b);
    }

    static Offset<FTriple> Serialise(FlatBufferBuilder b, PositionConfigurable sensor) {
      var pos = sensor.ObservationValue;
      FTriple.StartFTriple(b);
      FTriple.AddVec3(b,
                      FVector3.CreateFVector3(b,
                                              X : pos.x,
                                              Y : pos.y,
                                              Z : pos.z));
      return FTriple.EndFTriple(b);
    }

    static Offset<FConfigurable> Serialise(
        FlatBufferBuilder b,
        IConfigurable configurable,
        string identifier) {
      var n = b.CreateString(s : identifier);

      int observation_offset;
      FObservation observation_type;

      if (configurable is IHasQuaternionTransform) {
        observation_offset = Serialise(b : b, (IHasQuaternionTransform)configurable).Value;
        observation_type = FObservation.FQTObs;
      } else if (configurable is PositionConfigurable) {
        observation_offset = Serialise(b : b, (PositionConfigurable)configurable).Value;
        observation_type = FObservation.FTriple;
      } else if (configurable is IHasSingle) {
        observation_offset = Serialise(b : b, (IHasSingle)configurable).Value;
        observation_type = FObservation.FSingle;
        // ReSharper disable once SuspiciousTypeConversion.Global
      } else if (configurable is IHasDouble) {
        // ReSharper disable once SuspiciousTypeConversion.Global
        observation_offset = Serialise(b : b, (IHasDouble)configurable).Value;
        observation_type = FObservation.FDouble;
      } else if (configurable is EulerTransformConfigurable) {
        observation_offset = Serialise(b : b, (IHasEulerTransform)configurable).Value;
        observation_type = FObservation.FETObs;
      } else {
        FConfigurable.StartFConfigurable(b);
        FConfigurable.AddConfigurableName(b, configurableNameOffset : n);
        return FConfigurable.EndFConfigurable(b);
      }

      FConfigurable.StartFConfigurable(b);
      FConfigurable.AddConfigurableName(b, configurableNameOffset : n);
      FConfigurable.AddConfigurableValue(b, configurableValueOffset : observation_offset);
      FConfigurable.AddConfigurableValueType(b, configurableValueType : observation_type);
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
