using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;
using Lean.Touch; 


public class HandleSwipe : MonoBehaviour
{
    public ConvertedEntityHolder entityHolder;
    private Entity playerEntity;
    private EntityManager manager; 
    public LeanSwipeBase swipe;

    BuildPhysicsWorld buildPhysicsWorld;
    CollisionWorld collisionWorld;

    private void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager; 
    }


    protected virtual void OnEnable()
    {
        LeanTouch.OnFingerSwipe += HandleFingerSwipe;

    }

    protected virtual void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= HandleFingerSwipe;
    }
    private void Start()
    {
        playerEntity = entityHolder.GetEntity();


    }

    //Gets called by the OnDelta Event, subscribed via editor with Lean Finger Swipe
    public void GetSwipeDelta(Vector2 delta)
    {
        Debug.Log("DELTA IS " + delta.x + " , " + delta.y);
        manager.AddComponent<d_Direction>(playerEntity);
        manager.SetComponentData(playerEntity, new d_Direction { Value = delta });
    }


    //Gets called by the On Distance Event, subscribed via editor with Lean Finger Swipe
    public void GetDistance(float distance)
    {
        Debug.Log("DISTANCE IS  " + distance);
        manager.AddComponent<d_Distance>(playerEntity);
        manager.SetComponentData(playerEntity, new d_Distance { Value = distance });

    }



    //DOTS raycast function, returns an entity, can be jobified, but haven't bothered yet
    public Entity RayCastToEntity(float3 from, float3 to)
    {
        buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;

        RaycastInput raycastInput = new RaycastInput { Start = from, End = to, Filter = new CollisionFilter { BelongsTo = ~0u, CollidesWith = ~0u, GroupIndex = 0 } };


        //Note this is using Unity.Physics instead of UnityEngine.Physics
        Unity.Physics.RaycastHit raycastHit = new Unity.Physics.RaycastHit();

        if(collisionWorld.CastRay(raycastInput, out raycastHit))
        {
            //hitSomething
            Entity hitEntity = buildPhysicsWorld.PhysicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
            return hitEntity; 
           
        }
        else { return Entity.Null; }
    }




    //This is to check the player based the touch on the entity. Turned it off because it's a little janky right now
    private void HandleFingerSwipe(LeanFinger finger)
    {


        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance = 100f;
     if (RayCastToEntity(ray.origin, ray.direction * rayDistance) == playerEntity)
        {
            manager.AddComponent<tag_Touched>(playerEntity);



        }
    }

   
}







