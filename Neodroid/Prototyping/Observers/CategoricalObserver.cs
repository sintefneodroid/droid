using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  enum Category {
    One_,
    Two_,
    Three_
  }

  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Categorical" + ObserverComponentMenuPath._Postfix)]
  public class CategoricalObserver : Observer {
    void OneHotEncoding() { }
  }
}
