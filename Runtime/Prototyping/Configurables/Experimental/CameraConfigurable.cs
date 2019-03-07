using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Debugging;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.NeodroidCamera;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Camera"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Camera))]
  public class CameraConfigurable : Configurable {
    /// <summary>
    ///   Red
    /// </summary>
    string _fov_str;

    [SerializeField] Camera _camera;
    [SerializeField] SynchroniseCameraProperties _syncer;
    [SerializeField] float _random_fov_min = 30f;
    [SerializeField] float _random_fov_max = 90f;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._fov_str = this.Identifier + "Fov";
      if (!this._camera) {
        this._camera = this.GetComponent<Camera>();
      }

      if (!this._syncer) {
        this._syncer = this.GetComponent<SynchroniseCameraProperties>();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._fov_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>n
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(this, this._fov_str); }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif

      if (configuration.ConfigurableName == this._fov_str) {
        this._camera.fieldOfView = configuration.ConfigurableValue;
        if (this._syncer) {
          this._syncer.Sync_Cameras();
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>

    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration() {
      return new Configuration(this._fov_str,
                               Mathf.Clamp((float)Space1.ZeroOne.Sample()
                                           * (this._random_fov_max - this._random_fov_min)
                                           + this._random_fov_min,
                                           this._random_fov_min,
                                           this._random_fov_max));
    }
  }
}
