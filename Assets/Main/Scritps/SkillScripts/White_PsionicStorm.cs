using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class White_PsionicStorm : MonoBehaviour
{
    private SphereCollider col;
    private Player player;

    private void OnEnable()
    {
        player = Player.Instance;
        StartCoroutine(EnableCor());
        StartCoroutine(EnableCor2());
    }

    private IEnumerator EnableCor2()
    {
        col = GetComponent<SphereCollider>();
        float time = 4f;
        while (time > 0)
        {
            col.enabled = true;
            time -= 0.1f;
            yield return new WaitForSeconds(0.5f);
            col.enabled = false;
        }
        Destroy(this.gameObject);
    }

    private IEnumerator EnableCor()
    {
        float time = 4f;
        Vector3 temp = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = Vector3.zero;
        while (time > 0)
        {
            if(transform.localScale.x < 1.5f)
            {
                transform.localScale += temp;
            }
            else if(transform.localScale.x != 1.5f)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }

            time -= Time.deltaTime;
            transform.position = player.transform.position + Vector3.up;
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + 10f, 0f);
            yield return new WaitForFixedUpdate();
        }

        while(transform.localScale.x > 0)
        {
            transform.localScale -= temp;
            transform.position = player.transform.position + Vector3.up;
            yield return new WaitForFixedUpdate();
        }

        Destroy(this.gameObject);
    }
}
