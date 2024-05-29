using System.Collections;
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
        Player.Instance.currentCharacter.curKnockback = 2;

        for(int i = 0; i < 30; i++)
        {
            col.enabled = true;
            yield return new WaitForSeconds(0.05f);
            col.enabled = false;
        }

        
        
        Destroy(this.gameObject);
    }

    private IEnumerator EnableCor()
    {
        yield return null;
        float time = 2f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            transform.position += transform.forward * speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        GameObject temp = Instantiate(dragon_boom, transform.position, Quaternion.identity);
        temp.gameObject.SetActive(true);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && isFirst)
        {
            GameObject temp = Instantiate(dragon_boom, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), Quaternion.identity);
            temp.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }


}
