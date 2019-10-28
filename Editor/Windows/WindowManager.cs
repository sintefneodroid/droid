#if UNITY_EDITOR
using System;
using UnityEditor;

namespace droid.Editor.Windows {
  /// <summary>
  ///
  /// </summary>
  public class WindowManager : EditorWindow {
    static Type[] _desired_dock_next_toos = {
                                                typeof(RenderTextureConfiguratorWindow),
                                                typeof(CameraSynchronisationWindow),
                                                #if NEODROID_DEBUG
                                                typeof(DebugWindow),
                                                #endif
                                                typeof(SegmentationWindow),
                                                typeof(PrototypingWindow),
                                                typeof(TaskWindow),
                                                typeof(DemonstrationWindow),
                                                typeof(SimulationWindow)
                                            };

    /// <summary>
    ///
    /// </summary>
    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "ShowAllWindows")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "ShowAllWindows")]
    public static void ShowWindow() {
      //Show existing window instance. If one doesn't exist, make one.
      GetWindow<RenderTextureConfiguratorWindow>(_desired_dock_next_toos);
      GetWindow<CameraSynchronisationWindow>(_desired_dock_next_toos);
      #if NEODROID_DEBUG
      GetWindow<DebugWindow>(_desired_dock_next_toos);
      #endif
      GetWindow<SegmentationWindow>(_desired_dock_next_toos);
      GetWindow<PrototypingWindow>(_desired_dock_next_toos);
      GetWindow<TaskWindow>(_desired_dock_next_toos);
      GetWindow<DemonstrationWindow>(_desired_dock_next_toos);
      GetWindow<SimulationWindow>(_desired_dock_next_toos);
    }
  }
}
#endif
