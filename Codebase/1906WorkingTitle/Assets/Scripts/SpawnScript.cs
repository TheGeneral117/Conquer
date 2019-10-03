using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    #region SpawnerStats
    #region SerializedFields
    //Whether the spawner can spawn or not.
    [SerializeField] private bool spawnEnabled = true;
    [SerializeField] private bool multiSpawnpoint = false;
    [SerializeField] List<GameObject> spawnpoints = new List<GameObject>();
    [SerializeField] private bool isAreaCleared = false;
    public CameraTrigger room = null;
    public bool clearCheck = false;

    //List of different enemies the spawner can choose to spawn.
    [SerializeField] private List<GameObject> enemies = null;

    //list of doors.
    [SerializeField] private List<GameObject> doors = null;

    //How many points worth of enemies the spawner can spawn
    [SerializeField] private int points = 0;
    #endregion

    #region Other
    private List<EnemyStats> enemiesClone;
    private int pointsClone;
    private bool spawnAgain = true;

    //Number of seconds between spawns.
    public float timer = 3;

    //Tracks number of externally spawned enemies
    public int remainingChildren;

    //List of enemies spawned by the spawner.
    public List<GameObject> spawnedEnemies;
    #endregion
    #endregion

    void Start()
    {
        room = GetComponentInParent<CameraTrigger>();
        pointsClone = points;
        enemiesClone = new List<EnemyStats>();
        for (int i = 0; i < enemies.Count; i++)
            enemiesClone.Add(enemies[i].GetComponent<EnemyStats>());
    }

    void Update()
    {
            RefreshSpawnedEnemies();
            if (!isAreaCleared && spawnEnabled && spawnAgain)
                StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        spawnAgain = false;
        if (clearCheck && !isAreaCleared && remainingChildren <= 0 && spawnedEnemies.Count == 0 && pointsClone < 1)
        {
            clearCheck = false;
            room.ClearRoom(true);
            isAreaCleared = true;
            SetDoorLock(false);
        }

        if (!isAreaCleared)
        {
            if (spawnEnabled && pointsClone > 0)
            {
                for (int i = enemiesClone.Count - 1; i >= 0; i--)
                    if (enemiesClone[i].GetPoints() > pointsClone)
                        enemiesClone.Remove(enemiesClone[i]);

                int randomNum = Random.Range(0, enemiesClone.Count);
                GameObject enemyClone = null;

                if (!multiSpawnpoint)
                {
                    //Spawns an enemy at the spawner's position
                    enemyClone = Instantiate(enemiesClone[randomNum].gameObject, transform.position, Quaternion.identity);
                }
                else
                {
                    //choose a random spawnpoint
                    int randomNum2 = Random.Range(0, spawnpoints.Count);
                    Vector3 spawnpoint = spawnpoints[randomNum2].transform.position;
                    //Spawns an enemy at the chosen spawnpoint
                    enemyClone = Instantiate(enemiesClone[randomNum].gameObject, spawnpoint, Quaternion.identity);
                }
                EnemyStats enemyCloneStats = enemyClone.GetComponent<EnemyStats>();
                enemyCloneStats.SetSpawner(gameObject);

                //Adds the enemy to spawned enemies list
                spawnedEnemies.Add(enemyClone);

                //subtracts enemy points from spawner's
                pointsClone -= enemyCloneStats.GetPoints();

                //Adds children to remainingchildren counter
            //remainingChildren += enemyCloneStats.children;
            }
        }
        yield return new WaitForSeconds(timer);
        spawnAgain = true;
    }

    #region SpawnerFunctions
    public bool GetEnabled()
    {
        return spawnEnabled;
    }
    public void SetEnabled(bool _enable)
    {
        spawnEnabled = _enable;
    }

    //Function that takes in a bool and sets the doors to be active/inactive if the bool is true/false
    public void SetDoorLock(bool _lock)
    {
        for (int i = 0; i < doors.Count; i++)
            doors[i].SetActive(_lock);
        if (!_lock)
        {
            CameraTrigger ct = GetComponentInParent<CameraTrigger>();
            foreach (Transform child in transform.parent)
                if (child.gameObject.GetComponent<DartAI>() != null)
                    child.gameObject.GetComponent<DartAI>().DisableAttack();
        }
        else if (_lock)
        {
            foreach (Transform childs in transform.parent)
                if (childs.gameObject.GetComponent<DartAI>() != null)
                    childs.gameObject.GetComponent<DartAI>().EnableAttack();
        }
    }

    //Resets the spawner
    public void ResetSpawner()
    {
        if (!isAreaCleared)
        {
            pointsClone = points;
            if (enemies.Count > 0)
                for (int i = 0; i < enemies.Count; i++)
                    enemiesClone.Add(enemies[i].GetComponent<EnemyStats>());
        }
    }

    //For use with splitting enemies.
    public void AddEnemy(GameObject _enemy)
    {
        spawnedEnemies.Add(_enemy);
    }

    public void DespawnEnemies()
    {
        foreach(EnemyStats e in enemiesClone)
        {
            e.Despawn();
        }
    }

    //Searches list and removes any null values (dead enemies)
    public void RefreshSpawnedEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
            if (spawnedEnemies[i] == null)
                spawnedEnemies.RemoveAt(i);
    }

    public void AddRemainingChild()
    {
        remainingChildren++;
    }

    public void SubtractRemainingChild()
    {
        remainingChildren--;
    }

    public int GetNumEnemies()
    {
        return spawnedEnemies.Count;
    }

    public void SetCleared(bool _clear)
    {
        isAreaCleared = _clear;
        if (isAreaCleared)
        {
            spawnEnabled = false;
            enemies.Clear();
            points = 0;
            enemiesClone.Clear();
            pointsClone = 0;
        }
    }
    public void SetClearCheck(bool _clear)
    {
        clearCheck = _clear;
    }
    public int GetPointsRemaining()
    {
        return pointsClone;
    }
    #endregion
}