using System;
using UnityEngine;


[Serializable]
public class ObjectDashData
{
    [field: SerializeField][field: Range(1f, 100f)] public float SpeedModifier { get; set; } = 2f;
    [field: SerializeField] public Vector3 Direction { get; set; }
}

