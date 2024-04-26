using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Projectile : MonoBehaviour
{

    public float speed;
    public GameObject dragon_boom;
    public SphereCollider col;
    public bool isFirst;

    private void OnEnable()
    {
        if (isFirst)
            StartCoroutine(EnableCor());
        else
            StartCoroutine(BoomCor());

    }

    

    private IEnumerator BoomCor()
    {
        col = GetComponent<SphereCollider>();
        Player.Instance.currentCharacter.curKnockbackDir = Vector3.up;
        for(int i = 0; i < 10; i++)
        {
            col.enabled = true;
            yield return new WaitForSeconds(0.1f);
            col.enabled = false;
        }
        Destroy(this.gameObject);
    }

    private IEnumerator EnableCor()
    {
        yield return null;
        float time = 1f;
        while(time > 0)
        {
            time -= Time.deltaTime;
            transform.position += transform.forward * speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        GameObject temp = Instantiate(dragon_boom, transform.position, Quaternion.identity);
        temp.gameObject.SetActive(true);
        Destroy(this.gameObject);
    }

    
}
