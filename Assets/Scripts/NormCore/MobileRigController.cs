using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class MobileRigController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent eventToPerform;

    private MacroGameController macroGameController;

    public TMP_Text text, debugText;

    public GameObject scoreBoard;

    public int miniGamesPlayed; //int to know which game we are 0 = none 1 = beerpong 2 = shootCup 3 = roullete

    public int playerNumber;

    private void Awake()
    {
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        miniGamesPlayed = macroGameController.miniGamesPlayed;
    }
    
    private void Start()
    {
        if (!macroGameController.isMobileRig) return;

        if (GameObject.FindGameObjectWithTag("PlayerNumber") != null)
        {
            playerNumber = GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().playerNumber;
        }
        else playerNumber = GameObject.FindGameObjectsWithTag("MobileRig").Length + 1;

        if (GameObject.FindGameObjectWithTag("MobileRigPlayerNumber") == null) //mobile rig still don't have player number attached to
        {
            Debug.Log("This mobile rig is the " + playerNumber + "player");
        }
        
        if (miniGamesPlayed == 1) 
        {
            Transform spawnTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
            transform.parent.position = spawnTransform.position;
            transform.parent.rotation = spawnTransform.rotation;
        }

        
        var interceptorArray = GameObject.FindGameObjectsWithTag("Interceptor"); //update do action for beer pong
        foreach (var interceptor in interceptorArray)
        {
            if (interceptor.GetComponent<InterceptorRotation>().interceptorNumber == playerNumber)
            {
                //interceptor.transform.GetChild(1).gameObject.GetComponent<RealtimeView>().RequestOwnership();
                //interceptor.transform.GetChild(1).gameObject.SetActive(true);
                eventToPerform.AddListener(interceptor.GetComponent<InterceptorRotation>().ActivateInterceptor);
            }

        }
    }
    
    public void Action()
    {
        text.text = "action";
        eventToPerform.Invoke();

    }

    public void ShowScoreboard()
    {
        scoreBoard.SetActive(true);
    }


}
