using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moe : MonoBehaviour
{
    public float walkingSpeed = 500.0f;
    public float jumpSpeed = 1000.0f;
    public float climbingSpeed = 500.0f;
    public float pushForce = 100.0f;
    public float defaultGravityScale = 3.0f;

    public string groundLayer;
    public string pushableLayer;
    public string ladderLayer;
    public string platformLayer;

    private bool isPushingObject = false;
    private bool isClimbing = false;
    private GameObject pushedObject;
    private List<int> keys = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D controller = GetComponent<Rigidbody2D>();
        if (controller)
        {
            controller.gravityScale = defaultGravityScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D controller = GetComponent<Rigidbody2D>();
        if (controller)
        {
            controller.gravityScale = defaultGravityScale;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalVelocity = Time.deltaTime * walkingSpeed * horizontalInput;
            float verticalVelocity = controller.velocity.y;
            if (Input.GetAxis("Jump") != 0 && isGrounded())
            {
                verticalVelocity = Time.deltaTime * jumpSpeed;
            }
            else if (isClimbing)
            {
                verticalVelocity = Time.deltaTime * climbingSpeed * verticalInput;
                controller.gravityScale = 0;
            }
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(groundLayer), verticalVelocity > 1);
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(platformLayer), verticalVelocity > 1 || isClimbing || verticalInput < 0);

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
            LayerMask mask = LayerMask.GetMask(groundLayer, platformLayer);
            return collider.IsTouchingLayers(mask);
        }
        return false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(ladderLayer))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        Debug.Log(ladderLayer);
        if (collision.gameObject.layer == LayerMask.NameToLayer(ladderLayer))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(ladderLayer))
        {
            isClimbing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(pushableLayer))
        {
            pushedObject = collision.gameObject;
            isPushingObject = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(pushableLayer))
        {
            isPushingObject = false;
        }
    }

}
