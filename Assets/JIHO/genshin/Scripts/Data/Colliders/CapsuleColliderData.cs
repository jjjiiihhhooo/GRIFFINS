using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class CapsuleColliderData
    {
        public CapsuleCollider Collider { get; private set; }
        public Vector3 ColliderCenterInLocalSpace { get; private set; }

        public void Initalize(GameObject gameObject)
        {
            if(Collider != null)
            {
                return;
            }

            Collider = gameObject.GetComponent<CapsuleCollider>();

            UpdateColliderData();
        }

        public void UpdateColliderData()
        {
            ColliderCenterInLocalSpace = Collider.center;
        }
    }
}
