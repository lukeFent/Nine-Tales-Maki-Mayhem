﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities; 

[GenerateAuthoringComponent]
public struct d_Health : IComponentData
{
    public int Value; 
}
