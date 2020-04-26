using System.Collections;
using System.Collections.Generic;
using _Scripts.AISystem;
using UnityEngine;

public class TempManager : MonoBehaviour
{

    public AIManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        manager.FindAIPoints();
        manager.isCreationFinish = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
