using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
public class WaypointEditor : MonoBehaviour
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
        
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, .1f);

        Vector3 offset = new Vector3(0, 0.1f, 0);

        if (waypoint.previousWaypoint != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.previousWaypoint.transform.position + offset);
        }

        if (waypoint.nextWaypoint != null)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(waypoint.transform.position - offset, waypoint.nextWaypoint.transform.position - offset);
        }

        if (waypoint.branches != null)
        {
            foreach (Waypoint branch in waypoint.branches)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
            }
        }

    }
}
