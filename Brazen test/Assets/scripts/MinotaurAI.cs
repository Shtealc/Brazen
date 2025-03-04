using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurAI : MonoBehaviour
{
    // This defines the different states the Minotaur can be in
    public enum MinotaurState { Wandering, Charging, Searching, Calming, PreparingToCharge, Listening, Recovering }
    private MinotaurState currentState = MinotaurState.Wandering;

    [Header("Minotaur Settings")]
    public Transform player; // Reference to player
    public float wanderRadius = 50f; // Radius for random wandering, you can shrink or expand for testing
    public float noiseAttractionRadius = 15f; // How good the minotaur's hearing is
    public float visionAngle = 120f; // FOV
    public float visionDistance = 20f; // Max vision range
    public float chargeSpeed = 10f; // Speed of charge
    public float wanderSpeed = 2f; // Speed of walk
    public float searchSpeed = 8f; // Speed during search
    public float calmDownTime = 10f; // Time it takes to stop searching
    public float chargeDelay = 1.5f; // The delay before a charge
    public float recoveryTime = 2f; // Cooldown after charging
    public float chargeDuration = 3f; // How long the Minotaur charges forward

    [Header("Listening Settings")]
    public float listeningInterval = 20f; // Time between each listening phase
    public float listeningDuration = 6f; // Duration of listen, do not alter
    private float nextListeningTime = 0f; // Timer for listening phases

    [Header("Audio Settings")]
    public AudioClip wanderSound;
    public AudioClip chargeSound;
    public AudioClip recoverySound;
    public AudioClip listeningSound;
    private AudioSource audioSource;

    private NavMeshAgent agent; // This is the nav ai
    private Vector3 chargeDirection; // Stores direction of charge
    private float timeSinceLastSeen; // Timer since player last spotted

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        agent.speed = wanderSpeed;
        Wander(); // Start Astarion in wandering state
    }

    void Update()
    {
        switch (currentState)
        {
            case MinotaurState.Wandering:
                if (!audioSource.isPlaying || audioSource.clip != wanderSound)
                {
                    // Play the walking sound
                    PlaySound(wanderSound, true);
                }
                if (CanSeePlayer())
                {
                    StartCoroutine(ChargeDelay()); 
                }
                else if (PlayerMadeNoise())
                {
                    // If hear player, investigate noise
                    InvestigateNoise();
                }
                else if (Time.time >= nextListeningTime)
                {
                    // Enter listening state
                    StartListening();
                }
                else if (agent.remainingDistance < 1f)
                {
                    // Continue wandering
                    Wander();
                }
                break;

            case MinotaurState.Listening:
                // If the player is nearby when listening, go investigate
                if (PlayerMadeNoise())
                {
                    InvestigateNoise();
                }
                break;

            case MinotaurState.PreparingToCharge:
                break;

            case MinotaurState.Charging:
                // This is the charge
                agent.velocity = chargeDirection * chargeSpeed;
                break;

            case MinotaurState.Searching:
                agent.destination = agent.transform.position;
                if (agent.remainingDistance < 1f)
                {
                    StartCalmDown();
                }
                break;

            case MinotaurState.Calming:
                timeSinceLastSeen += Time.deltaTime;
                if (CanSeePlayer())
                {
                    ChargeDelay();
                }
                else if (timeSinceLastSeen >= calmDownTime)
                {
                    currentState = MinotaurState.Wandering;
                    agent.speed = wanderSpeed;
                    Wander();
                }
                break;

            case MinotaurState.Recovering:
                break;
        }
    }

    void StartListening()
    {
        currentState = MinotaurState.Listening;
        agent.isStopped = true;
        PlaySound(listeningSound, false);
        StartCoroutine(ListenForPlayer());
    }

    IEnumerator ListenForPlayer()
    {
        yield return new WaitForSeconds(listeningDuration);
        agent.isStopped = false;
        nextListeningTime = Time.time + listeningInterval;
        currentState = MinotaurState.Wandering;
        Wander();
    }

    void PlaySound(AudioClip clip, bool loop)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        Vector3 rayStart = transform.position + transform.forward * 1f;

        Debug.DrawRay(transform.position, directionToPlayer * visionDistance, Color.red, 0.1f);

        if (angle < visionAngle / 2f && Vector3.Distance(transform.position, player.position) < visionDistance)
        {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, visionDistance))
            {
                Debug.Log("Raycast Hit: " + hit.collider.gameObject.name); 

                if (hit.transform == player)
                {
                    return true;
                }
            }
        }
        return false;
    }


    bool PlayerMadeNoise()
    {
        return Vector3.Distance(transform.position, player.position) < noiseAttractionRadius;
    }

    void Wander()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
            randomPoint.y = transform.position.y;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit navHit, wanderRadius, NavMesh.AllAreas))
            {
                agent.destination = navHit.position;
                return;
            }
        }
    }


    IEnumerator ChargeDelay()
    {
        Debug.Log("Preparing to charge...");

        PlaySound(chargeSound, false);

        yield return new WaitForSeconds(chargeDelay);

        StartCharge();
    }

    void StartCharge()
    {
        if (player == null) return;

        currentState = MinotaurState.Charging;

        chargeDirection = (player.position - transform.position).normalized;

        agent.SetDestination(transform.position + chargeDirection * chargeSpeed * chargeDuration);

        agent.isStopped = false;
        agent.speed = chargeSpeed;

        StartCoroutine(ChargeRecovery());
    }

    IEnumerator ChargeRecovery()
    {
        yield return new WaitForSeconds(chargeDuration);
        agent.isStopped = true;
        currentState = MinotaurState.Recovering;
        PlaySound(recoverySound, false);
        yield return new WaitForSeconds(recoveryTime);
        agent.isStopped = false;
        currentState = MinotaurState.Wandering;
        Wander();
    }

    void StartCalmDown()
    {
        currentState = MinotaurState.Calming;
        timeSinceLastSeen = 0f;
    }

    public void InvestigateNoise()
    {
        agent.destination = player.position;
    }
}
