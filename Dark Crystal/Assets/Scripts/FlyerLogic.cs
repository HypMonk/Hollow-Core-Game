using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerLogic : MonoBehaviour
{
    public EnemyClass.Flyer _stats;

    [SerializeField]
    GameObject flyerProjectilePrefab;

    [SerializeField]
    BaseEnemyController _baseEnemyController;

    DarkCrystalConversions dcConversions;

    [HideInInspector]
    public int spawnCost, spawnQuantityMin, spawnQuantityMax;

    [SerializeField]
    AIMovementObstacleDetector obstacleDetector;
    [SerializeField]
    AIMovementTargetDetector targetDetector;

    [SerializeField]
    LayerMask commonObstaclesLayerMask, lightLayerMask, playerLayerMask, ignoreLayerMask;


    Directive directive;
    Directive lastDirective;

    enum Directive { Idle, Avoid, Guard, Special }

    // Start is called before the first frame update
    void Start()
    {
        _stats = (EnemyClass.Flyer)GetComponent<EnemyClass>().flyer;
        _baseEnemyController = GetComponent<BaseEnemyController>();
        _baseEnemyController._stats = _stats;

        spawnCost = _stats.SpawnCost;
        spawnQuantityMin = _stats.SpawnQuantityMin;
        spawnQuantityMax = _stats.SpawnQuantityMax;

        dcConversions = new DarkCrystalConversions();

        directive = Directive.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        lastDirective = directive;
        ActionLogic();
    }

    private void FixedUpdate()
    {
        switch (directive)
        {
            case Directive.Idle:
                if (lastDirective != directive)
                {
                    obstacleDetector.layerMask = commonObstaclesLayerMask | lightLayerMask;
                    obstacleDetector.layerMask &= ~(1 << ignoreLayerMask);

                    if (targetDetector.targetLayerMask != 0)
                    {
                        targetDetector.targetLayerMask = 0;
                    }

                    if (targetDetector.obstaclesLayerMask != 0)
                    {
                        targetDetector.obstaclesLayerMask = 0;
                    }
                }

                _baseEnemyController.IdleMovement();
                break;
            case Directive.Guard:
                if (lastDirective != directive)
                {
                    obstacleDetector.layerMask = commonObstaclesLayerMask | lightLayerMask;
                    obstacleDetector.layerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.obstaclesLayerMask = commonObstaclesLayerMask | _stats.TargetObject.layer;

                    targetDetector.targetLayerMask = _stats.TargetObject.layer;

                    targetDetector.obstaclesLayerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.targetLayerMask &= ~(1 << ignoreLayerMask);
                }

                Approach();
                break;
            case Directive.Avoid:
                if (lastDirective != directive)
                {
                    obstacleDetector.layerMask = commonObstaclesLayerMask | lightLayerMask;
                    obstacleDetector.layerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.obstaclesLayerMask = commonObstaclesLayerMask | _stats.TargetObject.layer;

                    targetDetector.targetLayerMask = _stats.TargetObject.layer;

                    targetDetector.obstaclesLayerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.targetLayerMask &= ~(1 << ignoreLayerMask);
                }

                _baseEnemyController.AvoidMovement();

                break;
            case Directive.Special:
                if (lastDirective != directive)
                {
                    obstacleDetector.layerMask = commonObstaclesLayerMask;
                    obstacleDetector.layerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.obstaclesLayerMask = commonObstaclesLayerMask | playerLayerMask | lightLayerMask | _stats.TargetObject.layer;

                    targetDetector.targetLayerMask = _stats.TargetObject.layer;

                    targetDetector.obstaclesLayerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.targetLayerMask &= ~(1 << ignoreLayerMask);
                }

                RoundedFlight();
                break;
        }
    }

    void ActionLogic()
    {
        if (_stats.CrystalHome == null)
        {
            _baseEnemyController.FindNewHome();
            return;
        }

        //if Enemy is near Home Crystal get in range of enemy
        //if the Home Crystal is attacked; Attack target
        //avoid player
        //avoid light
        //get the player between them and their Home crystal and Attack

        //change later if desire other targets
        GameObject playerTemp = GameObject.FindGameObjectWithTag("Player");

        if (_stats.CrystalHome.GetComponent<DarkCrystal>().InDanger)
        {
            _stats.TargetObject = playerTemp;
            directive = Directive.Guard;
            if (lastDirective != directive)
            {
                if (IsInvoking())
                {
                    CancelInvoke();
                }
            }
            return;
        }

        if (_stats.InLight)
        {
            _stats.TargetObject = _stats.AfflictingLight;
            directive = Directive.Avoid;
            if (lastDirective != directive)
            {
                if (IsInvoking())
                {
                    CancelInvoke();
                }
            }
            return;
        }

        if (playerTemp != null)
        {
            if (Vector2.Distance(transform.position, playerTemp.transform.position) < _stats.AttackRange-2)
            {
                _stats.TargetObject = playerTemp;
                directive = Directive.Avoid;
                if (lastDirective != directive)
                {
                    if (IsInvoking())
                    {
                        CancelInvoke();
                    }
                }
                return;
            }
        }

        if (_stats.CrystalHome.GetComponent<DarkCrystal>().IsThreatened)
        {
            _stats.TargetObject = playerTemp;
            directive = Directive.Special;
            if (lastDirective != directive)
            {
                if (IsInvoking())
                {
                    CancelInvoke();
                }
            }
            return;
        }

        directive = Directive.Idle;
        if (lastDirective != directive)
        {
            if (IsInvoking())
            {
                CancelInvoke();
            }
        }

    }

    void Approach()
    {
        if (_stats.TargetObject == null) return;
        if (_stats.TargetObject.tag == "Player")
        {
            if (_stats.TargetObject.GetComponent<PlayerController>().BeingCarried) return;
        }
        float distanceToTarget = Vector2.Distance(transform.position, _stats.TargetObject.transform.position);
        if (distanceToTarget > _stats.AttackRange - 1)
        {
            _baseEnemyController.TargetedMovement();
        } else
        {
            _baseEnemyController.AvoidMovement();
        }
        
        if(distanceToTarget <= _stats.AttackRange)
        {

            if (!IsInvoking())
            {
                InvokeRepeating("Attack", .1f, dcConversions.AttackSpeed(_stats.MoveSpeed, _stats.AttackSpeed));
            }
        }
    }

    void Attack()
    {
        if (_stats.TargetObject == null) return;
        Vector2 attackDirection = (this.transform.position - _stats.TargetObject.transform.position);
        attackDirection.x += Random.Range(-(_stats.ProjectileSpread), _stats.ProjectileSpread);
        attackDirection.y += Random.Range(-(_stats.ProjectileSpread), _stats.ProjectileSpread);
        attackDirection = attackDirection.normalized;
        GameObject projectileTemp = Instantiate(flyerProjectilePrefab, transform.position, Quaternion.identity);
        projectileTemp.GetComponent<EnemyProjectileController>().directionToTarget = -attackDirection;
        projectileTemp.GetComponent<EnemyProjectileController>().speed = _stats.ProjectileSpeed;
        projectileTemp.GetComponent<EnemyProjectileController>().damage = _stats.AttackDamage;
        projectileTemp.GetComponent<EnemyProjectileController>().knockback = _stats.KnockBackStrength;
        projectileTemp.GetComponent<EnemyProjectileController>().sourceTransform = transform;
    }

    void RoundedFlight()
    {
        if (_stats.TargetObject == null) return;
        Vector2 directionToTargetFromCrystal = (_stats.TargetObject.transform.position - _stats.CrystalHome.transform.position).normalized;

        Vector2 destination = ((directionToTargetFromCrystal * (_stats.AttackRange)) + (Vector2)_stats.TargetObject.transform.position);

        destination += _baseEnemyController.targetLocationOffset;

        Vector3 nextPosition;

        float distanceToTarget = Vector2.Distance(transform.position, _stats.TargetObject.transform.position);
        //modify speed based on distance
        if (distanceToTarget < _stats.AttackRange + 2)
        {
            nextPosition = Vector2.MoveTowards(transform.position,
            destination, (_stats.MoveSpeed / 2) * Time.deltaTime) - (Vector2)_stats.CrystalHome.transform.position;

            transform.position = (Vector2)_stats.CrystalHome.transform.position + 
                Vector2.ClampMagnitude(nextPosition, 
                (_stats.CrystalHome.GetComponent<DarkCrystal>().ThreatRange) + _baseEnemyController.randomLimitsOffset);
        }
        else
        {
            nextPosition = Vector2.MoveTowards(transform.position,
            destination, (_stats.MoveSpeed) * Time.deltaTime) - (Vector2)_stats.CrystalHome.transform.position;

            transform.position = (Vector2)_stats.CrystalHome.transform.position +
                Vector2.ClampMagnitude(nextPosition,
                (_stats.CrystalHome.GetComponent<DarkCrystal>().ThreatRange) + _baseEnemyController.randomLimitsOffset);
        }
    }


}
