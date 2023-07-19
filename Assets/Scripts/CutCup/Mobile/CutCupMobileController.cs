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

    private int maxAmmo;
    private int currentAmmo;

    public float ammoRechargeTime;
    private float rechargeTime;
    public TMP_Text ammoText;

    public Slider slider;
    private float initialTimer;


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

        Transform spawnTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;

        maxAmmo = 4 - macroGameController.playerNumbers; //set the max ammo the mobile player has based on the num of players
        currentAmmo = maxAmmo;
        ammoText.text = currentAmmo.ToString();

        initialTimer = gameController.timeLeft;
        slider.maxValue = initialTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //calculates time to recharge ammo
        rechargeTime += Time.deltaTime;
        if(rechargeTime >= ammoRechargeTime)
        {
            rechargeTime = 0;

            //if ammo is not maxed, add
            if (currentAmmo < maxAmmo)
            {
                currentAmmo += 1;
                ammoText.text = currentAmmo.ToString();
            }
            
        }

        slider.value = initialTimer - gameController.timeLeft; //update time slider

    }

    public void SpawnCup()
    {
        //if doesnt has ammo, return;
        if (currentAmmo > 0)
        {
            currentAmmo -= 1;
            ammoText.text = currentAmmo.ToString();

            eventCup.Invoke();
        }
    }

    public void SpawnBomb()
    {
        //if doesnt has ammo, return;
        if (currentAmmo > 0) 
        {
            Debug.Log(currentAmmo);
            currentAmmo -= 1;
            ammoText.text = currentAmmo.ToString();

            eventBomb.Invoke();
        } 
    }

    public void ShowScoreboard()
    {
        scoreBoard.SetActive(true);
        transform.parent.position += new Vector3(0, 50, 0);
    }
}
