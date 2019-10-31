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
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (var i = 1; i < spectrum.Length - 1; i++) {
          var prev = spectrum[i - 1];
          var cur = spectrum[i];
          var next = spectrum[i + 1];
          Debug.DrawLine(new Vector3(i - 1,  cur + 10, 0),
                         new Vector3(i, next + 10, 0),
                         Color.red);
          Debug.DrawLine(new Vector3(i - 1, Mathf.Log(prev) + 10, 2),
                         new Vector3(i, Mathf.Log(cur) + 10, 2),
                         Color.cyan);
          Debug.DrawLine(new Vector3(Mathf.Log(i - 1), prev- 10, 1),
                         new Vector3(Mathf.Log(i), cur - 10, 1),
                         Color.green);
          Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(prev), 3),
                         new Vector3(Mathf.Log(i), Mathf.Log(cur), 3),
                         Color.blue);
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
      AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
      this.ObservationArray = spectrum;
    }

    public Single[] ObservationArray { get; set; }
    public Space1[] ObservationSpace { get; } = new Space1[1];
  }
}
