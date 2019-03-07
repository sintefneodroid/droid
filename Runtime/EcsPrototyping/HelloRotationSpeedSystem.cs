#if ECS_EXISTS
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// This system updates all entities in the scene with both a HelloRotationSpeed and Rotation component.
namespace droid.Runtime.EcsPrototyping
{
    public class HelloRotationSpeedSystem : JobComponentSystem
    {
        // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
        [BurstCompile]
        struct HelloRotationSpeedJob : IJobProcessComponentData<Rotation, HelloRotationSpeed>
        {
            // The [ReadOnly] attribute tells the job scheduler that this job cannot write to dT.
            [ReadOnly] public float DeltaT;

            // rotation is read/write in this job
            public void Execute(ref Rotation rotation, [ReadOnly] ref HelloRotationSpeed rotSpeed)
            {
                // Rotate something about its up vector at the speed given by HelloRotationSpeed.
                rotation.Value =
 math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), rotSpeed.rotationalMultiplier * this.DeltaT));
            }
        }

        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new HelloRotationSpeedJob(){
                DeltaT = Time.deltaTime
            };

            return job.Schedule(this, inputDependencies);
        }
    }
}
#endif
