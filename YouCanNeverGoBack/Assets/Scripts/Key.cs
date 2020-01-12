using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int id;
    public bool changesMusic = true;

    public AK.Wwise.Event intenseMusic;
    public GameObject wwiseObj;

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
            if (changesMusic) intenseMusic.Post(wwiseObj);
            moe.collectKey(id);
            Destroy(gameObject);
        }
    }
}
