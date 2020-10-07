﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using Unity.Transforms;

public class sb_ApplyImpulse : SystemBase
{
    protected override void OnUpdate()
    {
        PhysicsWorld physicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;

       
        Entities.ForEach((Entity e, ref PhysicsVelocity vel, ref PhysicsMass mass, in LocalToWorld localToWorld, in d_Distance impulse, in d_Direction direction) => 
        {
            if(direction.Value.y > 0) // check to see if user swiped up
            { 
                
                int rigidbodyIndex = PhysicsWorldExtensions.GetRigidBodyIndex(physicsWorld, e);
            
                //Strength of the swipe
                float distance = impulse.Value / 10;


                //GetDirectionVector
                float3 directionVector = (new float3(direction.Value.x / 100 , 0, 0) + localToWorld.Forward) / 10;

                Debug.Log("DIRECTION VECTOR IS " + directionVector);

                /// Apply a linear impulse to the entity.
                ComponentExtensions.ApplyLinearImpulse(ref vel, mass,directionVector * distance);
            
            //Get rid of the swipe data    
            EntityManager.RemoveComponent<d_Distance>(e);
            EntityManager.RemoveComponent<d_Direction>(e);
            EntityManager.RemoveComponent<tag_Touched>(e);


            }

        }
        ).WithStructuralChanges().Run();
    }
}


public struct d_Distance : IComponentData 
{

    public float Value; 
}

public struct d_Direction : IComponentData
{

    public float2 Value;
}

public struct tag_Touched : IComponentData
{

}