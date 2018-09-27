using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Prototyping.Motors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Neodroid.Samples.MultiArmedBandit {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "MultiArmedBanditMotor" + MotorComponentMenuPath._Postfix)]
  public class MultiArmedBanditMotor : Motor {
    [SerializeField] Material[] _indicators;
    [SerializeField] Color _inactive_color;
    [SerializeField] Color _win_color = Color.green;
    [SerializeField] Color _lose_color = Color.red;
    [SerializeField] float[] _win_amounts;
    [SerializeField] float[] _win_likelihoods;
    [SerializeField] int _last_index;
    [SerializeField] bool _won;

    /// <summary>
    /// 
    /// </summary>
    public Single[] WinAmounts { get { return this._win_amounts; } set { this._win_amounts = value; } }

    /// <summary>
    /// 
    /// </summary>
    public Int32 LastIndex { get { return this._last_index; } set { this._last_index = value; } }

    /// <summary>
    /// 
    /// </summary>
    public Boolean Won { get { return this._won; } set { this._won = value; } }

    /// <summary>
    /// 
    /// </summary>
    public Single[] WinLikelihoods {
      get { return this._win_likelihoods; }
      set { this._win_likelihoods = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      var mvs = this.MotionValueSpace;
      mvs._Min_Value = 0;
      mvs._Max_Value = this._indicators.Length - 1;
      this.MotionValueSpace = mvs;
      if (this._win_likelihoods == null || this._win_likelihoods.Length == 0) {
        this._win_likelihoods = new Single[this._indicators.Length];
        for (var index = 0; index < this._indicators.Length; index++) {
          this._win_likelihoods[index] = 0.5f;
        }
      }

      if (this._win_amounts == null || this._win_amounts.Length == 0) {
        this._win_amounts = new Single[this._indicators.Length];
        for (var index = 0; index < this._indicators.Length; index++) {
          this._win_amounts[index] = 10f;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotorMotion motion) {
      foreach (var indicator in this._indicators) {
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
      if (random_value <= this._win_likelihoods[this._last_index]) {
        this._indicators[this._last_index].color = this._win_color;
        this._won = true;
      } else {
        this._indicators[this._last_index].color = this._lose_color;
        this._won = false;
      }
    }
  }
}
