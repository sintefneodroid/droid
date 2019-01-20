using System;
using droid.Runtime.Utilities.BoundingBoxes;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc cref="IEnvironment" />
  /// <summary>
  /// </summary>
  public interface IPrototypingEnvironment : IEnvironment,
                                             IHasRegister<IActor>,
                                             IHasRegister<IObserver>,
                                             IHasRegister<IConfigurable>,
                                             IHasRegister<IResetable>,
                                             IHasRegister<IDisplayer>,
                                             IHasRegister<IEnvironmentListener> {
    /// <summary>
    /// </summary>
    Transform Transform { get; }

    /// <summary>
    /// </summary>
    BoundingBox PlayableArea { get; }

    /// <summary>
    /// </summary>
    /// <param name="transform_forward"></param>
    /// <returns></returns>
    Vector3 TransformDirection(Vector3 transform_forward);

    /// <summary>
    /// </summary>
    /// <param name="transform_position"></param>
    /// <returns></returns>
    Vector3 TransformPosition(Vector3 transform_position);

    /// <summary>
    /// </summary>
    /// <param name="inv_pos"></param>
    /// <returns></returns>
    Vector3 InverseTransformPosition(Vector3 inv_pos);

    /// <summary>
    /// </summary>
    /// <param name="inv_dir"></param>
    /// <returns></returns>
    Vector3 InverseTransformDirection(Vector3 inv_dir);

    /// <summary>
    /// </summary>
    /// <param name="transform_rotation"></param>
    /// <returns></returns>
    Quaternion TransformRotation(Quaternion transform_rotation);

    /// <summary>
    /// </summary>
    event Action PreStepEvent;

    /// <summary>
    /// </summary>
    event Action StepEvent;

    /// <summary>
    /// </summary>
    event Action PostStepEvent;

    void Terminate(string reason);
  }
}
