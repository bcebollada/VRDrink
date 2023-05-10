using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;
using TMPro;


public class MobileRigController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent eventToPerform;

    private MacroGameController macroGameController;

    public TMP_Text text;

    public int miniGamesPlayed; //int to know which game we are 0 = none 1 = beerpong 2 = shootCup 3 = roullete

    private int mobileRigPlayerNumber;

    private void Awake()
    {
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        miniGamesPlayed = macroGameController.miniGamesPlayed;
    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("MobileRigPlayerNumber") == null) //mobile rig still don't have player number attached to
        {
            mobileRigPlayerNumber = GameObject.FindGameObjectsWithTag("MobileRig").Length;
            Debug.Log("This mobile rig is the " + mobileRigPlayerNumber + "player");
        }

        if (miniGamesPlayed == 1) 
        {
            Transform spawnTransform = GameObject.Find("MobileRigPosition").transform;
            transform.parent.position = spawnTransform.position;
            transform.parent.rotation = spawnTransform.rotation;
        }

        UpdateActions(); // update main features based on the mini game number
    }


    // Update is called once per frame
    void Update()
    {
        //UpdateActions(); //update what action main buitton on mobile rig will do

    }

    public void Action()
    {
        text.text = "action";
        eventToPerform.Invoke();

    }

    private void UpdateActions()
    {
        if (miniGamesPlayed == 0)
        {
            eventToPerform = null; //update do action for main menu
        }
        else if (miniGamesPlayed == 1)
        {
            //var interceptor = GameObject.FindGameObjectWithTag("Interceptor").GetComponent<InterceptorRotation>();

            var interceptorArray = GameObject.FindGameObjectsWithTag("Interceptor"); //update do action for beer pong
            foreach (var interceptor in interceptorArray)
            {
                if (interceptor.GetComponent<InterceptorRotation>().interceptorNumber == mobileRigPlayerNumber)
                {
                    interceptor.transform.GetChild(1).gameObject.GetComponent<RealtimeView>().RequestOwnership();
                    interceptor.transform.GetChild(1).gameObject.SetActive(true);
                    eventToPerform.AddListener(interceptor.GetComponent<InterceptorRotation>().ActivateInterceptor);
                }

            }

        }
        else if (miniGamesPlayed == 2)
        {
            eventToPerform = null; //update do action for shoot cup
        }
        else if (miniGamesPlayed == 3)
        {
            eventToPerform = null; //update do action for roullete
        }
    }
}
