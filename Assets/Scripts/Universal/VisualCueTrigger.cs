using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCueTrigger : MonoBehaviour
{
    public Transform UISpawnPos;
    public GameObject instructionPrefab;

    private GameObject instruction;

    private int numberOfShows;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && instruction == null && numberOfShows <= 2)
        {
            instruction = Instantiate(instructionPrefab, UISpawnPos.position, Quaternion.identity);
            numberOfShows += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(instruction);
        }
    }
}
