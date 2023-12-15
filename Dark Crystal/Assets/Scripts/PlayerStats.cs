using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    Dictionary<string, ParameterClass> playerParameters = new Dictionary<string, ParameterClass>();

    // Holds all of Players Information
    //Player Base Stats
    float _maxHealth, _maxStamina, _maxLightLevel, _staminaRechargeRate, _staminaRechargeDelay, _lightLevelRechargeRate;

    //Player Movement Stats
    float _speed, _slideSpeed, _slideDistance, _slideStaminaCost;

    //Player Interaction Stats
    float _torchLightAnimationTime, _torchPickUpAnimationTime, _itemDropAnimationTime, _carryingSpeedDebuff;

    //Player Combat Stats
    float _knockBackResistance;

    //Player Attack Stats
    float _lightMeleeAttackStaminaCost, _lightMeleeAttackLightLevelCost, _lightMeleeAttackAnimationTime, _lightMeleeAttackSpeed,
        _lightMeleeAttackDamage, _lightMeleeAttackKnockbackStrength, _lightMeleeAttackAoERadius, _lightMeleeAttackAoESize;
    float _heavyMeleeAttackStaminaCost, _heavyMeleeAttackLightLevelCost, _heavyMeleeAttackAnimationTime, _heavyMeleeAttackSpeed,
        _heavyMeleeAttackDamage, _heavyMeleeAttackKnockbackStrength, _heavyMeleeAttackAoERadius, _heavyMeleeAttackAoESize;
    float _lightRangedAttackStaminaCost, _lightRangedAttackLightLevelCost, _lightRangedAttackAnimationTime,
        _lightRangedAttackSpeed, _lightRangedAttackDamage, _lightRangedAttackKnockbackStrength, _lightRangedAttackProjectileSpeed;
    float _heavyRangedAttackStaminaCost, _heavyRangedAttackLightLevelCost, _heavyRangedAttackAnimationTime,
        _heavyRangedAttackSpeed, _heavyRangedAttackDamage, _heavyRangedAttackKnockbackStrength, _heavyRangedAttackProjectileSpeed;


    //RunTime Stats
    float _currentHealth, _currentStamina, _currentLightLevel, _speedDebuff;

    private void Awake()
    {
        playerParameters = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameParameters>().playerParameters;
        UpdateStats();

        _currentHealth = _maxHealth;
        _currentStamina = _maxStamina;
        _currentLightLevel = _maxLightLevel;
    }

    public void UpdateStats()
    {
        if (!GameManager.usingTestVariables)
        {
            _maxHealth = playerParameters["pb1"].DefaultValue;
            _maxStamina = playerParameters["pb2"].DefaultValue;
            _maxLightLevel = playerParameters["pb3"].DefaultValue;
            _staminaRechargeRate = playerParameters["pb4"].DefaultValue;
            _staminaRechargeDelay = playerParameters["pb5"].DefaultValue;
            _lightLevelRechargeRate = playerParameters["pb6"].DefaultValue;

            _speed = playerParameters["pm1"].DefaultValue;
            _slideSpeed = playerParameters["pm2"].DefaultValue;
            _slideDistance = playerParameters["pm3"].DefaultValue;
            _slideStaminaCost = playerParameters["pm4"].DefaultValue;

            _torchLightAnimationTime = playerParameters["pi1"].DefaultValue;
            _torchPickUpAnimationTime = playerParameters["pi2"].DefaultValue;
            _itemDropAnimationTime = playerParameters["pi3"].DefaultValue;
            _carryingSpeedDebuff = playerParameters["pi4"].DefaultValue;

            _knockBackResistance = playerParameters["pc1"].DefaultValue;

            _lightMeleeAttackStaminaCost = playerParameters["pa_lm1"].DefaultValue;
            _lightMeleeAttackLightLevelCost = playerParameters["pa_lm2"].DefaultValue;
            _lightMeleeAttackAnimationTime = playerParameters["pa_lm3"].DefaultValue;
            _lightMeleeAttackSpeed = playerParameters["pa_lm4"].DefaultValue;
            _lightMeleeAttackDamage = playerParameters["pa_lm5"].DefaultValue;
            _lightMeleeAttackKnockbackStrength = playerParameters["pa_lm6"].DefaultValue;
            _lightMeleeAttackAoERadius = playerParameters["pa_lm7"].DefaultValue;
            _lightMeleeAttackAoESize = playerParameters["pa_lm8"].DefaultValue;

            _heavyMeleeAttackStaminaCost = playerParameters["pa_hm1"].DefaultValue;
            _heavyMeleeAttackLightLevelCost = playerParameters["pa_hm2"].DefaultValue;
            _heavyMeleeAttackAnimationTime = playerParameters["pa_hm3"].DefaultValue;
            _heavyMeleeAttackSpeed = playerParameters["pa_hm4"].DefaultValue;
            _heavyMeleeAttackDamage = playerParameters["pa_hm5"].DefaultValue;
            _heavyMeleeAttackKnockbackStrength = playerParameters["pa_hm6"].DefaultValue;
            _heavyMeleeAttackAoERadius = playerParameters["pa_hm7"].DefaultValue;
            _heavyMeleeAttackAoESize = playerParameters["pa_hm8"].DefaultValue;

            _lightRangedAttackStaminaCost = playerParameters["pa_lr1"].DefaultValue;
            _lightRangedAttackLightLevelCost = playerParameters["pa_lr2"].DefaultValue;
            _lightRangedAttackAnimationTime = playerParameters["pa_lr3"].DefaultValue;
            _lightRangedAttackSpeed = playerParameters["pa_lr4"].DefaultValue;
            _lightRangedAttackDamage = playerParameters["pa_lr5"].DefaultValue;
            _lightRangedAttackKnockbackStrength = playerParameters["pa_lr6"].DefaultValue;
            _lightRangedAttackProjectileSpeed = playerParameters["pa_lr7"].DefaultValue;

            _heavyRangedAttackStaminaCost = playerParameters["pa_hr1"].DefaultValue;
            _heavyRangedAttackLightLevelCost = playerParameters["pa_hr2"].DefaultValue;
            _heavyRangedAttackAnimationTime = playerParameters["pa_hr3"].DefaultValue;
            _heavyRangedAttackSpeed = playerParameters["pa_hr4"].DefaultValue;
            _heavyRangedAttackDamage = playerParameters["pa_hr5"].DefaultValue;
            _heavyRangedAttackKnockbackStrength = playerParameters["pa_hr6"].DefaultValue;
            _heavyRangedAttackProjectileSpeed = playerParameters["pa_hr7"].DefaultValue;

        } else
        {
            
            _maxHealth = playerParameters["pb1"].TestValue != -1 ? playerParameters["pb1"].TestValue : playerParameters["pb1"].DefaultValue;
            _maxStamina = playerParameters["pb2"].TestValue != -1 ? playerParameters["pb2"].TestValue : playerParameters["pb2"].DefaultValue;
            _maxLightLevel = playerParameters["pb3"].TestValue != -1 ? playerParameters["pb3"].TestValue : playerParameters["pb3"].DefaultValue;
            _staminaRechargeRate = playerParameters["pb4"].TestValue != -1 ? playerParameters["pb4"].TestValue : playerParameters["pb4"].DefaultValue;
            _staminaRechargeDelay = playerParameters["pb5"].TestValue != -1 ? playerParameters["pb5"].TestValue : playerParameters["pb5"].DefaultValue;
            _lightLevelRechargeRate = playerParameters["pb6"].TestValue != -1 ? playerParameters["pb6"].TestValue : playerParameters["pb6"].DefaultValue;

            _speed = playerParameters["pm1"].TestValue != -1 ? playerParameters["pm1"].TestValue : playerParameters["pm1"].DefaultValue;
            _slideSpeed = playerParameters["pm2"].TestValue != -1 ? playerParameters["pm2"].TestValue : playerParameters["pm2"].DefaultValue;
            _slideDistance = playerParameters["pm3"].TestValue != -1 ? playerParameters["pm3"].TestValue : playerParameters["pm3"].DefaultValue;
            _slideStaminaCost = playerParameters["pm4"].TestValue != -1 ? playerParameters["pm4"].TestValue : playerParameters["pm4"].DefaultValue;

            _torchLightAnimationTime = playerParameters["pi1"].TestValue != -1 ? playerParameters["pi1"].TestValue : playerParameters["pi1"].DefaultValue;
            _torchPickUpAnimationTime = playerParameters["pi2"].TestValue != -1 ? playerParameters["pi2"].TestValue : playerParameters["pi2"].DefaultValue;
            _itemDropAnimationTime = playerParameters["pi3"].TestValue != -1 ? playerParameters["pi3"].TestValue : playerParameters["pi3"].DefaultValue;
            _carryingSpeedDebuff = playerParameters["pi4"].TestValue != -1 ? playerParameters["pi4"].TestValue : playerParameters["pi4"].DefaultValue;

            _knockBackResistance = playerParameters["pc1"].TestValue != -1 ? playerParameters["pc1"].TestValue : playerParameters["pc1"].DefaultValue;

            _lightMeleeAttackStaminaCost = playerParameters["pa_lm1"].TestValue != -1 ? playerParameters["pa_lm1"].TestValue : playerParameters["pa_lm1"].DefaultValue;
            _lightMeleeAttackLightLevelCost = playerParameters["pa_lm2"].TestValue != -1 ? playerParameters["pa_lm2"].TestValue : playerParameters["pa_lm2"].DefaultValue;
            _lightMeleeAttackAnimationTime = playerParameters["pa_lm3"].TestValue != -1 ? playerParameters["pa_lm3"].TestValue : playerParameters["pa_lm3"].DefaultValue;
            _lightMeleeAttackSpeed = playerParameters["pa_lm4"].TestValue != -1 ? playerParameters["pa_lm4"].TestValue : playerParameters["pa_lm4"].DefaultValue;
            _lightMeleeAttackDamage = playerParameters["pa_lm5"].TestValue != -1 ? playerParameters["pa_lm5"].TestValue : playerParameters["pa_lm5"].DefaultValue;
            _lightMeleeAttackKnockbackStrength = playerParameters["pa_lm6"].TestValue != -1 ? playerParameters["pa_lm6"].TestValue : playerParameters["pa_lm6"].DefaultValue;
            _lightMeleeAttackAoERadius = playerParameters["pa_lm7"].TestValue != -1 ? playerParameters["pa_lm7"].TestValue : playerParameters["pa_lm7"].DefaultValue;
            _lightMeleeAttackAoESize = playerParameters["pa_lm8"].TestValue != -1 ? playerParameters["pa_lm8"].TestValue : playerParameters["pa_lm8"].DefaultValue;

            _heavyMeleeAttackStaminaCost = playerParameters["pa_hm1"].TestValue != -1 ? playerParameters["pa_hm1"].TestValue : playerParameters["pa_hm1"].DefaultValue;
            _heavyMeleeAttackLightLevelCost = playerParameters["pa_hm2"].TestValue != -1 ? playerParameters["pa_hm2"].TestValue : playerParameters["pa_hm2"].DefaultValue; ;
            _heavyMeleeAttackAnimationTime = playerParameters["pa_hm3"].TestValue != -1 ? playerParameters["pa_hm3"].TestValue : playerParameters["pa_hm3"].DefaultValue; ;
            _heavyMeleeAttackSpeed = playerParameters["pa_hm4"].TestValue != -1 ? playerParameters["pa_hm4"].TestValue : playerParameters["pa_hm4"].DefaultValue;
            _heavyMeleeAttackDamage = playerParameters["pa_hm5"].TestValue != -1 ? playerParameters["pa_hm5"].TestValue : playerParameters["pa_hm5"].DefaultValue;
            _heavyMeleeAttackKnockbackStrength = playerParameters["pa_hm6"].TestValue != -1 ? playerParameters["pa_hm6"].TestValue : playerParameters["pa_hm6"].DefaultValue;
            _heavyMeleeAttackAoERadius = playerParameters["pa_hm7"].TestValue != -1 ? playerParameters["pa_hm7"].TestValue : playerParameters["pa_hm7"].DefaultValue;
            _heavyMeleeAttackAoESize = playerParameters["pa_hm8"].TestValue != -1 ? playerParameters["pa_hm8"].TestValue : playerParameters["pa_hm8"].DefaultValue;

            _lightRangedAttackStaminaCost = playerParameters["pa_lr1"].TestValue != -1 ? playerParameters["pa_lr1"].TestValue : playerParameters["pa_lr1"].DefaultValue;
            _lightRangedAttackLightLevelCost = playerParameters["pa_lr2"].TestValue != -1 ? playerParameters["pa_lr2"].TestValue : playerParameters["pa_lr2"].DefaultValue;
            _lightRangedAttackAnimationTime = playerParameters["pa_lr3"].TestValue != -1 ? playerParameters["pa_lr3"].TestValue : playerParameters["pa_lr3"].DefaultValue;
            _lightRangedAttackSpeed = playerParameters["pa_lr4"].TestValue != -1 ? playerParameters["pa_lr4"].TestValue : playerParameters["pa_lr4"].DefaultValue;
            _lightRangedAttackDamage = playerParameters["pa_lr5"].TestValue != -1 ? playerParameters["pa_lr5"].TestValue : playerParameters["pa_lr5"].DefaultValue;
            _lightRangedAttackKnockbackStrength = playerParameters["pa_lr6"].TestValue != -1 ? playerParameters["pa_lr6"].TestValue : playerParameters["pa_lr6"].DefaultValue;
            _lightRangedAttackProjectileSpeed = playerParameters["pa_lr7"].TestValue != -1 ? playerParameters["pa_lr7"].TestValue : playerParameters["pa_lr7"].DefaultValue;

            _heavyRangedAttackStaminaCost = playerParameters["pa_hr1"].TestValue != -1 ? playerParameters["pa_hr1"].TestValue : playerParameters["pa_hr1"].DefaultValue;
            _heavyRangedAttackLightLevelCost = playerParameters["pa_hr2"].TestValue != -1 ? playerParameters["pa_hr2"].TestValue : playerParameters["pa_hr2"].DefaultValue;
            _heavyRangedAttackAnimationTime = playerParameters["pa_hr3"].TestValue != -1 ? playerParameters["pa_hr3"].TestValue : playerParameters["pa_hr3"].DefaultValue;
            _heavyRangedAttackSpeed = playerParameters["pa_hr4"].TestValue != -1 ? playerParameters["pa_hr4"].TestValue : playerParameters["pa_hr4"].DefaultValue;
            _heavyRangedAttackDamage = playerParameters["pa_hr5"].TestValue != -1 ? playerParameters["pa_hr5"].TestValue : playerParameters["pa_hr5"].DefaultValue;
            _heavyRangedAttackKnockbackStrength = playerParameters["pa_hr6"].TestValue != -1 ? playerParameters["pa_hr6"].TestValue : playerParameters["pa_hr6"].DefaultValue;
            _heavyRangedAttackProjectileSpeed = playerParameters["pa_hr7"].TestValue != -1 ? playerParameters["pa_hr7"].TestValue : playerParameters["pa_hr7"].DefaultValue;
        }
        
    }


    //Player Base Stats
    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    public float MaxStamina { get { return _maxStamina; } set { _maxStamina = value; } }

    public float MaxLightLevel { get { return _maxLightLevel; } set { _maxLightLevel = value; } }

    public float StaminaRechargeRate { get { return _staminaRechargeRate; } set { _staminaRechargeRate = value; } }

    public float StaminaRechargeDelay { get { return _staminaRechargeDelay; } set { _staminaRechargeDelay = value; } }

    public float LightLevelRechargeRate { get { return _lightLevelRechargeRate; } set { _lightLevelRechargeRate = value; } }


    //Player Movement Stats
    public float Speed { get { return _speed; } set { _speed = value; } }

    public float SlideSpeed { get { return _slideSpeed; } set { _slideSpeed = value; } }

    public float SlideDistance { get { return _slideDistance; } set { _slideDistance = value; } }

    public float SlideStaminaCost { get { return _slideStaminaCost; } set { _slideStaminaCost = value; } }


    //Player Interaction Stats
    public float TorchLightAnimationTime { get { return _torchLightAnimationTime; } }

    public float TorchPickUpAnimationTime { get { return _torchPickUpAnimationTime; } }

    public float ItemDropAnimationTime { get { return _itemDropAnimationTime; } }

    public float CarryingSpeedDebuff { get { return _carryingSpeedDebuff; } set { _carryingSpeedDebuff = value; } }


    //Player Combat Stats
    public float KnockBackResistance { get { return _knockBackResistance; } set { _knockBackResistance = value; } }


    //Player Attack Stats

    //Light Melee Attack
    public float LightMeleeAttackStaminaCost { get { return _lightMeleeAttackStaminaCost; } set { _lightMeleeAttackStaminaCost = value; } }

    public float LightMeleeAttackLightLevelCost { get { return _lightMeleeAttackLightLevelCost; } set { _lightMeleeAttackLightLevelCost = value; } }

    public float LightMeleeAttackAnimationTime { get { return _lightMeleeAttackAnimationTime; } }

    public float LightMeleeAttackSpeed { get { return _lightMeleeAttackSpeed; } set { _lightMeleeAttackSpeed = value; } }

    public float LightMeleeAttackDamage { get { return _lightMeleeAttackDamage; } set { _lightMeleeAttackDamage = value; } }

    public float LightMeleeAttackKnockbackStrength { get { return _lightMeleeAttackKnockbackStrength; } set { _lightMeleeAttackKnockbackStrength = value; } }

    public float LightMeleeAttackAoERadius { get { return _lightMeleeAttackAoERadius; } set { _lightMeleeAttackAoERadius = value; } }

    public float LightMeleeAttackAoESize { get { return _lightMeleeAttackAoESize; } set { _lightMeleeAttackAoESize = value; } }


    //Heavy Melee Attack
    public float HeavyMeleeAttackStaminaCost { get { return _heavyMeleeAttackStaminaCost; } set { _heavyMeleeAttackStaminaCost = value; } }

    public float HeavyMeleeAttackLightLevelCost { get { return _heavyMeleeAttackLightLevelCost; } set { _heavyMeleeAttackLightLevelCost = value; } }

    public float HeavyMeleeAttackAnimationTime { get { return _heavyMeleeAttackAnimationTime; } }

    public float HeavyMeleeAttackSpeed { get { return _heavyMeleeAttackSpeed; } set { _heavyMeleeAttackSpeed = value; } }

    public float HeavyMeleeAttackDamage { get { return _heavyMeleeAttackDamage; } set { _heavyMeleeAttackDamage = value; } }

    public float HeavyMeleeAttackKnockbackStrength { get { return _heavyMeleeAttackKnockbackStrength; } set { _heavyMeleeAttackKnockbackStrength = value; } }

    public float HeavyMeleeAttackAoERadius { get { return _heavyMeleeAttackAoERadius; } set { _heavyMeleeAttackAoERadius = value; } }

    public float HeavyMeleeAttackAoESize { get { return _heavyMeleeAttackAoESize; } set { _heavyMeleeAttackAoESize = value; } }


    //Light Ranged Attack
    public float LightRangedAttackStaminaCost { get { return _lightRangedAttackStaminaCost; } set { _lightRangedAttackStaminaCost = value; } }

    public float LightRangedAttackLightLevelCost { get { return _lightRangedAttackLightLevelCost; } set { _lightRangedAttackLightLevelCost = value; } }

    public float LightRangedAttackAnimationTime { get { return _lightRangedAttackAnimationTime; } }

    public float LightRangedAttackSpeed { get { return _lightRangedAttackSpeed; } set { _lightRangedAttackSpeed = value; } }

    public float LightRangedAttackDamage { get { return _lightRangedAttackDamage; } set { _lightRangedAttackDamage = value; } }

    public float LightRangedAttackKnockbackStrength { get { return _lightRangedAttackKnockbackStrength; } set { _lightRangedAttackKnockbackStrength = value; } }

    public float LightRangedAttackProjectileSpeed { get { return _lightRangedAttackProjectileSpeed; } set { _lightRangedAttackProjectileSpeed = value; } }


    //Heavy Ranged Attack
    public float HeavyRangedAttackStaminaCost { get { return _heavyRangedAttackStaminaCost; } set { _heavyRangedAttackStaminaCost = value; } }

    public float HeavyRangedAttackLightLevelCost { get { return _heavyRangedAttackLightLevelCost; } set { _heavyRangedAttackLightLevelCost = value; } }

    public float HeavyRangedAttackAnimationTime { get { return _heavyRangedAttackAnimationTime; } }

    public float HeavyRangedAttackSpeed { get { return _heavyRangedAttackSpeed; } set { _heavyRangedAttackSpeed = value; } }

    public float HeavyRangedAttackDamage { get { return _heavyRangedAttackDamage; } set { _heavyRangedAttackDamage = value; } }

    public float HeavyRangedAttackKnockbackStrength { get { return _heavyRangedAttackKnockbackStrength; } set { _heavyRangedAttackKnockbackStrength = value; } }

    public float HeavyRangedAttackProjectileSpeed { get { return _heavyRangedAttackProjectileSpeed; } set { _heavyRangedAttackProjectileSpeed = value; } }


    //RunTime Stats
    public float Health { get { return _currentHealth; } set { _currentHealth = value; } }

    public float Stamina { get { return _currentStamina; } set { _currentStamina = value; } }

    public float LightLevel { get { return _currentLightLevel; } set { _currentLightLevel = value; } }

    public float SpeedDebuff { get { return _speedDebuff; } set { _speedDebuff = value; } }


}
