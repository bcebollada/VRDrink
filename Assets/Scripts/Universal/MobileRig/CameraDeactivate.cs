using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeactivate : MonoBehaviour
{
    private MacroGameController macroGameController;
    public Camera cam;

    private void Awake()
    {
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        if (!macroGameController.isMobileRig) Destroy(this.gameObject);  
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
