using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Prototyping.Motors;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Neodroid.Samples.MultiArmedBandit {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "MultiArmedBandit" + MotorComponentMenuPath._Postfix)]
  public class MultiArmedBanditMotor : Motor {
    [SerializeField] Color _inactive_color = Color.yellow;
    [FormerlySerializedAs("_indicators")] [SerializeField] protected Material[] _Indicators;
    [SerializeField] int _last_index;
    [SerializeField] Color _lose_color = Color.red;
    [FormerlySerializedAs("_win_amounts")] [SerializeField] protected float[] _Win_Amounts;
    [SerializeField] Color _win_color = Color.green;
    [FormerlySerializedAs("_win_likelihoods")] [SerializeField] protected float[] _Win_Likelihoods;
    [SerializeField] bool _won;

    /// <summary>
    /// </summary>
    public Single[] WinAmounts { get { return this._Win_Amounts; } set { this._Win_Amounts = value; } }

    /// <summary>
    /// </summary>
    public Int32 LastIndex { get { return this._last_index; } set { this._last_index = value; } }

    /// <summary>
    /// </summary>
    public Boolean Won { get { return this._won; } set { this._won = value; } }

    /// <summary>
    /// </summary>
    public Single[] WinLikelihoods {
      get { return this._Win_Likelihoods; }
      set { this._Win_Likelihoods = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      var mvs = this.MotionValueSpace;
      mvs._Min_Value = 0;
      mvs._Max_Value = this._Indicators.Length - 1;
      this.MotionValueSpace = mvs;
      if (this._Win_Likelihoods == null || this._Win_Likelihoods.Length == 0) {
        this._Win_Likelihoods = new Single[this._Indicators.Length];
        for (var index = 0; index < this._Indicators.Length; index++) {
          this._Win_Likelihoods[index] = 0.5f;
        }
      }

      if (this._Win_Amounts == null || this._Win_Amounts.Length == 0) {
        this._Win_Amounts = new Single[this._Indicators.Length];
        for (var index = 0; index < this._Indicators.Length; index++) {
          this._Win_Amounts[index] = 10f;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotorMotion motion) {
      foreach (var indicator in this._Indicators) {
        indicator.color = this._inactive_color;
      }

      var index = (int)motion.Strength;

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"MultiArmedBandit got index {index}");
      }
      #endif

      this._last_index = index;

      var random_value = Random.Range(0f, 1f);
      if (random_value <= this._Win_Likelihoods[this._last_index]) {
        this._Indicators[this._last_index].color = this._win_color;
        this._won = true;
      } else {
        this._Indicators[this._last_index].color = this._lose_color;
        this._won = false;
      }
    }
  }
}
