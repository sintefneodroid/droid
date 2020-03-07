using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  [CustomPropertyDrawer(type : typeof(StringIntDictionary))]
  public class StringIntDictionaryDrawer : SerializableDictionaryDrawer<string, int> {
    protected override SerializableKeyValueTemplate<string, int> GetTemplate() {
      return this.GetGenericTemplate<SerializableStringIntTemplate>();
    }
  }

  class SerializableStringIntTemplate : SerializableKeyValueTemplate<string, int> { }

  [CustomPropertyDrawer(type : typeof(GameObjectFloatDictionary))]
  public class GameObjectFloatDictionaryDrawer : SerializableDictionaryDrawer<GameObject, float> {
    protected override SerializableKeyValueTemplate<GameObject, float> GetTemplate() {
      return this.GetGenericTemplate<SerializableGameObjectFloatTemplate>();
    }
  }

  class SerializableGameObjectFloatTemplate : SerializableKeyValueTemplate<GameObject, float> { }

  [CustomPropertyDrawer(type : typeof(StringGameObjectDictionary))]
  public class StringGameObjectDictionaryDrawer : SerializableDictionaryDrawer<string, GameObject> {
    protected override SerializableKeyValueTemplate<string, GameObject> GetTemplate() {
      return this.GetGenericTemplate<SerializableStringGameObjectTemplate>();
    }
  }

  class SerializableStringGameObjectTemplate : SerializableKeyValueTemplate<string, GameObject> { }
}
#endif
