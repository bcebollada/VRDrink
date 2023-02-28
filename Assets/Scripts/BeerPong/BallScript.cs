using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.layer != 21)
        {
            Debug.Log("Ball hit enviorment");
            Destroy(this.gameObject, 1);
            gameController.Invoke("SpawnBall", 1.5f);
        }
    }

}
