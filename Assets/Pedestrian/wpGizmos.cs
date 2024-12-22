using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wpGizmos : MonoBehaviour
{
    [Header("Gizmos Settings")]
    [Tooltip("Colore della sfera dei waypoint")]
    public Color sphereColor = Color.green;

    [Tooltip("Raggio delle sfere dei waypoint")]
    [Range(0.1f, 2f)]
    public float sphereRadius = 0.5f;

    private void OnDrawGizmos()
    {
        // Ottieni tutti i figli come waypoints
        Transform[] childWaypoints = GetComponentsInChildren<Transform>();

        if (childWaypoints == null || childWaypoints.Length <= 1)
            return;

        Gizmos.color = sphereColor;

        for (int i = 1; i < childWaypoints.Length; i++) // Salta il primo elemento che è il genitore
        {
            if (childWaypoints[i] != null)
            {
                // Disegna la sfera del waypoint
                Gizmos.DrawSphere(childWaypoints[i].position, sphereRadius);

                // Disegna la linea verso il prossimo waypoint, se esiste
                if (i < childWaypoints.Length - 1 && childWaypoints[i + 1] != null)
                {                    
                    Gizmos.color = sphereColor; // Ritorna al colore della sfera
                }
            }
        }
    }
}
