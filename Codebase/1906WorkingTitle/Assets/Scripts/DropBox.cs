using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    //[SerializeField] private GameObject boxPiece = null, bullet = null;
    [SerializeField] private GameObject dialogueTrigger = null;
    [SerializeField] private GameObject spawner = null;
    private SpawnScript spawnScript = null;
    [SerializeField] private GameObject areaManager = null;

    private bool buffer = true;

    private void Start()
    {
        spawnScript = spawner.GetComponent<SpawnScript>();
    }
    private void Update()
    {
        if (buffer)
        {
            if (spawnScript.GetPointsRemaining() <= 0 && spawner.activeSelf)
            {
                if (spawnScript.spawnedEnemies.Count <= 0 && spawner.activeSelf)
                {
                    if(areaManager != null)
                        areaManager.GetComponent<AreaManager>().AreaReset(false);
                    DropLoot();
                    buffer = false;
                }
            }


        }
    }
    void DropLoot()
    {
        //boxPiece.SetActive(true);
        //bullet.SetActive(true);
        Inventory playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInv.AddBoxPiece();
        playerInv.AddBullet();
        dialogueTrigger.SetActive(true);
    }
}
