using System;
using UnityEngine;



[Serializable]
public class PlayerRotationData
{
    [field: SerializeField] public Vector3 TargetRotationReachTime { get; private set; }
}

