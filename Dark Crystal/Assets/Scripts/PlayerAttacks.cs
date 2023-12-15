using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField]
    GameObject lightMeleeObject, heavyMeleeObject, lightRangeObject, heavyRangeObject;
    [SerializeField]
    PlayerStats _stats;
    [HideInInspector]
    public PlayerAttack lightMeleeAttack, heavyMeleeAttack, lightRangeAttack, heavyRangeAttack;



    public class PlayerAttack
    {
        public float _attackSpeed, _staminaCost, _powerCost, _attackDamage, _AoESize, _AoERadius, _projectileSpeed, _knockBackStrength;
        public GameObject _projectile;
        public GameObject _swing;

        public PlayerAttack(float attackSpeed, float staminaCost, float powerCost, float attackDamage, float knockBackStrength, float AoESize, float AoERadius, GameObject swing)
        {
            _attackSpeed = attackSpeed;
            _staminaCost = staminaCost;
            _powerCost = powerCost;
            _attackDamage = attackDamage;
            _knockBackStrength = knockBackStrength;
            _AoESize = AoESize;
            _AoERadius = AoERadius;
            _swing = swing;
        }

        public PlayerAttack(float attackSpeed, float staminaCost, float powerCost, float attackDamage, float knockBackStrength, float projectileSpeed, GameObject projectile)
        {
            _attackSpeed = attackSpeed;
            _staminaCost = staminaCost;
            _powerCost = powerCost;
            _attackDamage = attackDamage;
            _knockBackStrength = knockBackStrength;
            _projectileSpeed = projectileSpeed;
            _projectile = projectile;
        }
    }

    private void Awake()
    {
        lightMeleeAttack = new PlayerAttack(_stats.LightMeleeAttackSpeed, _stats.LightMeleeAttackStaminaCost, 
            _stats.LightMeleeAttackLightLevelCost, _stats.LightMeleeAttackDamage, _stats.LightMeleeAttackKnockbackStrength,
            _stats.LightMeleeAttackAoESize, _stats.LightMeleeAttackAoERadius, lightMeleeObject);
        heavyMeleeAttack = new PlayerAttack(_stats.HeavyMeleeAttackSpeed, _stats.HeavyMeleeAttackStaminaCost,
            _stats.HeavyMeleeAttackLightLevelCost, _stats.HeavyMeleeAttackDamage, _stats.HeavyMeleeAttackKnockbackStrength,
            _stats.HeavyMeleeAttackAoESize, _stats.HeavyMeleeAttackAoERadius, heavyMeleeObject);
        lightRangeAttack = new PlayerAttack(_stats.LightRangedAttackSpeed, _stats.LightRangedAttackStaminaCost,
            _stats.LightRangedAttackLightLevelCost, _stats.LightRangedAttackDamage, _stats.LightRangedAttackKnockbackStrength,
            _stats.LightRangedAttackProjectileSpeed, lightRangeObject);
        heavyRangeAttack = new PlayerAttack(_stats.HeavyRangedAttackSpeed, _stats.HeavyRangedAttackStaminaCost,
            _stats.HeavyRangedAttackLightLevelCost, _stats.HeavyRangedAttackDamage, _stats.HeavyRangedAttackKnockbackStrength,
            _stats.HeavyRangedAttackProjectileSpeed, heavyRangeObject);
    }
}
