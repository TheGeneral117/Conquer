using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingLadyScript : BaseNPC
{
    public override void DoAction()
    {
       
    }

    public override Dialogue GetDialogue(string dialogue)
    {
        Random.InitState((int)Time.time);
        int val = Random.Range(0, 4);
        return dialogues[val];
    }
}
