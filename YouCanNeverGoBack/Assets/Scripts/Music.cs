using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AK.Wwise.Event Cracks;
    // Start is called before the first frame update
    void Start()
    {
        Cracks.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
