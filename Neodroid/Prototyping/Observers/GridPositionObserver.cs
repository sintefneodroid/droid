using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "GridPosition" + ObserverComponentMenuPath._Postfix)]
  public class GridPositionObserver : Observer,
                                      IHasSingle {
    /// <summary>
    ///
    /// </summary>
    int[,] _grid;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    int _height;

    [Header("Observation", order = 103)]
    [SerializeField]
    float _observation_value;

    [SerializeField] ValueSpace _observation_value_space;
    [SerializeField] int _width;

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingType { get { return "Value"; } }

    /// <summary>
    ///
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      set {
        this._observation_value = this.NormaliseObservationUsingSpace
                                      ? this._observation_value_space.ClipNormaliseRound(value)
                                      : value;
      }
    }

    public ValueSpace SingleSpace { get { return this._observation_value_space; } }

    protected override void PreSetup() {
      this._grid = new int[this._width, this._height];

      var k = 0;
      for (var i = 0; i < this._width; i++) {
        for (var j = 0; j < this._height; j++) {
          this._grid[i, j] = k++;
        }
      }

      this.FloatEnumerable = new[] {this.ObservationValue};
    }

    public override void UpdateObservation() {
      var x = this.transform.position.x + this._width;
      var z = this.transform.position.z + this._height;

      this.ObservationValue = this._grid[(int)x, (int)z];

      this.FloatEnumerable = new[] {this.ObservationValue};
    }
  }
}
