using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum Type {Coin, Health, Potion, Box, Bullet, Spells, EOF};
    public Type type = Type.Coin;
    [SerializeField] AudioClip clip = null;

    private void Update()
    {
        if(Type.Coin == type)
            transform.Rotate(Vector3.right * (100.0f * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            AudioSource source = other.GetComponentInParent<AudioSource>();
            if(clip != null)
                source.PlayOneShot(clip);
            switch (type)
            {
                case Type.Coin:
                    other.GetComponentInParent<Player>().AddCoins(1);
                    break;
                case Type.Health:
                    other.GetComponentInParent<Player>().RestoreHealth(10);
                    break;
                case Type.Potion:
                    other.GetComponentInParent<Inventory>().AddConsumable(GetComponent<Consumable>());
                    break;
                case Type.Box:
                    other.GetComponentInParent<Inventory>().AddBoxPiece();
                    break;
                case Type.Bullet:
                    other.GetComponentInParent<Inventory>().AddBullet();
                    break;
                case Type.Spells:
                    other.GetComponentInParent<Inventory>().AddConsumable(GetComponent<Consumable>());
                    break;
                case Type.EOF:
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
