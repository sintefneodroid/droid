using System;
using Neodroid.Runtime.Utilities.BoundingBoxes;
using UnityEngine;

namespace Neodroid.Runtime.Interfaces {
  public interface IPrototypingEnvironment : IEnvironment,
                                             IHasRegister<IActor>,
                                             IHasRegister<IObserver>,
                                             IHasRegister<IConfigurable>,
                                             IHasRegister<IResetable>,
                                             IHasRegister<IDisplayer>,
                                             IHasRegister<IEnvironmentListener> {
    Vector3 TransformDirection(Vector3 transform_forward);
    Vector3 TransformPosition(Vector3 transform_position);
    Vector3 InverseTransformPosition(Vector3 inv_pos);
    Vector3 InverseTransformDirection(Vector3 inv_dir);

    Quaternion TransformRotation(Quaternion transform_rotation);

    event Action PreStepEvent;
    event Action StepEvent;
    event Action PostStepEvent;

    Transform LocalTransform { get; set; }
    BoundingBox PlayableArea { get; set; }

    void Terminate(string reason);
  }
}