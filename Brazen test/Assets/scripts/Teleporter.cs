using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad; // Set the scene name in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            SceneManager.LoadScene(sceneToLoad); // Loads the new scene
        }
    }
}

