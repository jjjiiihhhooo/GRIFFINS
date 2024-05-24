using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float time;
    private float curTime;

    // Start is called before the first frame update
    private void OnEnable()
    {
        curTime = time;
    }

    // Update is called once per frame
    void Update()
    {

        if (curTime > 0) curTime -= Time.deltaTime;
        else this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        curTime = time;
    }
}
