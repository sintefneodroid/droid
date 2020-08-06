using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Auditory {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "AudioWaveform"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(requiredComponent : typeof(AudioListener))]
  public class AudioWaveformSensor : Sensor,
                                     IHasFloatArray {
    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<float> FloatEnumerable { get { return this.ObservationArray; } }

    #if NEODROID_DEBUG
    void LateUpdate() {
      if (this.Debugging) {
        var samples = new float[256];
        AudioListener.GetOutputData(samples : samples, 0);

        for (var i = 1; i < samples.Length - 1; i++) {
          var prev = samples[i - 1] * 3;
          var cur = samples[i] * 3;
          var next = samples[i + 1] * 3;
          Debug.DrawLine(start : new Vector3(x : i - 1, y : cur, 0),
                         end : new Vector3(x : i, y : next, 0),
                         color : Color.red);
        }
      }
    }
    #endif

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      var samples = new float[256];
      AudioListener.GetOutputData(samples : samples, 0);
      this.ObservationArray = samples;
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public float[] ObservationArray { get; set; }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Space1[] ObservationSpace { get; } = new Space1[1];
  }
}
