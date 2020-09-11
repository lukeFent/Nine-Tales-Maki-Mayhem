using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections; 

public class CameraTarget : MonoBehaviour
{
    private Entity entity;
    private EntityManager entityManager;
    private EntityQuery playerEntityQuery;
    private LocalToWorld entityTransform;

    public static CameraTarget instance;
    public Transform cameraTransform;
    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        playerEntityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<EntSpeedData>(), ComponentType.ReadOnly<LocalToWorld>());
        entity = playerEntityQuery.GetSingletonEntity();
    }


    private void Update()
    {
        if(entity != null)
        {
          entityTransform =  entityManager.GetComponentData<LocalToWorld>(entity);
            transform.position = entityTransform.Position; 
        }
    }
}
