using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class ObjectForward : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform startTransform;

        public Vector3 forward;

        private void Awake()
        {
            forward = targetTransform.position - startTransform.position;
        }
    }
}
