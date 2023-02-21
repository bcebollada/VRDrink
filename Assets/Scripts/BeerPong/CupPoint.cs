using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupPoint : MonoBehaviour
{
    private BeerGameController gameController;

    public int pointsWorth;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<BeerGameController>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            gameController.AddPoint(pointsWorth);
        }
    }
}
