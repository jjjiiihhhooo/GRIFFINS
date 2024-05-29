
using System.Collections;
using UnityEngine;

public class White_Explosion : MonoBehaviour
{
    private SphereCollider col;

    [SerializeField] private float speed;

    private void OnEnable()
    {
        StartCoroutine(EnableCor());
    }

    private IEnumerator EnableCor()
    {
        Vector3 finalPos = Player.Instance.transform.position + Player.Instance.transform.forward * 10f;
        transform.position = finalPos;
        yield return new WaitForEndOfFrame();

        //while(Vector3.Distance(transform.position, finalPos) > 0.1f)
        //{
        //    transform.position += finalPos - transform.position * speed * Time.deltaTime;
        //    yield return new WaitForEndOfFrame();
        //}

        //Destroy(this.gameObject);
    }
}
