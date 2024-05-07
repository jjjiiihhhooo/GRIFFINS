
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 endPos;

    public Vector3 EndPos { get => endPos; set => endPos = value; }



    private void Update()
    {
        
    }
}
