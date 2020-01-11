using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float floodingRate = 0.1f;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Transform>().localScale += new Vector3(0, floodingRate * Time.deltaTime, 0);
    }
}
