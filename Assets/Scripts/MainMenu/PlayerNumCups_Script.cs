using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerNumCups_Script : MonoBehaviour
{
    private Quaternion initialRotation;
    public bool shouldSpillDrink;
    public GameObject beerEffect;
    private float secondsSpilling;
    private MacroGameController macroGameController;
    public int cupNumber; //need to know so that it calls the right event

    [SerializeField] private UnityAction myEvent;

    private void Awake()
    {

    }


    void Start()
    {
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        if (cupNumber == 2)
        {
            myEvent = macroGameController.PlayersNum2;
            print("player2");
        }

        else if (cupNumber == 3) myEvent = macroGameController.PlayersNum3;
        if (cupNumber == 4) myEvent = macroGameController.PlayersNum4;

        beerEffect.SetActive(false);
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Quaternion.Angle(transform.localRotation, initialRotation);
        if ((transform.eulerAngles.x >= 90 && transform.eulerAngles.x <= 270) || (transform.eulerAngles.z >= 90 && transform.eulerAngles.z <= 270))
        {
            // Cup has been rotated 90 degrees or more
            Debug.Log("Cup rotated!");
            if (shouldSpillDrink) beerEffect.SetActive(true);
            secondsSpilling += Time.deltaTime;
            if (myEvent != null && secondsSpilling >= 0.8f) myEvent.Invoke();
        }
        else
        {
            beerEffect.SetActive(false);
            secondsSpilling = 0;
        }


    }
}
