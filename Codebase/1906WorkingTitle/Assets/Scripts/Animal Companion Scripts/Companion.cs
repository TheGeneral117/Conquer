using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    #region CompanionStats
    private int bulletDamage = 0;
    private Vector3 animalVelocity = Vector3.zero;
    private Vector3 playerPositionOffset = Vector3.zero;
    public bool isFollowing = false;
    private float lastTimeAttacked = 0.0f;
    private bool hasAttacked = false;
    private bool canAttack = true;
    private float previousExp = 0.0f;
    private string storageName = "";
    private bool hasDeactivated = false;
    private bool isLookingAtStorage = false;
    #endregion

    #region Unity Components
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private GameObject projectilePos = null;
    [SerializeField] private AudioClip fire = null;
    [SerializeField] private GameObject potion = null;
    private GameObject player = null;
    private GameObject target = null;
    private Player playerStats = null;
    private CapsuleCollider animalCollider = null;
    private Inventory playerInventory = null;
    private Pickup pickup = null;
    private GameObject companionAura = null;
    private Animator animator = null;
    private GameObject storagePosition = null;
    private AudioSource source = null;
    GameObject activePosition = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        activePosition = GameObject.Find("PlayerPositionOffset");
        playerStats = player.GetComponent<Player>();
        animalCollider = GetComponent<CapsuleCollider>();
        animalCollider.isTrigger = true;
        playerInventory = player.GetComponent<Inventory>();
        gameObject.layer = 0;
        if (name == "Shooter Companion" || name == "Stunner Companion")
        {
            source = GetComponent<AudioSource>();
            source.enabled = true;
            canAttack = true;
        }
        companionAura = GetComponentInChildren<ParticleSystem>().gameObject;
        companionAura.SetActive(false);
        animator = GetComponent<Animator>();
        storageName = $"{name} Storage";
        storagePosition = GameObject.Find(storageName);
        hasDeactivated = false;
        isLookingAtStorage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing == true)
        {
            if (name != "Item Grabber Companion" && name != "Melee Companion")
                transform.position = Vector3.SmoothDamp(transform.position, playerPositionOffset, ref animalVelocity, 0.5f);
            playerPositionOffset = activePosition.transform.position;
            Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            // Lock the rotation around X and Z Axes
            rotation.x = 0.0f;
            rotation.z = 0.0f;
            // Change the companion's tranform's rotation to the rotation Quaternion
            transform.rotation = rotation;
            if (name == "Movement Speed Companion" || name == "Scavenger Companion")
                transform.Rotate(0, 180, 0);
            if (name == "Defense Companion" || name == "Health Regen Companion" || name == "Item Grabber Companion" || name == "Melee Companion")
                transform.Rotate(0, 90, 0);
            if (name == "Health Regen Companion" && playerStats.GetIsRegenerating() == false)
                StartCoroutine(playerStats.HealthRegen());
            else if (name == "Item Grabber Companion")
            {
                target = GameObject.FindGameObjectWithTag("Pickups");
                if (target == null)
                    transform.position = Vector3.SmoothDamp(transform.position, playerPositionOffset, ref animalVelocity, 0.5f);
                if (target != null)
                {
                    pickup = target.GetComponent<Pickup>();
                    if (Vector3.Distance(target.transform.position, player.transform.position) > 10)
                        transform.position = Vector3.SmoothDamp(transform.position, playerPositionOffset, ref animalVelocity, 0.5f);
                    if (Vector3.Distance(target.transform.position, player.transform.position) <= 10)
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref animalVelocity, 0.5f);
                        rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                        // Lock the rotation around X and Z Axes
                        rotation.x = 0.0f;
                        rotation.z = 0.0f;
                        // Change the companion's tranform's rotation to the rotation Quaternion
                        transform.rotation = rotation;
                        transform.Rotate(0, 90, 0);
                    }
                }
            }
            else if (name == "Melee Companion")
            {
                target = GameObject.FindGameObjectWithTag("Enemy");
                if (target == null)
                    target = GameObject.FindGameObjectWithTag("BulletHell Enemy");
                if (target == null)
                    transform.position = Vector3.SmoothDamp(transform.position, playerPositionOffset, ref animalVelocity, 0.5f);
                if (target != null)
                {
                    if (hasAttacked == true || Vector3.Distance(target.transform.position, player.transform.position) > 10)
                        transform.position = Vector3.SmoothDamp(transform.position, playerPositionOffset, ref animalVelocity, 0.5f);
                    if (Vector3.Distance(target.transform.position, player.transform.position) <= 10 && hasAttacked == false)
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref animalVelocity, 0.5f);
                        rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                        // Lock the rotation around X and Z Axes
                        rotation.x = 0.0f;
                        rotation.z = 0.0f;
                        // Change the companion's tranform's rotation to the rotation Quaternion
                        transform.rotation = rotation;
                        transform.Rotate(0, 90, 0);
                    }
                }
                if (Time.time > lastTimeAttacked + 5)
                    hasAttacked = false;
            }
            else if (name == "Shooter Companion" || name == "Stunner Companion")
            {
                target = GameObject.FindGameObjectWithTag("Enemy");
                if (target == null)
                    target = GameObject.FindGameObjectWithTag("BulletHell Enemy");
                if (target != null)
                {
                    if (Vector3.Distance(target.transform.position, player.transform.position) <= 20)
                    {
                        rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                        // Lock the rotation around X and Z Axes
                        rotation.x = 0.0f;
                        rotation.z = 0.0f;
                        // Change the companion's tranform's rotation to the rotation Quaternion
                        transform.rotation = rotation;
                        if (canAttack)
                            StartCoroutine(Attack());
                    }
                }
            }
            else if (name == "Scavenger Companion")
                if (playerStats.GetExperience() > previousExp)
                {
                    int randomNumber = Random.Range(0, int.MaxValue);
                    if (randomNumber % 2 == 0)
                        Instantiate(potion, transform.position, transform.rotation);
                    previousExp = playerStats.GetExperience();
                }
            if (name != "Fire Resist Companion" && name != "Movement Speed Companion" && name != "Shooter Companion" && name != "XP Companion" && name != "Scavenger Companion")
            {
                if (transform.position.x <= playerPositionOffset.x + .1f && transform.position.x >= playerPositionOffset.x - .1f && transform.position.y <= playerPositionOffset.y + .1f && transform.position.y >= playerPositionOffset.y - .1f && transform.position.z <= playerPositionOffset.z + .1f && transform.position.z >= playerPositionOffset.z - .1f)
                    animator.SetBool("isWalking", false);
                else
                    animator.SetBool("isWalking", true);
            }
        }
        else
        {
            if (name != "Fire Resist Companion" && name != "Movement Speed Companion" && name != "Shooter Companion" && name != "XP Companion" && name != "Scavenger Companion")
                animator.SetBool("isWalking", false);
        }
        if (hasDeactivated)
        {
            transform.position = Vector3.SmoothDamp(transform.position, storagePosition.transform.position, ref animalVelocity, 1);
            if (transform.position.x <= storagePosition.transform.position.x + .1f && transform.position.x >= storagePosition.transform.position.x - .1f && transform.position.y <= storagePosition.transform.position.y + .1f && transform.position.y >= storagePosition.transform.position.y - .1f && transform.position.z <= storagePosition.transform.position.z + .1f && transform.position.z >= storagePosition.transform.position.z - .1f)
            {
                isLookingAtStorage = false;
                if (name != "Fire Resist Companion" && name != "Movement Speed Companion" && name != "Shooter Companion" && name != "XP Companion" && name != "Scavenger Companion")
                    animator.SetBool("isWalking", false);
            }
            else
            {
                isLookingAtStorage = true;
                if (name != "Fire Resist Companion" && name != "Movement Speed Companion" && name != "Shooter Companion" && name != "XP Companion" && name != "Scavenger Companion")
                    animator.SetBool("isWalking", true);
            }
            if (isLookingAtStorage)
            {
                Quaternion rotation = Quaternion.LookRotation(storagePosition.transform.position - transform.position);
                // Lock the rotation around X and Z Axes
                rotation.x = 0.0f;
                rotation.z = 0.0f;
                // Change the companion's tranform's rotation to the rotation Quaternion
                transform.rotation = rotation;
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
                // Lock the rotation around X and Z Axes
                rotation.x = 0.0f;
                rotation.z = 0.0f;
                // Change the companion's tranform's rotation to the rotation Quaternion
                transform.rotation = rotation;
                gameObject.layer = 0;
            }
            if (name == "Movement Speed Companion" || name == "Scavenger Companion")
                transform.Rotate(0, 180, 0);
            if (name == "Defense Companion" || name == "Health Regen Companion" || name == "Item Grabber Companion" || name == "Melee Companion")
                transform.Rotate(0, 90, 0);
        }
    }

    #region CompanionFunctions
    public void Activate()
    {
        if (name == "Attack Companion")
            playerStats.ModifyDamage(1);
        else if (name == "Coin Booster Companion")
            playerStats.CoinModifier(1);
        else if (name == "Defense Companion")
            playerStats.ModifyDefense(1);
        else if (name == "Fire Resist Companion")
            playerStats.isFireImmune = true;
        else if (name == "Health Regen Companion")
            playerStats.ModifyHealth(10);
        else if (name == "Ice Resist Companion")
            playerStats.isIceImmune = true;
        else if (name == "Item Grabber Companion")
            animalCollider.isTrigger = false;
        else if (name == "Melee Companion")
            animalCollider.isTrigger = false;
        else if (name == "Movement Speed Companion")
            playerStats.ModifySpeed(3);
        else if (name == "Stun Resist Companion")
            playerStats.isStunImmune = true;
        else if (name == "Shooter Companion")
            bulletDamage = 1;
        else if (name == "Stunner Companion")
            bulletDamage = 0;
        else if (name == "XP Companion")
            playerStats.XPModifier(0.5f);
        isFollowing = true;
        gameObject.layer = 21;
        playerStats.SetCompanion(this);
        previousExp = playerStats.GetExperience();
        companionAura.SetActive(true);
        hasDeactivated = false;
    }

    public void Deactivate()
    {
        playerStats.ResetCompanion();
        if (name == "Attack Companion")
            playerStats.ModifyDamage(-1);
        else if (name == "Coin Booster Companion")
            playerStats.CoinModifier(-1);
        else if (name == "Defense Companion")
            playerStats.ModifyDefense(-1);
        else if (name == "Fire Resist Companion")
            playerStats.isFireImmune = false;
        else if (name == "Health Regen Companion")
        {
            playerStats.ModifyHealth(-10);
            if (playerStats.GetHealth() > playerStats.GetMaxHealth())
                playerStats.SetHealth(playerStats.GetMaxHealth());
        }
        else if (name == "Ice Resist Companion")
            playerStats.isIceImmune = false;
        else if (name == "Item Grabber Companion")
            animalCollider.isTrigger = true;
        else if (name == "Melee Companion")
            animalCollider.isTrigger = true;
        else if (name == "Movement Speed Companion")
            playerStats.ModifySpeed(-3);
        else if (name == "Stun Resist Companion")
            playerStats.isStunImmune = false;
        else if (name == "XP Companion")
            playerStats.XPModifier(-0.5f);
        isFollowing = false;
        companionAura.SetActive(false);
        hasDeactivated = true;
    }

    public void SaveDeactivate()
    {
        playerStats.ResetCompanion();
        isFollowing = false;
        companionAura.SetActive(false);
        hasDeactivated = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (name == "Melee Companion")
            if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("BulletHell Enemy"))
            {
                if (target != null)
                    target.GetComponent<EnemyStats>().TakeDamage(Color.red);
                hasAttacked = true;
                lastTimeAttacked = Time.time;
            }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (name == "Item Grabber Companion")
            if (collider.CompareTag("Pickups"))
                if (pickup.type == Pickup.Type.Coin)
                {
                    playerInventory.AddCoins(1);
                    Destroy(collider.gameObject);
                }
                else if (pickup.type == Pickup.Type.Potion || pickup.type == Pickup.Type.Spells)
                {
                    playerInventory.AddConsumable(pickup.GetComponent<Consumable>());
                    Destroy(collider.gameObject);
                }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        GameObject clone = Instantiate(projectile, projectilePos.transform.position, projectilePos.transform.rotation);
        clone.GetComponent<CollisionScript>().bulletDamage = bulletDamage;
        clone.gameObject.layer = 10;
        clone.SetActive(true);
        source.PlayOneShot(fire);
        clone.GetComponent<Rigidbody>().velocity = transform.forward * 15;
        yield return new WaitForSeconds(5);
        canAttack = true;
    }
    #endregion
}
