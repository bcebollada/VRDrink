using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBall_Script : MonoBehaviour
{
    private Rigidbody rb;
    public float velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    private void Start()
    {
        rb.AddForce(transform.forward * velocity, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
