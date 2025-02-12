using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public float dampTime = 0.5f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Vector2 maxView = Vector2.one * 20;

    [SerializeField]
    private Camera cam;

    private void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        if (target) {
            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

            float maxViewX = (maxView.x / 2);
            float maxViewY = (maxView.y / 2);
            if (transform.position.x > maxViewX) {
                transform.position = new Vector3(maxViewX, transform.position.y, transform.position.z);
            } else if (transform.position.x < -maxViewX) {
                transform.position = new Vector3(-maxViewX, transform.position.y, transform.position.z);
            }
            if (transform.position.y > maxViewY) {
                transform.position = new Vector3(transform.position.x, maxViewY, transform.position.z);
            } else if (transform.position.y < -maxViewY) {
                transform.position = new Vector3(transform.position.x, -maxViewY, transform.position.z);
            }
        }

    }

    private void gizmosDrawRectangle(Vector3[] points) {
        for (int i = 0; i < points.Length; i++) {
            if (i + 1 < points.Length) {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
        Gizmos.DrawLine(points[points.Length - 1], points[0]);
    }

    private void OnDrawGizmosSelected() {
        if (cam != null) {
            Vector2 camSize = new Vector2((cam.orthographicSize * 2) * cam.aspect, cam.orthographicSize * 2);
            //Gizmos.DrawWireSphere(new Vector3(cam.orthographicSize))
            Vector2 distance = new Vector2(camSize.x + (maxView.x / 2), camSize.y + (maxView.y / 2));
            Vector3[] points = new Vector3[] {
                new Vector3(distance.x, distance.y, 0),
                new Vector3(distance.x, -distance.y, 0),
                new Vector3(-distance.x, -distance.y, 0),
                new Vector3(-distance.x, distance.y, 0)
            };
            Gizmos.color = Color.blue;
            gizmosDrawRectangle(points);
        }
        
    }
}
