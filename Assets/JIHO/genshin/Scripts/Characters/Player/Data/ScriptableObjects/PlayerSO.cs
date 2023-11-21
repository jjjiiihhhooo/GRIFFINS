using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace genshin
{
    [CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/Player")]
    [System.Serializable]
    public class PlayerSO : ScriptableObject
    {
        [field:SerializeField] public PlayerGroundedData GroundedData { get; private set; }
    }
}
