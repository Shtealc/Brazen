using System.Collections;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour
{
    public AudioSource audioSource; // Assign in the Inspector
    public float fadeDuration = 1.0f; // Time to fade out

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone. Starting fade-out.");
            StartFadeOut();
        }
    }

    public void StartFadeOut()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            Debug.Log("Audio source is either null or not playing.");
        }
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        Debug.Log("Starting fade-out with volume: " + startVolume);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        Debug.Log("Fade-out complete. Stopping audio.");
        audioSource.Stop();
    }
}
