using UnityEngine;

public class Twine : MonoBehaviour
{
    public AudioClip pickupSound;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        FirstPersonPlayer player = other.GetComponent<FirstPersonPlayer>();

        if (player != null)
        {
            player.AddTwine(5); // Call to increase twine count

            if (audioSource != null && pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound); // Play pickup sound
            }

            Destroy(gameObject); // Destroy twine obj after pickup
        }
    }
}
