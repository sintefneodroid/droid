using Boo.Lang;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using UnityEngine;


namespace droid.Runtime.Prototyping.Configurables {
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

    /// <summary>
    ///
    /// </summary>
    public override void PostEnvironmentSetup() {
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

      this.active.SetActive(true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      if (!Application.isPlaying) {
        return;
      }

      if (this.active) {
        this.active.SetActive(false);
      }

      if (this.children != null && (int)configuration.ConfigurableValue < this.len) {
        this.CurrentCategoryValue = (int)configuration.ConfigurableValue;
        this.active = this.children[this.CurrentCategoryValue];
      }

      this.active.SetActive(true);
    }

    public override IConfigurableConfiguration SampleConfiguration() {
      return new Configuration(this.Identifier, int.Parse(UnityEngine.Random.Range(0, this.len).ToString()));
    }

    public int CurrentCategoryValue { get; set; }
  }
}
