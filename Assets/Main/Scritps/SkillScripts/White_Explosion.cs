
using System.Collections;
using UnityEngine;

public class White_Explosion : MonoBehaviour
{
    private SphereCollider col;

    private void OnEnable()
    {
        StartCoroutine(BoomCor());
    }

    private IEnumerator BoomCor()
    {
        col = GetComponent<SphereCollider>();

        yield return new WaitForSeconds(0.3f);

        col.enabled = true;

        yield return null;
    }

}
