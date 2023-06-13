using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class BallScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public GameObject transparentSphere;
    public BeerGameController gameController;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<BeerGameController>();
        rb = GetComponent<Rigidbody>(); 
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grabbed()
    {
        transparentSphere.SetActive(false);
        rb.isKinematic = false;
        gameController.Invoke("SpawnBall", 2f);


    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);
        GetComponent<AudioSource>().Play();

        if (collision.gameObject.layer != 21 && collision.gameObject.layer != 20)
        {
            Debug.Log("Ball hit enviorment");
            StartCoroutine(DestroyRealtimeObject(this.gameObject, 1));
        }
    }

    private IEnumerator DestroyRealtimeObject(GameObject objectToDestroy, float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Realtime.Destroy(objectToDestroy);
    }

}
