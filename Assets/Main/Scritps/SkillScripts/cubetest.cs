using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class cubetest : MonoBehaviour
{
    public Rigidbody rb;
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public Transform a;
    public Vector3 dir;
    public Vector3 velocity;

    public float height;
    public float overshootYAxis;
    public float speed;

    private float time = 0.0f;
    private bool isMoving = false;
    private bool isJumpPressed = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        dir = a.position - transform.position;
        isJumpPressed = Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        if (isJumpPressed)
        {
            float gravity = Physics.gravity.y;
            float trajectoryHeight = a.position.y- (transform.position.y - height);
            //float highestPointOnArc = trajectoryHeight + overshootYAxis;
            // the cube is going to move upwards in 10 units per second
            //Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2f * gravity * highestPointOnArc);
           // rb.velocity = new Vector3(dir.x, velocityY.y, dir.z);
            isMoving = true;
            Debug.Log("jump");

            Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

            float grapplePointRelativeYPos = a.position.y - lowestPoint.y;
            float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

            if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

            velocity = CalculateJumpVelocity(transform.position, a.position, highestPointOnArc);
            Debug.Log(velocity);

            //rb.AddForce(velocity, ForceMode.Impulse);
            rb.velocity = velocity;
        }


        
    }


    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight) //목표 위치까지 포물선 trajectoryHeight 높이 추가
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
          + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}
