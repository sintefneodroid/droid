using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Configurables;
using UnityEngine;
using UnityEngine.UI;

namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <summary>
  ///
  /// </summary>
  public class ConfigurableSampleToggleList : MonoBehaviour {
    Configurable[] _all_configurables = null;
    [SerializeField] Toggle _sample_toggle_button_prefab = null;

    void Awake() {
      this._all_configurables = FindObjectsOfType<Configurable>();
      //this._all_configurables = FindObjectOfType<PrototypingEnvironment>().Configurables;

      foreach (var configurable in this._all_configurables) {
        if (configurable.enabled) {
          var button = Instantiate(original : this._sample_toggle_button_prefab, parent : this.transform);
          button.isOn = configurable.RandomSamplingPhaseEnum == RandomSamplingPhaseEnum.On_tick_;
          button.onValueChanged.AddListener(value => Set(configurable : configurable, value : value));
          var text = button.GetComponentInChildren<Text>();
          button.name = configurable.Identifier;
          text.text = configurable.Identifier;
        }
      }
    }

    void Toggle(IConfigurable configurable) {
      if (configurable.RandomSamplingPhaseEnum != RandomSamplingPhaseEnum.Disabled_) {
        configurable.RandomSamplingPhaseEnum = RandomSamplingPhaseEnum.Disabled_;
      } else {
        configurable.RandomSamplingPhaseEnum = RandomSamplingPhaseEnum.On_tick_;
      }
    }

    static void Set(IConfigurable configurable, bool value) {
      if (value) {
        configurable.RandomSamplingPhaseEnum = RandomSamplingPhaseEnum.On_tick_;
      } else {
        configurable.RandomSamplingPhaseEnum = RandomSamplingPhaseEnum.Disabled_;
      }
    }
  }
}
