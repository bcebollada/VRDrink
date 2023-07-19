using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class NewBehaviourScript : MonoBehaviour
{
    public CutCupGameController gamecontroller;

    public Vector3 areaCenter;
    public Vector3 areaSize;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;

    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        //instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;
    }

    public void LaunchOnStage()
    {
        var targetArea = areaCenter + new Vector3(Random.Range(-areaSize.x / 2, areaSize.x / 2), 0, Random.Range(-areaSize.z / 2, areaSize.z / 2));

        var Vo = CalculateVelocity(targetArea, gameObject.transform.position, 4f);

        //GameObject obj = Instantiate(bola, gameObject.transform.position, Quaternion.identity);
        GameObject obj = Realtime.Instantiate("CutCup", gameObject.transform.position, rotation: Quaternion.identity,instantiateOptions);

        obj.GetComponent<Rigidbody>().velocity = Vo;
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y first
        Vector3 distance = target - origin;
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
