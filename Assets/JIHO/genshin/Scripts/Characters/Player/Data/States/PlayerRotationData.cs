using System;
using UnityEngine;

namespace genshin
{
    [SerializeField]
    public class PlayerRotationData
    {
        [field: SerializeField] public Vector3 TargetRotationReachTime { get; private set; }
    }
}
