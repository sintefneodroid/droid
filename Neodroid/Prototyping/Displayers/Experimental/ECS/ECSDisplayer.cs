#if ECS_EXISTS
using droid.Neodroid.Environments;
using droid.Neodroid.Utilities;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Displayers.ECS {
  public abstract class EcsDisplayer : MonoBehaviour {
    PrototypingEnvironment _environment;

    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    [SerializeField] bool _debugging;

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public virtual string Identifier { get { return this.name + "Displayer"; } }

    protected virtual void Awake() {
      this.RegisterComponent();
      this.Setup();
    }

    protected virtual void Setup() { }

    void Start() { }

    public virtual void RefreshStart() { this.Start(); }

    protected virtual void RegisterComponent() {
      this._environment = NeodroidUtilities.MaybeRegisterComponent(this._environment, this);
    }

    public abstract void Display(float value);
    public abstract void Display(double value);
    public abstract void Display(float[] values);
    public abstract void Display(string value);
  }
}
#endif
