using System;
using System.Linq;
using droid.Runtime.GameObjects.StatusDisplayer.EventRecipients;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actuators;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace droid.Samples.MultiArmedBandit.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "MultiArmedBandit"
                    + ActuatorComponentMenuPath._Postfix)]
  public class MultiArmedBanditActuator : Actuator {
    [SerializeField] Color _inactive_color = Color.yellow;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected Material[] _Indicators;

    [SerializeField] int _last_index;
    [SerializeField] Color _lose_color = Color.red;

    [SerializeField] protected float[] _Win_Amounts;

    [SerializeField] Color _win_color = Color.green;

    [SerializeField] protected float[] _Win_Likelihoods;

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
    public override void Setup() {
      var mvs = this.MotionSpace;
      mvs.Min = 0;
      mvs.Max = 2;
      mvs.DecimalGranularity = 0;
      this.MotionSpace = mvs;
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

    public void UpdatePayoutArm1(Text amount) { this.UpdatePayoutArm(0, float.Parse(amount.text)); }
    public void UpdatePayoutArm2(Text amount) { this.UpdatePayoutArm(1, float.Parse(amount.text)); }
    public void UpdatePayoutArm3(Text amount) { this.UpdatePayoutArm(2, float.Parse(amount.text)); }

    public void GetPayoutArm1(DataPoller recipient) { recipient.PollData(this.GetPayoutArm(0)); }
    public void GetPayoutArm2(DataPoller recipient) { recipient.PollData(this.GetPayoutArm(1)); }
    public void GetPayoutArm3(DataPoller recipient) { recipient.PollData(this.GetPayoutArm(2)); }

    public void GetPctArm1(DataPoller recipient) { recipient.PollData(this.GetPctArm(0)); }
    public void GetPctArm2(DataPoller recipient) { recipient.PollData(this.GetPctArm(1)); }
    public void GetPctArm3(DataPoller recipient) { recipient.PollData(this.GetPctArm(2)); }

    public void UpdatePercentageArm1(Text amount) { this.UpdatePercentageArm(0, float.Parse(amount.text)); }
    public void UpdatePercentageArm2(Text amount) { this.UpdatePercentageArm(1, float.Parse(amount.text)); }
    public void UpdatePercentageArm3(Text amount) { this.UpdatePercentageArm(2, float.Parse(amount.text)); }

    void UpdatePayoutArm(int index, float amount) { this._Win_Amounts[index] = amount; }

    string GetPayoutArm(int index) { return this._Win_Amounts[index].ToString("F"); }

    string GetPctArm(int index) { return this._Win_Likelihoods[index].ToString("F"); }

    void UpdatePercentageArm(int index, float amount) { this._Win_Likelihoods[index] = amount; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
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
      if (random_value < this._Win_Likelihoods[this._last_index]) {
        this._Indicators[this._last_index].color = this._win_color;
        this._won = true;
      } else {
        this._Indicators[this._last_index].color = this._lose_color;
        this._won = false;
      }
    }

    public override string[] InnerMotionNames => this._Indicators.Select(m => this.Identifier).ToArray();
  }
}
