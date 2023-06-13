    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HurricaneVR.Framework.Core.UI;
using Normal.Realtime;

public class ScoreBoard_Script : MonoBehaviour
{
    public TMP_Text player1, player1Score, player2, player2Score, player3, player3Score, player4, player4Score;
    private MacroGameController macroGameController;
    private int[] playerShots;
    private HVRInputModule hvrInputModule;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;


    public Transform cupSpawn;


    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;
    }

    private void Start()
    {
        player1Score.text = "start void";

        //find macroController and get shots from him
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        player2Score.text = (GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>().ToString());

        if (!macroGameController.isMobileRig) //spawn cup
        {
            var cup = Realtime.Instantiate("Cup", cupSpawn.position, Quaternion.identity, instantiateOptions);
            cup.GetComponent<CupBehaviour>().myEvent.AddListener(ButtonPress); //link cup behavior action
        }


        if (SystemInfo.deviceModel.Contains("Quest 2") || SystemInfo.deviceModel.Contains("Raider") || Application.platform == RuntimePlatform.WindowsEditor) //if is not mobile
        {
            hvrInputModule = GameObject.Find("UIManager").GetComponent<HVRInputModule>();
            hvrInputModule.UICanvases.Add(this.GetComponent<Canvas>());
        }


        playerShots = macroGameController.playerLocalShots;

        player1Score.text = $"Take {playerShots[0]} shots";
        player2Score.text = $"Take {playerShots[1]} shots";

        //checks how many players are to deactivate labels on scoreBoard
        if (macroGameController.playerNumbers < 2)
        {
            player3.transform.parent.gameObject.SetActive(false);
            player3.text = "";
            player3Score.text = "";
        }
        else player3Score.text = $"Take {playerShots[2]} shots";

        //checks how many players are to deactivate labels on scoreBoard
        if (macroGameController.playerNumbers < 3)
        {
            player4.transform.parent.gameObject.SetActive(false);
            player4.text = "";
            player4Score.text = "";
        }
        else player4Score.text = $"Take {playerShots[3]} shots";
    }

    public void ButtonPress()
    {
        macroGameController.NextGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
