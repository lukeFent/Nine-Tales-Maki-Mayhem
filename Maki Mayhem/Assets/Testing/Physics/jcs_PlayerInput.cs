using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;

public class jcs_PlayerInput : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.ForEach((ref d_Move moveData) =>
        {
            moveData.hadMoved = false;
            moveData.directionX = 0;
            moveData.directionZ = 0;


            moveData.directionX += Input.GetKey(KeyCode.D) ? 1 : 0;
            
            
            moveData.directionX -= Input.GetKey(KeyCode.A) ? 1 : 0;


            moveData.directionZ += Input.GetKey(KeyCode.W) ? 1 : 0;


            moveData.directionZ -= Input.GetKey(KeyCode.S) ? 1 : 0;

            if (moveData.directionZ != 0 || moveData.directionX != 0)
                 moveData.hadMoved = true; 




        }).Run();

        return default;


    }

   
}
