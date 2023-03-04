using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupFlipCup_Script : MonoBehaviour
{
    public float secondsFlipped;
    public bool isTouchingTable, isStanding, isFlipped;
    private Rigidbody rb;
    private FlipCupGameController gameController;
    private int lastPoint; //used to not add continiously infinite points

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<FlipCupGameController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTouchingTable)
        {
            secondsFlipped += Time.deltaTime;
            if (rb.velocity.magnitude <= 0.1 && transform.up.y > 0) isStanding = true;
            else if (rb.velocity.magnitude <= 0.1 && transform.up.y < 0) isFlipped = true;
            if (isFlipped && gameController.points == lastPoint)
            {
                gameController.AddPoint(1);
            }

            else if(isStanding)
            {
                lastPoint = gameController.points;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Table"))
        {
            isTouchingTable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            isTouchingTable = false;
            isStanding = false;
            isFlipped = false;
        }
    }
}
