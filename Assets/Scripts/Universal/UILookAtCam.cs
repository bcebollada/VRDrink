using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCam : MonoBehaviour
{
    private MacroGameController macroGameController;
    public GameObject cam;


    private void Awake()
    {
        macroGameController = GameObject.Find("MacroGameController").GetComponent<MacroGameController>();

    }
    // Start is called before the first frame update
    void Start()
    {
        cam = macroGameController.GetComponent<MacroGameController>().playerCamera;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform);
    }
}
