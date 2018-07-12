using System;
using droid.Neodroid.Utilities.Interfaces;
using UnityEditor;
using UnityEngine;

namespace droid.Neodroid.Utilities.GameObjects {
  /// <summary>
  ///
  /// </summary>
  public class PrototypingGameObject : MonoBehaviour,
                                       IRegisterable {
    /// <summary>
    ///
    /// </summary>
    [SerializeField, Header("Naming", order = 10)]
    protected string _Custom_Name = "";

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected bool _Use_Custom_Name;

    /// <summary>
    ///
    /// </summary>
    [Header("Development", order = 90), SerializeField]
    bool _disables_children;

    #if UNITY_EDITOR
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    bool _editor_reregistering = true;
    #endif

    [SerializeField] bool _debugging;

    /// <summary>
    ///
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    /// <summary>
    ///
    /// </summary>
    protected void Start() {
      try {
        if (this.enabled && this.isActiveAndEnabled) {
          this.Setup();
          this.UnRegisterComponent();
          this.RegisterComponent();
        }
      } catch (ArgumentNullException e) {
        Debug.LogWarning(e);
        Debug.Log($"Exception happened on {this.GetType()}-{this}");
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected void Awake() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Clear();
      }
    }

    /// <summary>
    ///
    /// </summary>
    void OnDisable() {
      if (this._disables_children) {
        var children = this.GetComponentsInChildren<PrototypingGameObject>();
        foreach (var child in children) {
          if (child.gameObject != this.gameObject) {
            child.enabled = false;
            child.gameObject.SetActive(false);
          }
        }

        foreach (Transform child in this.transform) {
          if (child != this.transform) {
            child.GetComponent<PrototypingGameObject>()?.CallOnDisable();
            child.gameObject.SetActive(false);
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    void CallOnDisable() { this.OnDisable(); }

    /// <summary>
    ///
    /// </summary>
    void CallOnEnable() { this.OnEnable(); }

    /// <summary>
    ///
    /// </summary>
    void OnEnable() {
      if (this._disables_children) {
        foreach (Transform child in this.transform) {
          if (child != this.transform) {
            child.gameObject.SetActive(true);
            child.GetComponent<PrototypingGameObject>()?.CallOnEnable();
          }
        }

        var children = this.GetComponentsInChildren<PrototypingGameObject>();
        foreach (var child in children) {
          if (child.gameObject != this.gameObject) {
            child.enabled = true;
            child.gameObject.SetActive(true);
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void Setup() { }

    /// <summary>
    ///
    /// </summary>
    protected virtual void Clear() { }

    #if UNITY_EDITOR
    /// <summary>
    ///
    /// </summary>
    void OnValidate() { // Only called in the editor
      if (EditorApplication.isPlaying || !this._editor_reregistering) {
        return;
      }

      try {
        if (this.enabled && this.isActiveAndEnabled) {
          this.Setup();
          this.UnRegisterComponent();
          this.RegisterComponent();
        }
      } catch (NotImplementedException e) {
        Debug.Log(e);
        Debug.Log(
            $"You must override Register and UnRegisterComponent for component {this.GetType()} for gameobject {this.Identifier} in order to Re-register component on every 'OnValidate' while in edit-mode");
      }
    }
    #endif

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected virtual void UnRegisterComponent() { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected virtual void RegisterComponent() { throw new NotImplementedException(); }

    /// <summary>
    ///
    /// </summary>
    public void RefreshStart() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Start();
      }
    }

    /// <summary>
    ///
    /// </summary>
    public void RefreshAwake() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Awake();
      }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual string PrototypingTypeName { get { return "PrototypingGameObject"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public string Identifier {
      get {
        if (this._Use_Custom_Name) {
          return this._Custom_Name;
        }

        if (this.PrototypingTypeName != null) {
          return this.name + this.PrototypingTypeName;
        }

        return "CriticalFailure";
      }
    }
  }
}
