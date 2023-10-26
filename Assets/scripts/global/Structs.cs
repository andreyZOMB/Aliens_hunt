using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
[Serializable]
public struct PlayerStep
{
    public PlayerStep(int2 objCoords, int2 dir)
    {
        ObjCoords = objCoords;
        Dir = dir;
    }
    public int2 ObjCoords { get; set; }
    public int2 Dir { get; set; }
}
