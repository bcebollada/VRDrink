using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class DeactivateOnRemote : MonoBehaviour
{
    public GameObject[] objToDeativate;
    public RealtimeView realtimeView;

    private bool check;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (realtimeView.room.connected && !check)
        {
            if (realtimeView.isOwnedRemotelyInHierarchy) Deactivate();
            check = true;

        }
    }

    private void Deactivate()
    {
        foreach (var obj in objToDeativate)
        {
            obj.SetActive(false);
        }
    }
}
