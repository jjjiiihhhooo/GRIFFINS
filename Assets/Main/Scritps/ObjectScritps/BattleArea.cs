using System.Collections;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    public float size;
    public float plus;
    public Vector3 pos;


    private void Awake()
    {
        Create();
    }


    public void Create()
    {
        StartCoroutine(CreateCor());
    }

    public void Xpos(float x)
    {
        pos.x = x;
    }

    public void Ypos(float y)
    {
        pos.y = y;
    }
    public void Zpos(float z)
    {
        pos.z = z;
    }
    private IEnumerator CreateCor()
    {
        Vector3 scale = Vector3.zero;

        while (transform.localScale.x < size)
        {
            yield return new WaitForEndOfFrame();
            scale.x += plus;
            scale.y += plus;
            scale.z += plus;
            transform.localScale = scale;
        }
        GetComponent<Collider>().enabled = true;
    }

    public void DestroyAnim()
    {
        StartCoroutine(DestroyCor());
    }

    private IEnumerator DestroyCor()
    {
        Vector3 scale = transform.localScale;
        GetComponent<Collider>().enabled = false;

        while (transform.localScale.x > 1)
        {
            yield return new WaitForEndOfFrame();
            scale.x -= plus * 2;
            scale.y -= plus * 2;
            scale.z -= plus * 2;
            transform.localScale = scale;
        }
    }
}
