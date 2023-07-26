using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

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

        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(DestroyRealtimeObject(this.gameObject, 2f));
        }
    }

    private IEnumerator DestroyRealtimeObject(GameObject objectToDestroy, float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Realtime.Destroy(objectToDestroy);
    }
}
