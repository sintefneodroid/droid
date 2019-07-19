using System;
using UnityEngine;

namespace droid.Runtime.ScriptableObjects.Deprecated {
  [CreateAssetMenu(fileName = "Curriculum",
      menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath + "Curriculum",
      order = 1)]
  public class Curriculum : ScriptableObject {
    public Level[] _Levels;
  }

  [Serializable]
  public struct Level {
    public ConfigurableEntry[] _Configurable_Entries;
    public float _Min_Reward;
    public float _Max_Reward;
  }

  [Serializable]
  public struct ConfigurableEntry {
    public string _Configurable_Name;
    public float _Min_Value;
    public float _Max_Value;
  }
}
