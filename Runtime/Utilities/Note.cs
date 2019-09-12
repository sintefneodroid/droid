using System;
using UnityEngine;

#if UNITY_EDITOR

namespace droid.Runtime.Utilities {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [Serializable]
  public class Note : MonoBehaviour {
    /// <summary>
    /// </summary>
    [NonSerialized]
    public bool _Editing;

    /// <summary>
    /// </summary>
    [TextArea]
    [Tooltip("A component for holding notes or comments")]
    [SerializeField]
    public string _Text;

    /// <summary>
    /// </summary>
    public void EditToggle() { this._Editing = !this._Editing; }

    void Start() { this.enabled = false; }
  }
}
#endif
