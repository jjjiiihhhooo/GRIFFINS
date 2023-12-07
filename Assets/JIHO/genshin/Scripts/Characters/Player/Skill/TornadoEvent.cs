using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace genshin
{
    public class TornadoEvent : MonoBehaviour
    {
        public List<EnemyController> enemy_list;

        public float tempDamage;
        public GameObject posEffect;
        public GameObject TornadoEffect;

        public bool isTarget;

        private void Awake()
        {
            enemy_list = new List<EnemyController>();
        }

        public void Tornado()
        {
            
            StartCoroutine(TornadoDamage());
        }

        private IEnumerator TornadoDamage()
        {
            posEffect.SetActive(false);
            TornadoEffect.SetActive(true);
            isTarget = true;
            float time = 0;
            while(time < 2)
            {
                time += 0.2f;
                for(int i = 0; i < enemy_list.Count; i++)
                {
                    if (enemy_list[i] != null)
                    {
                        enemy_list[i].DamageMessage(tempDamage, enemy_list[i].transform.position);
                        Vector3 dir = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z) - enemy_list[i].transform.position;
                            
                        enemy_list[i].transform.position += dir.normalized * 5f * Time.deltaTime + Vector3.up;
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }

            for(int i = 0; i < enemy_list.Count; i++)
            {
                if (enemy_list[i] != null)
                {
                    enemy_list[i].GetComponent<Rigidbody>().useGravity = true;
                }
            }
            isTarget = false;
            posEffect.SetActive(true); 
            TornadoEffect.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void Update()
        {
            //Target();
        }

        private void Target()
        {
            if (enemy_list == null) return;

            if (isTarget) return;

            for(int i = 0; i < enemy_list.Count; i++)
            {
                if (enemy_list[i] != null)
                {
                    Vector3 dir = transform.position - enemy_list[i].transform.position;
                    enemy_list[i].transform.position += dir * 30f * Time.deltaTime;
                }
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Rigidbody>().useGravity = false;
                enemy_list.Add(collider.GetComponent<EnemyController>());
            }
        }
    }
}
