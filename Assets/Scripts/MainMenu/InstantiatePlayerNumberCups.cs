using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class InstantiatePlayerNumberCups : MonoBehaviour
{
    public Transform spawnPosition;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;

    public int cupNumber;


    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;
    }

    // Start is called before the first frame update
    void Start()
    {

        if ((SystemInfo.deviceModel.Contains("Oculus Quest") || SystemInfo.deviceModel.Contains("Raider") || Application.platform == RuntimePlatform.WindowsEditor))
        {
            var cup = Realtime.Instantiate("PlayerNumberCup", spawnPosition.position, Quaternion.identity, instantiateOptions);
            cup.GetComponent<PlayerNumCups_Script>().cupNumber = cupNumber;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
