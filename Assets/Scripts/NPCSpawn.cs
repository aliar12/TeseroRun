using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public GameObject NPCHelper;
    public GameObject NPCPrefab;

    private GameObject currentNPC; // Tracks the currently spawned NPC
                                   // Start is called before the first frame update
    public GameManager gameManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNPC()
    {

        // Deduct 10 points from the score
        //gameManager.AddScore(50);

        // Ensure only one NPC is active at a time
        if (currentNPC != null)
        {
            Debug.Log("An NPC is already active. Wait for it to disappear.");
            return;
        }

        // Find the main character by tag
        GameObject mainCharacterObj = GameObject.FindGameObjectWithTag("Player");

        if (mainCharacterObj == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        // Get the transform of the main character
        Transform mainCharacterTransform = mainCharacterObj.transform;

        // Calculate spawn position
        Vector3 spawnPosition = mainCharacterTransform.position + mainCharacterTransform.right * 2;
        spawnPosition.z = 0;

        // Instantiate the NPC and assign it to the currentNPC variable
        currentNPC = Instantiate(NPCPrefab, spawnPosition, Quaternion.identity);
        Invoke("DestroyNPC", 30f);
    }

    public void DestroyNPC()
    {
        Destroy(currentNPC);
    }
}
