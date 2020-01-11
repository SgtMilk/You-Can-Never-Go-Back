using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : TrigerrableEvent
{
    public override void trigger()
    {
        GetComponentInChildren<Animator>().SetTrigger("Open");
    }
}
