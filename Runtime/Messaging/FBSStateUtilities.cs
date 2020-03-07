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

      var states_vector_offset = FStates.CreateStatesVector(builder : b, data : state_offsets);

      var api_version_offset = b.CreateString(s : api_version);

      FStates.StartFStates(builder : b);
      FStates.AddStates(builder : b, statesOffset : states_vector_offset);
      FStates.AddApiVersion(builder : b, apiVersionOffset : api_version_offset);
      FStates.AddSimulatorConfiguration(builder : b,
                                        simulatorConfigurationOffset : Serialise(b : b,
                                                                                 configuration :
                                                                                 simulator_configuration));
      var states_offset = FStates.EndFStates(builder : b);

      FStates.FinishFStatesBuffer(builder : b, offset : states_offset);

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
      return FSimulatorConfiguration.CreateFSimulatorConfiguration(builder : b,
                                                                   Width : configuration.Width,
                                                                   Height : configuration.Height,
                                                                   FullScreen : configuration.FullScreen,
                                                                   QualityLevel : configuration.QualityLevel,
                                                                   TimeScale : configuration.TimeScale,
                                                                   TargetFrameRate :
                                                                   configuration.TargetFrameRate,
                                                                   SimulationType :
                                                                   (FSimulationType)configuration
                                                                       .SimulationType,
                                                                   FrameSkips : configuration.FrameSkips,
                                                                   0, //TODO: Remove
                                                                   NumOfEnvironments :
                                                                   configuration.NumOfEnvironments,
                                                                   DoSerialiseIndividualSensors :
                                                                   configuration.DoSerialiseIndividualSensors,
                                                                   DoSerialiseUnobservables : configuration
                                                                       .DoSerialiseUnobservables
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
        observables_vector = FState.CreateObservablesVector(builder : b, data : snapshot.Observables);
      }

      var unobservables = _null_unobservables_offset;
      if (do_serialise_unobservables) {
        var state_unobservables = snapshot.Unobservables;
        if (state_unobservables != null) {
          var bodies = state_unobservables.Bodies;

          FUnobservables.StartBodiesVector(builder : b, numElems : bodies.Length);
          for (var index = 0; index < bodies.Length; index++) {
            var rig = bodies[index];
            var vel = rig.Velocity;
            var ang = rig.AngularVelocity;
            FBody.CreateFBody(builder : b,
                              velocity_X : vel.x,
                              velocity_Y : vel.y,
                              velocity_Z : vel.z,
                              angular_velocity_X : ang.x,
                              angular_velocity_Y : ang.y,
                              angular_velocity_Z : ang.z);
          }

          var bodies_vector = b.EndVector();

          var poses = state_unobservables.Poses;

          FUnobservables.StartPosesVector(builder : b, numElems : poses.Length);
          for (var index = 0; index < poses.Length; index++) {
            var tra = poses[index];
            var pos = tra.position;
            var rot = tra.rotation;
            FQuaternionTransform.CreateFQuaternionTransform(builder : b,
                                                            position_X : pos.x,
                                                            position_Y : pos.y,
                                                            position_Z : pos.z,
                                                            rotation_X : rot.x,
                                                            rotation_Y : rot.y,
                                                            rotation_Z : rot.z,
                                                            rotation_W : rot.w);
          }

          var poses_vector = b.EndVector();

          FUnobservables.StartFUnobservables(builder : b);
          FUnobservables.AddPoses(builder : b, posesOffset : poses_vector);
          FUnobservables.AddBodies(builder : b, bodiesOffset : bodies_vector);
          unobservables = FUnobservables.EndFUnobservables(builder : b);
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

      FState.StartFState(builder : b);
      FState.AddEnvironmentName(builder : b, environmentNameOffset : n);

      FState.AddFrameNumber(builder : b, frameNumber : snapshot.FrameNumber);
      if (do_serialise_aggregated_float_array) {
        FState.AddObservables(builder : b, observablesOffset : observables_vector);
      }

      if (do_serialise_unobservables) {
        FState.AddUnobservables(builder : b, unobservablesOffset : unobservables);
      }

      FState.AddSignal(builder : b, signal : snapshot.Signal);

      FState.AddTerminated(builder : b, terminated : snapshot.Terminated);
      FState.AddTerminationReason(builder : b, terminationReasonOffset : t);

      if (snapshot.Description != null) {
        FState.AddEnvironmentDescription(builder : b, environmentDescriptionOffset : description_offset);
      }

      if (snapshot.DebugMessage != "") {
        FState.AddExtraSerialisedMessage(builder : b, extraSerialisedMessageOffset : d);
      }

      return FState.EndFState(builder : b);
    }

    static Offset<FActuator> Serialise(FlatBufferBuilder b, IActuator actuator, string identifier) {
      var n = b.CreateString(s : identifier);
      FActuator.StartFActuator(builder : b);
      FActuator.AddActuatorName(builder : b, actuatorNameOffset : n);
      FActuator.AddActuatorRange(builder : b,
                                 actuatorRangeOffset : FRange.CreateFRange(builder : b,
                                                                           DecimalGranularity :
                                                                           actuator.MotionSpace
                                                                                   .DecimalGranularity,
                                                                           MaxValue :
                                                                           actuator.MotionSpace.Max,
                                                                           MinValue :
                                                                           actuator.MotionSpace.Min,
                                                                           Normalised : actuator
                                                                                        .MotionSpace
                                                                                        .NormalisedBool));
      return FActuator.EndFActuator(builder : b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="sensor"></param>
    /// <returns></returns>
    static Offset<FETObs> Serialise(FlatBufferBuilder b, IHasEulerTransform sensor) {
      FETObs.StartFETObs(builder : b);
      Vector3 pos = sensor.Position, rot = sensor.Rotation, dir = sensor.Direction;
      FETObs.AddTransform(builder : b,
                          transformOffset : FEulerTransform.CreateFEulerTransform(builder : b,
                                                                                  position_X : pos.x,
                                                                                  position_Y : pos.y,
                                                                                  position_Z : pos.z,
                                                                                  rotation_X : rot.x,
                                                                                  rotation_Y : rot.y,
                                                                                  rotation_Z : rot.z,
                                                                                  direction_X : dir.x,
                                                                                  direction_Y : dir.y,
                                                                                  direction_Z : dir.z));

      return FETObs.EndFETObs(builder : b);
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
      FQTObs.StartFQTObs(builder : b);
      FQTObs.AddPosRange(builder : b,
                         posRangeOffset : FRange.CreateFRange(builder : b,
                                                              DecimalGranularity :
                                                              pos_range.DecimalGranularity,
                                                              MaxValue : pos_range.Max,
                                                              MinValue : pos_range.Min,
                                                              Normalised : pos_range.NormalisedBool));
      FQTObs.AddRotRange(builder : b,
                         rotRangeOffset : FRange.CreateFRange(builder : b,
                                                              DecimalGranularity :
                                                              rot_range.DecimalGranularity,
                                                              MaxValue : rot_range.Max,
                                                              MinValue : rot_range.Min,
                                                              Normalised : rot_range.NormalisedBool));
      FQTObs.AddTransform(builder : b,
                          transformOffset : FQuaternionTransform.CreateFQuaternionTransform(builder : b,
                                                                                            position_X :
                                                                                            pos.x,
                                                                                            position_Y :
                                                                                            pos.y,
                                                                                            position_Z :
                                                                                            pos.z,
                                                                                            rotation_X :
                                                                                            rot.x,
                                                                                            rotation_Y :
                                                                                            rot.y,
                                                                                            rotation_Z :
                                                                                            rot.z,
                                                                                            rotation_W :
                                                                                            rot.w));

      return FQTObs.EndFQTObs(builder : b);
    }

    static Offset<FByteArray> Serialise(FlatBufferBuilder b, IHasByteArray sensor) {
      var v_offset = FByteArray.CreateBytesVectorBlock(builder : b, data : sensor.Bytes);
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

      var c = FByteArray.CreateShapeVector(builder : b, data : sensor.Shape);

      FByteArray.StartFByteArray(builder : b);
      FByteArray.AddType(builder : b, type : a);
      FByteArray.AddShape(builder : b, shapeOffset : c);
      FByteArray.AddBytes(builder : b, bytesOffset : v_offset);
      return FByteArray.EndFByteArray(builder : b);
    }

    static Offset<FArray> Serialise(FlatBufferBuilder b, IHasFloatArray float_a) {
      var v_offset = FArray.CreateArrayVectorBlock(builder : b, data : float_a.ObservationArray);
      //var v_offset = CustomFlatBufferImplementation.CreateFloatVector(b, float_a.ObservationArray);

      FArray.StartRangesVector(builder : b, numElems : float_a.ObservationSpace.Length);
      for (var index = 0; index < float_a.ObservationSpace.Length; index++) {
        var tra = float_a.ObservationSpace[index];
        FRange.CreateFRange(builder : b,
                            DecimalGranularity : tra.DecimalGranularity,
                            MaxValue : tra.Max,
                            MinValue : tra.Min,
                            Normalised : tra.NormalisedBool);
      }

      var ranges_vector = b.EndVector();

      FArray.StartFArray(builder : b);
      FArray.AddArray(builder : b, arrayOffset : v_offset);

      FArray.AddRanges(builder : b, rangesOffset : ranges_vector);

      return FArray.EndFArray(builder : b);
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="rigidbody"></param>
    /// <returns></returns>
    static Offset<FRBObs> Serialise(FlatBufferBuilder b, IHasRigidbody rigidbody) {
      FRBObs.StartFRBObs(builder : b);
      var a = rigidbody.Velocity;
      var c = rigidbody.AngularVelocity;

      FRBObs.AddBody(builder : b,
                     bodyOffset : FBody.CreateFBody(builder : b,
                                                    velocity_X : a.x,
                                                    velocity_Y : a.y,
                                                    velocity_Z : a.z,
                                                    angular_velocity_X : c.x,
                                                    angular_velocity_Y : c.y,
                                                    angular_velocity_Z : c.z));
      return FRBObs.EndFRBObs(builder : b);
    }

    static Offset<FSingle> Serialise(FlatBufferBuilder b, IHasSingle numeral) {
      FSingle.StartFSingle(builder : b);
      FSingle.AddValue(builder : b, value : numeral.ObservationValue);

      var range_offset = FRange.CreateFRange(builder : b,
                                             DecimalGranularity : numeral.SingleSpace.DecimalGranularity,
                                             MaxValue : numeral.SingleSpace.Max,
                                             MinValue : numeral.SingleSpace.Min,
                                             Normalised : numeral.SingleSpace.NormalisedBool);
      FSingle.AddRange(builder : b, rangeOffset : range_offset);
      return FSingle.EndFSingle(builder : b);
    }

    static Offset<FDouble> Serialise(FlatBufferBuilder b, IHasDouble numeral) {
      FDouble.StartFDouble(builder : b);
      var vec2 = numeral.ObservationValue;

      var granularity = numeral.DoubleSpace.DecimalGranularity;
      var xs = numeral.DoubleSpace.Xspace;
      var ys = numeral.DoubleSpace.Yspace;

      FDouble.AddXRange(builder : b,
                        xRangeOffset : FRange.CreateFRange(builder : b,
                                                           DecimalGranularity : granularity,
                                                           MaxValue : xs.Max,
                                                           MinValue : xs.Min,
                                                           Normalised : xs.NormalisedBool));
      FDouble.AddYRange(builder : b,
                        yRangeOffset : FRange.CreateFRange(builder : b,
                                                           DecimalGranularity : granularity,
                                                           MaxValue : ys.Max,
                                                           MinValue : ys.Min,
                                                           Normalised : ys.NormalisedBool));
      FDouble.AddVec2(builder : b, vec2Offset : FVector2.CreateFVector2(builder : b, X : vec2.x, Y : vec2.y));

      return FDouble.EndFDouble(builder : b);
    }

    static Offset<FTriple> Serialise(FlatBufferBuilder b, IHasTriple numeral) {
      FTriple.StartFTriple(builder : b);
      var vec3 = numeral.ObservationValue;

      FTriple.AddVec3(builder : b,
                      vec3Offset : FVector3.CreateFVector3(builder : b,
                                                           X : vec3.x,
                                                           Y : vec3.y,
                                                           Z : vec3.z));
      var granularity = numeral.TripleSpace.DecimalGranularity;
      var xs = numeral.TripleSpace.Xspace;
      var ys = numeral.TripleSpace.Yspace;
      var zs = numeral.TripleSpace.Zspace;
      FTriple.AddXRange(builder : b,
                        xRangeOffset : FRange.CreateFRange(builder : b,
                                                           DecimalGranularity : granularity,
                                                           MaxValue : xs.Max,
                                                           MinValue : xs.Min,
                                                           Normalised : xs.NormalisedBool));
      FTriple.AddYRange(builder : b,
                        yRangeOffset : FRange.CreateFRange(builder : b,
                                                           DecimalGranularity : granularity,
                                                           MaxValue : ys.Max,
                                                           MinValue : ys.Min,
                                                           Normalised : ys.NormalisedBool));
      FTriple.AddZRange(builder : b,
                        zRangeOffset : FRange.CreateFRange(builder : b,
                                                           DecimalGranularity : granularity,
                                                           MaxValue : zs.Max,
                                                           MinValue : zs.Min,
                                                           Normalised : zs.NormalisedBool));
      return FTriple.EndFTriple(builder : b);
    }

    static Offset<FQuadruple> Serialise(FlatBufferBuilder b, IHasQuadruple numeral) {
      FQuadruple.StartFQuadruple(builder : b);
      var quad = numeral.ObservationValue;
      FQuadruple.AddQuat(builder : b,
                         quatOffset : FQuaternion.CreateFQuaternion(builder : b,
                                                                    X : quad.x,
                                                                    Y : quad.y,
                                                                    Z : quad.z,
                                                                    W : quad.z));
      var granularity = numeral.QuadSpace.DecimalGranularity;
      var xs = numeral.QuadSpace.Xspace;
      var ys = numeral.QuadSpace.Yspace;
      var zs = numeral.QuadSpace.Zspace;
      var ws = numeral.QuadSpace.Wspace;
      FQuadruple.AddXRange(builder : b,
                           xRangeOffset : FRange.CreateFRange(builder : b,
                                                              DecimalGranularity : granularity,
                                                              MaxValue : xs.Max,
                                                              MinValue : xs.Min,
                                                              Normalised : xs.NormalisedBool));
      FQuadruple.AddYRange(builder : b,
                           yRangeOffset : FRange.CreateFRange(builder : b,
                                                              DecimalGranularity : granularity,
                                                              MaxValue : ys.Max,
                                                              MinValue : ys.Min,
                                                              Normalised : ys.NormalisedBool));
      FQuadruple.AddZRange(builder : b,
                           zRangeOffset : FRange.CreateFRange(builder : b,
                                                              DecimalGranularity : granularity,
                                                              MaxValue : zs.Max,
                                                              MinValue : zs.Min,
                                                              Normalised : zs.NormalisedBool));
      FQuadruple.AddWRange(builder : b,
                           wRangeOffset : FRange.CreateFRange(builder : b,
                                                              DecimalGranularity : granularity,
                                                              MaxValue : ws.Max,
                                                              MinValue : ws.Min,
                                                              Normalised : ws.NormalisedBool));
      return FQuadruple.EndFQuadruple(builder : b);
    }

    static Offset<FString> Serialise(FlatBufferBuilder b, IHasString numeral) {
      var string_offset = b.CreateString(s : numeral.ObservationValue);
      FString.StartFString(builder : b);
      FString.AddStr(builder : b, strOffset : string_offset);

      return FString.EndFString(builder : b);
    }

    static Offset<FActor> Serialise(FlatBufferBuilder b,
                                    Offset<FActuator>[] actuators,
                                    IActor actor,
                                    string identifier) {
      var n = b.CreateString(s : identifier);
      var actuator_vector = FActor.CreateActuatorsVector(builder : b, data : actuators);
      FActor.StartFActor(builder : b);
      if (actor is KillableActor) {
        FActor.AddAlive(builder : b, alive : ((KillableActor)actor).IsAlive);
      } else {
        FActor.AddAlive(builder : b, true);
      }

      FActor.AddActorName(builder : b, actorNameOffset : n);
      FActor.AddActuators(builder : b, actuatorsOffset : actuator_vector);
      return FActor.EndFActor(builder : b);
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
          return FSensor.CreateFSensor(builder : b, sensor_nameOffset : n);
      }

      FSensor.StartFSensor(builder : b);
      FSensor.AddSensorName(builder : b, sensorNameOffset : n);
      FSensor.AddSensorValueType(builder : b, sensorValueType : observation_type);
      FSensor.AddSensorValue(builder : b, sensorValueOffset : observation_offset);
      return FSensor.EndFSensor(builder : b);
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

      var actors_vector_offset =
          FEnvironmentDescription.CreateActorsVector(builder : b, data : actors_offsets);

      var configurables_offsets = new Offset<FConfigurable>[snapshot.Description.Configurables.Values.Count];
      var k = 0;
      foreach (var configurable in snapshot.Description.Configurables) {
        configurables_offsets[k++] =
            Serialise(b : b, configurable : configurable.Value, identifier : configurable.Key);
      }

      var configurables_vector_offset =
          FEnvironmentDescription.CreateConfigurablesVector(builder : b, data : configurables_offsets);

      var objective_offset = Serialise(b : b, description : snapshot.Description);

      var sensors = new Offset<FSensor>[snapshot.Description.Sensors.Values.Count];
      var js = 0;
      foreach (var sensor in snapshot.Description.Sensors) {
        sensors[js++] = Serialise(b : b, identifier : sensor.Key, sensor : sensor.Value);
      }

      var sensors_vector = FEnvironmentDescription.CreateSensorsVector(builder : b, data : sensors);

      FEnvironmentDescription.StartFEnvironmentDescription(builder : b);

      FEnvironmentDescription.AddObjective(builder : b, objectiveOffset : objective_offset);
      FEnvironmentDescription.AddActors(builder : b, actorsOffset : actors_vector_offset);
      FEnvironmentDescription.AddConfigurables(builder : b,
                                               configurablesOffset : configurables_vector_offset);
      FEnvironmentDescription.AddSensors(builder : b, sensorsOffset : sensors_vector);

      return FEnvironmentDescription.EndFEnvironmentDescription(builder : b);
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
      FObjective.StartFObjective(builder : b);
      FObjective.AddMaxEpisodeLength(builder : b, maxEpisodeLength : ep_len);
      FObjective.AddSignalSpace(builder : b,
                                signalSpaceOffset : FRange.CreateFRange(builder : b,
                                                                        DecimalGranularity : a,
                                                                        MaxValue : f,
                                                                        MinValue : c,
                                                                        Normalised : d));
      FObjective.AddObjectiveName(builder : b, objectiveNameOffset : objective_name_offset);
      return FObjective.EndFObjective(builder : b);
    }

    static Offset<FTriple> Serialise(FlatBufferBuilder b, PositionConfigurable sensor) {
      var pos = sensor.ObservationValue;
      FTriple.StartFTriple(builder : b);
      FTriple.AddVec3(builder : b,
                      vec3Offset : FVector3.CreateFVector3(builder : b,
                                                           X : pos.x,
                                                           Y : pos.y,
                                                           Z : pos.z));
      return FTriple.EndFTriple(builder : b);
    }

    static Offset<FConfigurable> Serialise(
        FlatBufferBuilder b,
        IConfigurable configurable,
        string identifier) {
      var n = b.CreateString(s : identifier);

      int observation_offset;
      FObservation observation_type;

      if (configurable is IHasQuaternionTransform) {
        observation_offset = Serialise(b : b, sensor : (IHasQuaternionTransform)configurable).Value;
        observation_type = FObservation.FQTObs;
      } else if (configurable is PositionConfigurable) {
        observation_offset = Serialise(b : b, sensor : (PositionConfigurable)configurable).Value;
        observation_type = FObservation.FTriple;
      } else if (configurable is IHasSingle) {
        observation_offset = Serialise(b : b, numeral : (IHasSingle)configurable).Value;
        observation_type = FObservation.FSingle;
        // ReSharper disable once SuspiciousTypeConversion.Global
      } else if (configurable is IHasDouble) {
        // ReSharper disable once SuspiciousTypeConversion.Global
        observation_offset = Serialise(b : b, numeral : (IHasDouble)configurable).Value;
        observation_type = FObservation.FDouble;
      } else if (configurable is EulerTransformConfigurable) {
        observation_offset = Serialise(b : b, sensor : (IHasEulerTransform)configurable).Value;
        observation_type = FObservation.FETObs;
      } else {
        FConfigurable.StartFConfigurable(builder : b);
        FConfigurable.AddConfigurableName(builder : b, configurableNameOffset : n);
        return FConfigurable.EndFConfigurable(builder : b);
      }

      FConfigurable.StartFConfigurable(builder : b);
      FConfigurable.AddConfigurableName(builder : b, configurableNameOffset : n);
      FConfigurable.AddConfigurableValue(builder : b, configurableValueOffset : observation_offset);
      FConfigurable.AddConfigurableValueType(builder : b, configurableValueType : observation_type);
      FConfigurable.AddConfigurableRange(builder : b,
                                         configurableRangeOffset : FRange.CreateFRange(builder : b,
                                                                                       0,
                                                                                       0,
                                                                                       0,
                                                                                       false));
      return FConfigurable.EndFConfigurable(builder : b);
    }

    #endregion
  }
}
