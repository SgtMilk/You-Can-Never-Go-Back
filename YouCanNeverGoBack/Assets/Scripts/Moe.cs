using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moe : MonoBehaviour
{
    public float walkingSpeed = 500.0f;
    public float jumpSpeed = 1000.0f;
    public float climbingSpeed = 500.0f;
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

            bool ignorePlatformCollisions = verticalVelocity > 1 || isClimbing || verticalInput < 0;
            if (Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(platformLayer)))
            {
                
                ignorePlatformCollisions |= Physics2D.OverlapBox(transform.position, new Vector2(4, 5), 0, LayerMask.GetMask(platformLayer));  //collider.OverlapCollider(contactFilter, results) > 0;
            }
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(platformLayer), ignorePlatformCollisions);


            if (isPushingObject && Input.GetAxis("Fire1") == 1 && pushedObject)
            {
                Debug.DrawRay(pushedObject.transform.position, Vector3.up, Color.white);
                Rigidbody2D objectRigidBody = pushedObject.GetComponent<Rigidbody2D>();
                if (objectRigidBody)
                {
                    horizontalVelocity /= 2.0f;
                    objectRigidBody.velocity = new Vector2(horizontalVelocity, objectRigidBody.velocity.y);
                }
            }
            controller.velocity = new Vector2(horizontalVelocity, verticalVelocity);

            if (horizontalInput * transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        
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
            LayerMask mask = LayerMask.GetMask(groundLayer, platformLayer, pushableLayer);
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
        if (collision.gameObject.layer == LayerMask.NameToLayer(ladderLayer))
        {
            isClimbing = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer(pushableLayer))
        {
            pushedObject = collision.gameObject;
            isPushingObject = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(ladderLayer))
        {
            isClimbing = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer(pushableLayer))
        {
            isPushingObject = false;
        }
    }

}
