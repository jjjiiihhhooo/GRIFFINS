using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgTxt : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ActiveCor());
    }

    private IEnumerator ActiveCor()
    {
        float time = 0;
        Vector3 tempPos = transform.position;
        
        while(time < 1)
        {
            tempPos.y += 0.002f;
            time += Time.deltaTime;
            transform.position = tempPos;
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }

}
