using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollisionScript : MonoBehaviour
{
    #region CollsionScriptProperties
    private bool isIceImmune = false;
    private bool isFireImmune = false;
    private bool isStunImmune = false;
    private Color hitColor = Color.red;
    public float bulletDamage = 0.0f;
    #endregion

    #region Unity Components
    [SerializeField] private GameObject sparks = null;
    [SerializeField] private GameObject blood = null;
    [SerializeField] private GameObject fireCreep = null;
    [SerializeField] private GameObject iceCreep = null;
    [SerializeField] private GameObject owner = null;

    private Player player = null;
    private EnemyStats enemy = null;
    private EnemyAI ai = null;
    private NavMeshAgent nav = null;
    private AudioSource audioSource = null;
    private AudioClip hurt = null;
    #endregion

    #region CollisionScriptFunctions
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.collider.gameObject;
        if (target != owner)
        {
            #region Audio
            audioSource = target.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = 1.0f;
                if (hurt != null)
                    audioSource.PlayOneShot(hurt);
                audioSource.volume = 0.5f;
            }
            #endregion

            #region Get stats
            if (target.CompareTag("Player") || target.CompareTag("Enemy") || target.CompareTag("BulletHell Enemy"))
            {
                if (target.CompareTag("Player"))
                {
                    player = target.GetComponent<Player>();
                    isFireImmune = player.isFireImmune;
                    isIceImmune = player.isIceImmune;
                    isStunImmune = player.isStunImmune;
                }
                else if (target.CompareTag("Enemy") || target.CompareTag("BulletHell Enemy"))
                {
                    if (!target.CompareTag("BulletHell Enemy"))
                        ai = target.GetComponent<EnemyAI>();
                    enemy = target.GetComponent<EnemyStats>();
                    isFireImmune = enemy.isFireImmune;
                    isIceImmune = enemy.isIceImmune;
                    isStunImmune = enemy.isStunImmune;
                }
            }
            else
            {
                Instantiate(sparks, transform.position, sparks.transform.rotation);
                {
                    if (gameObject.CompareTag("FirePot"))
                        Instantiate(fireCreep, transform.position, fireCreep.transform.rotation);
                    if (gameObject.CompareTag("IcePot"))
                        Instantiate(iceCreep, transform.position, iceCreep.transform.rotation);
                }
            }
            #endregion
            if (!(isIceImmune && isFireImmune && isStunImmune))
            {
                ConditionManager con = target.GetComponent<ConditionManager>();
                if (con != null)
                {
                    //Apply extra on-hit effects here:
                    switch (gameObject.tag)
                    {
                        case "Fire Bullet":
                            {
                                if (!isFireImmune)
                                {
                                    SetHitColor(new Color(0.921f, 0.505f, 0f));
                                    DamageCheck(hitColor);
                                    //Burn sound effect
                                    //audioSource.PlayOneShot(burn);
                                    con.TimerAdd("fire", 179);
                                }
                                break;
                            }
                        case "Ice Bullet":
                            {
                                if (!isIceImmune)
                                {
                                    DamageCheck(new Color(0.360f, 0.952f, 0.960f));
                                    con.SubtractSpeed(0.6f);
                                    con.TimerAdd("thaw", 90);
                                }
                                break;
                            }
                        case "Stun Bullet":
                            {
                                if (!isStunImmune)
                                {
                                    DamageCheck(Color.yellow
);
                                    if (target.CompareTag("BulletHell Enemy"))
                                    {
                                        BulletHellEnemy bulletHellAI = enemy.GetComponent<BulletHellEnemy>();
                                        bulletHellAI.Stun();
                                        nav.enabled = false;
                                    }
                                    else if (!target.CompareTag("Player"))
                                    {
                                        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                                        if (enemyAI != null)
                                            enemyAI.Stun();
                                        else
                                        {
                                            BulletHellEnemy bulletHellAI = enemy.GetComponent<BulletHellEnemy>();
                                            bulletHellAI.Stun();
                                        }
                                    }
                                    else
                                        player.Stun();
                                    con.TimerAdd("stun", 16);
                                }
                                break;
                            }
                        case "Love Bullet":
                            {
                                if (ai != null)
                                {
                                    if (target.CompareTag("Enemy") && ai != null)
                                    {
                                        con.TimerAdd("love", 5);
                                        DamageCheck(hitColor);
                                    }
                                }
                                break;
                            }
                        case "FirePot":
                            {
                                Instantiate(sparks, transform.position, sparks.transform.rotation);
                                Instantiate(fireCreep, transform.position, fireCreep.transform.rotation);
                                break;
                            }
                        case "IcePot":
                            {
                                Instantiate(sparks, transform.position, sparks.transform.rotation);
                                Instantiate(iceCreep, transform.position, iceCreep.transform.rotation);
                                break;
                            }
                        case "Hex":
                            {
                                DamageCheck(hitColor);
                                if (!isFireImmune)
                                {
                                    //Burn sound effect
                                    //audioSource.PlayOneShot(burn);
                                    con.TimerAdd("fire", 179);
                                }
                                if (!isIceImmune)
                                {
                                    con.SubtractSpeed(0.6f);
                                    con.TimerAdd("thaw", 90);
                                }
                                if (!isStunImmune)
                                {
                                    if (target.CompareTag("BulletHell Enemy"))
                                    {
                                        BulletHellEnemy bulletHellAI = enemy.GetComponent<BulletHellEnemy>();
                                        bulletHellAI.Stun();
                                    }
                                    else if (!target.CompareTag("Player"))
                                    {
                                        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                                        enemyAI.Stun();
                                    }
                                    else
                                        player.Stun();
                                    con.TimerAdd("stun", 16);
                                }
                                break;
                            }

                        default:
                            {
                                DamageCheck(Color.red);
                                break;
                            }
                    }
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void DamageCheck(Color _color)
    {
        if (player != null)
        {
            player.TakeDamage(_color, bulletDamage);
            Instantiate(blood, transform.position, blood.transform.rotation);
        }
        if (enemy != null)
        {
            enemy.TakeDamage(_color, bulletDamage);
            Instantiate(blood, transform.position, blood.transform.rotation);
        }
    }

    public GameObject GetOwner()
    {
        return owner;
    }

    public void SetOwner(GameObject _owner)
    {
        owner = _owner;
    }

    public void SetHitColor(Color _color)
    {
        hitColor = _color;
    }
    #endregion
}