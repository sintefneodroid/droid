using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CreateAssetMenu(fileName = "Example Asset",
      menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath + "Example Asset",
      order = 1)]
  public class Example : ScriptableObject {
    /// <summary>
    /// </summary>
    [SerializeField]
    GameObjectFloatDictionary _game_object_float_store =
        GameObjectFloatDictionary.New<GameObjectFloatDictionary>();

    /// <summary>
    /// </summary>
    [SerializeField]
    StringIntDictionary _string_integer_store = StringIntDictionary.New<StringIntDictionary>();

    /// <summary>
    /// </summary>
    Dictionary<string, int> StringIntegers { get { return this._string_integer_store._Dict; } }

    /// <summary>
    /// </summary>
    Dictionary<GameObject, float> Screenshots { get { return this._game_object_float_store._Dict; } }
  }
}
