using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSuckerLogic : MonoBehaviour
{
    public EnemyClass.LightSucker _stats;
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
    

    Directive directive;
    Directive lastDirective;

    enum Directive { Idle, Guard, Special}

    // Start is called before the first frame update
    void Start()
    {
        _stats = (EnemyClass.LightSucker)GetComponent<EnemyClass>().lightSucker;
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

                Guard();
                break;
            case Directive.Special:
                if (lastDirective != directive)
                {
                    obstacleDetector.layerMask = commonObstaclesLayerMask | pitfallLayerMask;
                    obstacleDetector.layerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.obstaclesLayerMask = commonObstaclesLayerMask | pitfallLayerMask | playerLayerMask | lightLayerMask | _stats.TargetObject.layer;

                    targetDetector.targetLayerMask = _stats.TargetObject.layer;

                    targetDetector.obstaclesLayerMask &= ~(1 << ignoreLayerMask);
                    targetDetector.targetLayerMask &= ~(1 << ignoreLayerMask);
                }

                AttackLight();
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

        //if Home Crystal is hurt target culprit
        //if Crystal is in Light Suck light from culprit
        //avoid light
        //guard by getting inbetween player and crystal
        //attack the player

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

        if (_stats.CrystalHome.GetComponent<DarkCrystal>().InLight)
        {
            _stats.TargetObject = _stats.CrystalHome.GetComponent<DarkCrystal>().AfflictingLight;
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

        if (_stats.InLight)
        {
            _stats.TargetObject = _stats.AfflictingLight;
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

        if (_stats.CrystalHome.GetComponent<DarkCrystal>().IsThreatened)
        {
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

        if (_stats.CrystalHome.GetComponent<DarkCrystal>().InDanger)
        {
            _stats.TargetObject = GameObject.FindGameObjectWithTag("Player");
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

    void Guard()
    {
        _stats.TargetObject = GameObject.FindGameObjectWithTag("Player");
        _baseEnemyController.GuardMovement();
        
    }

    void AttackLight()
    {
        if (_stats.TargetObject == null) return;
        float distanceToTarget = Vector2.Distance(transform.position, _stats.TargetObject.transform.position);
        if (distanceToTarget > _stats.AttackRange)
        {
            _baseEnemyController.TargetedMovement();
        } else
        {
            if (_stats.TargetObject.tag == "Player")
            {
                if (_stats.TargetObject.GetComponent<PlayerController>().BeingCarried) return;
            }
            if (!IsInvoking())
            {
                InvokeRepeating("SuckLight", .1f, dcConversions.AttackSpeed(_stats.MoveSpeed, _stats.AttackSpeed));
            }
        }
        
    }

    void SuckLight()
    {
        if (_stats.TargetObject == null) return;
        if (_stats.TargetObject.tag == "Player")
        {
            if (_stats.TargetObject.GetComponent<PlayerController>().BeingCarried) return;
        }
        if (_stats.TargetObject.transform.parent == null) return;
        GameObject targetObjectHoldingLight = _stats.TargetObject.transform.parent.gameObject;
        string targetTag = targetObjectHoldingLight.tag;
        //Debug.Log(targetTag);
        switch (targetTag)
        {
            case "Player":
                targetObjectHoldingLight.GetComponent<PlayerController>().LoseLight(_stats.SuckStrength);
                break;
            case "Torch":
                targetObjectHoldingLight.GetComponent<TorchController>().LoseLight(_stats.SuckStrength);
                break;
        }
    }
}
