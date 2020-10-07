using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;



//This is a great way to create sync points between monobehaviour and ECS. Put this on any GameObject set to be converted to an entity, and this script will create a reference to that entity that is accessible to monobehaviour such as a GameManager
public class ConvertedEntityHolder : MonoBehaviour, IConvertGameObjectToEntity
{

    private Entity entity;
    private EntityManager entityManager;



    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        this.entity = entity;
        this.entityManager = dstManager;
    }


    public Entity GetEntity()
    {
        return entity;
    }

    public EntityManager GetEntityManager()
    {
        return entityManager;
    }


}