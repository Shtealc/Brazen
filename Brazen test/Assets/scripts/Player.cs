using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonPlayer : MonoBehaviour
{

    [Header("Character Settings")]
    public CharacterController controller;
    public float walkSpeed = 3f; // How fast you walk
    public float runSpeed = 6f; // See above, but running
    public float gravity = -9.81f;
    public Transform groundCheck; // Checks if grounded
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float noiseRadius = 15f; // Noise radius for alerting minotaur
    public MinotaurAI minotaur; // Reference to the minotaur

    [Header("Audio Settings")] // Sound clips
    public AudioClip walkSound;
    public AudioClip runSound;
    private AudioSource audioSource;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float walkBobSpeed = 5f; // This is just a slight bob on the walk, makes it look realer
    public float walkBobAmount = 0.04f;
    public float runBobSpeed = 12f;
    public float runBobAmount = 0.1f;
    private float defaultCameraY;
    private float bobTimer = 0;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning;

    [Header("Interactable Settings")]
    public static bool hasKey = false; // Tracks if the player has the key
    public int twineCount = 0; // Counts how much twine you have, reliant on maxTwine
    public Light playerLight; // Reference to player's light source
    public float minLightIntensity = 0.5f; // Min light, must be attatched to a light
    public float maxLightIntensity = 2.0f; // Max light
    public int maxTwine = 10; // The max amount of twine that effects the lighting

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        defaultCameraY = cameraTransform.localPosition.y;
        UpdateLightState();
    }

    void Update()
    {
        // Makes sure the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        isRunning = speed == runSpeed;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (isRunning)
        {
            AlertMinotaur(); // Send to AlertMinotaur
        }

        HandleFootstepSounds(move.magnitude > 0);
        ApplyCameraBobbing(move.magnitude > 0);
    }

    void AlertMinotaur()
    {
        if (Vector3.Distance(transform.position, minotaur.transform.position) < noiseRadius)
        {
            minotaur.InvestigateNoise(); // Alerts the minotaur if you are running, and in radius
        }
    }

    // Manages footstep sounds
    void HandleFootstepSounds(bool isMoving)
    {
        if (isMoving && isGrounded)
        {
            AudioClip clip = isRunning ? runSound : walkSound;
            if (!audioSource.isPlaying || audioSource.clip != clip)
            {
                PlayFootstepSound(clip);
            }
        }
        else if (!isMoving || !isGrounded)
        {
            StopFootstepSound();
        }
    }

    // Play footsteps
    void PlayFootstepSound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Stop footsteps
    void StopFootstepSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void ApplyCameraBobbing(bool isMoving)
    {
        if (isMoving && isGrounded)
        {
            float bobSpeed = isRunning ? runBobSpeed : walkBobSpeed;
            float bobAmount = isRunning ? runBobAmount : walkBobAmount;
            bobTimer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmount;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultCameraY + bobOffset, cameraTransform.localPosition.z);
        }
        else
        {
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, Mathf.Lerp(cameraTransform.localPosition.y, defaultCameraY, Time.deltaTime * 5f), cameraTransform.localPosition.z);
            bobTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If collide with minotaur, game over
        if (other.CompareTag("Minotaur"))
        {
            GameOver();
        }
        //If collide with key, collect key
        else if (other.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(other.gameObject);
        }
    }

    // Increases twine, and updates lighting intensity
    public void AddTwine(int amount)
    {
        twineCount += amount;
        UpdateLightState();
    }

    // Updates the lighting intensity based on twine count
    void UpdateLightState()
    {
        if (playerLight != null)
        {
            float intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, Mathf.Clamp01((float)twineCount / maxTwine));
            playerLight.intensity = intensity;
            Debug.Log("Light Intensity Updated: " + intensity);
        }
    }

    // Sends to game over screen
    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
