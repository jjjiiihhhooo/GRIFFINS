using System;
using UnityEngine;


[Serializable]
public class PlayerDownStreamData
{
    [field: SerializeField][field: Range(1f, 50f)] public float SpeedModifier { get; private set; } = 2f;
}
