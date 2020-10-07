using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections; 


//This is a small script that updates that tracks the camera's y-rotation and passes it to the Entity Component System. It's a little expensive but for the moment works until we figure out the camera system. 
public class CameraTarget : MonoBehaviour
{
    private Entity playerEntity;
    private EntityManager entityManager;
    private EntityQuery playerEntityQuery;
    private LocalToWorld entityTransform;


    private Entity cameraEntity;
    private EntityArchetype cameraEntityArchetype;
    private EntityQuery cameraQuery; 

    public static CameraTarget instance;
    public Transform cameraTransform;
    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        playerEntityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<d_Speed>(), ComponentType.ReadOnly<LocalToWorld>());
        playerEntity = playerEntityQuery.GetSingletonEntity();

        cameraEntityArchetype = entityManager.CreateArchetype(typeof(d_CameraEuler), typeof(Translation));
        cameraEntity = entityManager.CreateEntity(cameraEntityArchetype);
        entityManager.SetName(cameraEntity, "CameraEntity");
        //cameraQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<tag_Camera>(), ComponentType.ReadOnly<LocalToWorld>());


    }


    private void Update()
    {
        if(playerEntity != null)
        {
          entityTransform =  entityManager.GetComponentData<LocalToWorld>(playerEntity);
            transform.position = entityTransform.Position; 
        }

        if(cameraEntity != null)
        {
            entityManager.SetComponentData(cameraEntity, new Translation { Value = cameraTransform.position });
            entityManager.SetComponentData(cameraEntity, new d_CameraEuler { Value = cameraTransform.eulerAngles.y });

        }

    }
}


