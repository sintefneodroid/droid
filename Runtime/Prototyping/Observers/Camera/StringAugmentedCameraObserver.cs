using System;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Managers;
using Neodroid.Runtime.Utilities.Misc.Drawing;
using Neodroid.Runtime.Utilities.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using NeodroidUtilities = Neodroid.Runtime.Utilities.Misc.NeodroidUtilities;
using Object = System.Object;

namespace Neodroid.Runtime.Prototyping.Observers.Camera {
  /// <inheritdoc cref="Observer" />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath
       + "StringAugmentedCamera"
       + ObserverComponentMenuPath._Postfix), ExecuteInEditMode, RequireComponent(typeof(UnityEngine.Camera))]
  public class StringAugmentedCameraObserver : CameraObserver,
                                               IHasString {
    /// <summary>
    ///
    /// </summary>
    [Header("Observation", order = 103)]
    [SerializeField]
    protected string _Serialised_String;

    public const string _Cam_Obs_Identifier = "cam";
    public const string _Seg_Obs_Identifier = "seg";
    
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this,_Cam_Obs_Identifier);
      
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this,_Seg_Obs_Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(this,_Cam_Obs_Identifier);
      this.ParentEnvironment?.UnRegister(this,_Seg_Obs_Identifier);
    }

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
