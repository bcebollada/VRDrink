using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class CutCupMobileController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent eventBomb, eventCup;


    private MacroGameController macroGameController;
    public CutCupGameController gameController;

    public GameObject scoreBoard;


    private void Awake()
    {
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CutCupGameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameController);
        eventBomb.AddListener(gameController.SpawnBomb);
        eventCup.AddListener(gameController.SpawnCup);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCup()
    {
        eventCup.Invoke();
    }

    public void SpawnBomb()
    {
        eventBomb.Invoke();
    }

    public void ShowScoreboard()
    {
        scoreBoard.SetActive(true);
        transform.parent.position += new Vector3(0, 50, 0);
    }
}
