using System;
using droid.Runtime.ScriptableObjects.Deprecated;
using droid.Runtime.Utilities.Drawing;
using UnityEngine;

namespace droid.Runtime.Managers.Experimental {
  [AddComponentMenu("Neodroid/Managers/NotUsed/Curriculum")]
  public class CurriculumManager : AbstractNeodroidManager {
    [SerializeField] Curriculum _curriculum = null;

    [SerializeField] bool _draw_levels = false;

    public Curriculum Curriculum1 { get { return this._curriculum; } set { this._curriculum = value; } }

    public Boolean DrawLevels { get { return this._draw_levels; } set { this._draw_levels = value; } }

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
                Gizmos.color = new Color(frac,
                                         0,
                                         1 - frac,
                                         0.1F);
                var position = configurable.transform.position;
                Gizmos.DrawSphere(position, entry._Max_Value);
                Gizmos.color = new Color(1,
                                         1,
                                         1,
                                         0.4F);
                Gizmos.DrawWireSphere(position, entry._Max_Value);
                var pos_up = position;
                pos_up.y += entry._Max_Value;
                NeodroidUtilities.DrawString(i.ToString(),
                                             pos_up,
                                             new Color(1,
                                                       1,
                                                       1,
                                                       1));
                var pos_left = position;
                pos_left.x += entry._Max_Value;
                NeodroidUtilities.DrawString(i.ToString(),
                                             pos_left,
                                             new Color(1,
                                                       1,
                                                       1,
                                                       1));
                var pos_forward = position;
                pos_forward.z += entry._Max_Value;
                NeodroidUtilities.DrawString(i.ToString(),
                                             pos_forward,
                                             new Color(1,
                                                       1,
                                                       1,
                                                       1));
                var pos_down = position;
                pos_down.y -= entry._Max_Value;
                NeodroidUtilities.DrawString(i.ToString(),
                                             pos_down,
                                             new Color(1,
                                                       1,
                                                       1,
                                                       1));
                var pos_right = position;
                pos_right.x -= entry._Max_Value;
                NeodroidUtilities.DrawString(i.ToString(),
                                             pos_right,
                                             new Color(1,
                                                       1,
                                                       1,
                                                       1));
                var pos_backward = position;
                pos_backward.z -= entry._Max_Value;
                NeodroidUtilities.DrawString(i.ToString(),
                                             pos_backward,
                                             new Color(1,
                                                       1,
                                                       1,
                                                       1));
              }
            }
          }
        }
      }
    }
    #endif
    public override void Setup() { throw new NotImplementedException(); }
  }
}
