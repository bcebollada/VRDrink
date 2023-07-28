using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupFallTrigger : MonoBehaviour
{
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
        if (other.gameObject.CompareTag("Cup"))
        {
            if(other.gameObject.GetComponent<CupBehaviour>() != null)
            {
                other.gameObject.GetComponent<CupBehaviour>().ReturnInitialTranform();
            }
            else if(other.gameObject.GetComponent<PlayerNumCups_Script>() != null)
            {
                other.gameObject.GetComponent<PlayerNumCups_Script>().ReturnInitialTranform();
            }
        }
    }
}
