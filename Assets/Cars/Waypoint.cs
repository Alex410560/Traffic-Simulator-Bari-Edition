using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    public List<Waypoint> branches;     // Crea array dinamico

    [Range(0f, 1f)]
    public float branchRatio = 0.5f;

    public float maxSpeed = 50f;

    [SerializeField] TrafficCongestion trafficCongestion;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    void Update()
    {
        if (trafficCongestion != null)
        {
            if (trafficCongestion.GetStatus() == true)
                branchRatio = 0f;
            else
                branchRatio = 0.5f;
        }
    }
}
