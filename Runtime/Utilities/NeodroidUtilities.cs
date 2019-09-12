using UnityEngine;

namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static partial class NeodroidDefaultsUtilities {
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static AnimationCurve DefaultAnimationCurve() {
      return new AnimationCurve(new Keyframe(1, 1), new Keyframe(0, 0));
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public static Gradient DefaultGradient() {
      var gradient = new Gradient {
                                      // The number of keys must be specified in this array initialiser
                                      colorKeys = new[] {
                                                            // Add your colour and specify the stop point
                                                            new GradientColorKey(new Color(1, 1, 1), 0),
                                                            new GradientColorKey(new Color(1, 1, 1), 1f),
                                                            new GradientColorKey(new Color(1, 1, 1), 0)
                                                        },
                                      // This sets the alpha to 1 at both ends of the gradient
                                      alphaKeys = new[] {
                                                            new GradientAlphaKey(1, 0),
                                                            new GradientAlphaKey(1, 1),
                                                            new GradientAlphaKey(1, 0)
                                                        }
                                  };

      return gradient;
    }
  }
}
