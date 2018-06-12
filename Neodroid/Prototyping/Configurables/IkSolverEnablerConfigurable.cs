#if BIOIK_EXISTS
using SceneAssets.Manipulator.Excluded.BioIK;
#endif

using System.Collections;
using droid.Neodroid.Utilities.Messaging.Messages;

using UnityEngine;
using Random = System.Random;

namespace droid.Neodroid.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class IkSolverEnablerConfigurable : ConfigurableGameObject {
    #if BIOIK_EXISTS
    /// <summary>
    /// 
    /// </summary>
    public BioIK _Enablee;

    [SerializeField] bool _every_frame;

    const float _tolerance = float.Epsilon;

    /// <summary>
    /// 
    /// </summary>
    protected override void PreSetup() {
      base.PreSetup();
      if (!this._Enablee) {
        this._Enablee = this.GetComponent<BioIK>();
      }

      this.Disable();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    public override void ApplyConfiguration(Configuration obj) {
      if (Mathf.Abs(obj.ConfigurableValue) < _tolerance) {
        this.Disable();
      } else {
        this.Enable();
        if (!this._every_frame) {
          this.StartCoroutine(this.DisableNextFrame());
        }
      }
    }

    IEnumerator DisableNextFrame() {
      yield return new WaitForEndOfFrame();
      this.Disable();
    }

    public void Disable() {
      if(this._Enablee) {
        this._Enablee.enabled = false;
      }
    }

    public void Enable() {
      if(this._Enablee) {
        this._Enablee.enabled = true;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override Configuration SampleConfiguration(Random random_generator) {
      return new Configuration(this.Identifier, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ActiveToggle() {
      if (this._Enablee) {
        this._Enablee.enabled = !this._Enablee.enabled;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Activate() {
      var conf = new Configuration(this.Identifier, 1);

      this.ApplyConfiguration(conf);
    }
    #endif
  }
}
