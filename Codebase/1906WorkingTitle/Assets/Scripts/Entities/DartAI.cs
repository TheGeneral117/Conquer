using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartAI : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool attackEnabled = false;
    [SerializeField] private float attackRate = 0.7f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int bulletDamage = 1;
    private bool isPaused = false;

    [SerializeField] GameObject projectile = null;
    [SerializeField] GameObject projectilePos = null;
    [SerializeField] AudioClip fire = null;
    AudioSource source = null;
    #endregion

    void Start()
    {
        source = GetComponentInParent<AudioSource>();
        source.enabled = true;
    }

    void Update()
    {
        if (!isPaused)
            if (attackEnabled)
                StartCoroutine(DartAttack());
    }

    IEnumerator DartAttack()
    {
        attackEnabled = false;
        GameObject clone = Instantiate(projectile, projectilePos.transform.position, projectile.transform.rotation);
        CollisionScript cs = clone.GetComponent<CollisionScript>();
        cs.bulletDamage = bulletDamage;
        cs.SetOwner(gameObject);
        clone.gameObject.layer = 12;
        clone.SetActive(true);
        source.PlayOneShot(fire);
        clone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        yield return new WaitForSeconds(attackRate);
        attackEnabled = true;
    }

    #region Pause/Unpause
    public void OnPauseGame()
    {
        isPaused = true;
    }
    public void OnResumeGame()
    {
        isPaused = false;
    }
    #endregion

    #region AttackEnable
    public void DisableAttack()
    {
        StopAllCoroutines();
        attackEnabled = false;
    }
    public void EnableAttack()
    {
        attackEnabled = true;
    }
    #endregion
}
