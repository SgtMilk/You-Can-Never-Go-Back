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

    public AK.Wwise.Event KeyPickup;
    public AK.Wwise.Event sndJump;
    public AK.Wwise.Event sndPush;
    public AK.Wwise.Event sndPushStop;
    public AK.Wwise.Event footstep;
    public AK.Wwise.Event footstepStop;

    public GameObject wwiseObj;

    private bool inputsActivated = true;

    private bool canPushObject = false;
    private bool isPushingObject = false;
    private bool isClimbing = false;
    private bool isWalking = false;
    private bool isJumping = false;
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
        sndPushStop.Post(wwiseObj);
        footstepStop.Post(wwiseObj);
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D controller = GetComponent<Rigidbody2D>();
        if (inputsActivated && controller)
        {
            controller.gravityScale = defaultGravityScale;
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalVelocity = Time.deltaTime * horizontalInput * (Input.GetAxisRaw("Run") == 1 ? runningSpeed : walkingSpeed);
            float verticalVelocity = controller.velocity.y;
            
            if (Input.GetAxisRaw("Jump") != 0 && isGrounded() && !isJumping)
            {
                isJumping = true;
                sndJump.Post(wwiseObj);
                verticalVelocity = Time.deltaTime * jumpSpeed;
                isClimbing = false;
            }
            else 
            {
                isJumping = false;
                if (isClimbing)
                {
                    verticalVelocity = Time.deltaTime * climbingSpeed * verticalInput;
                    controller.gravityScale = 0;
                }
            }

            bool ignorePlatformCollisions = verticalVelocity > 1 || isClimbing || verticalInput < 0;
            if (Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(platformLayer)))
            {
                
                ignorePlatformCollisions |= Physics2D.OverlapBox(transform.position, new Vector2(4, 5), 0, LayerMask.GetMask(platformLayer));  //collider.OverlapCollider(contactFilter, results) > 0;
            }
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(platformLayer), ignorePlatformCollisions);

            bool push = false;
            if (Input.GetAxisRaw("Action") == 1)
            {
                if (canPushObject && pushedObject)
                {
                    push = true;
                    if (!isPushingObject)
                    {
                        sndPush.Post(wwiseObj);
                        isPushingObject = true;
                    }
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

            if (!push && isPushingObject)
            {
                sndPushStop.Post(wwiseObj);
                isPushingObject = false;
            }

            float absoluteVelocity = Mathf.Abs(horizontalVelocity);
            if (!isWalking && absoluteVelocity > 0.1)
            {
                footstep.Post(wwiseObj);
                isWalking = true;
            }
            else if (isWalking && absoluteVelocity <= 0.1)
            {
                footstepStop.Post(wwiseObj);
                isWalking = false;
            }
            GetComponent<Animator>().SetFloat("horizontalVelocity", absoluteVelocity);

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
        KeyPickup.Post(wwiseObj);
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

    public void sceneCompleted()
    {
        inputsActivated = false;
        sndPushStop.Post(wwiseObj);
        footstepStop.Post(wwiseObj);
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
            canPushObject = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer(waterLayer))
        {
            sndPushStop.Post(wwiseObj);
            footstepStop.Post(wwiseObj);
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
            canPushObject = false;
        }
    }

}
