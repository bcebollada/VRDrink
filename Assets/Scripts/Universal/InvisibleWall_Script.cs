using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall_Script : MonoBehaviour
{
    public MeshRenderer meshRender;
    private void Awake()
    {
        meshRender.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8) meshRender.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8) meshRender.enabled = false;
    }
}
