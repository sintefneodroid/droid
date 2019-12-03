
using UnityEngine;
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine.Rendering.PostProcessing;

namespace droid.Runtime.Utilities.PostProcessesEffects {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  [PostProcess(typeof(GrayscaleRenderer),
      PostProcessEvent.AfterStack,
      "Neodroid/Grayscale")]
  public sealed class Grayscale : PostProcessEffectSettings {
    /// <summary>
    ///
    /// </summary>
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter {value = 0.5f};
  }

  /// <summary>
  ///
  /// </summary>
  public sealed class GrayscaleRenderer : PostProcessEffectRenderer<Grayscale> {
    static readonly Int32 _blend = Shader.PropertyToID("_Blend");
    static readonly Shader _s = Shader.Find("Neodroid/PostProcessing/Grayscale");

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public override void Render(PostProcessRenderContext context) {
      var sheet = context.propertySheets.Get(_s);
      sheet.properties.SetFloat(_blend, this.settings.blend);
      context.command.BlitFullscreenTriangle(context.source,
                                             context.destination,
                                             sheet,
                                             0);
    }
  }
}
#endif
