using System.Linq;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Configurables;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurableSampleToggleList : MonoBehaviour
{
  Configurable[] _all_configurables;
  [SerializeField] Toggle _sample_toggle_button_prefab;

  void Awake()
  {
    this._all_configurables = FindObjectsOfType<Configurable>();
    //this._all_configurables = FindObjectOfType<PrototypingEnvironment>().Configurables;

    foreach (var configurable in this._all_configurables)
    {
      if (configurable.enabled)
      {
        var button = Instantiate(this._sample_toggle_button_prefab, this.transform);
        button.isOn = configurable.SampleRandom;
        button.onValueChanged.AddListener((value) => this.Set(configurable, value));
        var text = button.GetComponentInChildren<Text>();
        button.name = configurable.Identifier;
        text.text = configurable.Identifier;
      }
    }
  }

  void Toggle(IConfigurable configurable)
  {
    configurable.SampleRandom = !configurable.SampleRandom;
  }

  void Set(IConfigurable configurable, bool value)
  {
    configurable.SampleRandom = value;
  }
}