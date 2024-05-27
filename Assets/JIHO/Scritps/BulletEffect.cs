using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Kill", 1f);
    }

    private void Kill()
    {
        Destroy(this.gameObject);
    }
}
