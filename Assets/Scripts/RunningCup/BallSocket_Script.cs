using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSocket_Script : MonoBehaviour
{
    public Transform objectToAttach;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = objectToAttach.position;
    }
}
