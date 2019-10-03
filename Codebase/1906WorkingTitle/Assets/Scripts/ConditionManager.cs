using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConditionManager : MonoBehaviour
{
    #region ConditionManagerProperties
    #region Fields
    //thawIncrement: How much the player's  movement speed increases on a thaw tick
    [SerializeField] private float thawIncrement = 0.1f;
    [SerializeField] private int fireTimer = 0;
    [SerializeField] private int thawTimer = 0;
    private float fireDamage = 1.0f;
    private float auraDamage = 0.05f;
    private int stunTimer = 0;
    private int auraTimer = 0;
    private bool isPlayer = false;
    private float speed = 0.0f;
    private float maxSpeed = 0.0f;
    private float minFrozenSpeed = 0.0f;
    private bool isPaused = false;
    #endregion

    #region Unity Components
    [SerializeField] private GameObject fireParticle = null;
    [SerializeField] private GameObject iceParticle = null;
    [SerializeField] private GameObject stunParticle = null;
    private Component statsScript = null;
    private Component aiScript = null;
    private Renderer enemyRender = null;
    #endregion
    #endregion

    public void Start()
    {
        string tag = gameObject.tag;
        if (tag.Equals("Player"))
        {
            isPlayer = true;
            aiScript = GetComponentInParent<Player>();
            statsScript = GetComponentInParent<Player>();
        }
        else
        {
            if (tag.Equals("Enemy"))
                aiScript = GetComponentInParent<EnemyAI>();
            if (tag.Equals("BulletHell Enemy"))
                aiScript = GetComponentInParent<BulletHellEnemy>();
            isPlayer = false;
            statsScript = GetComponentInParent<EnemyStats>();
            enemyRender = ((EnemyStats)statsScript).GetRenderer();
        }
        if (auraDamage == 0f)
            auraDamage = .017f;
        if (fireParticle != null)
            fireParticle.SetActive(false);
        if (iceParticle != null)
            iceParticle.SetActive(false);
        speed = GetSpeed();
        maxSpeed = GetSpeed();
    }

    public void Update()
    {
        if (!isPaused)
        {
            if (fireTimer > 0 || thawTimer > 0 || stunTimer > 0 || auraTimer > 0)
            {
                #region FireTimer
                if (fireTimer > 0)
                {
                    if (fireParticle.activeSelf == false && fireParticle != null)
                        fireParticle.SetActive(true);
                    fireTimer--;
                    if (fireTimer % 60 == 0)
                        Damage(new Color(0.921f, 0.505f, 0f), fireDamage);
                }
                #endregion
                #region Thawtimer
                if (thawTimer > 0)
                {
                    if (iceParticle.activeSelf == false && iceParticle != null)
                        iceParticle.SetActive(true);
                    thawTimer--;
                    float speedThaw = GetSpeed() + (thawIncrement / 30f);
                    if (speedThaw <= maxSpeed)
                        AddSpeed(thawIncrement / 30f);
                }
                #endregion
                #region StunTimer
                if (stunTimer > 0)
                {
                    if (!stunParticle.activeSelf)
                        stunParticle.SetActive(true);
                    stunTimer--;
                    if (stunTimer == 0)
                        Unstun();
                }
                #endregion
                #region AuraTimer
                if (auraTimer > 0)
                {
                    Damage(Color.green, auraDamage);
                    auraTimer--;
                }
                #endregion
            }
            #region Timer End
            if (fireParticle.activeSelf && fireTimer == 0)
            {
                if (fireParticle != null)
                    fireParticle.SetActive(false);
            }
            if (iceParticle.activeSelf && thawTimer == 0)
            {
                if (iceParticle != null)
                    iceParticle.SetActive(false);
            }
            if (stunParticle.activeSelf && stunTimer == 0)
            {
                    stunParticle.SetActive(false);
            }
            #endregion
        }
    }

    #region TimerManagement
    public void TimerAdd(string condition, int ticks)
    {
        switch (condition)
        {
            case "fire":
                {
                    fireTimer += ticks;
                    break;
                }
            case "thaw":
                {
                    thawTimer += ticks;
                    break;
                }
            case "stun":
                {
                    stunTimer += ticks;
                    break;
                }
            case "aura":
                {
                    auraTimer += ticks;
                    break;
                }
            case "love":
                {
                    StartCoroutine(GetComponent<EnemyAI>().FallInLove(5f));
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void TimerSet(string condition, int ticks)
    {
        switch (condition)
        {
            case "fire":
                {
                    fireTimer = ticks;
                    break;
                }
            case "thaw":
                {
                    thawTimer = ticks;
                    break;
                }
            case "stun":
                {
                    stunTimer = ticks;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    #endregion

    #region GetSetters

    #region Speed
    //Cast get-setters (This get-set speed workaround feels silly and wrong)
    public float GetSpeed()
    {
        if (isPlayer)
            return ((Player)statsScript).GetMovementSpeed();
        else
            return ((EnemyStats)statsScript).GetMovementSpeed();
    }
    public void SetSpeed(float _speed)
    {
        if (isPlayer)
            ((Player)statsScript).SetMovementSpeed(_speed);
        else
            ((EnemyStats)statsScript).SetMovementSpeed(_speed);
    }
    public void SubtractSpeed(float _speed)
    {
        if (GetSpeed() - _speed >= 0)
            SetSpeed(GetSpeed() - _speed);
    }
    public void AddSpeed(float _speed)
    {
        if(GetSpeed() + _speed <= maxSpeed)
        {
            SetSpeed(GetSpeed() + _speed);
        }
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public void SetMaxSpeed(float _maxSpeed)
    {
        maxSpeed = _maxSpeed;
    }
    public void Modify(float _amountToIncrease = 1)
    {
        maxSpeed += _amountToIncrease;
        SetSpeed(maxSpeed);
    }
    #endregion

    #region Nonspeed
    public void Damage(Color _color, float _damage)
    {
        if (isPlayer)
            ((Player)statsScript).TakeDamage(_color, _damage);
        else
            ((EnemyStats)statsScript).TakeDamage(_color, _damage);
    }

    public float GetThawIncrement()
    {
        return thawIncrement;
    }

    public float GetMinFrozenSpeed()
    {
        return minFrozenSpeed;
    }

    public float GetFireDamage()
    {
        return fireDamage;
    }

    public void SetThawIncrement(float _thawIncrement)
    {
        thawIncrement = _thawIncrement;
    }

    public void SetMinFrozenSpeed(float _minFrozenSpeed)
    {
        minFrozenSpeed = _minFrozenSpeed;
    }

    public void SetFireDamage(float _fireDamage)
    {
        fireDamage = _fireDamage;
    }

    void OnPauseGame()
    {
        isPaused = true;
    }

    void OnResumeGame()
    {
        isPaused = false;
    }

    public void Unstun()
    {
        if (gameObject.CompareTag("Player"))
            ((Player)aiScript).Unstun();
        else
        {
            NavMeshAgent nav = GetComponentInParent<NavMeshAgent>();
            nav.enabled = true;
            if (gameObject.CompareTag("BulletHell Enemy"))
                ((BulletHellEnemy)aiScript).Unstun();
            else
                ((EnemyAI)aiScript).Unstun();
        }
    }
    #endregion

    #endregion
}