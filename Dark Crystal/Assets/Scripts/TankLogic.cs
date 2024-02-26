using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankLogic : MonoBehaviour
{
    public EnemyClass.Tank _stats;

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
    LayerMask commonObstaclesLayerMask, lightLayerMask, pitfallLayerMask, playerLayerMask, ignoreLayerMask;

    [SerializeField]
    Directive directive;
    Directive lastDirective;

    enum Directive { Idle, Avoid, Guard, Attack, Special }

    //special variables
    bool carrying = false;

    // Start is called before the first frame update
    void Start()
    {
        _stats = (EnemyClass.Tank)GetComponent<EnemyClass>().tank;
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
                    obstacleDetector.layerMask = commonObstaclesLayerMask | pitfallLayerMask | lightLayerMask;
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
                    obstacleDetector.layerMask = commonObstaclesLayerMask | pitfallLayerMask;
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

                _baseEnemyController.GuardMovement();
                break;
            case Directive.Avoid:
                if (lastDirective != directive)
                {
                    obstacleDetector.layerMask = commonObstaclesLayerMask | pitfallLayerMask | lightLayerMask;
                    obstacleDetector.layerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.obstaclesLayerMask = commonObstaclesLayerMask | _stats.TargetObject.layer;

                    targetDetector.targetLayerMask = _stats.TargetObject.layer;

                    targetDetector.obstaclesLayerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.targetLayerMask &= ~(1 << ignoreLayerMask);
                }

                _baseEnemyController.AvoidMovement();

                break;
            case Directive.Attack:
                if (lastDirective != directive)
                {
                    obstacleDetector.layerMask = commonObstaclesLayerMask | pitfallLayerMask;
                    obstacleDetector.layerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.obstaclesLayerMask = commonObstaclesLayerMask | playerLayerMask | pitfallLayerMask | _stats.TargetObject.layer;

                    targetDetector.targetLayerMask = _stats.TargetObject.layer;

                    targetDetector.obstaclesLayerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.targetLayerMask &= ~(1 << ignoreLayerMask);
                }

                AttackTarget();
                break;
            case Directive.Special:
                if (lastDirective != directive)
                {
                    obstacleDetector.layerMask = commonObstaclesLayerMask | pitfallLayerMask;
                    obstacleDetector.layerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.obstaclesLayerMask = commonObstaclesLayerMask | playerLayerMask | pitfallLayerMask | lightLayerMask | _stats.TargetObject.layer;

                    targetDetector.targetLayerMask = _stats.TargetObject.layer;

                    targetDetector.obstaclesLayerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.targetLayerMask &= ~(1 << ignoreLayerMask);
                }

                ThrowTargetSpecial();
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

        //if Home Crystal is hurt throw the target culprit
        //avoid light
        //guard by getting inbetween player and crystal
        //attack the player

        if (carrying)
        {
            directive = Directive.Special;
        }

        if (_stats.CrystalHome.GetComponent<DarkCrystal>().DamageSource != null)
        {
            _stats.TargetObject = _stats.CrystalHome.GetComponent<DarkCrystal>().DamageSource;
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

        if (_stats.CrystalHome.GetComponent<DarkCrystal>().InDanger)
        {
            //Debug.Log("Attack");
            _stats.TargetObject = GameObject.FindGameObjectWithTag("Player");
            directive = Directive.Attack;
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

        if (_stats.CrystalHome.GetComponent<DarkCrystal>().IsThreatened)
        {
            _stats.TargetObject = GameObject.FindGameObjectWithTag("Player");
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

        directive = Directive.Idle;
        if (lastDirective != directive)
        {
            if (IsInvoking())
            {
                CancelInvoke();
            }
        }

    }

    void ThrowTargetSpecial()
    {
        if (_stats.TargetObject.GetComponent<PlayerController>().BeingCarried && !carrying) return;

        float distanceToTarget = Vector2.Distance(transform.position, _stats.TargetObject.transform.position);
        if (distanceToTarget > _stats.AttackRange)
        {
            _baseEnemyController.TargetedMovement();
        }
        else
        {
            _stats.TargetObject.GetComponent<PlayerController>().PickedUp(this.gameObject);
            carrying = true;
            CarryOff();
        }
    }

    void CarryOff()
    {
        Vector2 directionToCarryOff = (_stats.TargetObject.transform.position - _stats.CrystalHome.transform.position).normalized;

        Vector2 destination = (directionToCarryOff * _stats.CrystalHome.GetComponent<DarkCrystal>().DangerRange) +
            (Vector2)_stats.CrystalHome.transform.position;

        transform.position = Vector2.MoveTowards(transform.position,
            destination, (_stats.MoveSpeed / _stats.SpeedDebuff) * Time.deltaTime);

        float distanceToDestination = Vector2.Distance(transform.position, destination);

        RaycastHit2D obstacleHit = 
            Physics2D.Raycast(transform.position, directionToCarryOff, 1.5f, commonObstaclesLayerMask | pitfallLayerMask);
        if (obstacleHit)
        {
            Throw((transform.position - _stats.CrystalHome.transform.position).normalized);
            return;
        }

        if (distanceToDestination < 1)
        {
            Throw((transform.position - _stats.CrystalHome.transform.position).normalized);
        }
    }

    void Throw(Vector2 direction)
    {
        _stats.TargetObject.GetComponent<PlayerController>().Dropped();
        _stats.TargetObject.GetComponent<PlayerController>().Thrown(_stats.ThrowStrength, -direction);
        _stats.CrystalHome.GetComponent<DarkCrystal>().DamageSource = null;
        carrying = false;
        StartCoroutine(ThrowVelocityCorrection());
    }

    //prevents thrower from reacting to the physics and being pushed
    IEnumerator ThrowVelocityCorrection()
    {
        yield return new WaitForSeconds(.1f);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void Drop()
    {
        if (carrying)
        {
            _stats.TargetObject.GetComponent<PlayerController>().Dropped();
            carrying = false;
        }
    }

    void AttackTarget()
    {
        if (_stats.TargetObject == null) return;
        if (_stats.TargetObject.tag == "Player")
        {
            if (_stats.TargetObject.GetComponent<PlayerController>().BeingCarried) return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, _stats.TargetObject.transform.position);
        if (distanceToTarget > _stats.AttackRange)
        {
            if (IsInvoking())
            {
                CancelInvoke();
            }

            _baseEnemyController.TargetedMovement();
        }
        else
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
        if (_stats.TargetObject.GetComponent<PlayerController>() == null) return;

        _stats.TargetObject.GetComponent<PlayerController>().Damage(_stats.AttackDamage, _stats.KnockBackStrength, transform.position);
    }
}
