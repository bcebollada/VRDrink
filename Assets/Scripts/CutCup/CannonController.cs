using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class CannonController : MonoBehaviour
{
    public Transform cannon, launchPoint;
    public int number;
    public CutCupGameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CutCupGameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void RequestOwner()
    {
        cannon.gameObject.GetComponent<RealtimeView>().RequestOwnership();
        GetComponent<RealtimeView>().RequestOwnership();
        cannon.gameObject.GetComponent<RealtimeTransform>().RequestOwnership();
        GetComponent<RealtimeTransform>().RequestOwnership();
    }

    public void UpdateCannonAim(Vector3 target)
    {
        var Vo = CalculateVelocity(target, gameController.launchTimeDuration);
        cannon.LookAt(cannon.position + Vo); // Point the cannon towards the target position based on Vo
    }

    Vector3 CalculateVelocity(Vector3 target, float time)
    {
        //define the distance x and y first
        Vector3 distance = target - launchPoint.position;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        //create a float the represent our distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }

}
