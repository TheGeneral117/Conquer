using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region PlayerStats
    [SerializeField] private float playerMovementSpeed = 8.0f;
    [SerializeField] private float playerHealth = 10.0f;
    [SerializeField] private float maxPlayerHealth = 10.0f;
    [SerializeField] private int playerDefense = 1;
    [SerializeField] private float playerAttackSpeed = 1.0f;
    [SerializeField] private float playerTrueAttackSpeed = 1.0f;
    [SerializeField] private int visualAttackSpeed = 1;
    [SerializeField] private int playerAttackDamage = 1;
    [SerializeField] private int playerSpendingPoints = 0;
    [SerializeField] private int playerLives = 5;
    [SerializeField] private uint bulletVelocity = 0;
    [SerializeField] private float playerExperience = 0.0f;
    [SerializeField] private int playerLevel = 1;

    //If player is immune to status conditions
    public bool isIceImmune = false;
    public bool isFireImmune = false;
    public bool isStunImmune = false;

    //Other misc fields
    public bool isStunned = false;
    public bool isDashing = false;
    private bool isRegenerating = false;
    private float playerExperienceModifier = 1;
    private int playerCoinModifier = 1;
    private int bulletChoice = 1;
    private float nextLevelExperience = 10.0f;
    private float lastTimeFired = 0.0f;
    private bool isAbleToDash = true;
    public bool enemyRespawn = false;
    public bool isInvincible = false;
    [HideInInspector] public int iceWall = 0;
    [HideInInspector] public int cactusWall = 0;
    bool canRotate = false;
    #endregion

    #region UnityComponents
    [SerializeField] private AudioClip fire = null;
    [SerializeField] private Texture2D crosshairs = null;
    [SerializeField] private GameObject mainUI = null;
    [SerializeField] private GameObject deathAura = null;
    [SerializeField] private GameObject iceSpell = null;
    [SerializeField] private GameObject gameOver = null;
    [SerializeField] private GameObject saveUI = null;
    [SerializeField] private GameObject loadUI = null;

    private Animator animator = null;
    private GameObject dashTrail = null;
    private SaveScript save = null;
    private Companion currentCompanion = null;
    private ConditionManager con;
    private CharacterController characterController = null;
    private Renderer playerRenderer = null;
    private Color playerColor = Color.black;
    private AudioSource source = null;
    private Inventory inventory = null;
    #endregion

    #region PlayerMovementProperties
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 mousePosition = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField] Camera mainCamera = null;
    private float playerY = 0.0f;
    private bool paused = false;
    #endregion

    #region Projectiles
    [SerializeField] private GameObject projectile0 = null;
    [SerializeField] private GameObject projectile1 = null;
    [SerializeField] private GameObject projectile2 = null;
    [SerializeField] private GameObject projectile3 = null;
    [SerializeField] private GameObject projectilePosition = null;
    #endregion

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inventory = GetComponent<Inventory>();
        playerRenderer = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        save = GetComponent<SaveScript>();
        con = GetComponent<ConditionManager>();

        dashTrail = GameObject.Find("DashTrail");
        gameOver = GameObject.FindGameObjectWithTag("GameOver");
        if (GameObject.Find("Main UI"))
            mainUI = GameObject.Find("Main UI");

        #if UNITY_STANDALONE
                Cursor.SetCursor(crosshairs, new Vector2(16, 16), CursorMode.Auto);
        #elif UNITY_WEBGL
                Cursor.SetCursor(crosshairs, new Vector2(16, 16), CursorMode.ForceSoftware);
        #endif

        if (gameOver != null)
            gameOver.SetActive(false);
        dashTrail.SetActive(false);
        playerColor = playerRenderer.material.color;
        playerY = transform.position.y;
        source.enabled = true;
        lastTimeFired = 0.0f;
        isDashing = false;
        isAbleToDash = true;
        isRegenerating = false;
        bulletChoice = 1;
        deathAura.SetActive(false);
        iceSpell.SetActive(false);
        enemyRespawn = false;
        if (saveUI != null)
            saveUI.SetActive(false);
        isInvincible = false;
        canRotate = true;
    }

    void Update()
    {
        if (!paused)
        {
            #region PlayerRotation
            // Rotate the Player Transform to face the Mouse Position
            mousePosition = Input.mousePosition;
            targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            Vector3 relativePosition = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePosition);

            // Lock the rotation around X and Z Axes
            rotation.x = 0.0f;
            rotation.z = 0.0f;

            // Change the player's tranform's rotation to the rotation Quaternion
            if (canRotate)
                transform.rotation = rotation;
            #endregion

            #region HitFeedback
            // Player reverting to original color after hit
            if (playerRenderer.material.color != playerColor)
                playerRenderer.material.color = Color.Lerp(playerRenderer.material.color, playerColor, 0.1f);
            #endregion

            if (!isStunned)
            {
                #region PlayerMovement
                //Move the Player GameObject when the WASD or Arrow Keys are pressed
                if (isDashing == false)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                    moveDirection *= playerMovementSpeed;
                    characterController.Move(moveDirection * Time.deltaTime);
                }
                else
                    characterController.Move(moveDirection / playerMovementSpeed);

                //Player Dash if Spacebar is pressed
                if (Input.GetButtonDown("Dash") && moveDirection != Vector3.zero)
                    if (isAbleToDash == true)
                        StartCoroutine(PlayerDash());

                //Set animator values
                if (animator != null)
                {
                    animator.SetBool("Walk", true);
                    animator.SetFloat("Horizontal", moveDirection.x);
                    animator.SetFloat("Vertical", moveDirection.z);
                }
                #endregion

                #region PlayerAttack
                if (Input.GetButtonDown("Bullet 1") && inventory.GetBulletCount() >= 0)
                    bulletChoice = 1;
                if (Input.GetButtonDown("Bullet 2") && inventory.GetBulletCount() >= 1)
                    bulletChoice = 2;
                if (Input.GetButtonDown("Bullet 3") && inventory.GetBulletCount() >= 2)
                    bulletChoice = 3;
                if (Input.GetButtonDown("Bullet 4") && inventory.GetBulletCount() >= 3)
                    bulletChoice = 4;
                // If the corresponding button is clicked call ShootBullet
                if (Input.GetAxis("Fire1") > 0f)
                    ShootBullet(bulletChoice);
                #endregion

                if (playerExperience >= nextLevelExperience)
                    LevelUp();
            }
        }
        transform.position = new Vector3(transform.position.x, playerY, transform.position.z);
        if (isInvincible)
            playerRenderer.material.color = Color.yellow;
    }

    #region PlayerFunctions
    public void ShootBullet(int type)
    {
        //Instantiate a projectile and set the projectile's velocity towards the forward vector of the player transform
        if (Time.time > lastTimeFired + playerAttackSpeed)
        {
            GameObject clone;
            CollisionScript collisionScript = null;
            switch (type)
            {
                case 1:
                    {
                        clone = Instantiate(projectile0, projectilePosition.transform.position, transform.rotation);
                        collisionScript = clone.GetComponent<CollisionScript>();
                        collisionScript.SetHitColor(new Color(0.913f, 0.541f, 0.109f));
                        break;
                    }
                case 2:
                    {
                        clone = Instantiate(projectile1, projectilePosition.transform.position, transform.rotation);
                        collisionScript = clone.GetComponent<CollisionScript>();
                        collisionScript.SetHitColor(new Color(0.921f, 0.505f, 0f));
                        break;
                    }
                case 3:
                    {
                        clone = Instantiate(projectile2, projectilePosition.transform.position, transform.rotation);
                        collisionScript = clone.GetComponent<CollisionScript>();
                        collisionScript.SetHitColor(new Color(0.360f, 0.952f, 0.960f));
                        break;
                    }
                case 4:
                    {
                        clone = Instantiate(projectile3, projectilePosition.transform.position, transform.rotation);
                        collisionScript = clone.GetComponent<CollisionScript>();
                        collisionScript.SetHitColor(Color.yellow);
                        break;
                    }
                default:
                    {
                        clone = null;
                        break;
                    }
            }
            if (clone != null)
            {

                clone.GetComponent<TrailRenderer>().time = .1125f;
                collisionScript.SetOwner(gameObject);
                collisionScript.bulletDamage = playerAttackDamage;
                clone.gameObject.layer = 10;
                clone.gameObject.SetActive(true);
                clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * bulletVelocity);
                lastTimeFired = Time.time;
                source.PlayOneShot(fire);
                animator.SetTrigger("Attack");
                StartCoroutine(StopRotation());
            }
        }
    }

    IEnumerator StopRotation()
    {
        canRotate = false;
        yield return new WaitForSeconds(0.6428572f);
        canRotate = true;
    }

    public void ThrowConsumable(GameObject consumable)
    {
        GameObject clone = Instantiate(consumable, projectilePosition.transform.position, transform.rotation);
        CollisionScript collisionScript = clone.GetComponent<CollisionScript>();
        clone.GetComponent<TrailRenderer>().time = .1125f;
        collisionScript.bulletDamage = 0;
        Color hitColor = Color.red;
        switch (clone.tag)
        {
            case "Love Bullet":
                hitColor = new Color(0.960f, 0.619f, 0.921f);
                break;
            case "Hex":
                hitColor = new Color(0.498f, 0.011f, 0.729f);
                break;
            case "FireBall":
                hitColor = new Color(0.921f, 0.505f, 0f);
                break;
            case "Default":
                hitColor = Color.yellow;
                break;
            default:
                break;
        }
        collisionScript.SetHitColor(hitColor);
        clone.gameObject.layer = 10;
        clone.gameObject.SetActive(true);
        clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * bulletVelocity);
        lastTimeFired = Time.time;
        source.PlayOneShot(fire);
    }

    IEnumerator PlayerDash()
    {
        dashTrail.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        isDashing = true;
        isAbleToDash = false;
        gameObject.layer = 15;
        //characterController.Move(moveDirection);
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
        gameObject.layer = 9;
        yield return new WaitForSeconds(0.1f);
        dashTrail.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        isAbleToDash = true;
    }

    public void BlinkOnHit(Color _color)
    {
        animator.SetTrigger("On Hit");
        playerRenderer.material.color = _color;
    }

    public void Death()
    {
        gameOver.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("BulletHell Enemy"))
            TakeDamage(Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SavePoint"))
            saveUI.SetActive(true);
        if (other.CompareTag("Companion"))
        {
            if (currentCompanion != null)
                currentCompanion.Deactivate();
            other.GetComponent<Companion>().Activate();
        }

    }

    #endregion

    #region AccessorsAndMutators

    #region Health
    public void TakeDamage(Color _color, float amountOfDamage = 1)
    {
        if (!isInvincible)
        {
            //Decrease health by amountOfDamage until 0 or less
            if (amountOfDamage >= 1)
                BlinkOnHit(_color);
            amountOfDamage /= playerDefense;
            playerHealth -= amountOfDamage;
            if (mainUI != null && mainUI.activeSelf)
                mainUI.GetComponent<UpdateUI>().TakeDamage();
            if (playerHealth < 1)
            {
                playerLives--;
                StartCoroutine(Invincible());
                if (playerLives <= 0)
                    Death();
                playerHealth = maxPlayerHealth;
            }
        }
    }

    public void RestoreHealth(float amountOfHealth)
    {
        playerHealth += amountOfHealth;
        if (playerHealth > maxPlayerHealth)
            playerHealth = maxPlayerHealth;
    }

    public float GetHealth()
    {
        return playerHealth;
    }

    public float GetMaxHealth()
    {
        return maxPlayerHealth;
    }

    public void ModifyHealth(float _playerHealth)
    {
        maxPlayerHealth += _playerHealth;
    }

    public bool GetIsRegenerating()
    {
        return isRegenerating;
    }

    public IEnumerator HealthRegen()
    {
        isRegenerating = true;
        if (playerHealth < maxPlayerHealth)
            playerHealth += 0.5f;
        yield return new WaitForSeconds(2.5f);
        isRegenerating = false;
    }

    public void SetHealth(float _playerHealth)
    {
        playerHealth = _playerHealth;
    }

    public void SetMaxHealth(float _playerMaxHealth)
    {
        maxPlayerHealth = _playerMaxHealth;
    }

    public IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1);
        isInvincible = false;
    }

    #endregion

    #region Coins
    public void AddCoins(int amountOfCoins)
    {
        amountOfCoins *= playerCoinModifier;
        inventory.AddCoins(amountOfCoins);
    }

    public int GetCoins()
    {
        return inventory.GetCoins();
    }

    public void CoinModifier(int _coinModifier)
    {
        playerCoinModifier += _coinModifier;
    }
    #endregion

    #region Defense
    public int GetDefense()
    {
        return playerDefense;
    }

    public void AddDefense()
    {
        playerDefense++;
        playerSpendingPoints--;
    }

    public void ModifyDefense(int _playerDefense)
    {
        playerDefense += _playerDefense;
    }

    public void SetDefense(int _playerDefense)
    {
        playerDefense = _playerDefense;
    }
    #endregion

    #region Damage
    public int GetDamage()
    {
        return playerAttackDamage;
    }

    public void AddDamage()
    {
        playerAttackDamage++;
        playerSpendingPoints--;
    }

    public void ModifyDamage(int _playerDamage)
    {
        playerAttackDamage += _playerDamage;
    }

    public void SetDamage(int _playerDamage)
    {
        playerAttackDamage = _playerDamage;
    }
    #endregion

    #region AttackSpeed
    public int GetAttackSpeed()
    {
        return visualAttackSpeed;
    }

    public void AddAttackSpeed()
    {
        if (playerAttackSpeed > 0.2f)
        {
            visualAttackSpeed++;
            playerAttackSpeed -= 0.1f;
            playerSpendingPoints--;
            playerTrueAttackSpeed = playerAttackSpeed;
        }
    }

    public void ModifyAttackSpeed(float _attackSpeed)
    {
        playerAttackSpeed -= _attackSpeed;
        visualAttackSpeed += (int)(_attackSpeed * 10f);
    }

    public void SetAttackSpeed(int _attackSpeed)
    {
        visualAttackSpeed = _attackSpeed;
    }

    public float GetFireRate()
    {
        return playerAttackSpeed;
    }

    public float GetTrueFireRate()
    {
        return playerTrueAttackSpeed;
    }

    public void SetFireRate(float _playerAttackSpeed)
    {
        playerAttackSpeed = _playerAttackSpeed;
    }

    public float GetLastTimeFired()
    {
        return lastTimeFired;
    }

    public void SetLastTimeFired(float _lastTimeFired)
    {
        lastTimeFired = _lastTimeFired;
    }
    #endregion

    #region MovementSpeed
    public void SetMovementSpeed(float newMovementSpeed)
    {
        playerMovementSpeed = newMovementSpeed;
    }

    public float GetMovementSpeed()
    {
        return playerMovementSpeed;
    }

    public void AddPlayerSpeed(float _sum)
    {
        playerMovementSpeed += _sum;
    }

    public void ModifySpeed(float _playerSpeed)
    {
        con.Modify(_playerSpeed);
    }
    #endregion

    #region LevelAndXP
    public void LevelUp()
    {
        maxPlayerHealth += 3;
        playerHealth = maxPlayerHealth;
        if (playerMovementSpeed < 15)
            playerMovementSpeed++;
        playerSpendingPoints++;
        playerExperience -= nextLevelExperience;
        nextLevelExperience += 5;
        playerLevel++;
        GetComponent<ConditionManager>().Modify();
        if (mainUI != null && mainUI.activeSelf)
            mainUI.GetComponent<UpdateUI>().LevelUp();
    }

    public float GetExperience()
    {
        return playerExperience;
    }

    public void SetExperience(float _playerExp)
    {
        playerExperience = _playerExp;
    }

    public float GetNextLevelExperience()
    {
        return nextLevelExperience;
    }

    public void SetNextLevelExperience(float _nextLevel)
    {
        nextLevelExperience = _nextLevel;
    }

    public int GetSpendingPoints()
    {
        return playerSpendingPoints;
    }

    public void SetSpendingPoints(int _spendPoints)
    {
        playerSpendingPoints = _spendPoints;
    }

    public int GetLevel()
    {
        return playerLevel;
    }

    public void SetLevel(int _level)
    {
        playerLevel = _level;
    }

    public void GainExperience(float playerEXP)
    {
        playerEXP *= playerExperienceModifier;
        playerExperience += playerEXP;
    }

    public void XPModifier(float _XPModifier)
    {
        playerExperienceModifier += _XPModifier;
    }
    #endregion

    #region PlayerLives
    public int GetLives()
    {
        return playerLives;
    }

    public void IncreaseLives()
    {
        playerLives++;
    }

    public void SetLives(int _lives)
    {
        playerLives = _lives;
    }
    #endregion

    #region Pause
    public void OnPauseGame()
    {
        paused = true;
    }

    public void OnResumeGame()
    {
        paused = false;
    }
    #endregion

    #region Stun
    public void Stun()
    {
        isStunned = true;
    }

    public void Unstun()
    {
        isStunned = false;
    }
    #endregion

    #region Position
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 _position)
    {
        transform.position = _position;
    }
    #endregion

    #region Companion
    public void SetCompanion(Companion _companion)
    {
        currentCompanion = _companion;
    }

    public void ResetCompanion()
    {
        currentCompanion = null;
    }

    public Companion GetCompanion()
    {
        return currentCompanion;
    }
    #endregion

    #region Death Aura
    public void EnableDeathAura()
    {
        if (deathAura.activeSelf)
        {
            DisableThis dt = deathAura.GetComponent<DisableThis>();
            dt.AddTime(180);
        }
        else
            deathAura.SetActive(true);
    }

    public void EnableConeofCold()
    {
        if (iceSpell.activeSelf)
        {
            DisableThis dt = iceSpell.GetComponent<DisableThis>();
            dt.AddTime(180);
        }
        else
            iceSpell.SetActive(true);
    }
    #endregion

    #region LoadUI
    public GameObject GetLoadUI()
    {
        return loadUI;
    }
    #endregion
    #endregion
}