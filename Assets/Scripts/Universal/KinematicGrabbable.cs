using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicGrabbable : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grab()
    {
        rb.isKinematic = false;
    }

    public void Realese()
    {
        rb.isKinematic = true;
    }
}
