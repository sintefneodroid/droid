using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Categorical" + ObserverComponentMenuPath._Postfix)]
  public class CategoricalObserver : Observer, IHasSingle
  {

    [SerializeField] PrototypingGameObject _categoryProvider;
    [SerializeField] float _observationValue;

    //void OneHotEncoding() { }

    /*public override string PrototypingTypeName {
      get { return "CategoricalObserver"; }
    }*/

    public override IEnumerable<float> FloatEnumerable { get {return new[] {this.ObservationValue};} }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation()
    {
      if(this._categoryProvider is ICategoryProvider provider)
      {
        this.ObservationValue =  provider.CurrentCategoryValue;
      }

    }

    public float ObservationValue
    {
      get => this._observationValue;
      private set => this._observationValue = value;
    }

    public ValueSpace SingleSpace { get; }
  }
}
