using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletHellEnemy : MonoBehaviour
{
    #region SerializeFields
    //What projectile the enemy shoots
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private float attackRate = 1;
    [SerializeField] private int bulletSpeed = 2;    
    [SerializeField] private AudioClip fire = null;
    #endregion

    #region EnemyStats
    private int rotationSpeed = 1;
    private float lastTimeFired = 0.0f;
    private bool isPaused = false;
    private bool isStunned = false;
    private int bulletDamage = 1;
    private float timeMade = 0.0f;
    #endregion

    #region UnityComponents
    private AudioSource source = null;
    private NavMeshAgent agent = null;
    private GameObject player = null;
    [SerializeField] private GameObject gameWin = null;
    #endregion

    private void OnEnable()
    {
        timeMade = Time.time;
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        bulletDamage = GetComponent<EnemyStats>().GetDamage();        
        source = GetComponentInParent<AudioSource>();
        source.enabled = true;
        rotationSpeed = 120;
    }
    
    void Update()
    {
        if (!isPaused & !isStunned)
        {
            agent.SetDestination(player.transform.position);
            agent.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            ShootBullet();
        }
    }

    #region EnemyFunctions
    public void ShootBullet()
    {
        if (Time.time > lastTimeFired + attackRate)
        {
            if (Time.time >= timeMade + 15)
            {
                GameObject clone4 = CreateBullet();
                clone4.GetComponent<Rigidbody>().velocity = transform.right * -bulletSpeed;
                GameObject clone3 = CreateBullet();
                clone3.GetComponent<Rigidbody>().velocity = transform.right * bulletSpeed;
                GameObject clone2 = CreateBullet();
                clone2.GetComponent<Rigidbody>().velocity = transform.forward * -bulletSpeed;
            }
            else if (Time.time >= timeMade + 10)
            {
                GameObject clone3 = CreateBullet();
                clone3.GetComponent<Rigidbody>().velocity = transform.right * bulletSpeed;
                GameObject clone2 = CreateBullet();
                clone2.GetComponent<Rigidbody>().velocity = transform.forward * -bulletSpeed;
            }
            else if (Time.time >= timeMade + 5)
            {
                GameObject clone2 = CreateBullet();
                clone2.GetComponent<Rigidbody>().velocity = transform.forward * -bulletSpeed;
            }
            GameObject clone = CreateBullet();
            clone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            lastTimeFired = Time.time;
            source.PlayOneShot(fire);
        }
    }

    public void Stun()
    {
        isStunned = true;
    }

    public void Unstun()
    {
        isStunned = false;
    }

    public void OnPauseGame()
    {
        isPaused = true;
    }

    public void OnResumeGame()
    {
        isPaused = false;
    }

    GameObject CreateBullet()
    {
        GameObject clone = Instantiate(projectile, transform.position, transform.rotation);
        clone.GetComponent<CollisionScript>().bulletDamage = bulletDamage;
        clone.gameObject.layer = 12;
        clone.gameObject.SetActive(true);
        clone.GetComponent<TrailRenderer>().startColor = Color.red;
        clone.GetComponent<TrailRenderer>().endColor = Color.white;
        return clone;
    }

    public void Death()
    {
        Instantiate(gameWin);
    }

    #endregion
}