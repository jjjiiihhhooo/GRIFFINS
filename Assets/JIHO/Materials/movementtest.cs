using UnityEngine;

public class movementtest : MonoBehaviour
{

    public Rigidbody rb;
    public float jumpForce = 5.0f;
    public float speed = 5.0f;
    public float dashSpeed = 5.0f;
    public float super;

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0, 5, 0);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.down * super, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalMovement, 0.0f, verticalMovement) * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    //Rigidbody rigid;
    //public float speed;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    rigid = GetComponent<Rigidbody>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.Space))
    //    //{
    //    //    rigid.velocity = Vector3.zero;
    //    //    transform.position = new Vector3(0, 5, 0);
    //    //}

    //    //if(Input.GetMouseButtonDown(0))
    //    //{
    //    //    rigid.AddForce(Vector3.right * speed, ForceMode.Impulse);
    //    //}

    //    //if (Input.GetMouseButton(0))
    //    //{
    //    //    rigid.AddForce(Vector3.right * speed, ForceMode.Impulse);
    //    //}
    //}
}
