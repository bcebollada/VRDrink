using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollowPlayer : MonoBehaviour
{
    public GameObject camera;
    public bool isInstantiated;
    private MacroGameController macroGameController;



    private void Awake()
    {
        if (isInstantiated) camera = GameObject.Find("MacroGameController").GetComponent<MacroGameController>().playerCamera;

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform);
    }

    public void CupGrabbed()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }

    public void CupRealesed()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(true);
        }
    }
}
