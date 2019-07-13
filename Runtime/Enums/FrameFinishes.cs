namespace droid.Runtime.Enums {
  /// <summary>
  ///   Determines where in the monobehaviour cycle a frame/step is finished
  /// </summary>
  public enum FrameFinishes {
    /// <summary>
    ///   When ever all scripts has run their respective updates
    ///   NOTE: Not working as expected, does not seem to work with physics engine.
    /// </summary>
    Late_update_,

    /// <summary>
    ///   NOTE: Not working as expected, does not seem to work with physics engine.
    /// </summary>
    End_of_frame_,

    /// <summary>
    ///   When ever the scene has been rendered
    /// </summary>
    On_render_image_,

    /// <summary>
    ///   When ever the scene has been rendered, default
    /// </summary>
    On_post_render_
  }
}
