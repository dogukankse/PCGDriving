using System;
using System.Linq;
using _Scripts;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public int objCount;
    
    public AIObj[] prefabs;
    public AIPoint[] aiPoints;

    public bool isCreationFinish = false;
    

    private void Update()
    {
        if (!isCreationFinish) return;
        if (transform.childCount < objCount)
        {
            AIPoint startPoint = PickStartPoint();
            AIObj obj =  Instantiate(prefabs.ToArray().GetRandomFrom(), startPoint.transform.position, Quaternion.identity, transform);
            obj.startPoint = startPoint;
        }
    }

    public void FindAIPoints()
    {
        aiPoints = FindObjectsOfType<AIPoint>();
    }

    private AIPoint PickStartPoint()
    {
        return aiPoints.GetRandomFrom();
    }
}