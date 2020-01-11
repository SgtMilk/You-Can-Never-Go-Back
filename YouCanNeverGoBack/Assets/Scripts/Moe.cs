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
        Rigidbody2D controller = GetComponent<Rigidbody2D>();
        if (controller)
        {
            controller.velocity = new Vector2(Input.GetAxis("Horizontal") == 0 ? 0 : Time.deltaTime * walkingSpeed * Input.GetAxis("Horizontal"),
                                              Input.GetAxis("Jump") == 0 || !isGrounded() ? controller.velocity.y : Time.deltaTime * jumpSpeed);
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
    }
}
