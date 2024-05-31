
using UnityEngine;
using UnityEngine.Events;

public class ObjectEvent : MonoBehaviour
{

    [SerializeField] private UnityEvent _event;
    [SerializeField] private Transform[] moveTransforms;
    [SerializeField] private ParticleSystem colEffect;
    [SerializeField] private bool isMove;
    [SerializeField] private float moveSpeed;


    private int moveIndex = 0;

    private void Update()
    {
        if (isMove)
        {
            Move();
        }
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, moveTransforms[moveIndex].position) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTransforms[moveIndex].position, moveSpeed);
        }
        else
        {
            if (moveIndex == 0) moveIndex = 1;
            else moveIndex = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "useObject")
        {
            if (colEffect != null)
            {
                colEffect.transform.position = other.contacts[0].point;
                if (!colEffect.gameObject.activeSelf)
                {
                    colEffect.gameObject.SetActive(true);
                    colEffect.transform.parent = null;
                    colEffect.Play();
                }

            }
            _event?.Invoke();
        }
    }
}
