using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    [Serializable]
    public class PlayerAttackData
    {
        [field: SerializeField][field: Range(0f, 100f)] public float AttackDamage { get; private set; } = 1f;
    }
}
