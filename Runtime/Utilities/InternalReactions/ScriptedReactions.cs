using droid.Runtime.Managers;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Utilities.InternalReactions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public abstract class ScriptedReactions : MonoBehaviour {
    #if UNITY_EDITOR
    const int _script_execution_order = -10000;
    #endif

    /// <summary>
    /// </summary>
    [SerializeField]
    bool _debugging;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected AbstractNeodroidManager _Manager;

    /// <summary>
    /// </summary>
    public static ScriptedReactions Instance { get; private set; }

    #if NEODROID_DEBUG
    /// <summary>
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endif
    /// <summary>
    /// </summary>
    void Awake() {
      if (Instance == null) {
        Instance = this;
      } else {
        Debug.LogWarning("WARNING! Multiple PlayerReactions in the scene! Only using " + Instance);
      }

      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        var manager_script = MonoScript.FromMonoBehaviour(this);
        if (MonoImporter.GetExecutionOrder(manager_script) != _script_execution_order) {
          MonoImporter.SetExecutionOrder(manager_script,
                                         _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          Debug.LogWarning("Execution Order changed, you will need to press play again to make everything function correctly!");
          EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif
    }
  }
}
