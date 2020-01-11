using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public TrigerrableEvent eventToTrigger;
    public Canvas textComponent;

    public void Start()
    {
        textComponent.enabled = false;
    }

    public void pull()
    {
        eventToTrigger.trigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        textComponent.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        textComponent.enabled = false;
    }


}
