using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Auditory {


  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "AudioWaveform"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(AudioListener))]
  public class AudioWaveformSensor : Sensor,
                                   IHasFloatArray {

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<Single> FloatEnumerable { get { return this.ObservationArray; } }

    #if NEODROID_DEBUG
    void LateUpdate() {
      if (this.Debugging) {
        var samples = new float[256];
        AudioListener.GetOutputData(samples, 0);

        for (var i = 1; i < samples.Length - 1; i++) {
          var prev = samples[i - 1]* 3;
          var cur = samples[i] * 3;
          var next = samples[i + 1]* 3;
          Debug.DrawLine(new Vector3(i - 1,  cur, 0),
                         new Vector3(i, next, 0),
                         Color.red);
        }
      }
    }
    #endif

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      var samples = new float[256];
      AudioListener.GetOutputData(samples, 0);
      this.ObservationArray = samples;
    }

    /// <summary>
    ///
    /// </summary>
    public Single[] ObservationArray { get; set; }
    /// <summary>
    ///
    /// </summary>
    public Space1[] ObservationSpace { get; } = new Space1[1];
  }
}
