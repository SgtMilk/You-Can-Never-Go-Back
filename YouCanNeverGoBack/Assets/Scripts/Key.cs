using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Moe moe = collision.gameObject.GetComponent<Moe>();
        if (moe)
        {
            moe.collectKey(id);
            Destroy(gameObject);
        }
    }

    /*private void OnTriggerEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }*/
}
