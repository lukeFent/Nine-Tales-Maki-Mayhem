using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics; 


[GenerateAuthoringComponent]

public struct d_Move : IComponentData
{
    public float directionX;
    public float directionZ;
    public float speed;
    public bool jump; 
    public bool hadMoved;
 
}


