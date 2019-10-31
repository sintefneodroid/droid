using System;
using droid.Runtime.Interfaces;
using UnityEditor;
using UnityEngine;

namespace droid.Runtime.GameObjects {
  /// <inheritdoc cref="IRegisterable" />
  /// <summary>
  /// </summary>
  public abstract class PrototypingGameObject : MonoBehaviour,
                                                IRegisterable {
    #if NEODROID_DEBUG
    /// <summary>
    /// </summary>

    [field : SerializeField]
    public bool Debugging { get; set; }
    #endif

    ///
    public virtual string PrototypingTypeName { get { return this.GetType().Name; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public string Identifier {
      get {
        if (!string.IsNullOrWhiteSpace(this.CustomName)) {
          return this.CustomName.Trim();
        }

        return this.name + this.PrototypingTypeName;
      }
    }

    /// <summary>
    /// </summary>
    [field : Header("Development", order = 90)]
    [field : SerializeField]
    public Boolean DisablesChildren { get; set; } = false;

    [field : SerializeField]
    public Boolean UnregisterAtDisable { get; set; } = false;

    /// <summary>
    /// </summary>
    [field : SerializeField]
    protected String CustomName { get; set; } = "";

    /// <summary>
    /// </summary>
    protected void Start() { this.ReRegister(); }

    /// <summary>
    ///
    /// </summary>
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
      if (this.DisablesChildren) {
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

      if (this.UnregisterAtDisable) {
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
      if (this.DisablesChildren) {
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

      #if UNITY_EDITOR
      if (EditorApplication.isPlayingOrWillChangePlaymode) {
        return;
      }
      #endif

      this.ReRegister();
    }

    /// <summary>
    /// </summary>
    public virtual void Setup() { this.PreSetup(); }

    /// <summary>
    /// </summary>
    public virtual void PreSetup() { }

    /// <summary>
    ///
    /// </summary>
    public virtual void RemotePostSetup() {  }

    /// <summary>
    /// </summary>
    protected virtual void Clear() { }

    /// <summary>
    /// </summary>
    public virtual void Tick() { }

    /// <summary>
    ///
    /// </summary>
    public virtual void PrototypingReset() {  }

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    void OnValidate() { // Only called in the editor
      if (!EditorApplication.isPlayingOrWillChangePlaymode) {
        this.ReRegister();
      }
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
