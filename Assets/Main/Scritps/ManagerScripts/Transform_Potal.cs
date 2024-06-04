using UnityEngine;

public class Transform_Potal : MonoBehaviour
{
    public Transform targetTransform;

    public bool fade;

    private void PlayerToTarget()
    {
        if (fade) GameManager.Instance.uiManager.FadeInOut();
        Player.Instance.transform.position = targetTransform.position;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlayerToTarget();
        }
    }
}
