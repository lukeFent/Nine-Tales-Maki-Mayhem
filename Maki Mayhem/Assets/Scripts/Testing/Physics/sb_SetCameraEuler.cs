using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
public class sb_SetCameraEuler : SystemBase
{
    protected override void OnUpdate()
    {
        float cameraEuler = CameraTarget.instance.cameraTransform.eulerAngles.y;

        Entities.ForEach((ref d_CameraEuler euler) => 
        {
            euler.Value = cameraEuler;
        }).Schedule();


       
    }

   
}



