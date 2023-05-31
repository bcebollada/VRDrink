using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RequestOwnership : MonoBehaviour
{
    private bool hasRequested;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<RealtimeView>().room.connected && !hasRequested)
        {
            if (SystemInfo.deviceModel.Contains("Quest") || SystemInfo.deviceModel.Contains("Raider") || Application.platform == RuntimePlatform.WindowsEditor) 
                StartCoroutine("RequestOwenerShip"); //runs just on quest or  PC
        }
    }

    private IEnumerator RequestOwenerShip()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Requesting ownership");

        hasRequested = true;
        if(GetComponent<RealtimeTransform>() != null) GetComponent<RealtimeTransform>().RequestOwnership();
        GetComponent<RealtimeView>().RequestOwnership();
    }
}
