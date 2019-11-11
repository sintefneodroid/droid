using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Selection {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "ChildSelector"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class ChildSelectorConfigurable : Configurable,
                                           ICategoryProvider {
    [SerializeField] GameObject active;
    [SerializeField] GameObject[] children;
    [SerializeField] int len;
    [SerializeField] SampleSpace1 _configurable_value_space = new SampleSpace1();

    /// <summary>
    ///
    /// </summary>
    public override void RemotePostSetup() {
      if (!Application.isPlaying) {
        return;
      }

      var la = new List<GameObject>();
      foreach (Transform child in this.transform) {
        var o = child.gameObject;
        o.SetActive(false);
        this.active = o;
        la.Add(o);
      }

      this.children = la.ToArray();

      this.len = this.transform.childCount;

      if (this.active) {
        this.active.SetActive(true);
      }

      this._configurable_value_space._space.DecimalGranularity = 0;
      this._configurable_value_space._space.Max = this.len-1;
    }

    /// <summary>
    ///
    /// </summary>
    public override ISamplable ConfigurableValueSpace { get { return this._configurable_value_space; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      if (!Application.isPlaying) {
        return;
      }

      /*if (this.active) {
        this.active.SetActive(false);
      }*/
      foreach (var c in this.children) {
        c.SetActive(false);
      }

      if (this.children != null && (int)configuration.ConfigurableValue < this.len) {
        this.CurrentCategoryValue = (int)configuration.ConfigurableValue;
        this.active = this.children[this.CurrentCategoryValue];
      }

      if (this.active) {
        this.active.SetActive(true);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      return new[] {new Configuration(this.Identifier, this._configurable_value_space.Sample())};
    }

    /// <summary>
    ///
    /// </summary>
    public int CurrentCategoryValue { get; set; }

    public Space1 Space1 { get { return this._configurable_value_space._space; } }
  }
}
