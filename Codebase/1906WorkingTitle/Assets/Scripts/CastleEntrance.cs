using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleEntrance : MonoBehaviour
{
    [SerializeField] private GameObject castleGate = null;
    private Inventory playerInventory = null;
    
    void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }
    
    void Update()
    {
        if(playerInventory.GetBoxPieces() == 3)
            StartCoroutine(OpenGate());
    }

    IEnumerator OpenGate()
    {
        yield return new WaitForSeconds(.5f);
        castleGate.SetActive(false);
    }
}
