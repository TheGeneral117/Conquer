using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    #region Variable
    #region Stats
    //How many points the enemy is worth to the spawner, and how much experience it grants on kill.
    [SerializeField] private int enemyPoints = 1;

    //How much damage it takes to kill the enemy.
    [SerializeField] private float health = 2;

    //How much damage the enemy deals on hit.
    [SerializeField] private int damage = 2;

    //Amount of seconds between attacks.
    [SerializeField] private float attackRate = 1;

    //Speed at which this enemies bullets travel
    [SerializeField] private float bulletSpeed = 10;

    //Condition immunities
    public bool isFireImmune = false;
    public bool isIceImmune = false;
    public bool isStunImmune = false;

    //Has this enemy been spawned externally? (through splitter enemy)
    private bool isChild = false;
    #endregion

    #region UnityComponents

    //Children enemies that this enemy spawns on death
    [SerializeField] private GameObject childEnemy = null;

    //Need this to notify the spawner to add new enemies on split.
    [SerializeField] private GameObject spawnerObject = null;

    //How many children this enemy spawns on death
    public int children = 0;
    
    //Please don't set colors to null. They're non-nullable types that generate errors if set to null.
    public Color enemyColor;

    //Enemy's color and renderer
    private Renderer enemyRender = null;
    private Animator anim = null;
    private GameObject player = null;
    private Player playerScript = null;
    private SpawnScript spawnerScript = null;
    #endregion
    #endregion

    public void Start()
    {
        GetComponent<AudioSource>().enabled = true;
        enemyRender = GetComponentInChildren<Renderer>();
        enemyColor = enemyRender.material.color;
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
            playerScript = player.GetComponentInParent<Player>();
        if(GetComponent<Animator>() != null)
            anim = GetComponent<Animator>();
        if (spawnerObject != null)
            spawnerScript = spawnerObject.GetComponent<SpawnScript>();
    }

    public void Update()
    {
        if (enemyRender.material.color != enemyColor)
            enemyRender.material.color = Color.Lerp(enemyRender.material.color, enemyColor, 0.1f);
    }

    #region Getters and Setters
    public int GetPoints()
    {
        return enemyPoints;
    }

    public float GetHealth()
    {
        return health;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetAttackRate()
    {
        return attackRate;
    }

    public float GetMovementSpeed()
    {
        return GetComponent<NavMeshAgent>().speed;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    public GameObject GetSpawner()
    {
        return spawnerObject;
    }

    public void SetHealth(float _health)
    {
        health = _health;
    }

    public void SetDamage(int _damage)
    {
        damage = _damage;
    }

    public void SetAttackRate(float _attackRate)
    {
        attackRate = _attackRate;
    }

    public void SetMovementSpeed(float _speed)
    {
        GetComponent<NavMeshAgent>().speed = _speed;
    }

    public void SetBulletSpeed(float _speed)
    {
        bulletSpeed = _speed;
    }

    public void SetSpawner(GameObject _spawner)
    {
        spawnerObject = _spawner;
    }
    #endregion

    #region EnemyFunctions
    //Our enemy is damaged
    public void TakeDamage(Color _color, float _damage = 1)
    {
        if (_damage > 0f)
        {
            BlinkOnHit(_color);
            health -= _damage;
            if (health <= 0f)
            {
                Kill();
            }
        }
    }

    //Kill function
    public void Kill()
    {
        if(GetComponent<DropChance>())
            GetComponent<DropChance>().Drop();
        if (CompareTag("BulletHell Enemy"))
            GetComponent<BulletHellEnemy>().Death();
        if (childEnemy != null && children > 0)
            Split(children);
        if (isChild)
            spawnerScript.SubtractRemainingChild();
        if (playerScript != null)
            playerScript.GainExperience(enemyPoints);
        Destroy(gameObject);
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

    //Generate children enemies.
    public void Split(int children)
    {
        for (int i = 0; i < children; i++)
        {
            GameObject child = Instantiate(childEnemy, transform.position, Quaternion.identity);
            EnemyStats childStats = child.GetComponent<EnemyStats>();
            childStats.isChild = true;

            if (spawnerObject != null)
            {
                childStats.SetSpawner(spawnerObject);
                spawnerScript.AddEnemy(child);
                spawnerScript.AddRemainingChild();
            }
        }
    }

    //Color feedback on damage taken
    public void BlinkOnHit(Color _color)
    {
        if (anim != null)
        {
            for (int i = 0; i < anim.parameterCount; i++)
            {
                 if(anim.GetParameter(i).name == "On Hit")
                    anim.SetTrigger("On Hit");
            }
        }
        enemyRender.material.color = _color;
    }

    public Renderer GetRenderer()
    {
        return enemyRender;
    }
    #endregion
}