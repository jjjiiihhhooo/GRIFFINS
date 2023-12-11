using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    [Serializable]
    public class ObjectDashData
    {
        [field: SerializeField][field: Range(1f, 100f)] public float SpeedModifier { get;  set; } = 2f;
        [field: SerializeField] public Vector3 Direction { get;  set; }
    }
}
