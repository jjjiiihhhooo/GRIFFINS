using System.Collections;
using UnityEngine;

public class White_Explosion : MonoBehaviour
{
    private SphereCollider col;

    private void OnEnable()
    {
        StartCoroutine(EnableCor());
    }

    private IEnumerator EnableCor()
    {
        Vector3 temp = Vector3.zero;
        col = GetComponent<SphereCollider>();
        while (transform.localScale.x < 2)
        {
            temp.x += 0.4f;
            temp.y += 0.4f;
            temp.z += 0.4f;
            transform.localScale = temp;
            yield return new WaitForFixedUpdate();
        }
        col.enabled = true;
        yield return new WaitForSeconds(0.5f);

        while (transform.localScale.x > 0)
        {
            temp.x -= 0.4f;
            temp.y -= 0.4f;
            temp.z -= 0.4f;
            transform.localScale = temp;
            yield return new WaitForFixedUpdate();
        }

        Destroy(this.gameObject);
    }
}
