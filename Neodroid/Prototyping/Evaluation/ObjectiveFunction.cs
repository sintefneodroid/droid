using System;
using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Prototyping.Evaluation.Terms;
using Neodroid.Prototyping.Motors;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Evaluation {
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
    public override String PrototypingType { get { return ""; } }

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
      this._Extra_Term_Weights = new Dictionary<Term, float>();
      this._Extra_Terms_Dict = new Dictionary<string, Term>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="term"></param>
    public void UnRegister(Term term) { this.UnRegisterTerm(term.Identifier); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      //foreach (var go in this._extra_terms_external)
      //  this.Register(go);

      if (!this.ParentEnvironment) {
        this.ParentEnvironment = FindObjectOfType<PrototypingEnvironment>();
      }
    }

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

      signal = signal * Mathf.Pow(this._in_mdp_discount_factor, this._environment.CurrentFrameNumber);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(signal);
      }
      #endif

      return signal;
    }

    /// <summary>
    ///
    /// </summary>
    public void Reset() { this.InternalReset(); }

    /// <summary>
    ///
    /// </summary>
    public virtual void InternalReset() { }

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

    [Header("References", order = 100)]
    [SerializeField]
    float _in_mdp_discount_factor = 1.0f;

    [SerializeField] PrototypingEnvironment _environment;

    //[SerializeField] Term[] _extra_terms_external;

    [SerializeField] protected Dictionary<string, Term> _Extra_Terms_Dict = new Dictionary<string, Term>();

    [SerializeField] protected Dictionary<Term, float> _Extra_Term_Weights = new Dictionary<Term, float>();

    [Header("General", order = 101)]
    [SerializeField]
    float _solved_threshold;

    #endregion
  }
}
