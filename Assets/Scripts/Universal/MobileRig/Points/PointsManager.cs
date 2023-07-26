using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Normal.Realtime;

public class PointsManager : MonoBehaviour
{
    private Points1ModelCommunicator player1Model;
    private Points2ModelCommunicator player2Model;
    private Points3ModelCommunicator player3Model;
    private Points4ModelCommunicator player4Model;

    public int player1Points, player2Points, player3Points, player4Points;

    public bool isMain;

    private void Awake()
    {
        player1Model = GetComponent<Points1ModelCommunicator>();
        player2Model = GetComponent<Points2ModelCommunicator>();
        player3Model = GetComponent<Points3ModelCommunicator>();
        player4Model = GetComponent<Points4ModelCommunicator>();

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (GameObject.FindGameObjectsWithTag("PlayerPoints").Length > 1)
        {
            if (!isMain) Destroy(this.gameObject);
        }
    }


    public void SetPoints(int player, int points)
    {
        if (player == 1) player1Model.SetPoints(points);
        else if (player == 2) player2Model.SetPoints(points);
        else if (player == 3) player3Model.SetPoints(points);
        else player4Model.SetPoints(points);

        UpdatePoints();
    }

    public void AddPoints(int player, int points)
    {
        if (player == 1) player1Model.SetPoints(player1Model.player1Points + points);
        else if (player == 2) player2Model.SetPoints(player2Model.player2Points + points);
        else if (player == 3) player3Model.SetPoints(player3Model.player3Points + points);
        else player4Model.SetPoints(player4Model.player4Points + points);

        UpdatePoints();
    }

    private void UpdatePoints()
    {
        player1Points = player1Model.player1Points;
        player2Points = player2Model.player2Points;
        player3Points = player3Model.player3Points;
        player4Points = player4Model.player4Points;
    }

    private void Update()
    {
        UpdatePoints();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }
}
