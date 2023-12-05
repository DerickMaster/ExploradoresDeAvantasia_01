using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HitboxData
{
    public Vector3 bounds;
    public Vector3 offset;
}

[System.Serializable]
public class Attack
{
    public string name;

    public HitboxData hitbox;
    public GameObject originObj;
}