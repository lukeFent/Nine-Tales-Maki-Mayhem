using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics; 




public class sb_RemoveHealth : SystemBase
{

    EntityQuery ricePrefab;
    Entity rice; 

    protected override void OnCreate()
    {
        ricePrefab = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<d_RicePrefab>());
        //d_RicePrefab prefab = ricePrefab.GetSingleton<d_RicePrefab>();
        //rice = prefab.Value; 
    }
    protected override void OnUpdate()
    {
        
        Entities.ForEach((ref d_Health health, in d_Direction impulse) =>
        {
            float distance = Mathf.Sqrt((impulse.Value.x * impulse.Value.x) + (impulse.Value.y * impulse.Value.y));
            int damage = Mathf.RoundToInt(distance) / 100 ;
            health.Value -= damage;
           // d_RicePrefab prefab = ricePrefab.GetSingleton<d_RicePrefab>();
           // rice = prefab.Value;
           // EntityManager.Instantiate(rice);

        }).Schedule();
    }
}
