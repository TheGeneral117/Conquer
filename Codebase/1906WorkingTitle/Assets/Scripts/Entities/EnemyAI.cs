using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    #region Variables
    #region EnemyStats
    [SerializeField] private float attackRate = 0.0f;
    [SerializeField] private float bulletSpeed = 0.0f;
    [SerializeField] private int bulletDamage = 0;
    private bool isStunned = false;
    private bool isPaused, inLove = false;
    private bool attackEnabled = true;
    #endregion

    #region UnityComponents
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private GameObject projectilePos = null;
    [SerializeField] private GameObject loveParticle = null;
    [SerializeField] private AudioClip fire = null;

    private EnemyStats enemyStats = null;
    private SpawnScript spawnScript = null;
    private Animator anim = null;
    private NavMeshAgent agent = null;
    private GameObject player = null;
    private GameObject target = null;
    private AudioSource source = null;
    #endregion
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyStats = GetComponent<EnemyStats>();
        if(enemyStats.GetSpawner() != null)
            spawnScript = enemyStats.GetSpawner().GetComponent<SpawnScript>();
        attackRate = enemyStats.GetAttackRate();
        bulletSpeed = enemyStats.GetBulletSpeed();
        bulletDamage = enemyStats.GetDamage();
        source = GetComponentInParent<AudioSource>();
        source.enabled = true;
        target = player;
    }
    
    void Update()
    {
        if (!isPaused && !isStunned)
        {
            //If charmed and there are enemies left to fight, SetTarget to new enemy
            if (inLove && spawnScript.GetNumEnemies() > 1 && target == null && player != null)
                SetTarget();

            //If there is a target, move towards their position.
            if(target != null && target.transform.position != null && player != null)
                agent.SetDestination(target.transform.position);

            //If is within stopping distance of the target, just rotate towards them.
            if (target != null && (agent.remainingDistance < agent.stoppingDistance || agent.speed <= 0))
            {
                Vector3 targetPosition = target.transform.position;
                Vector3 relativePosition = targetPosition - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePosition);
                // Lock the rotation around X and Z Axes
                rotation.x = 0.0f;
                rotation.z = 0.0f;
                // Change the enemy's tranform's rotation to the rotation Quaternion
                agent.transform.rotation = rotation;
            }

            if (attackEnabled)
                StartCoroutine(EnemyAttack());
        }
    }

    #region EnemyFunctions
    //Call this function after you change either attackRate or bulletSpeed in EnemyStats while the enemy is active.
    public void RefreshStats()
    {
        EnemyStats enemyStats = GetComponent<EnemyStats>();
        attackRate = enemyStats.GetAttackRate();
        bulletSpeed = enemyStats.GetBulletSpeed();
    }

    public void OnPauseGame()
    {
        isPaused = true;
    }

    public void OnResumeGame()
    {
        isPaused = false;
    }

    //Set enemy to stunned
    public void Stun()
    {
        isStunned = true;
    }

    //Set enemy to not stunned
    public void Unstun()
    {
        isStunned = false;
    }

    //Manages charmed condition
    public IEnumerator FallInLove(float time)
    {
        loveParticle.SetActive(true);
        SetTarget();
        inLove = true;
        yield return new WaitForSeconds(5f);
        inLove = false;
        loveParticle.SetActive(false);
        target = player;
    }

    //Finds a enemy for this charmed enemy to attack.
    private void SetTarget()
    {
        List<GameObject> gameObjectList = new List<GameObject>();
        //Switching array to a list for the purpose of accessing list functions
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in gameObjects)
            gameObjectList.Add(go);
        //Using this check to prevent errors from using a love bullet when only one enemy on screen.
        if (gameObjectList.Count > 1)
        {
            GameObject closest = null;
            float distance = float.MaxValue;
            Vector3 position = transform.position;
            foreach (GameObject gameobject in gameObjects)
            {
                if (gameobject != gameObject)
                {
                    float curDistance = Vector3.Distance(position, gameobject.transform.position);
                    if (curDistance < distance)
                    {
                        closest = gameobject;
                        distance = curDistance;
                    }
                }
            }
            target = closest;
        }
    }

    public bool InLove()
    {
        return inLove;
    }

    IEnumerator EnemyAttack()
    {
        attackEnabled = false;
        GameObject clone = Instantiate(projectile, projectilePos.transform.position, projectile.transform.rotation);
        CollisionScript cs = clone.GetComponent<CollisionScript>();
        cs.bulletDamage = bulletDamage;
        cs.SetOwner(gameObject);

        //Need these layer sets so that enemies don't shoot each other outside of love condition.
        if (inLove)
            clone.layer = 10;
        else
            clone.layer = 12;
        clone.SetActive(true);
        source.PlayOneShot(fire);
        clone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        if (anim != null && !isStunned)
        {
            for (int i = 0; i < anim.parameterCount; i++)
            {
                if (anim.GetParameter(i).name == "Attack")
                    anim.SetTrigger("Attack");
            }
        }
        yield return new WaitForSeconds(attackRate);
        attackEnabled = true;
    }
    #endregion
}