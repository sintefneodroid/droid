using System;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers {
  /// <summary>
  /// 
  /// </summary>
  enum Category {
    One_,
    Two_,
    Three_
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Categorical" + ObserverComponentMenuPath._Postfix)]
  public class CategoricalObserver : Observer {
    void OneHotEncoding() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override void UpdateObservation() { throw new NotImplementedException(); }
  }
}
