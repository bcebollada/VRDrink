using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupPoint : MonoBehaviour
{
    private BeerGameController gameController;

    public int pointsWorth;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BeerGameController>();
        
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
            Debug.Log("Ball enter cup");
            gameController.AddPoint(pointsWorth);
            //Destroy(other.gameObject, 3);
            //gameController.SpawnBall();
            gameController.PlaySound(BeerGameController.soundNames.niceShot);
        }
    }
}
