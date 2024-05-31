using System;
using UnityEngine;



[Serializable]
public class PlayerAttackData
{
    [field: SerializeField][field: Range(0f, 100f)] public float AttackDamage { get; private set; } = 1f;
}

