using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CupBehaviour : MonoBehaviour
{
    private Quaternion initialRotation;
    public bool shouldSpillDrink;
    public GameObject beerEffect;
    private float secondsSpilling;

    public bool nextGameEvent;

    [SerializeField] public UnityEvent myEvent;

    private bool hasPerformed;


    void Start()
    {
        beerEffect.SetActive(false);
        initialRotation = transform.localRotation;

        MacroGameController macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        if (nextGameEvent && myEvent == null) myEvent.AddListener(macroGameController.NextGame);
    }

    void Update()
    {
        float angle = Quaternion.Angle(transform.localRotation, initialRotation);
        if ((transform.eulerAngles.x >= 90 && transform.eulerAngles.x <= 270) || (transform.eulerAngles.z >= 90 && transform.eulerAngles.z <= 270))
        {
            if (hasPerformed) return;
            // Cup has been rotated 90 degrees or more
            Debug.Log("Cup rotated!");
            if(shouldSpillDrink) beerEffect.SetActive(true);
            secondsSpilling += Time.deltaTime;
            if (myEvent != null && secondsSpilling >= 0.8f)
            {
                myEvent.Invoke();
                hasPerformed = true;
            }
        }
        else
        {
            beerEffect.SetActive(false);
            secondsSpilling = 0;
        }


    }
}
