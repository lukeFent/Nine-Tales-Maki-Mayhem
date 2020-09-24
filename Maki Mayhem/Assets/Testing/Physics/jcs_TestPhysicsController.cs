using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics.Systems; 
using Unity.Burst;
public class jcs_TestPhysicsController : SystemBase
{

    protected override void OnUpdate()
    {

        float dT = (float)Time.DeltaTime; 

         Entities.WithName("Moving_Player").ForEach((ref d_Move move, ref PhysicsVelocity vel, ref d_Speed speed, ref Rotation rot, ref LocalToWorld local) =>
        {


            vel.Linear.z += local.Forward.z * speed.value * dT;
            vel.Linear.x += local.Forward.x * speed.value * dT;

        }).WithBurst().Schedule();

    }


  
}


public class jcs_TestRotation : SystemBase
{
    private EntityQuery cameraQuery;
   // private EndSimulationEntityCommandBufferSystem system;

    protected override void OnCreate()
    {
        // system = 
       // cameraQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<d_CameraEulerY>());

    }

    protected override void OnUpdate()
    {

        Entities.WithName("Rotating_Player").ForEach((ref Rotation rot, in d_Move move, in d_CameraEuler eulerAngleY) =>
        {
            float targetAngle = math.atan2(move.directionX, move.directionZ) * Mathf.Rad2Deg + eulerAngleY.Value;
            var q = rot.Value;
            var sinP = 2 * ((q.value.w * q.value.y) - (q.value.z * q.value.x));
            var pitch = math.abs(sinP) >= 1 ? math.sign(sinP) * math.PI / 2 : math.asin(sinP);

            //float angle = Mathf.SmoothDampAngle(pitch, targetAngle, ref turnSmoothVelocity, 0.1f);
            rot.Value = Quaternion.Euler(0, targetAngle, 0);


        }).WithBurst().ScheduleParallel();
    }

   

   
}



public class jcs_TestJump : SystemBase
{

    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsVelocity vel, in d_Move moveData, in d_Jump jump) =>
        {

            if (moveData.jump)
            {
                vel.Linear.y = jump.jumpForce;
            }

        }).Run();
    }
}

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class sb_TestTrigger : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem bufferSystem;
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld; 
    protected override void OnCreate()
    {
        bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

    }
    //protected override void OnUpdate()
    //{ 
    //    JobHandle job = new TestTriggerJob(bufferSystem.CreateCommandBuffer()).Schedule(stepPhysicsWorld.Simulation);
    //}

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //EntityCommandBuffer buffer = bufferSystem.CreateCommandBuffer();
        //JobHandle job = new TestTriggerJob(buffer).Schedule(this);
        return inputDeps; 
    }

    struct TestTriggerJob : ITriggerEventsJob
    {
        EntityCommandBuffer commandBuffer; 

        public TestTriggerJob(EntityCommandBuffer cb)
        {
            commandBuffer = cb; 
        }

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;

            commandBuffer.AddComponent<tag_Triggered>(entityA);

        }
    }
}

public struct tag_Triggered : IComponentData
{

}
public struct CharacterControllerStepInput
{
    public PhysicsWorld World;
    public float DeltaTime;
    public float3 Gravity;
    public float3 Up;
    public int MaxIterations;
    public float Tau;
    public float Damping;
    public float SkinWidth;
    public float ContactTolerance;
    public float MaxSlope;
    public int RigidBodyIndex;
    public float3 CurrentVelocity;
    public float MaxMovementSpeed;
}