using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float floodingRate = 0.25f;
    public float horizontalSpeed = 3.0f;
    public float horizontalPeriod = 3;

    private float totalTime = 0;
    private bool moveRight = true;

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        totalTime += deltaTime;
        if (totalTime >= horizontalPeriod)
        {
            totalTime -= horizontalPeriod;
            moveRight = !moveRight;
        }
        transform.Translate((moveRight ? 1 : -1) * deltaTime * horizontalSpeed, deltaTime * floodingRate, 0);
    }
}
