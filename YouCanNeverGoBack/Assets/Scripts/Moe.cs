using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Moe : MonoBehaviour
{
    public float walkingSpeed = 750.0f;
    public float runningSpeed = 1000.0f;
    public float jumpSpeed = 1000.0f;
    public float climbingSpeed = 500.0f;
    public float defaultGravityScale = 3.0f;

    public string groundLayer;
    public string pushableLayer;
    public string ladderLayer;
    public string platformLayer;
    public string leverLayer;
    public string waterLayer;

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
        Debug.DrawRay(transform.position, Vector3.up, isClimbing? Color.green:Color.red);
        Rigidbody2D controller = GetComponent<Rigidbody2D>();
        if (controller)
        {
            controller.gravityScale = defaultGravityScale;
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalVelocity = Time.deltaTime * horizontalInput * (Input.GetAxisRaw("Run") == 1 ? runningSpeed : walkingSpeed);
            float verticalVelocity = controller.velocity.y;
            
            if (Input.GetAxisRaw("Jump") != 0 && isGrounded())
            {
                verticalVelocity = Time.deltaTime * jumpSpeed;
                isClimbing = false;
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

            if (Input.GetAxisRaw("Action") == 1)
            {
                if (isPushingObject && pushedObject)
                {
                    Debug.DrawRay(pushedObject.transform.position, Vector3.up, Color.white);
                    Rigidbody2D objectRigidBody = pushedObject.GetComponentInChildren<Rigidbody2D>();
                    if (objectRigidBody)
                    {
                        horizontalVelocity /= 2.0f;
                        objectRigidBody.velocity = new Vector2(horizontalVelocity, objectRigidBody.velocity.y);
                    }
                }
                else
                {
                    Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(4, 5), 0, LayerMask.GetMask(leverLayer));
                    if (collider) {
                        Lever lever = collider.gameObject.GetComponent<Lever>();
                        if (lever)
                        {
                            lever.pull();
                        }

                    }
                }
            }

            GetComponent<Animator>().SetFloat("horizontalVelocity", Mathf.Abs(horizontalVelocity));

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
        if (isClimbing)
            return true;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider)
        {
            LayerMask mask = LayerMask.GetMask(groundLayer, platformLayer, pushableLayer);
            if (collider.IsTouchingLayers(mask))
            {
                RaycastHit2D[] hits = new RaycastHit2D[1];
                return Physics2D.RaycastNonAlloc(transform.position, Vector2.down, hits, 3, mask) > 0;
            }
        }
        return false;
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

        if (collision.gameObject.layer == LayerMask.NameToLayer(waterLayer))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
