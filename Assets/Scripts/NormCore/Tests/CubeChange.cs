using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeChange : MonoBehaviour
{
    public bool isScaled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isScaled)
        {
            transform.localScale = new Vector3(2, 2, 2);    
        }
        else transform.localScale = new Vector3(1, 1, 1);

        if(GameObject.Find("GameController") != null)
        {
            if(GameObject.Find("GameController").GetComponent<CubeChangeButton>().text.text == "clicked") transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
