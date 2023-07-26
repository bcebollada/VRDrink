using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;


public class MobileRigController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent eventToPerform;

    private MacroGameController macroGameController;

    private BeerGameController beerGameController;

    public TMP_Text text, pointsText , debugText;

    public GameObject scoreBoard;

    public int miniGamesPlayed; //int to know which game we are 0 = none 1 = beerpong 2 = shootCup 3 = roullete

    public int playerNumber;

    public Slider slider;
    private float initialTimer;


    public float blockCooldown;
    private float coolDownTime;
    public Button blockButton;


    private void Awake()
    {
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        beerGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BeerGameController>();
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
                interceptor.transform.GetChild(1).gameObject.SetActive(true);
                Destroy(interceptor.transform.GetChild(1).gameObject, 3);
                eventToPerform.AddListener(interceptor.GetComponent<InterceptorRotation>().ActivateInterceptor);
            }

        }

        initialTimer = beerGameController.timeLeft;
        slider.maxValue = initialTimer;
    }

    private void Update()
    {
        if (beerGameController.gameStartCountdown || beerGameController.timerRunning) debugText.text = "started";
        else debugText.text = "no";

        slider.value = initialTimer - beerGameController.timeLeft;

        //calculates time of cooldown
        coolDownTime += Time.deltaTime;
        if (coolDownTime >= blockCooldown)
        {
            coolDownTime = 0;
            blockButton.interactable = true;
        }

        pointsText.text = beerGameController.points.ToString()  ;
    }

    public void Action()
    {
        //text.text = "action";
        eventToPerform.Invoke();

        blockButton.interactable = false;
        coolDownTime = 0;
    }

    public void ShowScoreboard()
    {
        scoreBoard.SetActive(true);
        transform.parent.position += new Vector3(0, 50, 0);
    }


}
