using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Canvas textComponent;

    public void Start()
    {
        textComponent.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody)
        {
            if (collision.gameObject.GetComponent<Moe>() && rigidBody.bodyType == RigidbodyType2D.Dynamic)
            textComponent.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Moe>())
            textComponent.enabled = false;
    }
}
