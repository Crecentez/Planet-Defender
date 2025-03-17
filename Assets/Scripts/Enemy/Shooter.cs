using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shooter : Enemy
{
    //protected enum StateEnum {
    //    Attacking,
    //    Chasing,
    //    Fleeing,
    //    Orbiting
    //}

    [SerializeField]
    protected float MaxSpeed = 1f;
    [SerializeField]
    protected float Acceleration = 1f;

    [SerializeField]
    protected float RotSpeed = 150f;

    [SerializeField]
    protected float SightRadius = 20f;
    [SerializeField]
    protected float AttackRadius = 10f;
    [SerializeField]
    protected float FleeRaduius = 5f;

    [SerializeField]
    protected float FireRate = 0.5f;
    [SerializeField]
    protected GameObject BulletPrefab;

    //[SerializeField]
    //private StateEnum State = StateEnum.Chasing;

    private Rigidbody2D rb;
    private bool CanShoot = true;

    private void Start() {
        Health = MaxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        UpdateRotation();
        UpdateMovement();
    }

    private void UpdateRotation() {
        gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z + RotSpeed * Time.deltaTime);
    }

    private void UpdateMovement() {

        GameObject player = GameObject.FindWithTag("Player");

        // If cant find Player then move towards planet
        if (player == null) {

            MoveToPlanet();

        } else {

            float pDistance = (player.transform.position - transform.position).magnitude;

            // can see player
            if (pDistance > SightRadius) {

                MoveToPlanet();

                // withen sight radius but not attack radius
            } else {

                if (pDistance <= AttackRadius) {

                    if (pDistance <= FleeRaduius) {

                        Vector3 movDir = (transform.position - player.transform.position).normalized;
                        Vector2 vel = new Vector2(movDir.x, movDir.y) * Acceleration;

                        if (rb.velocity.magnitude < MaxSpeed * 1.5) {
                            rb.AddForce(vel);
                        }

                    } else {

                        Shoot(player.transform);

                    }

                } else {
                    Vector3 movDir = (transform.position - player.transform.position).normalized;
                    Vector2 vel = new Vector2(movDir.x, movDir.y) * Acceleration * -1;

                    if (rb.velocity.magnitude < MaxSpeed) {
                        rb.AddForce(vel);
                    }
                }
            }

        }
    }

    //private Vector3 GetRandomTargetPos(float minRadius, float maxRadius) {
    //    GameObject player = GameObject.FindWithTag("Player");
    //    Vector2 rndPos = Random.insideUnitCircle * (maxRadius - minRadius);
    //    rndPos += rndPos.normalized * minRadius;
    //    return new Vector3(player.transform.position.x + rndPos.x, player.transform.position.y, player.transform.position.z + rndPos.y);
    //}

    private void Shoot(Transform Target) {
        if (CanShoot) {

            CanShoot = false;

            GameObject b = Instantiate(BulletPrefab);
            b.transform.position = transform.position;
            Vector3 targ = Target.position;
            targ.z = 0f;

            Vector3 objectPos = b.transform.position;
            targ.x = targ.x - objectPos.x;
            targ.y = targ.y - objectPos.y;

            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            b.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

            Enemy_Projectile ep = b.GetComponent<Enemy_Projectile>();
            if (ep != null) {
                ep.enemy = this;
            }

            Invoke("ResetShot", FireRate);

        }

    }

    private void ResetShot() {
        CanShoot = true;
    }

    private void MoveToPlanet() {

        GameObject planet = GameObject.FindWithTag("Planet");

        if (planet != null) {

            float pDistance = (planet.transform.position - transform.position).magnitude;

            if (pDistance <= AttackRadius) {

                Shoot(planet.transform);

            } else {

                Vector3 movDir = (transform.position - planet.transform.position).normalized;
                Vector2 vel = new Vector2(movDir.x, movDir.y) * Acceleration * -1;

                if (rb.velocity.magnitude < MaxSpeed) {
                    rb.AddForce(vel);
                }

            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, FleeRaduius);
    }

}
