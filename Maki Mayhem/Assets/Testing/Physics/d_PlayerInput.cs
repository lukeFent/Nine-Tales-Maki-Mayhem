using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]

public struct d_PlayerInput : IComponentData
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;

}
