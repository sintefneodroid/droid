using System;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators.Auditory {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(requiredComponent : typeof(AudioSource))]
  public class AuditoryEmitterActuator : Actuator {
    AudioSource _audio_source;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      base.PreSetup();
      this._audio_source = this.GetComponent<AudioSource>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (this._audio_source && this._audio_source.clip && !this._audio_source.mute) {
        this._audio_source.Play();
      }
    }

    public override String[] InnerMotionNames { get; }
  }
}
