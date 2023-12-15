using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCrystal : MonoBehaviour
{
    Dictionary<string, ParameterClass> crystalParameters = new Dictionary<string, ParameterClass>();

    //Holds all of Crystals Information
    //Crystal Base Stats
    int _maxHealth, _maxSpawnEnergy;
    float _threatRange, _dangerRange, _selfRange;

    //Crystal Spawning Stats
    float _spawnCooldownTime, _spawnEnergyRechargeTime;
    int _spawnRadius;

    //Crystal Combat Stats
    int _shardSpawnMin, _shardSpawnMax;

    //Runtime Stats
    int _currentHealth, _currentSpawnEnergy;
    bool _isRecharging, _inLight, _isThreatened, _inDanger;
    GameObject _damageSource, _afflictingLight;

    [SerializeField]
    GameObject crystalShard;

    private void Awake()
    {
        crystalParameters = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameParameters>().crystalParameters;
        UpdateStats();

        _currentHealth = _maxHealth;
        _currentSpawnEnergy = _maxSpawnEnergy;
        _isRecharging = false;
        _inLight = false;
        _isThreatened = false;
        _inDanger = false;
    }

    public void UpdateStats()
    {
        if (!GameManager.usingTestVariables)
        {
            _maxHealth = (int)crystalParameters["cb1"].DefaultValue;
            _maxSpawnEnergy = (int)crystalParameters["cb2"].DefaultValue;
            _threatRange = (int)crystalParameters["cb3"].DefaultValue;
            _dangerRange = crystalParameters["cb4"].DefaultValue;
            _selfRange = crystalParameters["cb5"].DefaultValue;

            _spawnCooldownTime = crystalParameters["cs1"].DefaultValue;
            _spawnEnergyRechargeTime = crystalParameters["cs2"].DefaultValue;
            _spawnRadius = (int)crystalParameters["cs3"].DefaultValue;

            _shardSpawnMin = (int)crystalParameters["cc1"].DefaultValue;
            _shardSpawnMax = (int)crystalParameters["cc2"].DefaultValue;
        } else
        {
            _maxHealth = (int)crystalParameters["cb1"].TestValue != -1 ? (int)crystalParameters["cb1"].TestValue : (int)crystalParameters["cb1"].DefaultValue;
            _maxSpawnEnergy = (int)crystalParameters["cb2"].TestValue != -1 ? (int)crystalParameters["cb2"].TestValue : (int)crystalParameters["cb2"].DefaultValue;
            _threatRange = (int)crystalParameters["cb3"].TestValue != -1 ? (int)crystalParameters["cb3"].TestValue : (int)crystalParameters["cb3"].DefaultValue;
            _dangerRange = crystalParameters["cb4"].TestValue != -1 ? crystalParameters["cb4"].TestValue : crystalParameters["cb4"].DefaultValue;
            _selfRange = crystalParameters["cb5"].TestValue != -1 ? crystalParameters["cb5"].TestValue : crystalParameters["cb5"].DefaultValue;

            _spawnCooldownTime = crystalParameters["cs1"].TestValue != -1 ? crystalParameters["cs1"].TestValue : crystalParameters["cs1"].DefaultValue;
            _spawnEnergyRechargeTime = crystalParameters["cs2"].TestValue != -1 ? crystalParameters["cs2"].TestValue : crystalParameters["cs2"].DefaultValue;
            _spawnRadius = (int)crystalParameters["cs3"].TestValue != -1 ? (int)crystalParameters["cs3"].TestValue : (int)crystalParameters["cs3"].DefaultValue;

            _shardSpawnMin = (int)crystalParameters["cc1"].TestValue != -1 ? (int)crystalParameters["cc1"].TestValue : (int)crystalParameters["cc1"].DefaultValue;
            _shardSpawnMax = (int)crystalParameters["cc2"].TestValue != -1 ? (int)crystalParameters["cc2"].TestValue : (int)crystalParameters["cc2"].DefaultValue;
        }
    }

    
    

    //Crystal Base Stats
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public int MaxSpawnEnergy { get { return _maxSpawnEnergy; } set { _maxSpawnEnergy = value; } }
    public float ThreatRange { get { return _threatRange; } set { _threatRange = value; } }
    public float DangerRange { get { return _dangerRange; } set { _dangerRange = value; } }
    public float SelfRange { get { return _selfRange; } set { _selfRange = value; } }

    //Crystal Spawning Stats
    public float SpawnCooldownTime { get { return _spawnCooldownTime; } set { _spawnCooldownTime = value; } }
    public float SpawnEnergyRechargeTime { get { return _spawnEnergyRechargeTime; } set { _spawnEnergyRechargeTime = value; } }
    public int SpawnRadius { get { return _spawnRadius; } set { _spawnRadius = value; } }

    //Crystal Combat Stats
    public int ShardSpawnMin { get { return _shardSpawnMin; } set { _shardSpawnMin = value; } }
    public int ShardSpawnMax { get { return _shardSpawnMax; } set { _shardSpawnMax = value; } }

    //Runtime Stats
    public int Health { get { return _currentHealth; } set { _currentHealth = value; } }
    public int SpawnEnergy { get { return _currentSpawnEnergy; } set { _currentSpawnEnergy = value; } }
    public bool IsRecharging { get { return _isRecharging; } set { _isRecharging = value; } }
    public bool InLight { get { return _inLight; } set { _inLight = value; } }
    public bool IsThreatened { get { return _isThreatened; } set { _isThreatened = value; } }
    public bool InDanger { get { return _inDanger; } set { _inDanger = value; } }
    public GameObject DamageSource { get { return _damageSource; } set { _damageSource = value; } }
    public GameObject AfflictingLight { get { return _afflictingLight; } set { _afflictingLight = value; } }
    
    
    

    public void Damage(int damage, Transform sourceLocation)
    {
        if (_inLight)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                StartCoroutine(Destroyed());
            }
            _damageSource = sourceLocation.gameObject;
            Debug.Log("Health of Crystal: " + _currentHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public bool CanSpendEnergy(int usedEnergy)
    {
        int testEnergy = _currentSpawnEnergy;

        testEnergy -= usedEnergy;

        if (testEnergy < 0)
        {
            return false;
        } else
        {
            _currentSpawnEnergy -= usedEnergy;
            return true;
        }
    }

    public IEnumerator RechargeEnergy()
    {
        _isRecharging = true;
        Debug.Log("Charging");
        yield return new WaitForSeconds(_spawnEnergyRechargeTime);
        _currentSpawnEnergy = _maxSpawnEnergy;
        _isRecharging = false;
    }

    IEnumerator Destroyed()
    {
        Destroy(this.GetComponent<CapsuleCollider2D>());
        yield return new WaitForSeconds(1f);
        int shardAmount = Mathf.RoundToInt(Random.Range(_shardSpawnMin, _shardSpawnMax));
        for (int amountSpawned = 0; amountSpawned <= shardAmount; amountSpawned++)
        {
            Instantiate(crystalShard, transform.position + new Vector3(Random.Range(-2,2), Random.Range(-2, 2),0), Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
