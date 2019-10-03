using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialDone : MonoBehaviour
{
    [SerializeField] GameObject dialogueCanvas = null;

  //After The Tutorial is done this changes to the game scene.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            dialogueCanvas.SetActive(false);
            GameObject clone = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneLoader"));
            StartCoroutine(clone.GetComponent<SceneLoader>().LoadNewScene(2));
        }
    }
}
