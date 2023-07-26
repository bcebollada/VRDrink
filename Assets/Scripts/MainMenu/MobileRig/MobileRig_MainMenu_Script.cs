using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Normal.Realtime;
using UnityEngine.SceneManagement;

public class MobileRig_MainMenu_Script : MonoBehaviour
{
    public int playerNumber;
    public TMP_Text playerNumberText, debugText;

    private MacroGameController macroGameController;
    private PointsManager pointsManager;

    private RealtimeView realtimeView;

    public GameObject cam;

    private bool connected;



    private void Awake()
    {           
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        pointsManager = GameObject.FindGameObjectWithTag("PlayerPoints").GetComponent<PointsManager>();
        realtimeView = GetComponent<RealtimeView>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!realtimeView.isOwnedLocallySelf) cam.SetActive(false); //deactivate cameras if is not the one playing

        if (!connected)
        {
            if (macroGameController.isMobileRig && realtimeView.isOwnedLocallyInHierarchy)
            {

                /*if (pointsManager.player2Points == 0)
                {
                    playerNumber = 2; //is player 2 
                    pointsManager.SetPoints(2, 1);
                }
                else if (pointsManager.player3Points == 0)
                {
                    playerNumber = 3; //is player 3
                    pointsManager.SetPoints(3, 1);
                }
                else
                {
                    playerNumber = 4; //is player 4
                    pointsManager.SetPoints(4, 1);
                }*/

                playerNumber = GameObject.FindGameObjectsWithTag("MobileRig").Length + 1;


                playerNumberText.text = $"Your are Player {playerNumber}";
                GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().playerNumber = playerNumber; //saves variable to dont destroy on load object

                macroGameController.setMobileNumber = true;
                connected = true;
            }
        }
    }

    public void Button()
    {
        GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<PlayersNumberCommunicator>().SetNumber(1);
    }

}
