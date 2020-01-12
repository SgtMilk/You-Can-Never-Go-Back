using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public TrigerrableEvent eventToTrigger;
    public Canvas textComponent;

    private bool isBeingPulled = false;

    public void Start()
    {
        textComponent.enabled = false;
    }

    public void pull()
    {
        if (!isBeingPulled)
        {
            isBeingPulled = true;
            GetComponent<Animator>().SetTrigger("Pull");
            eventToTrigger.trigger();
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2);
        isBeingPulled = false;
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
