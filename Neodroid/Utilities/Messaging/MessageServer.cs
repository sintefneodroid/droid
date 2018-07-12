using System;
using System.Linq;
using System.Threading;
using AsyncIO;
using droid.Neodroid.Utilities.Messaging.FBS;
using droid.Neodroid.Utilities.Messaging.Messages;
using FlatBuffers;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

namespace droid.Neodroid.Utilities.Messaging {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public class MessageServer {
    #region PublicMembers

    /// <summary>
    ///
    /// </summary>
    public bool _Listening_For_Clients;

    #endregion

    #region PrivateMembers

    /// <summary>
    ///
    /// </summary>
    Thread _polling_thread;
    #if NEODROID_DEBUG
    int _last_send_frame_number = 0;

    float _last_send_time = 0;
    #endif

    /// <summary>
    ///
    /// </summary>
    Thread _wait_for_client_thread;

    /// <summary>
    ///
    /// </summary>
    object _stop_lock = new object();

    object _thread_lock = new object();

    /// <summary>
    ///
    /// </summary>
    bool _stop_thread;

    /// <summary>
    ///
    /// </summary>
    bool _waiting_for_main_loop_to_send;

    /// <summary>
    ///
    /// </summary>
    bool _use_inter_process_communication;

    /// <summary>
    ///
    /// </summary>
    bool _debugging;

    /// <summary>
    ///
    /// </summary>
    ResponseSocket _socket;

    //PairSocket _socket;
    /// <summary>
    ///
    /// </summary>
    string _ip_address;

    /// <summary>
    ///
    /// </summary>
    int _port;

    /// <summary>
    ///
    /// </summary>
    byte[] _byte_buffer;

    /// <summary>
    ///
    /// </summary>
    Double _wait_time_seconds;

    const string _api_version = "0.0.1";

    #endregion

    #region PrivateMethods

    #region Threads

