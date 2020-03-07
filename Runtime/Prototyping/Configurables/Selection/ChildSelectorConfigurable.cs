using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Selection {
  /// <inheritdoc cref="Configurable" />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                               + "ChildSelector"
                               + ConfigurableComponentMenuPath._Postfix)]
  public class ChildSelectorConfigurable : Configurable,
                                           ICategoryProvider {
    [SerializeField] Renderer active;
    [SerializeField] Renderer[] children;
    [SerializeField] int len;
    [SerializeField] SampleSpace1 _configurable_value_space = new SampleSpace1();

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void Setup() {
      base.Setup();

      if (!Application.isPlaying) {
        return;
      }

      var la = new List<Renderer>();
      foreach (Transform child in this.transform) {
        var o = child.gameObject.GetComponent<Renderer>();
        o.enabled = false;
        this.active = o;
        la.Add(item : o);
      }

      this.children = la.ToArray();

      this.len = this.children.Length;

      if (this.active) {
        this.active.enabled = true;
      }

      this._configurable_value_space._space.DecimalGranularity = 0;
      this._configurable_value_space._space.Max = this.len - 1;
    }

    /// <summary>
    ///
    /// </summary>
    public ISamplable ConfigurableValueSpace { get { return this._configurable_value_space; } }

    public override void UpdateCurrentConfiguration() { }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      if (!Application.isPlaying) {
        return;
      }

      for (var index = 0; index < this.children.Length; index++) {
        var c = this.children[index];
        c.enabled = false;
      }

      if (this.children != null && (int)configuration.ConfigurableValue < this.len) {
        this.CurrentCategoryValue = Mathf.RoundToInt(f : configuration.ConfigurableValue);
        this.active = this.children[this.CurrentCategoryValue];
      }

      if (this.active) {
        this.active.enabled = true;
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override Configuration[] SampleConfigurations() {
      return new[] {
                       new Configuration(configurable_name : this.Identifier,
                                         configurable_value : this._configurable_value_space.Sample())
                   };
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    [field : SerializeField]
    public int CurrentCategoryValue { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Space1 Space1 { get { return this._configurable_value_space._space; } }
  }
}
