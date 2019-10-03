using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZookeeperScript : BaseNPC
{

    public override string GetString(string text)
    {
        switch (text)
        {
            case "Attack Companion":
                return dialogues[0].textArray[0];
            case "Coin Booster Companion":
                return dialogues[0].textArray[1];
            case "Defense Companion":
                return dialogues[0].textArray[2];
            case "Fire Resist Companion":
                return dialogues[0].textArray[3];
            case "Health Regen Companion":
                return dialogues[0].textArray[4];
            case "Ice Resist Companion":
                return dialogues[0].textArray[5];
            case "Item Grabber Companion":
                return dialogues[0].textArray[6];
            case "Melee Companion":
                return dialogues[0].textArray[7];
            case "Movement Speed Companion":
                return dialogues[0].textArray[8];
            case "Scavenger Companion":
                return dialogues[0].textArray[9];
            case "Shooter Companion":
                return dialogues[0].textArray[10];
            case "Stun Resist Companion":
                return dialogues[0].textArray[11];
            case "Stunner Companion":
                return dialogues[0].textArray[12];
            case "XP Companion":
                return dialogues[0].textArray[13];
            default:
                return dialogues[0].textArray[14];
        }
    }

    public override void DoAction()
    {

    }
}
