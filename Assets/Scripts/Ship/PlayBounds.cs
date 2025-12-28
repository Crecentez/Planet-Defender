using System.Collections;
using UnityEngine;
using Damage;

namespace Ship {

    public class PlayBounds : MonoBehaviour {

        #region Variables

        //[SerializeField] private Vector2 _safeZone = Vector2.zero;
        [SerializeField] private Vector2 _lethalBorder = Vector2.one;

        [SerializeField] private GameObject _outOfBoundsWarning;

        [SerializeField] private bool _showGizmos = false;
        [SerializeField] private float _flashTime = 1f;
        [SerializeField] private int _killTime = 10;

        private Coroutine _warningCoroutine;
        private Coroutine _killCoroutine;

        #endregion

        #region Unity Methods

        private void Start() {
            if (_outOfBoundsWarning == null) {
                Debug.LogWarning("OutOfBoundsWarning GameObject not Found!");
                
            }
        }

        private void Update() {
            GameObject ship = GameObject.FindGameObjectWithTag("Player");
            if (ship) {
                if (InLethalZone(ship)) {
                    StartWarning();
                    //if (InLethalZone())
                    //    _ship.Kill();   

                } else StopWarning();
            }
            

        }

        #endregion

        #region Methods

        private void StartWarning() {
            if (_outOfBoundsWarning != null) {
                if (_warningCoroutine == null) {
                    _warningCoroutine = StartCoroutine(WarningLoop());
                }
                if (_killCoroutine == null) {
                    _killCoroutine = StartCoroutine(KillTimer());
                }
            } 
                

        }

        private void StopWarning() {
            if (_warningCoroutine != null) {
                StopCoroutine(_warningCoroutine);
                _warningCoroutine = null;
                if (_outOfBoundsWarning != null) 
                    _outOfBoundsWarning.SetActive(false);
            }
            if (_killCoroutine != null) {
                StopCoroutine(_killCoroutine);
                _killCoroutine = null;
            }
        }

        private IEnumerator WarningLoop() {
            if (_outOfBoundsWarning != null) {
                _outOfBoundsWarning.SetActive(true);
                GameObject bgimage = _outOfBoundsWarning.transform.Find("BGImage")?.gameObject;
                
                while (true) {
                    if (bgimage != null) {
                        _outOfBoundsWarning.SetActive(true);
                        bgimage.SetActive(!bgimage.activeInHierarchy);
                    } else {
                        _outOfBoundsWarning.SetActive(!_outOfBoundsWarning.activeInHierarchy);
                    }
                    yield return new WaitForSeconds(_flashTime);
                }
            }

            
        }

        private IEnumerator KillTimer() {
            yield return new WaitForSeconds(_killTime);
            GameObject ship = GameObject.FindGameObjectWithTag("Player");
            if (ship && InLethalZone(ship)) {
                Damageable damageable = ship.GetComponent<Damageable>();
                damageable.Kill();
            }
        }

        //private bool OutOfBounds(GameObject gm) {
        //    bool oobX = gm.transform.position.x > (_safeZone.x / 2) || gm.transform.position.x < -(_safeZone.x / 2);
        //    bool oobY = gm.transform.position.y > (_safeZone.y / 2) || gm.transform.position.y < -(_safeZone.y / 2);
        //    return oobX || oobY;
        //}

        private bool InLethalZone(GameObject gm) {
            bool oobX = gm.transform.position.x > (_lethalBorder.x / 2) || gm.transform.position.x < -(_lethalBorder.x / 2);
            bool oobY = gm.transform.position.y > (_lethalBorder.y / 2) || gm.transform.position.y < -(_lethalBorder.y / 2);
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
            if (_showGizmos) {

                // Safe Zone
            //    Vector3[] safeZonePoints = new Vector3[4] {
            //    new Vector3(_safeZone.x / 2, _safeZone.y / 2, 0),
            //    new Vector3(-_safeZone.x / 2, _safeZone.y / 2, 0),
            //    new Vector3(-_safeZone.x / 2, -_safeZone.y / 2, 0),
            //    new Vector3(_safeZone.x / 2, -_safeZone.y / 2, 0)
            //};
            //    Gizmos.color = Color.yellow;
            //    GizmosDrawRectangle(safeZonePoints);

                // Lethal Zone
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

}