using System.Collections;
using UnityEngine;

public class White_Laser : MonoBehaviour
{
    public Transform endTransform;
    public Transform[] startTransforms;



    public LineRenderer[] lines;

    private void Awake()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetPosition(0, startTransforms[i].position);
            lines[i].SetPosition(1, endTransform.position);
        }

        StartCoroutine(DestroyCor());
    }

    private IEnumerator DestroyCor()
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
    }
}
