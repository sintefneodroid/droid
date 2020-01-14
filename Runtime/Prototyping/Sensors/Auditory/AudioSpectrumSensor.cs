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
                    + "AudioSpectrum"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(AudioListener))]
  public class AudioSpectrumSensor : Sensor,
                                     IHasFloatArray {
    #if NEODROID_DEBUG
    void LateUpdate() {
      if (this.Debugging) {
        var spectrum = new float[256];
        AudioListener.GetSpectrumData(samples : spectrum, 0, window : FFTWindow.Rectangular);

        for (var i = 1; i < spectrum.Length - 1; i++) {
          var prev = spectrum[i - 1];
          var cur = spectrum[i];
          var next = spectrum[i + 1];
          Debug.DrawLine(new Vector3(i - 1,  cur + 10, 0),
                         new Vector3(x : i, next + 10, 0),
                         color : Color.red);
          Debug.DrawLine(new Vector3(i - 1, Mathf.Log(f : prev) + 10, 2),
                         new Vector3(x : i, Mathf.Log(f : cur) + 10, 2),
                         color : Color.cyan);
          Debug.DrawLine(new Vector3(Mathf.Log(i - 1), prev- 10, 1),
                         new Vector3(Mathf.Log(f : i), cur - 10, 1),
                         color : Color.green);
          Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(f : prev), 3),
                         new Vector3(Mathf.Log(f : i), Mathf.Log(f : cur), 3),
                         color : Color.blue);
        }
      }
    }
    #endif

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable { get { return this.ObservationArray; } }

    public override void UpdateObservation() {
      var spectrum = new float[256];
      AudioListener.GetSpectrumData(samples : spectrum, 0, window : FFTWindow.Rectangular);
      this.ObservationArray = spectrum;
    }

    public Single[] ObservationArray { get; set; }
    public Space1[] ObservationSpace { get; } = new Space1[1];
  }
}
