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
                               + "AudioSpectrum"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(requiredComponent : typeof(AudioListener))]
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
          Debug.DrawLine(start : new Vector3(x : i - 1, y : cur + 10, z : 0),
                         end : new Vector3(x : i, y : next + 10, 0),
                         color : Color.red);
          Debug.DrawLine(start : new Vector3(x : i - 1, y : Mathf.Log(f : prev) + 10, z : 2),
                         end : new Vector3(x : i, y : Mathf.Log(f : cur) + 10, 2),
                         color : Color.cyan);
          Debug.DrawLine(start : new Vector3(x : Mathf.Log(f : i - 1), y : prev - 10, z : 1),
                         end : new Vector3(x : Mathf.Log(f : i), y : cur - 10, z : 1),
                         color : Color.green);
          Debug.DrawLine(start : new Vector3(x : Mathf.Log(f : i - 1), y : Mathf.Log(f : prev), z : 3),
                         end : new Vector3(x : Mathf.Log(f : i), y : Mathf.Log(f : cur), z : 3),
                         color : Color.blue);
        }
      }
    }
    #endif

    FFTWindow _fft_window = FFTWindow.Rectangular;

    [SerializeField] readonly Single[] _observation_array = new float[256];

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<Single> FloatEnumerable { get { return this.ObservationArray; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      AudioListener.GetSpectrumData(samples : this._observation_array, 0, window : this._fft_window);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Single[] ObservationArray { get { return this._observation_array; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space1[] ObservationSpace { get; } = new Space1[1];
  }
}
