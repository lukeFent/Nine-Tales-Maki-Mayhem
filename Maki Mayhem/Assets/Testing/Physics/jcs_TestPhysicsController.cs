using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics; 
public class jcs_TestPhysicsController : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

         Entities.ForEach((ref d_Move move, ref PhysicsVelocity vel, ref EntSpeedData speed, ref LocalToWorld trans) =>
        {
            vel.Linear.z = move.directionZ * speed.speed;
            vel.Linear.x = move.directionX * speed.speed;

            //1
            //float3 cammyRight = CameraTarget.instance.cameraTransform.TransformDirection(Vector3.right);
            //float3 cammyFront = CameraTarget.instance.cameraTransform.TransformDirection(Vector3.forward);
            //cammyRight.y = 0;
            //cammyFront.y = 0;
            //math.normalize(cammyRight);
            //math.normalize(cammyFront);

            //2
            //float3 movement = trans.Right * move.directionX * speed.speed * UnityEngine.Time.deltaTime;
            //movement += trans.Forward * move.directionZ * speed.speed * UnityEngine.Time.deltaTime;
            //movement.y = 0.0f;
            //float3 targetPosition = trans.Position + movement;
            //float3 movementDirection = targetPosition - trans.Position;
            //movementDirection.y = 0.0f;
            //math.normalize(movementDirection);
            //vel.Linear = movementDirection; 


            //3
            //float3 currentFacingXZ = trans.Forward;
            //currentFacingXZ.y = 0.0f;
            //float angleDifferenceForward = Vector3.SignedAngle(movementDirection, currentFacingXZ, Vector3.up);
            //Vector3 targetAngularVelocity = Vector3.zero;
            //targetAngularVelocity.y = angleDifferenceForward * Mathf.Deg2Rad;
            //Vector3 currentRotation = trans.Forward;
            //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, cammyFront * vertical + cammyRight * horizontal, rotateSpeed * Time.fixedDeltaTime, 0.0f));
            //Quaternion syncRotation = transform.rotation;
            //rigidBody.MovePosition(targetPosition);
            //if (movement.sqrMagnitude > Mathf.Epsilon)
            //{
            //    rigidBody.MoveRotation(transform.rotation);
            //}



            //float x = move.directionX * speed.speed;
            //float z = move.directionZ * speed.speed;        
            //float3 moveVector = new float3(x, vel.Linear.y, z) ;
            //vel.Linear = moveVector * transform.Forward; 


        }).Run();

        return inputDeps; 
    }


  
}


public class jcs_TestRotation : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        Entities.ForEach((ref d_Move move, ref Rotation rot, ref LocalToWorld local) =>
        {
            float3 moveVector = new float3(move.directionX, 0, move.directionZ) + local.Position;
            float3 direction = moveVector - local.Position;
            math.forward(rot.Value);
            rot.Value = Quaternion.LookRotation(direction, new Vector3(0, 1, 0));

        }).Run();

        return inputDeps;
    }



}
