
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    [SerializeField] private GameObject[] objs;
    [SerializeField] private GameObject areaCool;
    public float time;
    public float curTime = 0;

    private void Update()
    {
        if (curTime < time)
        {
            if (areaCool != null)
                areaCool.transform.localScale = new Vector3(curTime / time, curTime / time, curTime / time);
            curTime += Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < objs.Length; i++)
                Instantiate(objs[i], transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }


}
