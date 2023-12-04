using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    [Serializable]
    public class PlayerDownStreamData
    {
        [field: Tooltip("Having higher numbers might not read collisions with shallow colliders correctly.")]
        [field: SerializeField][field: Range(0f, 1000f)] public float FallSpeedLimit { get; private set; } = 1000f;
        [field: SerializeField][field: Range(0f, 100f)] public float MinimumDistanceToBeConsideredHardFall { get; private set; } = 3f;

    }
}
