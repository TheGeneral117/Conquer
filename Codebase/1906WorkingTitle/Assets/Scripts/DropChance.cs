using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropChance : MonoBehaviour
{
    public void Drop()
    {
        int rand = Random.Range(1,50);
        GameObject pickup = null;
        switch (rand)
        {
            case 1:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Health Potion");
                break;
            case 2:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Cone of Cold");
                break;
            case 3:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Damage Buff Potion");
                break;
            case 4:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Death Aura");
                break;
            case 5:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Defense Potion");
                break;
            case 6:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Fire Potion");
                break;
            case 7:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Fireball");
                break;
            case 8:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Hex Shot");
                break;
            case 9:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Ice Potion");
                break;
            case 10:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Love Potion");
                break;
            case 11:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Speed Potion");
                break;
            case 12:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/Lightning Bolt");
                break;
            default:
                pickup = Resources.Load<GameObject>("Prefabs/PickUps/coin");
                break;
        }

        if (pickup != null)
        {
            Vector3 vec = GetComponent<Transform>().position;
            vec = new Vector3(vec.x, vec.y + 0.5f, vec.z);
            Instantiate(pickup, vec, pickup.transform.rotation);
        }
    }
}
