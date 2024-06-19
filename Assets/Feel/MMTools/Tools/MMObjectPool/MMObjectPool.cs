using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
    public class MMObjectPool : MonoBehaviour
    {
        [MMReadOnly]
        public List<GameObject> PooledGameObjects;
    }
}