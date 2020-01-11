using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moe : MonoBehaviour
{
    public float walkingSpeed = 100.0f;
    public float jumpSpeed = 250.0f;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0)
        {
            Rigidbody2D controller = GetComponent<Rigidbody2D>();
            if(controller)
            {
                controller.velocity = new Vector2(Time.deltaTime * walkingSpeed * Input.GetAxis("Horizontal"), controller.velocity.y);
            }
        }

        if(Input.GetAxis("Jump") != 0 && isGrounded())
        {
            Rigidbody2D controller = GetComponent<Rigidbody2D>();
            if (controller)
            {
                controller.velocity = new Vector2(controller.velocity.x, Time.deltaTime * jumpSpeed);
            }
        }
    }

    private bool isGrounded()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider)
        {
            return collider.IsTouchingLayers(groundLayer);
        }
        return false;
        /*
        Vector2 position = transform.position;
        Vector2 direction = Vector3.down;
        float distance = 1.0f;
        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        return hit.collider != null;
        */
    }
}
