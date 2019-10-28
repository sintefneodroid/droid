using System;
using droid.Runtime.GameObjects.NeodroidCamera;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

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

    /// <summary>
    ///   Red
    /// </summary>
    string _focal_str;

    /// <summary>
    ///   Red
    /// </summary>
    string _sensor_width_str;

    /// <summary>
    /// </summary>
    string _sensor_height_str;

    /// <summary>
    /// </summary>
    string _lens_shift_y_str;

    /// <summary>
    /// </summary>
    string _lens_shift_x_str;

    string _gate_fit_str;

    [SerializeField] Camera _camera;
    [SerializeField] SynchroniseCameraProperties _syncer;
    [SerializeField] SampleSpace1 _fov_space = new SampleSpace1 {_space = new Space1 {Min = 60f, Max = 90f}};
    [SerializeField] SampleSpace1 _focal_space = new SampleSpace1 {_space = new Space1 {Min = 2f, Max = 3f}};

    [SerializeField]
    SampleSpace2 _sensor_size_space =
        new SampleSpace2 {_space = new Space2 {Min = new Vector2(2.5f, 2.5f), Max = new Vector2(5, 5)}};

    [SerializeField]
    SampleSpace2 _lens_shift_space = new SampleSpace2 {
                                                          _space = new Space2 {
                                                                                     Min = new Vector2(-0.1f,
                                                                                                       -0.1f),
                                                                                     Max = new Vector2(0.1f,
                                                                                                       0.1f)
                                                                                 }
                                                      };

    [SerializeField]
    SampleSpace1 _gate_fit_space = new SampleSpace1 {_space = Space1.DiscreteZeroOne*4};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._fov_str = this.Identifier + "Fov";
      this._focal_str = this.Identifier + "Focal";
      this._sensor_width_str = this.Identifier + "SensorWidth";
      this._sensor_height_str = this.Identifier + "SensorHeight";
      this._lens_shift_x_str = this.Identifier + "LensShiftX";
      this._lens_shift_y_str = this.Identifier + "LensShiftY";
      this._gate_fit_str = this.Identifier + "GateFit";
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
      if (!this._camera.usePhysicalProperties) {
        this.ParentEnvironment =
            NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                            (Configurable)this,
                                                            this._fov_str);
      } else {
        this.ParentEnvironment =
            NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                            (Configurable)this,
                                                            this._focal_str);
        this.ParentEnvironment =
            NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                            (Configurable)this,
                                                            this._sensor_width_str);
        this.ParentEnvironment =
            NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                            (Configurable)this,
                                                            this._sensor_height_str);
        this.ParentEnvironment =
            NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                            (Configurable)this,
                                                            this._lens_shift_x_str);
        this.ParentEnvironment =
            NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                            (Configurable)this,
                                                            this._lens_shift_y_str);
        this.ParentEnvironment =
            NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                            (Configurable)this,
                                                            this._gate_fit_str);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>n
    protected override void UnRegisterComponent() {
      if (!this._camera.usePhysicalProperties) {
        this.ParentEnvironment?.UnRegister(this, this._fov_str);
      } else {
        this.ParentEnvironment?.UnRegister(this, this._focal_str);
        this.ParentEnvironment?.UnRegister(this, this._sensor_width_str);
        this.ParentEnvironment?.UnRegister(this, this._sensor_height_str);
        this.ParentEnvironment?.UnRegister(this, this._lens_shift_x_str);
        this.ParentEnvironment?.UnRegister(this, this._lens_shift_y_str);
        this.ParentEnvironment?.UnRegister(this, this._gate_fit_str);
      }
    }

    public override ISamplable ConfigurableValueSpace { get { return this._sensor_size_space; } }

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
      } else if (configuration.ConfigurableName == this._focal_str) {
        this._camera.focalLength = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._sensor_width_str) {
        var a = this._camera.sensorSize;
        a.x = configuration.ConfigurableValue;
        this._camera.sensorSize = a;
      } else if (configuration.ConfigurableName == this._sensor_height_str) {
        var a = this._camera.sensorSize;
        a.y = configuration.ConfigurableValue;
        this._camera.sensorSize = a;
      } else if (configuration.ConfigurableName == this._lens_shift_x_str) {
        var a = this._camera.lensShift;
        a.x = configuration.ConfigurableValue;
        this._camera.lensShift = a;
      } else if (configuration.ConfigurableName == this._lens_shift_y_str) {
        var a = this._camera.lensShift;
        a.y = configuration.ConfigurableValue;
        this._camera.lensShift = a;
      } else if (configuration.ConfigurableName == this._gate_fit_str) {
        Enum.TryParse(((int)configuration.ConfigurableValue).ToString(),
                      out Camera.GateFitMode gate_fit_mode);
        this._camera.gateFit = gate_fit_mode;
      }

      if (this._syncer) {
        this._syncer.Sync_Cameras();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public override Configuration[] SampleConfigurations() {
      if (!this._camera.usePhysicalProperties) {
        return new[] {new Configuration(this._fov_str, this._fov_space.Sample())};
      }

      var r = Random.Range(0, 6);
      switch (r) {
        case 0:
          return new[] {new Configuration(this._focal_str, this._focal_space.Sample())};
        case 1:
          return new[] {new Configuration(this._sensor_width_str, this._sensor_size_space.Sample().x)};

        case 2:
          return new[] {new Configuration(this._sensor_height_str, this._sensor_size_space.Sample().y)};

        case 3:
          return new[] {new Configuration(this._lens_shift_x_str, this._lens_shift_space.Sample().x)};

        case 4:
          return new[] {new Configuration(this._lens_shift_y_str, this._lens_shift_space.Sample().y)};

        case 5:
          return new[] {new Configuration(this._gate_fit_str, this._gate_fit_space.Sample())};
        default:
          throw new IndexOutOfRangeException();
      }
    }
  }
}
