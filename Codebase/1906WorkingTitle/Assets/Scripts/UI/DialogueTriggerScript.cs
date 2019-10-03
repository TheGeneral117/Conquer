using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTriggerScript : MonoBehaviour
{
    [SerializeField] private BaseNPC baseNPC = null;
    [SerializeField] private Canvas dialogueCanvas = null;
    [SerializeField] private Player playerScript = null;
    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private DialogueManager dialogueManager = null;
    [SerializeField] private string dialogueName = null;
    [SerializeField] private bool chain = true;
    private bool debouncer = true;

    private void Start()
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Player will be stopped and NPC Knight will walk towards him
        if (other.tag == "Player" && debouncer)
        {
            debouncer = false;
            dialogueManager.dialogueTriggerScript = this;
            baseNPC.dialogueTriggerScript = this;
            playerScript.isStunned = true;
            playerAnimator.enabled = false;
            dialogueCanvas.gameObject.SetActive(true);
            if (dialogueName == "Welcome")
                gameObject.SetActive(false);

            if (baseNPC.name == "Zookeeper")
            {
              if (playerScript.GetCompanion() != null)
                    dialogueName = playerScript.GetCompanion().gameObject.name;
            }

            if (chain)
            {
                dialogueManager.dialogue = baseNPC.GetDialogue(dialogueName);
                dialogueManager.DisplayText();
            }
            else
            {
                dialogueManager.DisplayText(baseNPC.GetString(dialogueName));
            }
            baseNPC.DoAction();
            debouncer = true;
        }
    }

    public void OnDialogueEnd()
    {
        playerAnimator.enabled = true;
        playerScript.isStunned = false;
        baseNPC.OnDialogueEnd();
    }
}