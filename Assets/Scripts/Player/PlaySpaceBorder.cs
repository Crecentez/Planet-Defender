using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpaceBorder : MonoBehaviour
{
    public Vector2 SafeZone = Vector2.zero;
    public Vector2 LethalBorder = Vector2.one;
    public GameObject OutOfBoundsWarning;

    [SerializeField]
    private bool showGizmos = false;

    [SerializeField]
    private float flashTime = 1f;

    private bool showWarning = false;

    private void Update() {
        if (outOfBounds() && !showWarning) {
            showWarning = true;
            startWarning();
        } else if (!outOfBounds() && showWarning) {
            showWarning = false;
            OutOfBoundsWarning.SetActive(false);
        }

        if (inLethalZone()) {
            GetComponent<PlayerController>().Kill();
        }
    }

    private void startWarning() {
        OutOfBoundsWarning.SetActive(true);
        Invoke("warn", flashTime);
    }

    private void warn() {
        if (showWarning) {
            OutOfBoundsWarning.SetActive(!OutOfBoundsWarning.activeInHierarchy);
            Invoke("warn", flashTime);
        }
    }

    private bool outOfBounds() {
        bool oobX = transform.position.x > (SafeZone.x / 2) || transform.position.x < -(SafeZone.x / 2);
        bool oobY = transform.position.y > (SafeZone.y / 2) || transform.position.y < -(SafeZone.y / 2);
        return oobX || oobY;
    }

    private bool inLethalZone() {
        bool oobX = transform.position.x > (LethalBorder.x / 2) || transform.position.x < -(LethalBorder.x / 2);
        bool oobY = transform.position.y > (LethalBorder.y / 2) || transform.position.y < -(LethalBorder.y / 2);
        return oobX || oobY;
    }

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
