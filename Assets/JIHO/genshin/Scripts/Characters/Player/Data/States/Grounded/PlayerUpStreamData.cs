using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    [Serializable]
    public class PlayerUpStreamData
    {
        [field: SerializeField][field: Range(1f, 3f)] public float SpeedModifier { get; private set; } = 2f;
        [field: SerializeField] public PlayerRotationData RotationData { get; private set; }
        [field: SerializeField][field: Range(0f, 1000f)] public float upPower { get; private set; } = 1f;
        [field: SerializeField][field: Range(1, 10)] public int ConsecutiveDashesLimitAmount { get; private set; } = 2;
        [field: SerializeField][field: Range(0f, 5f)] public float DashLimitReachedCooldown { get; private set; } = 1.75f;

        public float fallCount = 0f;
    }
}
