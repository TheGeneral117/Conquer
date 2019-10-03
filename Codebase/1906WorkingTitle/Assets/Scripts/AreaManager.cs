using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawners = null;
    private List<SpawnScript> spawnScripts = null;
    [SerializeField] private int areaNumber = 0;

    private void Start()
    {
        spawnScripts = new List<SpawnScript>();
        foreach (GameObject s in spawners)
            spawnScripts.Add(s.GetComponent<SpawnScript>());
        int keys = 0;
        keys = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().GetBoxPieces();
        if (keys >= areaNumber)
            AreaReset(true);
    }

    public void AreaReset(bool _cleared)
    {
        foreach (SpawnScript s in spawnScripts)
        {
            s.room.ClearRoom(_cleared);
            s.SetCleared(_cleared);
        }
    }
}
