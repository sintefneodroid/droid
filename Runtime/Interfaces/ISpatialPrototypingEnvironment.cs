using System;
using droid.Runtime.GameObjects.BoundingBoxes;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc cref="IEnvironment" />
  /// <summary>
  /// </summary>
  public interface ISpatialPrototypingEnvironment : IEnvironment,
                                                    IHasRegister<ISensor>,
                                                    IHasRegister<IConfigurable>,
                                                    IHasRegister<IUnobservable>,
                                                    IHasRegister<IDisplayer> {
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
    /// <param name="point"></param>
    /// <returns></returns>
    Vector3 TransformPoint(Vector3 point);

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    Vector3 InverseTransformPoint(Vector3 point);

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
    ///
    /// </summary>
    /// <param name="transform_rotation"></param>
    /// <returns></returns>
    Quaternion InverseTransformRotation(Quaternion transform_rotation);

    /// <summary>
    /// </summary>
    event Action PreStepEvent;

    /// <summary>
    /// </summary>
    event Action StepEvent;

    /// <summary>
    /// </summary>
    event Action PostStepEvent;

    /// <summary>
    ///
    /// </summary>
    IEpisodicObjectiveFunction ObjectiveFunction { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reason"></param>
    void Terminate(string reason);
  }
}
