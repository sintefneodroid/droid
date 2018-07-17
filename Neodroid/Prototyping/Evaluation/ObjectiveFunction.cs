using System;
using System.Collections.Generic;
using System.Globalization;
using droid.Neodroid.Environments;
using droid.Neodroid.Prototyping.Evaluation.Terms;
using droid.Neodroid.Utilities.EventRecipients.droid.Neodroid.Utilities.Unsorted;
using droid.Neodroid.Utilities.GameObjects;
using droid.Neodroid.Utilities.Interfaces;
using UnityEngine;
using Object = System.Object;

namespace droid.Neodroid.Prototyping.Evaluation {
  /// <inheritdoc cref="ObjectiveFunction" />
  /// <summary>
  /// </summary>
  [Serializable]
  public abstract class ObjectiveFunction : PrototypingGameObject,
                                            IHasRegister<Term> {
    /// <summary>
    ///
    /// </summary>
    public float SolvedThreshold {
      get { return this._solved_threshold; }
      set { this._solved_threshold = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return ""; } }

    /// <summary>
    ///
    /// </summary>
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="term"></param>
    public virtual void Register(Term term) { this.Register(term, term.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="term"></param>
    /// <param name="identifier"></param>
    public void Register(Term term, string identifier) {
      if (!this._Extra_Terms_Dict.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"ObjectiveFunction {this.name} has registered term {identifier}");
        }
        #endif

        this._Extra_Terms_Dict.Add(identifier, term);
        this._Extra_Term_Weights.Add(term, 1);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log(
              $"WARNING! Please check for duplicates, ObjectiveFunction {this.name} already has term {identifier} registered");
        }
        #endif
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="identifier"></param>
    public void UnRegisterTerm(string identifier) {
      if (this._Extra_Terms_Dict.ContainsKey(identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"ObjectiveFunction {this.name} unregistered term {identifier}");
        }
        #endif

        this._Extra_Term_Weights.Remove(this._Extra_Terms_Dict[identifier]);
        this._Extra_Terms_Dict.Remove(identifier);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clear() {
      this._Extra_Term_Weights.Clear();
      this._Extra_Terms_Dict.Clear();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="term"></param>
    public void UnRegister(Term term) { this.UnRegisterTerm(term.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected sealed override void Setup() {
      //foreach (var go in this._extra_terms_external)
      //  this.Register(go);

      if (!this.ParentEnvironment) {
        this.ParentEnvironment = FindObjectOfType<PrototypingEnvironment>();
      }

      this.PostSetup();
    }

    protected virtual void PostSetup() { }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void SignalString(DataPoller recipient) {
      recipient.PollData(
          $"{this._last_signal.ToString(CultureInfo.InvariantCulture)}, {this._Episode_Return}");
    }

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    public float _Episode_Return;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public abstract float InternalEvaluate();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();
      signal += this.EvaluateExtraTerms();

      //signal = signal * Mathf.Pow(this._internal_discount_factor, this._environment.CurrentFrameNumber);

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(signal);
      }
      #endif

      this._last_signal = signal;

      this._Episode_Return += signal;

      return signal;
    }

    /// <summary>
    ///
    /// </summary>
    public void Reset() {
      this._last_signal = 0;
      this._Episode_Return = 0;
      this.InternalReset();
    }

    /// <summary>
    ///
    /// </summary>
    public abstract void InternalReset();

    /// <summary>
    ///
    /// </summary>
    /// <param name="term"></param>
    /// <param name="new_weight"></param>
    public virtual void AdjustExtraTermsWeights(Term term, float new_weight) {
      if (this._Extra_Term_Weights.ContainsKey(term)) {
        this._Extra_Term_Weights[term] = new_weight;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual float EvaluateExtraTerms() {
      float extra_terms_output = 0;
      foreach (var term in this._Extra_Terms_Dict.Values) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log($"Extra term: {term}");
        }
        #endif

        extra_terms_output += this._Extra_Term_Weights[term] * term.Evaluate();
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Extra terms signal: {extra_terms_output}");
      }
      #endif
      return extra_terms_output;
    }

    #region Fields

    [Header("References", order = 100), SerializeField]
    //[SerializeField]float _internal_discount_factor = 1.0f;
    PrototypingEnvironment _environment;

    //[SerializeField] Term[] _extra_terms_external;

    [SerializeField] protected Dictionary<string, Term> _Extra_Terms_Dict = new Dictionary<string, Term>();

    [SerializeField] protected Dictionary<Term, float> _Extra_Term_Weights = new Dictionary<Term, float>();

    [Header("General", order = 101), SerializeField]
    float _solved_threshold;

    [SerializeField] float _last_signal;

    #endregion
  }
}
