using Neodroid.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Neodroid.Environments {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class StatusUpdater : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    Text _status_text;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    PausableManager _time_simulation_manager;

    // Use this for initialization
    /// <summary>
    /// 
    /// </summary>
    void Start() {
      if (!this._time_simulation_manager) {
        this._time_simulation_manager = FindObjectOfType<PausableManager>();
      }

      this._status_text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    /// <summary>
    /// 
    /// </summary>
    void Update() {
      if (this._time_simulation_manager) {
        this._status_text.text = this._time_simulation_manager.GetStatus();
      }
    }
  }
}
