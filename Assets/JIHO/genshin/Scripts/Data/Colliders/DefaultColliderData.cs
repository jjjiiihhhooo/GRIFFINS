using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    [Serializable]
    public class DefaultColliderData
    {
        [field: SerializeField] public float Height { get; private set; } = 0.96f;
        [field: SerializeField] public float CenterY { get; private set; } = 0.48f;
        [field: SerializeField] public float Radius { get; private set; } = 0.1f;

    }
}
