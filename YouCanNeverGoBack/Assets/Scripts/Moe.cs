using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moe : MonoBehaviour
{
    public float walkingSpeed = 100.0f;
    public float jumpSpeed = 250.0f;
    public float pushForce = 100.0f;
    public LayerMask groundLayer;
    public LayerMask pushableLayer;

    private bool isPushingObject = false;
    private GameObject pushedObject;
    private List<int> keys = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D controller = GetComponent<Rigidbody2D>();
        if (controller)
        {
            controller.velocity = new Vector2(Input.GetAxis("Horizontal") == 0 ? 0 : Time.deltaTime * walkingSpeed * Input.GetAxis("Horizontal"),
                                              Input.GetAxis("Jump") == 0 || !isGrounded() ? controller.velocity.y : Time.deltaTime * jumpSpeed);
        }

        if (isPushingObject && pushedObject)
        {
            Rigidbody2D objectRigidBody = pushedObject.GetComponent<Rigidbody2D>();
            if (objectRigidBody)
            {
                objectRigidBody.AddForce(controller.velocity.normalized * pushForce);
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
            return collider.IsTouchingLayers(groundLayer);
        }
        return false;
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
