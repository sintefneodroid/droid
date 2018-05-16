using Neodroid.Utilities;
using Neodroid.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Managers.Experimental {
  [AddComponentMenu("Neodroid/Managers/NotUsed/Curriculum")]
  public class CurriculumManager : NeodroidManager {
    [SerializeField] Curriculum _curriculum;

    [SerializeField] bool _draw_levels;

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this._draw_levels) {
        var i = 0;
        var len = this._curriculum._Levels.Length;
        foreach (var level in this._curriculum._Levels) {
          if (level._Configurable_Entries != null && level._Configurable_Entries.Length > 0) {
            var frac = i++ / (float)len;
            foreach (var entry in level._Configurable_Entries) {
              var configurable = GameObject.Find(entry._Configurable_Name);
              if (configurable != null) {
                Gizmos.color = new Color(frac, 0, 1 - frac, 0.1F);
                Gizmos.DrawSphere(configurable.transform.position, entry._Max_Value);
                Gizmos.color = new Color(1, 1, 1, 0.4F);
                Gizmos.DrawWireSphere(configurable.transform.position, entry._Max_Value);
                var pos_up = configurable.transform.position;
                pos_up.y += entry._Max_Value;
                Utilities.Unsorted.NeodroidUtilities.DrawString(i.ToString(), pos_up, new Color(1, 1, 1, 1));
                var pos_left = configurable.transform.position;
                pos_left.x += entry._Max_Value;
                Utilities.Unsorted.NeodroidUtilities.DrawString(i.ToString(), pos_left, new Color(1, 1, 1, 1));
                var pos_forward = configurable.transform.position;
                pos_forward.z += entry._Max_Value;
                Utilities.Unsorted.NeodroidUtilities.DrawString(i.ToString(), pos_forward, new Color(1, 1, 1, 1));
                var pos_down = configurable.transform.position;
                pos_down.y -= entry._Max_Value;
                Utilities.Unsorted.NeodroidUtilities.DrawString(i.ToString(), pos_down, new Color(1, 1, 1, 1));
                var pos_right = configurable.transform.position;
                pos_right.x -= entry._Max_Value;
                Utilities.Unsorted.NeodroidUtilities.DrawString(i.ToString(), pos_right, new Color(1, 1, 1, 1));
                var pos_backward = configurable.transform.position;
                pos_backward.z -= entry._Max_Value;
                Utilities.Unsorted.NeodroidUtilities.DrawString(i.ToString(), pos_backward, new Color(1, 1, 1, 1));
              }
            }
          }
        }
      }
    }
    #endif
  }
}
