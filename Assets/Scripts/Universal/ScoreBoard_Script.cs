    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HurricaneVR.Framework.Core.UI;

public class ScoreBoard_Script : MonoBehaviour
{
    public TMP_Text player1, player1Score, player2, player2Score, player3, player3Score, player4, player4Score;
    private MacroGameController macroGameController;
    private int[] playerShots;
    private HVRInputModule hvrInputModule;

    private void Awake()
    {
        macroGameController = GameObject.Find("MacroGameController").GetComponent<MacroGameController>();


        hvrInputModule = GameObject.Find("UIManager").GetComponent<HVRInputModule>();
        hvrInputModule.UICanvases.Add(this.GetComponent<Canvas>());

        playerShots = macroGameController.playerShots;
    }

    private void Start()
    {
        player1Score.text = playerShots[0].ToString();
        player2Score.text = playerShots[1].ToString();

        //checks if points is negative to know if player exists
        if(playerShots[2] < 0)
        {
            player3.text = "";
            player3Score.text = "";
        }
        else player3Score.text = playerShots[2].ToString();

        //checks if points is negative to know if player exists
        if (playerShots[3] < 0)
        {
            player4.text = "";
            player4Score.text = "";
        }
        else player4Score.text = playerShots[3].ToString();
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
