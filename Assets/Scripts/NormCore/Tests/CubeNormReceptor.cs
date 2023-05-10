using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeNormReceptor : MonoBehaviour
{
    public CubeScale cubeScale;

    // Start is called before the first frame update
    void Start()
    {
        cubeScale = GetComponent<CubeScale>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scale()
    {
        cubeScale.SetScale(new Vector3(2, 2, 2));
    }
}
