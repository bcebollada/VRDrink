using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Normal.Realtime;

public class CubeChangeButton : MonoBehaviour
{
    public TMP_Text text;
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScale()
    {
        text.text = "clicked";
        cube = GameObject.FindWithTag("Obstacle");
        if(cube != null)
        {
            cube.GetComponent<RealtimeView>().RequestOwnership();
            cube.GetComponent<RealtimeTransform>().RequestOwnership();
            cube.GetComponent<CubeNormReceptor>().Scale();
            text.text = cube.transform.localScale.ToString();
        }
    }
}
