using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CupBehaviour : MonoBehaviour
{
    private Quaternion initialRotation;
    public GameObject beerEffect;
    private float secondsSpilling;

    [SerializeField] private UnityEvent myEvent;


    void Start()
    {
        beerEffect.SetActive(false);
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Quaternion.Angle(transform.localRotation, initialRotation);
        if (angle >= 90.0f)
        {
            // Cup has been rotated 90 degrees or more
            Debug.Log("Cup rotated!");
            beerEffect.SetActive(true);
            secondsSpilling += Time.deltaTime;
            if (myEvent != null && secondsSpilling >= 3) myEvent.Invoke();
        }
        else
        {
            beerEffect.SetActive(false);
            secondsSpilling = 0;
        }


    }
}
