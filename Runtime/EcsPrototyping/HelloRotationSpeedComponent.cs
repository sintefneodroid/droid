#if ECS_EXISTS
using System;
using Unity.Entities;
using UnityEngine.Serialization;

namespace droid.Runtime.EcsPrototyping
{
// Serializable attribute is for editor support.
    [Serializable]
    public struct HelloRotationSpeed : IComponentData
    {
       public float rotationalMultiplier;
    }

// ComponentDataWrapper is for creating a Monobehaviour representation of this component (for editor support).
    [UnityEngine.DisallowMultipleComponent]
    public class HelloRotationSpeedComponent : ComponentDataWrapper<HelloRotationSpeed> { }
}
#endif
