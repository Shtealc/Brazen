using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light lightSource; // Assign the light in the Inspector
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;

    private void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }

        if (lightSource != null)
        {
            InvokeRepeating("Flicker", 0f, flickerSpeed);
        }
    }

    private void Flicker()
    {
        if (lightSource != null)
        {
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
