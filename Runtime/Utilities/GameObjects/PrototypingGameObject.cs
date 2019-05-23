using System;
using droid.Runtime.Interfaces;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.Utilities.GameObjects {
  /// <inheritdoc cref="IRegisterable" />
  /// <summary>
  /// </summary>
  public abstract class PrototypingGameObject : MonoBehaviour,
                                                IRegisterable {
    /// <summary>
    /// </summary>
    [SerializeField]
    [Header("Naming", order = 10)]
    protected string _Custom_Name = "";
    #if NEODROID_DEBUG
    [SerializeField] bool _debugging;
    #endif
    /// <summary>
    /// </summary>
    [Header("Development", order = 90)]
    [SerializeField]
    bool _disables_children = false;

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    [SerializeField]
    bool _editor_reregistering = true;
    #endif

    [SerializeField] bool _unregister_at_disable = false;

    #if NEODROID_DEBUG
    /// <summary>
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }
    #endif
    ///
    public virtual string PrototypingTypeName { get { return this.GetType().Name; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public string Identifier {
      get {
        if (!string.IsNullOrWhiteSpace(this._Custom_Name)) {
          return this._Custom_Name.Trim();
        }

        return this.name + this.PrototypingTypeName;
      }
    }

    /// <summary>
    /// </summary>
    protected void Start() { this.ReRegister(); }

    void ReRegister() {
      try {
        if (this.enabled && this.isActiveAndEnabled) {
          this.Setup();
          this.UnRegisterComponent();
          this.RegisterComponent();
        }
      } catch (ArgumentNullException e) {
        Debug.LogWarning(e);
        Debug.Log($"You must override RegisterComponent and UnRegisterComponent for component {this.GetType()} for gameobject {this.Identifier} in order to Re-register component on every 'OnValidate' while in edit-mode");
      }
    }

    /// <summary>
    /// </summary>
    protected void Awake() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Clear();
      }
    }

    /// <summary>
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

      if (this._unregister_at_disable) {
        this.UnRegisterComponent();
      }
    }

    /// <summary>
    /// </summary>
    void CallOnDisable() { this.OnDisable(); }

    /// <summary>
    /// </summary>
    void CallOnEnable() { this.OnEnable(); }

    /// <summary>
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

      this.ReRegister();
    }

    /// <summary>
    /// </summary>
    protected virtual void Setup() { }

    /// <summary>
    /// </summary>
    protected virtual void Clear() { }

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    void OnValidate() { // Only called in the editor
      if (EditorApplication.isPlaying || !this._editor_reregistering) {
        return;
      }

      this.ReRegister();
    }
    #endif

    /// <summary>
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected abstract void UnRegisterComponent();

    /// <summary>
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected abstract void RegisterComponent();

    /// <summary>
    /// </summary>
    public void RefreshStart() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Start();
      }
    }

    /// <summary>
    /// </summary>
    public void RefreshAwake() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Awake();
      }
    }
  }
}
