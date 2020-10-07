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



//These are a series of systems that repersent how physics systems can be used. They include:
//sb_TestPhysicsController -- moves the entity continually forward
//sb_TestRotation -- rotate entity to match camera's y EulerAngle (works with CameraTarget script)
//sb_TestJump -- jumps with spacebar (uses d_Move which reads the jump command in jcs_PlayerInput)
//jcs_TestTrigger -- shows how triggers can be accessed
//jcs_TestCollisions -- shows how collisions can be accessed


public class sb_TestPhysicsController : SystemBase
{

    protected override void OnUpdate()
    {

        float dT = (float)Time.DeltaTime; 

         Entities.WithName("Moving_Player").ForEach((ref d_Move move, ref PhysicsVelocity vel, ref d_Speed speed, ref Rotation rot, ref LocalToWorld local) =>
        {
            //vel.Linear.z += local.Forward.z * speed.value * dT;
            //vel.Linear.x += local.Forward.x * speed.value * dT;

        }).WithBurst().Schedule();

    }


  
}


public class sb_TestRotation : SystemBase
{
   
    protected override void OnUpdate()
    {

        Entities.WithName("Rotating_Player").ForEach((ref Rotation rot, in d_Move move, in d_CameraEuler eulerAngleY) =>
        {
            //float targetAngle = math.atan2(move.directionX, move.directionZ) * Mathf.Rad2Deg + eulerAngleY.Value;
            //rot.Value = Quaternion.Euler(0, targetAngle, 0);


        }).WithBurst().ScheduleParallel();
    }

   

   
}



public class sb_TestJump : SystemBase
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
public class jcs_TestTrigger : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem bufferSystem;
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;
    EntityCommandBuffer buffer; 

   
    protected override void OnCreate()
    {
        bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        buffer = bufferSystem.CreateCommandBuffer();


    }
    

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        //EntityCommandBuffer buffer = bufferSystem.CreateCommandBuffer();
        JobHandle job = new TestTriggerJob { physicsVelocityEntities = GetComponentDataFromEntity<PhysicsVelocity>()}.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        bufferSystem.AddJobHandleForProducer(job);
        return job; 
    }


    [BurstCompile]
    struct TestTriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<PhysicsVelocity> physicsVelocityEntities; 

        //public TestTriggerJob(EntityCommandBuffer cb)
        //{
        //    commandBuffer = cb; 
        //}

        public void Execute(TriggerEvent triggerEvent)
        {
          
            if(physicsVelocityEntities.HasComponent(triggerEvent.Entities.EntityA))
            {
                PhysicsVelocity aVelocity = physicsVelocityEntities[triggerEvent.Entities.EntityA];
                aVelocity.Linear.y = 100f;
                physicsVelocityEntities[triggerEvent.Entities.EntityA] = aVelocity;


            }

            if (physicsVelocityEntities.HasComponent(triggerEvent.Entities.EntityB))
            {
                PhysicsVelocity bVelocity = physicsVelocityEntities[triggerEvent.Entities.EntityB];
                bVelocity.Linear.y = 100f;
                physicsVelocityEntities[triggerEvent.Entities.EntityB] = bVelocity;


            }


        }
    }
}




//[UpdateAfter(typeof(EndFramePhysicsSystem))]
//public class jcs_TestCollisions : JobComponentSystem
//{
//    EndSimulationEntityCommandBufferSystem bufferSystem;
//    BuildPhysicsWorld buildPhysicsWorld;
//    StepPhysicsWorld stepPhysicsWorld;
//    EntityCommandBuffer buffer;


//    protected override void OnCreate()
//    {
//        bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
//        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
//        buffer = bufferSystem.CreateCommandBuffer();


//    }


//    protected override JobHandle OnUpdate(JobHandle inputDeps)
//    {

//        //EntityCommandBuffer buffer = bufferSystem.CreateCommandBuffer();
//        JobHandle job = new TestCollisionJob { physicsVelocityEntities = GetComponentDataFromEntity<PhysicsVelocity>() }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
//        //bufferSystem.AddJobHandleForProducer(job);
//        return job;
//    }


//    [BurstCompile]
//    struct TestCollisionJob : ICollisionEventsJob
//    {
//        public ComponentDataFromEntity<PhysicsVelocity> physicsVelocityEntities;

//        public void Execute(CollisionEvent collisionEvent)
//        {

//            if (physicsVelocityEntities.HasComponent(collisionEvent.Entities.EntityA))
//            {
//                PhysicsVelocity aVelocity = physicsVelocityEntities[collisionEvent.Entities.EntityA];
//                aVelocity.Linear.y = 1;
//                physicsVelocityEntities[collisionEvent.Entities.EntityA] = aVelocity;


//            }

//            if (physicsVelocityEntities.HasComponent(collisionEvent.Entities.EntityB))
//            {
//                PhysicsVelocity bVelocity = physicsVelocityEntities[collisionEvent.Entities.EntityB];
//                bVelocity.Linear.y = 100f;
//                physicsVelocityEntities[collisionEvent.Entities.EntityB] = bVelocity;


//            }
//        }
//    }
//}




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