using System.Collections.Generic;
using UnityEngine;

public class AIPoint : MonoBehaviour
{
    private float gizmosSize = .5f;
    public List<AIPoint> connectedPoints;

    public void AddNode(AIPoint point)
    {
        connectedPoints.Add(point);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(gizmosSize, gizmosSize, gizmosSize));

        Gizmos.color = Color.white;
        foreach (AIPoint point in connectedPoints)
        {
            var offset = new Vector3(0, 0.1f, 0);
            var curPos = transform.position;
            var nextPos = point.transform.position;


            Gizmos.color = Color.white;
            Gizmos.DrawLine(curPos + offset, nextPos + offset);
            var dir = curPos - nextPos;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(curPos - dir.normalized * (dir.magnitude / 2 + gizmosSize / 2) + offset,
                new Vector3(gizmosSize / 2, gizmosSize / 2, gizmosSize / 2));
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(curPos - dir.normalized * (dir.magnitude / 2) + offset,
                gizmosSize / 2);
        }
    }
}