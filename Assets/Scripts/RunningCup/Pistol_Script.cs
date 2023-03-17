using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Script : MonoBehaviour
{
    public GameObject ball;
    public int bullets;
    public Transform shootPoint;
    private Rigidbody rb;

    public GameObject[] bulletsCase = new GameObject[5];

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if(bullets > 0)
        {
            Instantiate(ball, shootPoint.position, shootPoint.rotation);
            bullets -= 1;

            foreach(GameObject gameObject in bulletsCase)
            {
                gameObject.SetActive(false);
            }
            for (int i = 0; i < bullets; i++)
            {
                bulletsCase[i].SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            bullets += 1;
            Destroy(other.gameObject);

            for (int i = 0; i < bullets; i++)
            {
                bulletsCase[i].SetActive(true);
            }
        }
    }

    public void Grab()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}
