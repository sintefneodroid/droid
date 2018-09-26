using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Managers;
using Neodroid.Runtime.Utilities.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace Neodroid.Runtime.Prototyping.Observers.Camera {

  /// <inheritdoc cref="Observer" />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath + "StringAugmentedCamera" + ObserverComponentMenuPath._Postfix),
   ExecuteInEditMode, RequireComponent(typeof(UnityEngine.Camera))]
  public class StringAugmentedCameraObserver : CameraObserver,
                                IHasSerialisedString {
    /// <summary>
    ///
    /// </summary>
    [FormerlySerializedAs("_serialised_string")]
    [Header("Observation", order = 103)]


    [SerializeField]
    protected string _Serialised_String;


    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      this._grab = true;
      if (this._manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        this._camera.Render();
        this.UpdateBytes();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public String ObservationValue { get { return this._Serialised_String; } }
  }
}