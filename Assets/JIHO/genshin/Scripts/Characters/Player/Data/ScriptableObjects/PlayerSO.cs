using System;
using UnityEngine;

namespace genshin
{
    [CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field: SerializeField] public PlayerGroundedData GroundedData { get; private set; }
    }
}
