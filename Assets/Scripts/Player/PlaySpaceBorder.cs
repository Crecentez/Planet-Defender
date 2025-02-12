using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpaceBorder : MonoBehaviour
{
    public Vector2 SafeZone = Vector2.zero;
    public Vector2 LethalBorder = Vector2.one;


    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(SafeZone.x / 2, SafeZone.y / 2, 0), new Vector3(-SafeZone.x / 2, SafeZone.y / 2, 0));
        Gizmos.DrawLine(new Vector3(SafeZone.x / 2, SafeZone.y / 2, 0), new Vector3(SafeZone.x / 2, -SafeZone.y / 2, 0));
        Gizmos.DrawLine(new Vector3(-SafeZone.x / 2, -SafeZone.y / 2, 0), new Vector3(-SafeZone.x / 2, SafeZone.y / 2, 0));
        Gizmos.DrawLine(new Vector3(-SafeZone.x / 2, -SafeZone.y / 2, 0), new Vector3(SafeZone.x / 2, -SafeZone.y / 2, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(LethalBorder.x / 2, LethalBorder.y / 2, 0), new Vector3(-LethalBorder.x / 2, LethalBorder.y / 2, 0));
        Gizmos.DrawLine(new Vector3(LethalBorder.x / 2, LethalBorder.y / 2, 0), new Vector3(LethalBorder.x / 2, -LethalBorder.y / 2, 0));
        Gizmos.DrawLine(new Vector3(-LethalBorder.x / 2, -LethalBorder.y / 2, 0), new Vector3(-LethalBorder.x / 2, LethalBorder.y / 2, 0));
        Gizmos.DrawLine(new Vector3(-LethalBorder.x / 2, -LethalBorder.y / 2, 0), new Vector3(LethalBorder.x / 2, -LethalBorder.y / 2, 0));
    }
}
