using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moe : MonoBehaviour
{
    public float walkingSpeed = 500.0f;
    public float jumpSpeed = 1000.0f;
    public float climbingSpeed = 500.0f;
    public float pushForce = 100.0f;
    public LayerMask groundLayer;
    public LayerMask pushableLayer;
    public LayerMask ladderLayer;

    private bool isPushingObject = false;
    private bool isClimbing = false;
    private GameObject pushedObject;
    private List<int> keys = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 10, 0), Vector3.up, isClimbing ? Color.red : Color.green);
        Rigidbody2D controller = GetComponent<Rigidbody2D>();
        if (controller)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float horizontalVelocity = Time.deltaTime * walkingSpeed * horizontalInput;
            float verticalVelocity = controller.velocity.y;
            if (Input.GetAxis("Jump") != 0 && isGrounded())
            {
                verticalVelocity = Time.deltaTime * jumpSpeed;
            }
            else if (isClimbing)
            {
                verticalVelocity = Time.deltaTime * climbingSpeed * Input.GetAxis("Vertical");
            }
            controller.velocity = new Vector2(horizontalVelocity, verticalVelocity);

            if (horizontalInput * transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        /*
        if (isPushingObject && pushedObject)
        {
            Rigidbody2D objectRigidBody = pushedObject.GetComponent<Rigidbody2D>();
            if (objectRigidBody)
            {
                objectRigidBody.AddForce(controller.velocity.normalized * pushForce);
            }
        }*/
    }

    public bool hasKey(int keyId)
    {
        return keys.Contains(keyId);
    }

    public void collectKey(int keyId)
    {
        keys.Add(keyId);
    }

    private bool isGrounded()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider)
        {
            return collider.IsTouchingLayers(groundLayer);
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ladderLayer)
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ladderLayer)
        {
            isClimbing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == pushableLayer)
        {
            pushedObject = collision.gameObject;
            isPushingObject = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == pushableLayer)
        {
            isPushingObject = false;
        }
    }

}
