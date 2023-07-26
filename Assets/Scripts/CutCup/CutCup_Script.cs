using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class CutCup_Script : MonoBehaviour
{
    private CutCupGameController gameController;
    private Realtime.InstantiateOptions instantiateOp = new Realtime.InstantiateOptions();


    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CutCupGameController>();
        instantiateOp.ownedByClient = true;
        instantiateOp.useInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {

        Realtime.Instantiate("Beer Impact", transform.position, transform.rotation, instantiateOp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("cup collided with" + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            //gameController.GameOver();
            Destroy(this.gameObject);
        }
    }
}
