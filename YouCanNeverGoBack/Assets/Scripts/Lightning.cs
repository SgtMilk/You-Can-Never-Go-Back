using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float maxIntensity = 5;
    public float mediumIntensity = 3;
    public float minIntensity = 2;
    public float minDurationBetween = 4;
    public float maxDurationBetween = 16;
    public float cameraShakeMaxAmount = 5;
    public CameraShake cameraShake;
    private new Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        light.intensity = 0;
        StartCoroutine(generateLigthning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator generateLigthning()
    {   
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minDurationBetween, maxDurationBetween));
            for (int i = 0; i < 2; i++)
            {
                light.intensity = Random.Range(minIntensity, mediumIntensity);
                float duration = Random.Range(0.02f, 0.1f);
                cameraShake.ShakeCamera(light.intensity / maxIntensity * cameraShakeMaxAmount, duration);
                yield return new WaitForSeconds(duration);
                light.intensity = Random.Range(light.intensity, maxIntensity);
                duration = Random.Range(0.05f, 0.1f);
                cameraShake.ShakeCamera(light.intensity / maxIntensity * cameraShakeMaxAmount, duration);
                yield return new WaitForSeconds(duration);
                light.intensity = Random.Range(light.intensity, mediumIntensity);
                duration = Random.Range(0.05f, 0.1f);
                cameraShake.ShakeCamera(light.intensity / maxIntensity * cameraShakeMaxAmount, duration);
                yield return new WaitForSeconds(duration);
                light.intensity = Random.Range(light.intensity, minIntensity);
                duration = Random.Range(0.02f, 0.05f);
                cameraShake.ShakeCamera(light.intensity / maxIntensity * cameraShakeMaxAmount, duration);
                yield return new WaitForSeconds(duration);
                light.intensity = 0;
                duration = Random.Range(0.1f, 0.2f);
                cameraShake.ShakeCamera(light.intensity / maxIntensity * cameraShakeMaxAmount, duration);
                yield return new WaitForSeconds(duration);
            }
        }
    }
}
