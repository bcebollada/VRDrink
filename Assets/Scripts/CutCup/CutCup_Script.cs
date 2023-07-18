using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCup_Script : MonoBehaviour
{
    private CutCupGameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CutCupGameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameController.GameOver();
        }
    }
}
