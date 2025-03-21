using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum CanDamageTypes {
    Enemy,
    Player,
    Planet,
    Turret
}

public class Projectile : MonoBehaviour
{



    #region Variables

    [EnumFlags] public CanDamageTypes canDamageTypes = CanDamageTypes.Enemy;

    private const float LIFE_TIME = 8f;

    //private Gun gun;
    //private Planet planet;

    #endregion

    #region Unity Methods

    private void Start() {
        StartLifeTimer();
        //GameObject planet_gm = GameObject.FindGameObjectWithTag("Planet");
        //if (planet_gm != null) {
        //    planet = planet_gm.GetComponent<Planet>();
        //} else {
        //    Destroy(gameObject);
        //}
    }

    #endregion

    #region Methods

    protected void StartLifeTimer() {
        StartCoroutine(LifeTimeTimer(LIFE_TIME));
    }
    protected void StartLifeTimer(float lifeTime) {
        StartCoroutine(LifeTimeTimer(lifeTime));
    }

    private IEnumerator LifeTimeTimer(float lifeTime) {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    protected void ApplyKnockback(GameObject gm, Vector2 Knockback) {
        Rigidbody2D other_rb = gm.GetComponent<Rigidbody2D>();
        if (other_rb) { other_rb.AddForce(Knockback); }
    }

    protected bool CanDamage(CanDamageTypes type) {
        List<string> canDamage = EnumFlagsAttribute.GetSelectedStrings(canDamageTypes);
        for (int i = 0; i < canDamage.Count; i++) {
            if (canDamage[i].Equals(type.ToString())) { return true; }
        }
        return false;
    }

    protected void Damage(PlayerController pc, int damage) {
        GameObject spawner_gm = GameObject.FindGameObjectWithTag("Spawner");
        if (spawner_gm != null) {
            WaveHandler spawner = spawner_gm.GetComponent<WaveHandler>();
            if (spawner != null) {
                WaveHandler.EnemyScaling scale = spawner.GetEnemyScaling();
                if (scale != null) {
                    pc.Damage(Mathf.FloorToInt(damage * scale.playerDamageScale * spawner.GetWave()));
                } else {
                    pc.Damage(damage);
                }
                return;
            }
        }
    }

    protected void Damage(Planet planet, int damage) {
        GameObject spawner_gm = GameObject.FindGameObjectWithTag("Spawner");
        if (spawner_gm != null) {
            WaveHandler spawner = spawner_gm.GetComponent<WaveHandler>();
            if (spawner != null) {
                WaveHandler.EnemyScaling scale = spawner.GetEnemyScaling();
                if (scale != null) {
                    planet.Damage(Mathf.FloorToInt(damage * scale.planetDamageScale * spawner.GetWave()));
                } else {
                    planet.Damage(damage);
                }
                return;
            }
        }
    }
    protected void Damage(Enemy enemy, int damage) {

        enemy.Damage(damage);

        //GameObject planet_gm = GameObject.FindGameObjectWithTag("Planet");
        //if (planet_gm != null) {
        //    Planet planet = planet_gm.GetComponent<Planet>();
        //    if (planet != null) {

                

        //        return;
        //    }
        //}
        //Debug.LogWarning("Planet not found!");
        //Destroy(gameObject);
    }

    #endregion
}
