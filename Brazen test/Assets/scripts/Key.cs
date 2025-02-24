using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioSource pickupAudioSource; // Assign in Inspector
    public AudioSource secondaryAudioSource; // Second audio source

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonPlayer.hasKey = true;

            if (pickupAudioSource != null)
            {
                pickupAudioSource.Play(); // Play the first audio
            }

            if (secondaryAudioSource != null)
            {
                secondaryAudioSource.Play(); // Play the second audio
            }

            Destroy(gameObject);
        }
    }
}