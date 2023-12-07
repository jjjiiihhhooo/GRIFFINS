using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    [Serializable]
    public class PlayerDownStreamData
    {
        [field: SerializeField][field: Range(1f, 50f)] public float SpeedModifier { get; private set; } = 2f;
    }
}
