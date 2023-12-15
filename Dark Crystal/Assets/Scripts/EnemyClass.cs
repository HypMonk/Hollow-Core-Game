using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    
    [HideInInspector]
    public Enemy lightSucker, flyer, tank;
    public Dictionary<string, ParameterClass> baseEnemyParameters = new Dictionary<string, ParameterClass>();
    public Dictionary<string, ParameterClass> lightSuckerParameters = new Dictionary<string, ParameterClass>();
    public Dictionary<string, ParameterClass> tankParameters = new Dictionary<string, ParameterClass>();
    public Dictionary<string, ParameterClass> flyerParameters = new Dictionary<string, ParameterClass>();

    public abstract class Enemy
    {
        int _health, _knockbackStrength, _spawnCost, _spawnQuantityMin, _spawnQuantityMax, _homeScanRadius, _shardSpawnMin, _shardSpawnMax;
        float _attackSpeed, _moveSpeed, _attackRange, _attackDamage, _knockbackResistance, _speedDebuff, _idleMovementPauseTimeMin, _idleMovementPauseTimeMax, _idleMovementDistance;
        GameObject _targetObject, _crystalHome, _afflictingLight;
        bool _inLight;

        public Enemy(int Health, int KnockBackStrength, int SpawnCost, int SpawnQuantityMin, int SpawnQuantityMax, int HomeScanRadius, int ShardSpawnMin, int ShardSpawnMax,
            float AttackSpeed, float MoveSpeed, float AttackRange, float AttackDamage, float KnockBackResistance, float SpeedDebuff, float IdleMovementPauseTimeMin, float IdleMovementPauseTimeMax, float IdleMovementDistance)
        {
            _health = Health;
            _knockbackStrength = KnockBackStrength;
            _spawnCost = SpawnCost;
            _spawnQuantityMin = SpawnQuantityMin;
            _spawnQuantityMax = SpawnQuantityMax;
            _homeScanRadius = HomeScanRadius;
            _shardSpawnMin = ShardSpawnMin;
            _shardSpawnMax = ShardSpawnMax;
            _attackSpeed = AttackSpeed;
            _moveSpeed = MoveSpeed;
            _attackRange = AttackRange;
            _attackDamage = AttackDamage;
            _knockbackResistance = KnockBackResistance;
            _speedDebuff = SpeedDebuff;
            _idleMovementPauseTimeMin = IdleMovementPauseTimeMin;
            _idleMovementPauseTimeMin = IdleMovementPauseTimeMax;
            _idleMovementDistance = IdleMovementDistance;
        }

        public int Health { get { return _health; } set { _health = value; } }
        public int KnockBackStrength { get { return _knockbackStrength; } }
        public int SpawnCost { get { return _spawnCost; } }
        public int SpawnQuantityMin { get { return _spawnQuantityMin; } }
        public int SpawnQuantityMax { get { return _spawnQuantityMax; } }
        public int HomeScanRadius { get { return _homeScanRadius; } }
        public int ShardSpawnMin { get { return _shardSpawnMin; } }
        public int ShardSpawnMax { get { return _shardSpawnMax; } }
        public float AttackSpeed { get { return _attackSpeed; } }
        public float MoveSpeed { get { return _moveSpeed; } }
        public float AttackRange { get { return _attackRange; } }
        public float AttackDamage { get { return _attackDamage; } set { _attackDamage = value; } }
        public float KnockBackResistance { get { return _knockbackResistance; } }
        public float SpeedDebuff { get { return _speedDebuff; } set { _speedDebuff = value; } }
        public float IdleMovementPauseTimeMin { get { return _idleMovementPauseTimeMin; } }
        public float IdleMovementPauseTimeMax { get { return _idleMovementPauseTimeMax; } }
        public float IdleMovementDistance { get { return _idleMovementDistance; } }


        public GameObject TargetObject { get { return _targetObject; } set { _targetObject = value; } }
        public GameObject CrystalHome { get { return _crystalHome; } set { _crystalHome = value; } }
        public GameObject AfflictingLight { get { return _afflictingLight; } set { _afflictingLight = value; } }

        public bool InLight { get { return _inLight; } set { _inLight = value; } }
    }

    public class LightSucker : Enemy
    {
        float _suckStrength;

        public LightSucker(int Health, int KnockBackStrength, int SpawnCost, int SpawnQuantityMin, int SpawnQuantityMax, 
            int HomeScanRadius, int ShardSpawnMin, int ShardSpawnMax, float AttackSpeed, float MoveSpeed, float AttackRange, 
            float AttackDamage, float KnockBackResistance, float SpeedDebuff, float IdleMovementPauseTimeMin, 
            float IdleMovementPauseTimeMax, float IdleMovementDistance, float SuckStrength): 
            base(Health, KnockBackStrength, SpawnCost, SpawnQuantityMin, SpawnQuantityMax,
                HomeScanRadius, ShardSpawnMin, ShardSpawnMax, AttackSpeed, MoveSpeed, AttackRange, AttackDamage, 
                KnockBackResistance, SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax, IdleMovementDistance)
        {
            _suckStrength = SuckStrength;
        }

        public float SuckStrength { get { return _suckStrength; } }
    }

    public class Tank : Enemy
    {
        int _throwStrength;

        public Tank(int Health, int KnockBackStrength, int SpawnCost, int SpawnQuantityMin, int SpawnQuantityMax,
            int HomeScanRadius, int ShardSpawnMin, int ShardSpawnMax, float AttackSpeed, float MoveSpeed, float AttackRange, 
            float AttackDamage, float KnockBackResistance, float SpeedDebuff, float IdleMovementPauseTimeMin,
            float IdleMovementPauseTimeMax, float IdleMovementDistance, int ThrowStrength): 
            base(Health, KnockBackStrength, SpawnCost, SpawnQuantityMin, SpawnQuantityMax,
                HomeScanRadius, ShardSpawnMin, ShardSpawnMax, AttackSpeed, MoveSpeed, AttackRange, AttackDamage,
                KnockBackResistance, SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax, IdleMovementDistance)
        {
            _throwStrength = ThrowStrength;
        }
        public int ThrowStrength { get { return _throwStrength; } }
    }

    public class Flyer : Enemy
    {
        float _projectileSpeed, _projectileSpread;

        public Flyer(int Health, int KnockBackStrength, int SpawnCost, int SpawnQuantityMin, int SpawnQuantityMax,
            int HomeScanRadius, int ShardSpawnMin, int ShardSpawnMax, float AttackSpeed, float MoveSpeed, float AttackRange,
            float AttackDamage, float KnockBackResistance, float SpeedDebuff, float IdleMovementPauseTimeMin,
            float IdleMovementPauseTimeMax, float IdleMovementDistance, float ProjectileSpeed, float ProjectileSpread) : 
            base(Health, KnockBackStrength, SpawnCost, SpawnQuantityMin, SpawnQuantityMax,
                HomeScanRadius, ShardSpawnMin, ShardSpawnMax, AttackSpeed, MoveSpeed, AttackRange, AttackDamage,
                KnockBackResistance, SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax, IdleMovementDistance)
        {
            _projectileSpeed = ProjectileSpeed;
            _projectileSpread = ProjectileSpread;
        }
        public float ProjectileSpeed { get { return _projectileSpeed; } }
        public float ProjectileSpread { get { return _projectileSpread; } }
    }

    private void Awake()
    {
        UpdateParameters();
    }

    public void UpdateParameters()
    {
        baseEnemyParameters = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameParameters>().baseEnemyParameters;
        lightSuckerParameters = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameParameters>().lightSuckerParameters;
        tankParameters = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameParameters>().tankParameters;
        flyerParameters = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameParameters>().flyerParameters;
        BuildEnemy();
    }

    void BuildEnemy()
    {
        if (!GameManager.usingTestVariables)
        {
            int Health = (int)lightSuckerParameters["lsb1"].DefaultValue;
            int KnockBackStrength = (int)lightSuckerParameters["lsc1"].DefaultValue;
            int SpawnCost = (int)lightSuckerParameters["lsb2"].DefaultValue;
            int SpawnQuantityMin = (int)lightSuckerParameters["lsb3"].DefaultValue;
            int SpawnQuantityMax = (int)lightSuckerParameters["lsb4"].DefaultValue;
            int HomeScanRadius = (int)baseEnemyParameters["eb1"].DefaultValue;
            int ShardSpawnMin = (int)lightSuckerParameters["lsc7"].DefaultValue;
            int ShardSpawnMax = (int)lightSuckerParameters["lsc8"].DefaultValue;
            float AttackSpeed = lightSuckerParameters["lsc2"].DefaultValue;
            float MoveSpeed = lightSuckerParameters["lsm1"].DefaultValue;
            float AttackRange = lightSuckerParameters["lsc3"].DefaultValue;
            float AttackDamage = lightSuckerParameters["lsc4"].DefaultValue;
            float KnockBackResistance = lightSuckerParameters["lsc5"].DefaultValue;
            float SpeedDebuff = lightSuckerParameters["lsm2"].DefaultValue;
            float IdleMovementPauseTimeMin = baseEnemyParameters["eb2"].DefaultValue;
            float IdleMovementPauseTimeMax = baseEnemyParameters["eb3"].DefaultValue;
            float IdleMovementDistance = baseEnemyParameters["eb4"].DefaultValue;
            float SuckStrength = lightSuckerParameters["lsc6"].DefaultValue;

            lightSucker = new LightSucker(Health, KnockBackStrength, SpawnCost,
                SpawnQuantityMin, SpawnQuantityMax, HomeScanRadius, ShardSpawnMin, ShardSpawnMax,
                AttackSpeed, MoveSpeed, AttackRange, AttackDamage, KnockBackResistance,
                SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax, IdleMovementDistance, SuckStrength);

            
            Health = (int)flyerParameters["fb1"].DefaultValue;
            KnockBackStrength = (int)flyerParameters["fc1"].DefaultValue;
            SpawnCost = (int)flyerParameters["fb2"].DefaultValue;
            SpawnQuantityMin = (int)flyerParameters["fb3"].DefaultValue;
            SpawnQuantityMax = (int)flyerParameters["fb4"].DefaultValue;
            ShardSpawnMin = (int)flyerParameters["fc8"].DefaultValue;
            ShardSpawnMax = (int)flyerParameters["fc9"].DefaultValue;
            AttackSpeed = flyerParameters["fc2"].DefaultValue;
            MoveSpeed = flyerParameters["fm1"].DefaultValue;
            AttackRange = flyerParameters["fc3"].DefaultValue;
            AttackDamage = flyerParameters["fc4"].DefaultValue;
            KnockBackResistance = flyerParameters["fc5"].DefaultValue;
            SpeedDebuff = flyerParameters["fm2"].DefaultValue;
            float ProjectileSpeed = flyerParameters["fc6"].DefaultValue;
            float ProjectileSpread = flyerParameters["fc7"].DefaultValue;

            flyer = new Flyer(Health, KnockBackStrength, SpawnCost,
                SpawnQuantityMin, SpawnQuantityMax, HomeScanRadius, ShardSpawnMin, ShardSpawnMax,
                AttackSpeed, MoveSpeed, AttackRange, AttackDamage, KnockBackResistance,
                SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax,
                IdleMovementDistance, ProjectileSpeed, ProjectileSpread);


            Health = (int)tankParameters["tb1"].DefaultValue;
            KnockBackStrength = (int)tankParameters["tc1"].DefaultValue;
            SpawnCost = (int)tankParameters["tb2"].DefaultValue;
            SpawnQuantityMin = (int)tankParameters["tb3"].DefaultValue;
            SpawnQuantityMax = (int)tankParameters["tb4"].DefaultValue;
            ShardSpawnMin = (int)tankParameters["tc7"].DefaultValue;
            ShardSpawnMax = (int)tankParameters["tc8"].DefaultValue;
            AttackSpeed = tankParameters["tc2"].DefaultValue;
            MoveSpeed = tankParameters["tm1"].DefaultValue;
            AttackRange = tankParameters["tc3"].DefaultValue;
            AttackDamage = tankParameters["tc4"].DefaultValue;
            KnockBackResistance = tankParameters["tc5"].DefaultValue;
            SpeedDebuff = tankParameters["tm2"].DefaultValue;
            int ThrowStrength = (int)tankParameters["tc6"].DefaultValue;

            tank = new Tank(Health, KnockBackStrength, SpawnCost,
                SpawnQuantityMin, SpawnQuantityMax, HomeScanRadius, ShardSpawnMin, ShardSpawnMax,
                AttackSpeed, MoveSpeed, AttackRange, AttackDamage, KnockBackResistance,
                SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax, IdleMovementDistance, ThrowStrength);
        } else
        {
            int Health = (int)lightSuckerParameters["lsb1"].TestValue != -1 ? (int)lightSuckerParameters["lsb1"].TestValue : (int)lightSuckerParameters["lsb1"].DefaultValue;
            int KnockBackStrength = (int)lightSuckerParameters["lsc1"].TestValue != -1 ? (int)lightSuckerParameters["lsc1"].TestValue : (int)lightSuckerParameters["lsc1"].DefaultValue;
            int SpawnCost = (int)lightSuckerParameters["lsb2"].TestValue != -1 ? (int)lightSuckerParameters["lsb2"].TestValue : (int)lightSuckerParameters["lsb2"].DefaultValue;
            int SpawnQuantityMin = (int)lightSuckerParameters["lsb3"].TestValue != -1 ? (int)lightSuckerParameters["lsb3"].TestValue : (int)lightSuckerParameters["lsb3"].DefaultValue;
            int SpawnQuantityMax = (int)lightSuckerParameters["lsb4"].TestValue != -1 ? (int)lightSuckerParameters["lsb4"].TestValue : (int)lightSuckerParameters["lsb4"].DefaultValue;
            int HomeScanRadius = (int)baseEnemyParameters["eb1"].TestValue != -1 ? (int)baseEnemyParameters["eb1"].TestValue : (int)baseEnemyParameters["eb1"].DefaultValue;
            int ShardSpawnMin = (int)lightSuckerParameters["lsc7"].TestValue != -1 ? (int)lightSuckerParameters["lsc7"].TestValue : (int)lightSuckerParameters["lsc7"].DefaultValue;
            int ShardSpawnMax = (int)lightSuckerParameters["lsc8"].TestValue != -1 ? (int)lightSuckerParameters["lsc8"].TestValue : (int)lightSuckerParameters["lsc8"].DefaultValue;
            float AttackSpeed = lightSuckerParameters["lsc2"].TestValue != -1 ? lightSuckerParameters["lsc2"].TestValue : lightSuckerParameters["lsc2"].DefaultValue;
            float MoveSpeed = lightSuckerParameters["lsm1"].TestValue != -1 ? lightSuckerParameters["lsm1"].TestValue : lightSuckerParameters["lsm1"].DefaultValue;
            float AttackRange = lightSuckerParameters["lsc3"].TestValue != -1 ? lightSuckerParameters["lsc3"].TestValue : lightSuckerParameters["lsc3"].DefaultValue;
            float AttackDamage = lightSuckerParameters["lsc4"].TestValue != -1 ? lightSuckerParameters["lsc4"].TestValue : lightSuckerParameters["lsc4"].DefaultValue;
            float KnockBackResistance = lightSuckerParameters["lsc5"].TestValue != -1 ? lightSuckerParameters["lsc5"].TestValue : lightSuckerParameters["lsc5"].DefaultValue;
            float SpeedDebuff = lightSuckerParameters["lsm2"].TestValue != -1 ? lightSuckerParameters["lsm2"].TestValue : lightSuckerParameters["lsm2"].DefaultValue;
            float IdleMovementPauseTimeMin = baseEnemyParameters["eb2"].TestValue != -1 ? baseEnemyParameters["eb2"].TestValue : baseEnemyParameters["eb2"].DefaultValue;
            float IdleMovementPauseTimeMax = baseEnemyParameters["eb3"].TestValue != -1 ? baseEnemyParameters["eb3"].TestValue : baseEnemyParameters["eb3"].DefaultValue;
            float IdleMovementDistance = baseEnemyParameters["eb4"].TestValue != -1 ? baseEnemyParameters["eb4"].TestValue : baseEnemyParameters["eb4"].DefaultValue;
            float SuckStrength = lightSuckerParameters["lsc6"].TestValue != -1 ? lightSuckerParameters["lsc6"].TestValue : lightSuckerParameters["lsc6"].DefaultValue;

            lightSucker = new LightSucker(Health, KnockBackStrength, SpawnCost,
                SpawnQuantityMin, SpawnQuantityMax, HomeScanRadius, ShardSpawnMin, ShardSpawnMax,
                AttackSpeed, MoveSpeed, AttackRange, AttackDamage, KnockBackResistance,
                SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax, IdleMovementDistance, SuckStrength);


            Health = (int)flyerParameters["fb1"].TestValue != -1 ? (int)flyerParameters["fb1"].TestValue : (int)flyerParameters["fb1"].DefaultValue;
            KnockBackStrength = (int)flyerParameters["fc1"].TestValue != -1 ? (int)flyerParameters["fc1"].TestValue : (int)flyerParameters["fc1"].DefaultValue;
            SpawnCost = (int)flyerParameters["fb2"].TestValue != -1 ? (int)flyerParameters["fb2"].TestValue : (int)flyerParameters["fb2"].DefaultValue;
            SpawnQuantityMin = (int)flyerParameters["fb3"].TestValue != -1 ? (int)flyerParameters["fb3"].TestValue : (int)flyerParameters["fb3"].DefaultValue;
            SpawnQuantityMax = (int)flyerParameters["fb4"].TestValue != -1 ? (int)flyerParameters["fb4"].TestValue : (int)flyerParameters["fb4"].DefaultValue;
            ShardSpawnMin = (int)flyerParameters["fc8"].TestValue != -1 ? (int)flyerParameters["fc8"].TestValue : (int)flyerParameters["fc8"].DefaultValue;
            ShardSpawnMax = (int)flyerParameters["fc9"].TestValue != -1 ? (int)flyerParameters["fc9"].TestValue : (int)flyerParameters["fc9"].DefaultValue;
            AttackSpeed = flyerParameters["fc2"].TestValue != -1 ? flyerParameters["fc2"].TestValue : flyerParameters["fc2"].DefaultValue;
            MoveSpeed = flyerParameters["fm1"].TestValue != -1 ? flyerParameters["fm1"].TestValue : flyerParameters["fm1"].DefaultValue;
            AttackRange = flyerParameters["fc3"].TestValue != -1 ? flyerParameters["fc3"].TestValue : flyerParameters["fc3"].DefaultValue;
            AttackDamage = flyerParameters["fc4"].TestValue != -1 ? flyerParameters["fc4"].TestValue : flyerParameters["fc4"].DefaultValue;
            KnockBackResistance = flyerParameters["fc5"].TestValue != -1 ? flyerParameters["fc5"].TestValue : flyerParameters["fc5"].DefaultValue;
            SpeedDebuff = flyerParameters["fm2"].TestValue != -1 ? flyerParameters["fm2"].TestValue : flyerParameters["fm2"].DefaultValue;
            float ProjectileSpeed = flyerParameters["fc6"].TestValue != -1 ? flyerParameters["fc6"].TestValue : flyerParameters["fc6"].DefaultValue;
            float ProjectileSpread = flyerParameters["fc7"].TestValue != -1 ? flyerParameters["fc7"].TestValue : flyerParameters["fc7"].DefaultValue;

            flyer = new Flyer(Health, KnockBackStrength, SpawnCost,
                SpawnQuantityMin, SpawnQuantityMax, HomeScanRadius, ShardSpawnMin, ShardSpawnMax,
                AttackSpeed, MoveSpeed, AttackRange, AttackDamage, KnockBackResistance,
                SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax,
                IdleMovementDistance, ProjectileSpeed, ProjectileSpread);


            Health = (int)tankParameters["tb1"].TestValue != -1 ? (int)tankParameters["tb1"].TestValue : (int)tankParameters["tb1"].DefaultValue;
            KnockBackStrength = (int)tankParameters["tc1"].TestValue != -1 ? (int)tankParameters["tc1"].TestValue : (int)tankParameters["tc1"].DefaultValue;
            SpawnCost = (int)tankParameters["tb2"].TestValue != -1 ? (int)tankParameters["tb2"].TestValue : (int)tankParameters["tb2"].DefaultValue;
            SpawnQuantityMin = (int)tankParameters["tb3"].TestValue != -1 ? (int)tankParameters["tb3"].TestValue : (int)tankParameters["tb3"].DefaultValue;
            SpawnQuantityMax = (int)tankParameters["tb4"].TestValue != -1 ? (int)tankParameters["tb4"].TestValue : (int)tankParameters["tb4"].DefaultValue;
            ShardSpawnMin = (int)tankParameters["tc7"].TestValue != -1 ? (int)tankParameters["tc7"].TestValue : (int)tankParameters["tc7"].DefaultValue;
            ShardSpawnMax = (int)tankParameters["tc8"].TestValue != -1 ? (int)tankParameters["tc8"].TestValue : (int)tankParameters["tc8"].DefaultValue;
            AttackSpeed = tankParameters["tc2"].TestValue != -1 ? tankParameters["tc2"].TestValue : tankParameters["tc2"].DefaultValue;
            MoveSpeed = tankParameters["tm1"].TestValue != -1 ? tankParameters["tm1"].TestValue : tankParameters["tm1"].DefaultValue;
            AttackRange = tankParameters["tc3"].TestValue != -1 ? tankParameters["tc3"].TestValue : tankParameters["tc3"].DefaultValue;
            AttackDamage = tankParameters["tc4"].TestValue != -1 ? tankParameters["tc4"].TestValue : tankParameters["tc4"].DefaultValue;
            KnockBackResistance = tankParameters["tc5"].TestValue != -1 ? tankParameters["tc5"].TestValue : tankParameters["tc5"].DefaultValue;
            SpeedDebuff = tankParameters["tm2"].TestValue != -1 ? tankParameters["tm2"].TestValue : tankParameters["tm2"].DefaultValue;
            int ThrowStrength = (int)tankParameters["tc6"].TestValue != -1 ? (int)tankParameters["tc6"].TestValue : (int)tankParameters["tc6"].DefaultValue;

            tank = new Tank(Health, KnockBackStrength, SpawnCost,
                SpawnQuantityMin, SpawnQuantityMax, HomeScanRadius, ShardSpawnMin, ShardSpawnMax,
                AttackSpeed, MoveSpeed, AttackRange, AttackDamage, KnockBackResistance,
                SpeedDebuff, IdleMovementPauseTimeMin, IdleMovementPauseTimeMax, IdleMovementDistance, ThrowStrength);
        }
        
    }


}
