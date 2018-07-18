#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Neodroid.Utilities.SerialisableDictionary {
  [CustomPropertyDrawer(typeof(StringIntDictionary))]
  public class StringIntDictionaryDrawer : SerializableDictionaryDrawer<string, int> {
    protected override SerializableKeyValueTemplate<string, int> GetTemplate() {
      return this.GetGenericTemplate<SerializableStringIntTemplate>();
    }
  }

  class SerializableStringIntTemplate : SerializableKeyValueTemplate<string, int> { }

  [CustomPropertyDrawer(typeof(GameObjectFloatDictionary))]
  public class GameObjectFloatDictionaryDrawer : SerializableDictionaryDrawer<GameObject, float> {
    protected override SerializableKeyValueTemplate<GameObject, float> GetTemplate() {
      return this.GetGenericTemplate<SerializableGameObjectFloatTemplate>();
    }
  }

  class SerializableGameObjectFloatTemplate : SerializableKeyValueTemplate<GameObject, float> { }

  [CustomPropertyDrawer(typeof(StringGameObjectDictionary))]
  public class StringGameObjectDictionaryDrawer : SerializableDictionaryDrawer<string, GameObject> {
    protected override SerializableKeyValueTemplate<string, GameObject> GetTemplate() {
      return this.GetGenericTemplate<SerializableStringGameObjectTemplate>();
    }
  }

  class SerializableStringGameObjectTemplate : SerializableKeyValueTemplate<string, GameObject> { }
}
#endif
