using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Bomb_Script : MonoBehaviour
{
    public GameObject explosionEffect;
    private bool hasCollided;

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
        Realtime.Instantiate("Fire Impact", transform.position, transform.rotation, instantiateOp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
        else if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 21) && !hasCollided)
        {
            if (!gameController.macroGameController.isMobileRig)
            {
                hasCollided = true;
                gameController.PlaySound(CutCupGameController.soundNames.goodAim);
                gameController.AddLocalMobilePoint(1);
                Destroy(this.gameObject);
            }
        }
    }
}
