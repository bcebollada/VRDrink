using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Normal.Realtime;


public class WaitingBoard : MonoBehaviour
{
    private MacroGameController macroGameController;

    public TMP_Text player1Status, player2Status, player3Status, player4Status;

    public bool areAllPlayersConnected; //check bool to see if everyone connected already

    public GameObject startStand;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;

    public Transform cupSpawn;



    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;

        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!macroGameController.isMobileRig) //spawn cup
        {
            var cup = Realtime.Instantiate("Cup", cupSpawn.position, Quaternion.identity, instantiateOptions);
        }

        StartCoroutine(LinkEvent());

        if (macroGameController.playerNumbers == 1) //2 players playing
        {
            //deativate texts for others players
            player3Status.gameObject.transform.parent.gameObject.SetActive(false);
            player4Status.gameObject.transform.parent.gameObject.SetActive(false);
        }

        else if (macroGameController.playerNumbers == 2) //3 players playing
        {
            //deativate texts for others players
            player4Status.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] mobileRigs = GameObject.FindGameObjectsWithTag("MobileRig");
        if(mobileRigs.Length >= 1 && !areAllPlayersConnected)
        {
            player2Status.text = "Ready";
            if (macroGameController.playerNumbers - 1 == 0) areAllPlayersConnected = true;
        }
        if (mobileRigs.Length >= 2 && !areAllPlayersConnected)
        {
            player3Status.text = "Ready";
            if (macroGameController.playerNumbers - 2 == 0) areAllPlayersConnected = true;
        }
        if (mobileRigs.Length == 3 && !areAllPlayersConnected)
        {
            player4Status.text = "Ready";
            if (macroGameController.playerNumbers - 3 == 0) areAllPlayersConnected = true;
        }

        if (areAllPlayersConnected) startStand.SetActive(true);
    }

    private IEnumerator LinkEvent()
    {
        yield return new WaitForSeconds(2);

        GameObject[] cups = GameObject.FindGameObjectsWithTag("Cup");
        foreach (var cup in cups)
        {
            cup.GetComponent<CupBehaviour>().myEvent.AddListener(ButtonPress); //link cup behavior action
        }
    }

    public void ButtonPress()
    {
        macroGameController.NextGame();
    }
}
