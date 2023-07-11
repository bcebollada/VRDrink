using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBall_Script : MonoBehaviour
{
    private Rigidbody rb;
    public float velocity;

    private RunningCupGameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<RunningCupGameController>();
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

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<AudioSource>().Play();

        if (collision.gameObject.CompareTag("MobileRig"))
        {
            //collision.gameObject.GetComponent<RunningCupMobileRigController>().Hit();
        }
    }
}
