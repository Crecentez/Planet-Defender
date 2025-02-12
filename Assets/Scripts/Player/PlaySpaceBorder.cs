using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpaceBorder : MonoBehaviour
{
    public Vector2 SafeZone = Vector2.zero;
    public Vector2 LethalBorder = Vector2.one;

    [SerializeField]
    private bool showGizmos = false;

    private void gizmosDrawRectangle(Vector3[] points) {
        for (int i = 0; i < points.Length; i++) {
            if (i+1 < points.Length) {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
        Gizmos.DrawLine(points[points.Length - 1], points[0]);
    }

    private void OnDrawGizmos() {
        if (showGizmos) {

            Vector3[] safeZonePoints = new Vector3[4] {
                new Vector3(SafeZone.x / 2, SafeZone.y / 2, 0),
                new Vector3(-SafeZone.x / 2, SafeZone.y / 2, 0),
                new Vector3(-SafeZone.x / 2, -SafeZone.y / 2, 0),
                new Vector3(SafeZone.x / 2, -SafeZone.y / 2, 0)
            };
            
            Gizmos.color = Color.white;
            gizmosDrawRectangle(safeZonePoints);

            Vector3[] lethalZonePoints = new Vector3[4] {
                new Vector3(LethalBorder.x / 2, LethalBorder.y / 2, 0),
                new Vector3(-LethalBorder.x / 2, LethalBorder.y / 2, 0),
                new Vector3(-LethalBorder.x / 2, -LethalBorder.y / 2, 0),
                new Vector3(LethalBorder.x / 2, -LethalBorder.y / 2, 0)
            };

            Gizmos.color = Color.red;
            gizmosDrawRectangle(lethalZonePoints);
        }
    }
}
