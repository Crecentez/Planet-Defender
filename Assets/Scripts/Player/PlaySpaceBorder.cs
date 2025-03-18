using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpaceBorder : MonoBehaviour
{

    #region Variables
    [SerializeField] private Vector2 _safeZone = Vector2.zero;
    [SerializeField] private Vector2 _lethalBorder = Vector2.one;

    [SerializeField] private GameObject _outOfBoundsWarning;
    [SerializeField] private PlayerController _controller;


    [SerializeField] private bool showGizmos = false;
    [SerializeField] private float flashTime = 1f;

    private Coroutine _warningCoroutine;

    #endregion

    #region Unity Methods

    private void Update() {
        if (OutOfBounds()) 
            StartWarning();
        else 
            StopWarning();
        

        if (InLethalZone()) {
            _controller.Kill();
        }
    }

    #endregion

    #region Methods

    private void StartWarning() {
        if (_warningCoroutine == null) {
            StartCoroutine(WarningLoop());
        }
    }

    private void StopWarning() {
        if (_warningCoroutine != null) {
            StopCoroutine(_warningCoroutine);
            _warningCoroutine = null;
            _outOfBoundsWarning.SetActive(false);
        }
    }

    private IEnumerator WarningLoop() {
        _outOfBoundsWarning.SetActive(true);
        while (true) {
            _outOfBoundsWarning.SetActive(!_outOfBoundsWarning.activeInHierarchy);
            yield return new WaitForSeconds(flashTime);
        }
    }

    private void warn() {
        
    }

    private bool OutOfBounds() {
        bool oobX = transform.position.x > (_safeZone.x / 2) || transform.position.x < -(_safeZone.x / 2);
        bool oobY = transform.position.y > (_safeZone.y / 2) || transform.position.y < -(_safeZone.y / 2);
        return oobX || oobY;
    }

    private bool InLethalZone() {
        bool oobX = transform.position.x > (_lethalBorder.x / 2) || transform.position.x < -(_lethalBorder.x / 2);
        bool oobY = transform.position.y > (_lethalBorder.y / 2) || transform.position.y < -(_lethalBorder.y / 2);
        return oobX || oobY;
    }

    #endregion

    #region Gizmos
    private void GizmosDrawRectangle(Vector3[] points) {
        for (int i = 0; i < points.Length; i++) {
            if (i + 1 < points.Length) {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
        Gizmos.DrawLine(points[points.Length - 1], points[0]);
    }

    private void OnDrawGizmos() {
        if (showGizmos) {

            Vector3[] safeZonePoints = new Vector3[4] {
                new Vector3(_safeZone.x / 2, _safeZone.y / 2, 0),
                new Vector3(-_safeZone.x / 2, _safeZone.y / 2, 0),
                new Vector3(-_safeZone.x / 2, -_safeZone.y / 2, 0),
                new Vector3(_safeZone.x / 2, -_safeZone.y / 2, 0)
            };
            
            Gizmos.color = Color.yellow;
            GizmosDrawRectangle(safeZonePoints);

            Vector3[] lethalZonePoints = new Vector3[4] {
                new Vector3(_lethalBorder.x / 2, _lethalBorder.y / 2, 0),
                new Vector3(-_lethalBorder.x / 2, _lethalBorder.y / 2, 0),
                new Vector3(-_lethalBorder.x / 2, -_lethalBorder.y / 2, 0),
                new Vector3(_lethalBorder.x / 2, -_lethalBorder.y / 2, 0)
            };

            Gizmos.color = Color.red;
            GizmosDrawRectangle(lethalZonePoints);
        }
    }

    #endregion
}
