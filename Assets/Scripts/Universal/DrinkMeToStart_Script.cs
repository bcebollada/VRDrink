using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrinkMeToStart_Script : MonoBehaviour
{
    public GameObject cup, camera;
    public float textOffset;
    public bool isInstantiated;
    public TMP_Text playerNameText;
    private MacroGameController macroGameController;



    private void Awake()
    {
        macroGameController = GameObject.Find("MacroGameController").GetComponent<MacroGameController>();

    }

    private void Start()
    {
        playerNameText.text = macroGameController.playerPlaying;
        if (isInstantiated) camera = macroGameController.GetComponent<MacroGameController>().playerCamera;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(cup.transform.position.x, cup.transform.position.y + textOffset, cup.transform.position.z);
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
