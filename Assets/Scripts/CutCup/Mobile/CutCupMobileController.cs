using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using Normal.Realtime;

public class CutCupMobileController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent eventBomb, eventCup;


    private MacroGameController macroGameController;
    public CutCupGameController gameController;
    public int playerNumber;

    public GameObject scoreBoard;

    private int maxAmmo;
    private int currentAmmo;

    public float ammoRechargeTime;
    private float rechargeTime;

    public float shootCooldown;
    private float coolDownTime;

    public TMP_Text ammoText, mobilePointsTxt, VRPointsTxt;

    public Slider slider;
    private float initialTimer;

    public Button bombButton;
    public Button cupButton;
    public GameObject aimImage;
    public FixedJoystick joystick;
    public Vector2 maxAimPos;
    public float aimMoveSpeed;
    private Vector2 aimPos;

    public Camera cam;

    public Image[] iconsArray;
    private CannonController ownedCannon;

    public RealtimeView realtimeView;

    private void Awake()
    {
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CutCupGameController>();

        //set player number
        if (GameObject.FindGameObjectWithTag("PlayerNumber") != null)
        {
            playerNumber = GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().playerNumber;
        }
        else playerNumber = GameObject.FindGameObjectsWithTag("MobileRig").Length + 1;
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

        //update ammo icons
        //UpdateIcon();

        initialTimer = gameController.timeLeft;
        slider.maxValue = initialTimer;

        //request cannon
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            var cannons = GameObject.FindObjectsOfType<CannonController>();
            foreach (var cannon in cannons)
            {
                if (cannon.number == playerNumber)
                {
                    ownedCannon = cannon;
                    cannon.RequestOwner();
                    gameController.laucherTransform = cannon.launchPoint;
                    break;
                }
            }
        }
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

                //update ammo icons
                //UpdateIcon();
            }
            
        }

        UpdateIcon();
        
        //update cannon aim position
        if(ownedCannon != null && realtimeView.isOwnedLocallyInHierarchy)
        {
            var target = CalculateShooting();
            ownedCannon.UpdateCannonAim(target);
            
        }

        //calculates time of cooldown
        coolDownTime += Time.deltaTime;
        if (coolDownTime >= shootCooldown)
        {
            coolDownTime = 0;
            bombButton.interactable = true;
            cupButton.interactable = true;  
        }

        slider.value = initialTimer - gameController.timeLeft; //update time slider

        //calculate aim  position and apply to aim image
        //aimPos = Mathf.Clamp(joystick.Horizontal * aimMoveSpeed, maxAimPos / 2, maxAimPos);
        aimPos.x = Mathf.Clamp(joystick.Horizontal * aimMoveSpeed + aimImage.transform.localPosition.x, -maxAimPos.x, maxAimPos.x);
        aimPos.y = Mathf.Clamp(joystick.Vertical * aimMoveSpeed + aimImage.transform.localPosition.y, -maxAimPos.y, maxAimPos.y);
        aimImage.transform.localPosition = new Vector3( aimPos.x, aimPos.y, aimImage.transform.localPosition.z);

        mobilePointsTxt.text = gameController.pointsCommunicator.mobilePoints.ToString();
        VRPointsTxt.text = gameController.pointsCommunicator.VRPoints.ToString();
    }

    public void SpawnCup()
    {
        //if doesnt has ammo, return;
        if (currentAmmo > 0)
        {
            currentAmmo -= 1;
            ammoText.text = currentAmmo.ToString();

            int randomInt = Random.Range(0, 2);
            string objName;
            if (randomInt != 0) objName = "CutCup";
            else objName = "Bomb";

            var target = CalculateShooting();

            gameController.LaunchObj(objName, target);

            rechargeTime = 0;
            coolDownTime = 0;
            bombButton.interactable = false;
            cupButton.interactable = false;
        }
    }

    private void UpdateIcon()
    {
        for (int i = 0; i < iconsArray.Length; i++)
        {
            if (i >= currentAmmo)
            {
                iconsArray[i].gameObject.SetActive(false);
            }
            else
            {
                //Color newColor = new Color(0.2588235f, 0.3803922f, 0.2470588f);
                //iconsArray[i].color = newColor;

                iconsArray[i].gameObject.SetActive(true);

            }


            if ( i == currentAmmo)
            {
                iconsArray[i].gameObject.SetActive(true);

                //fades in recharging ammo
                Color newColor = Color.white;
                newColor.a = Mathf.Lerp(0, 1, rechargeTime / ammoRechargeTime);
                iconsArray[i].color = newColor;
            }
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
            //eventBomb.Invoke();

            var target = CalculateShooting();
            gameController.LaunchObj("Bomb", target);

            //rechargeTime = 0;
            coolDownTime = 0;
            bombButton.interactable = false;
            cupButton.interactable = false;
        } 
    }

    public void ShowScoreboard()
    {
        scoreBoard.SetActive(true);
        transform.parent.position += new Vector3(0, 50, 0);
    }

    Vector3 CalculateShooting()
    {
        // Get the position of the UI image in screen space
        Vector2 aimScreenPosition = RectTransformUtility.WorldToScreenPoint(cam, aimImage.transform.position);

        //Create raycast to see where the obj should land
        Ray aimRay = cam.ScreenPointToRay(aimScreenPosition);

        if (Physics.Raycast(aimRay, out RaycastHit hit, 1000))
        {
            Vector3 result = hit.point;
            return result;

        }
        else return Vector3.zero;
    }
}
