using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkMeToStart_Script : MonoBehaviour
{
    public GameObject cup, camera;
    public float textOffset;
    public bool isInstantiated;


    private void Awake()
    {
        if(isInstantiated) camera = GameObject.Find("MacroGameController").GetComponent<MacroGameController>().playerCamera;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(cup.transform.position.x, cup.transform.position.y + textOffset, cup.transform.position.z);
        transform.LookAt(camera.transform);
    }

    public void CupGrabbed()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CupRealesed()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
