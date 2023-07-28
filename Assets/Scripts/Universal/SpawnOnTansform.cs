using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTansform : MonoBehaviour
{

    public Transform transformStart;
    // Start is called before the first frame update
    private void Awake()
    {
        if(transformStart == null)
        {
            Debug.Log("Player doesn't have start tranform");
        }
        else
        {
            transform.position = transformStart.position;
            transform.rotation = transformStart.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
