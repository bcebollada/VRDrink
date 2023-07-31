using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkInstruction : MonoBehaviour
{
    public GameObject instruction;

    // Start is called before the first frame update
    void Start()
    {
        instruction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInstruction()
    {
        instruction.SetActive(true);
    }

    public void HideInstruction()
    {
        instruction.SetActive(false);
    }
}
