using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class TestObject : MonoBehaviour
    {
        public Transform leftwall;
        public Transform rightwall;
        public Transform leftpos;
        public Transform rightpos;


        public void CorStart()
        {
            StartCoroutine(dd());
        }

        private IEnumerator dd()
        {
            while(Vector3.Distance(leftwall.position, leftpos.position) > 1)
            {
                leftwall.transform.position += Vector3.forward * Time.deltaTime;
                rightwall.transform.position -= Vector3.forward * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }
    }
}
