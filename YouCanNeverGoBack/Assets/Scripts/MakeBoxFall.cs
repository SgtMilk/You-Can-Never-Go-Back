using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBoxFall : TrigerrableEvent
{
    private Rigidbody2D rigidBody;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody)
        {
            rigidBody.bodyType = RigidbodyType2D.Static;
        }
    }
    public override void trigger()
    {
        if (rigidBody)
        {
            rigidBody.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
