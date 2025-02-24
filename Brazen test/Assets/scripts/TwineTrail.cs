using System.Collections;
using UnityEngine;

public class TwineTrail : MonoBehaviour
{
    public GameObject trailPrefab; // Assign a prefab in the Inspector
    public float trailSpawnRate = 0.2f;
    public int twineCostPerTrail = 1;

    private FirstPersonPlayer player;
    private bool isUsingTwine = false;

    void Start()
    {
        player = GetComponent<FirstPersonPlayer>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && player.twineCount > 0 && !isUsingTwine)
        {
            StartCoroutine(UseTwine());
        }
    }

    private IEnumerator UseTwine()
    {
        isUsingTwine = true;

        while (Input.GetKey(KeyCode.Space) && player.twineCount > 0)
        {
            Instantiate(trailPrefab, transform.position, Quaternion.identity);
            player.AddTwine(-twineCostPerTrail); // Reduce twine count
            yield return new WaitForSeconds(trailSpawnRate);
        }

        isUsingTwine = false;
    }
}