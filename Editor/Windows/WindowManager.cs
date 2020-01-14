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
      GetWindow<RenderTextureConfiguratorWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<CameraSynchronisationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      #if NEODROID_DEBUG
      GetWindow<DebugWindow>(desiredDockNextTo : _desired_dock_next_toos);
      #endif
      GetWindow<SegmentationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<PrototypingWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<TaskWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<DemonstrationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<SimulationWindow>(desiredDockNextTo : _desired_dock_next_toos);
    }
  }
}
#endif