    /// <summary>
    ///
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="debug_callback"></param>
    void BindSocket(Action callback, Action<String> debug_callback) {
      if (this._debugging) {
        debug_callback?.Invoke("Start listening for clients");
      }

      try {
        if (this._use_inter_process_communication) {
          this._socket.Bind("ipc:///tmp/neodroid/messages");
        } else {
          this._socket.Bind("tcp://" + this._ip_address + ":" + this._port);
        }

        callback?.Invoke();
        if (this._debugging) {
          debug_callback?.Invoke("Now listening for clients");
        }

        this._Listening_For_Clients = true;
      } catch (Exception exception) {
        if (this._debugging) {
          debug_callback?.Invoke($"BindSocket threw exception: {exception}");
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="wait_time"></param>
    /// <returns></returns>
    public Reaction[] Receive(TimeSpan wait_time) {
      //this._socket.Poll(); // TODO: MAYBE WAIT FOR CLIENT TO SEND

      Reaction[] reactions = null;
      lock (this._thread_lock) {
        try {
          byte[] msg;

          if (wait_time > TimeSpan.Zero) {
            #if NEODROID_DEBUG
            var received = this._socket.TryReceiveFrameBytes(wait_time, out msg);
            if (this.Debugging) {
              if (received) {
                Debug.Log($"Received frame bytes");
              } else {
                Debug.Log($"Received nothing in {wait_time} seconds");
              }
            }
            #else
            this._socket.TryReceiveFrameBytes(wait_time, out msg);
            #endif
          } else {
            msg = this._socket
                .ReceiveFrameBytes(); //TODO: Occasionally receives non-valid reactions framebytes either before or after a valid reaction.
          }

          if (msg != null) { //&& msg.Length >= 4) {
            var flat_reaction = FReactions.GetRootAsFReactions(new ByteBuffer(msg));
            var tuple = FbsReactionUtilities.deserialise_reactions(flat_reaction);
            reactions = tuple.Item1; //TODO: Change tuple to the Reactions class
            var close = tuple.Item2;
            var api_version = tuple.Item3;
            var simulator_configuration = tuple.Item4;
          }
        } catch (Exception exception) {
          if (exception is TerminatingException) {
            return reactions;
          }

          Debug.Log(exception);
        }
      }

      return reactions;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="receive_callback"></param>
    /// <param name="disconnect_callback"></param>
    /// <param name="debug_callback"></param>
    void PollingThread(
        Action<Reaction[]> receive_callback,
        Action disconnect_callback,
        Action<string> debug_callback) {
      while (this._stop_thread == false) {
        lock (this._thread_lock) {
          if (!this._waiting_for_main_loop_to_send) {
            var reactions = this.Receive(TimeSpan.FromSeconds(this._wait_time_seconds));
            if (reactions != null) {
              receive_callback(reactions);
              this._waiting_for_main_loop_to_send = true;
            }
          } else {
            if (this._debugging) {
              debug_callback("Waiting for main loop to send reply");
            }
          }
        }
      }

      disconnect_callback();
      if (this._use_inter_process_communication) {
        this._socket.Disconnect("inproc://neodroid");
      } else {
        this._socket.Disconnect("tcp://" + this._ip_address + ":" + this._port);
      }

      try {
        this._socket.Dispose();
        this._socket.Close();
      } finally {
        NetMQConfig.Cleanup(false);
      }
    }

    #endregion

    #endregion

    #region PublicMethods

    ///  <summary>
    /// 
    ///  </summary>
    ///  <param name="environment_states"></param>
    /// <param name="do_serialise_unobservables"></param>
    /// <param name="serialise_indidual_observables"></param>
    /// <param name="simulator_configuration_message"></param>
    /// <param name="api_vesion"></param>
    public void SendStates(
        EnvironmentState[] environment_states,
        bool do_serialise_unobservables = false,
        bool serialise_indidual_observables = false,
        SimulatorConfigurationMessage simulator_configuration_message = null,
        string api_vesion = _api_version) {
      lock (this._thread_lock) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          var environment_state = environment_states.ToArray();
          if (environment_state.Length > 0) {
            if (environment_state[0] != null) {
              var frame_number = environment_state[0].FrameNumber;
              var time = environment_state[0].Time;
              var frame_number_duplicate = this._last_send_frame_number == frame_number;
              if (frame_number_duplicate && frame_number > 0) {
                Debug.LogWarning($"Sending duplicate frame! Frame number: {frame_number}");
              }

              if (frame_number <= this._last_send_frame_number) {
                Debug.LogWarning(
                    $"The current frame number {frame_number} is less or equal the last {this._last_send_frame_number}, SINCE AWAKE ({Time.frameCount})");
              }

              if (time <= this._last_send_time) {
                Debug.LogWarning($"The current time {time} is less or equal the last {this._last_send_time}");
              }

              if (environment_state[0].Description != null) {
                Debug.Log($"State has description: {environment_state[0].Description}");
              }

              this._last_send_frame_number = frame_number;
              this._last_send_time = time;
            }
          } else {
            Debug.LogWarning($"No environment states where send.");
          }
        }
        #endif

        this._byte_buffer = FbsStateUtilities.serialise_states(
            environment_states,
            do_serialise_unobservables : do_serialise_unobservables,
            serialise_indidual_observables : serialise_indidual_observables,
            simulator_configuration : simulator_configuration_message,
            api_version : api_vesion);
        this._socket.SendFrame(this._byte_buffer);
        this._waiting_for_main_loop_to_send = false;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="debug_callback"></param>
    public void ListenForClientToConnect(Action<string> debug_callback) {
      this.BindSocket(null, debug_callback);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="debug_callback"></param>
    public void ListenForClientToConnect(Action callback, Action<string> debug_callback) {
      this._wait_for_client_thread =
          new Thread(unused_param => this.BindSocket(callback, debug_callback)) {IsBackground = true};
      // Is terminated with foreground threads, when they terminate
      this._wait_for_client_thread.Start();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cmd_callback"></param>
    /// <param name="disconnect_callback"></param>
    /// <param name="debug_callback"></param>
    public void StartReceiving(
        Action<Reaction[]> cmd_callback,
        Action disconnect_callback,
        Action<string> debug_callback) {
      this._polling_thread =
          new Thread(unused_param => this.PollingThread(cmd_callback, disconnect_callback, debug_callback)) {
              IsBackground = true
          };
      // Is terminated with foreground threads, when they terminate
      this._polling_thread.Start();
    }

    #region Contstruction

    public MessageServer(
        string ip_address = "127.0.0.1",
        int port = 6969,
        bool use_inter_process_communication = false,
        bool debug = false,
        Double wait_time_seconds = 2) {
      this._wait_time_seconds = wait_time_seconds;
      this.Debugging = debug;
      this._ip_address = ip_address;
      this._port = port;
      this._use_inter_process_communication = use_inter_process_communication;

      if (!this._use_inter_process_communication) {
        ForceDotNet.Force();
      }

      this._socket = new ResponseSocket();
    }

    public MessageServer(bool debug = false) : this("127.0.0.1", 6969, false, debug) { }
    
    

    #endregion

    #region Getters

    /// <summary>
    ///
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endregion

    #endregion

    #region Deconstruction

    /// <summary>
    ///
    /// </summary>
    public void Destroy() { this.CleanUp(); }

    /// <summary>
    ///
    /// </summary>
    public void CleanUp() {
      try {
        lock (this._stop_lock) {
          this._stop_thread = true;
        }

        if (this._use_inter_process_communication) {
          this._socket.Disconnect("ipc:///tmp/neodroid/messages");
        } else {
          this._socket.Disconnect("tcp://" + this._ip_address + ":" + this._port);
        }

        try {
          this._socket.Dispose();
          this._socket.Close();
        } finally {
          NetMQConfig.Cleanup(false);
        }

        this._wait_for_client_thread?.Join();
        this._polling_thread?.Join();
      } catch {
        Console.WriteLine("Exception thrown while killing threads");
      }
    }

    #endregion
  }
}
