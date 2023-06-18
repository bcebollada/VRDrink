using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class InterceptorRotation : MonoBehaviour
{
    public bool playAnimation = false; // The bool that triggers the animation
    public bool isMobileRig;
    private bool isRotating;
    private InterceptorBoolArrayCommunicator interceptorBool;

    public int interceptorNumber; //so we know which interceptor this is and which rig should have i

    public Transform meshHolder;

    public float rotateDuration = 1.0f;


    public float moveDuration = 1.0f;
    public float moveDistance = 1.5f;

    IEnumerator MoveUp()
    {
        Vector3 startPosition = meshHolder.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0, moveDistance, 0);

        float t = 0.0f;
        while (t < moveDuration)
        {
            meshHolder.localPosition = Vector3.Lerp(startPosition, endPosition, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(MoveBackToOriginal(startPosition)); // Start the reverse movement coroutine
    }

    IEnumerator MoveBackToOriginal(Vector3 endPosition)
    {
        Vector3 startPosition = meshHolder.localPosition;

        float t = 0.0f;
        while (t < moveDuration)
        {
            meshHolder.localPosition = Vector3.Lerp(startPosition, endPosition, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }

        meshHolder.localPosition = endPosition; // Set it back to original position
        AnimationComplete();
    }
    IEnumerator Rotate90Degrees()
    {
        Quaternion startRotation = meshHolder.localRotation;
        Quaternion endRotation = Quaternion.Euler(90, 0, 0);

        float t = 0.0f;
        while (t < rotateDuration)
        {
            meshHolder.localRotation = Quaternion.Lerp(startRotation, endRotation, t / rotateDuration);
            t += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(RotateBackToOriginal(startRotation)); // Start the reverse rotation coroutine
    }

    IEnumerator RotateBackToOriginal(Quaternion endRotation)
    {
        Quaternion startRotation = meshHolder.localRotation;


        float t = 0.0f;
        while (t < rotateDuration)
        {
            meshHolder.localRotation = Quaternion.Lerp(startRotation, endRotation, t / rotateDuration);
            t += Time.deltaTime;
            yield return null;
        }

        meshHolder.localRotation = endRotation; // Set it back to original rotation
        AnimationComplete();
    }

    private void Awake()
    {
        interceptorBool = GetComponent<InterceptorBoolArrayCommunicator>();
    }
    private void Update()
    {
        if (interceptorNumber == 2) playAnimation = interceptorBool.activateInterceptor1;
        else if (interceptorNumber == 3) playAnimation = interceptorBool.activateInterceptor2;
        else if (interceptorNumber == 4) playAnimation = interceptorBool.activateInterceptor3;

        //playAnimation = interceptorBool.playAnimation;

        // Check if the bool is true and the animation isn't already playing
        if (playAnimation && !isRotating && isMobileRig)
        {
            Debug.Log("Interceptor rotating");
            isRotating = true;
            StartCoroutine(MoveUp());
        }
    }

    public void AnimationComplete()
    {
        interceptorBool.SetBool(false, interceptorNumber - 1);

        Debug.Log("Interceptor rotating complete");

        isRotating = false;
    }

    public void ActivateInterceptor()
    {
        Debug.Log("Activating Interceptor");
        interceptorBool.SetBool(true, interceptorNumber - 1);
        Debug.Log("Interceptor bool set");
    }
}
