using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Bomb_Script : MonoBehaviour
{
    public GameObject explosionEffect;

    private CutCupGameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CutCupGameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Realtime.InstantiateOptions instantiateOp = new Realtime.InstantiateOptions();
        instantiateOp.ownedByClient = true;
        Realtime.Instantiate("Fire Impact", transform.position, transform.rotation, instantiateOp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
