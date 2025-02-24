using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public AudioClip gateOpenSound;
    public AudioSource audioSource;
    public Transform gateTransform;
    public float lowerDistance = 7.6f; // Distance to lower the gate
    public float lowerSpeed = .7f; // Speed of lowering

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource not assigned on Gate script.");
        }
        if (gateTransform == null)
        {
            Debug.LogWarning("Gate Transform not assigned on Gate script.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && FirstPersonPlayer.hasKey)
        {
            StartCoroutine(LowerGate());
        }
    }

    private IEnumerator LowerGate()
    {
        if (gateOpenSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gateOpenSound);
        }

        yield return new WaitForSeconds(1f); // 1-second delay before lowering

        Vector3 startPosition = gateTransform.position;
        Vector3 targetPosition = startPosition - new Vector3(0, lowerDistance, 0);
        float elapsedTime = 0f;

        while (elapsedTime < lowerDistance / lowerSpeed)
        {
            gateTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / (lowerDistance / lowerSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gateTransform.position = targetPosition;
    }
}

