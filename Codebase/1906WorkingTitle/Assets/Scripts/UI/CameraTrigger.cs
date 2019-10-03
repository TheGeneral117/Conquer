using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private bool hasVisited = false;
    [SerializeField] private GameObject knight = null;
    [SerializeField] public GameObject spawner = null;
    private GameObject camPos = null;
    private SpawnScript spawn = null;
    
    void Start()
    {
        //Finds each camera position object in each room
        camPos = transform.Find("Camera Position").gameObject;
        spawn = spawner.GetComponent<SpawnScript>();
        spawn.SetEnabled(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Changes the cameras position
            Camera.main.transform.position = camPos.transform.position;
            if (!hasVisited)
            {
                //Spawns enemies and locks rooms
                spawn.SetDoorLock(true);
                spawn.SetEnabled(true);
                spawn.SetClearCheck(true);
                spawn.ResetSpawner();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //spawn.DespawnEnemies();
        if (other.tag == "Player")
        {
            if(knight != null)
            {
                knight.SetActive(false);
            }
        }
        if (other.CompareTag("Enemy"))
            other.GetComponent<EnemyStats>().Kill();
    }

    public void ClearRoom(bool _cleared)
    {
        hasVisited = _cleared;
    }
}
