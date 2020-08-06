using UnityEngine;
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine.Rendering.PostProcessing;

namespace droid.Runtime.Utilities.PostProcessesEffects {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  [PostProcess(renderer : typeof(GrayscaleRenderer),
               eventType : PostProcessEvent.AfterStack,
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
    static readonly int _blend = Shader.PropertyToID("_Blend");
    static readonly Shader _s = Shader.Find("Neodroid/PostProcessing/Grayscale");

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public override void Render(PostProcessRenderContext context) {
      var sheet = context.propertySheets.Get(shader : _s);
      sheet.properties.SetFloat(nameID : _blend, value : this.settings.blend);
      context.command.BlitFullscreenTriangle(source : context.source,
                                             destination : context.destination,
                                             propertySheet : sheet,
                                             0);
    }
  }
}
#endif
