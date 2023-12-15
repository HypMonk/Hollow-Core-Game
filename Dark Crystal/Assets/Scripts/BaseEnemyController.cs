using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
    [HideInInspector]
    public EnemyClass.Enemy _stats;

    [HideInInspector]
    public GameObject homeCrystal;

    [SerializeField]
    GameObject _crystalShardPrefab;

    [SerializeField]
    SpriteRenderer _spriteRenderer;

    [SerializeField]
    List<AIMovementDetector> detectors;

    [SerializeField]
    List<AISteeringBehavior> steeringBehaviors;

    [SerializeField]
    AIContextSolver movDirSolver;

    [SerializeField]
    AIMovementData aiData;

    [SerializeField]
    Rigidbody2D rb2D;

    float detectionDelay = .05f, aiUpdateDelay = .06f;

    bool idleTargetReached = true;
    bool movePaused = false;
    Vector2 idleTarget;

    public Vector2 targetLocationOffset;
    public float randomLimitsOffset;

    float facingAngle;

    private void Start()
    {
        if (_stats != null)
        {
            _stats.CrystalHome = homeCrystal;
        }

        //set a random offset for each enemy
        targetLocationOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomLimitsOffset = Random.Range(-2, 2);

        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void Update()
    {
        _stats.InLight = CheckInLight();
        _spriteRenderer.enabled = _stats.InLight;

    }

    public void IdleMovement()
    {
        aiData.freeMovement = true;

        //Determine if we reached our target
        if (aiData.targetPosition == Vector2.zero) idleTargetReached = true;

        //If we Reached target Select a new one
        if (idleTargetReached && !movePaused)
        {
            StartCoroutine(ChangeDestination());
        }

        //Checking if reached target if yes reset
        float distanceToFreeTarget = Vector2.Distance(transform.position, aiData.targetPosition);
        if (distanceToFreeTarget < 1)
        {
            //if we reached target remove target to select new target
            aiData.targetPosition = Vector2.zero;

        } else // if no move to target
        {
            //modify speed based on distance
            if (distanceToFreeTarget < 3)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                movDirSolver.GetDirectionToMove(steeringBehaviors, aiData) + (Vector2)transform.position,
                (_stats.MoveSpeed / 3) * Time.deltaTime);
            } else
            {
                transform.position = Vector2.MoveTowards(transform.position,
                movDirSolver.GetDirectionToMove(steeringBehaviors, aiData) + (Vector2)transform.position,
                _stats.MoveSpeed * Time.deltaTime);
            }
        }
    }

    public void AvoidMovement()
    {
        aiData.freeMovement = false;

        if (_stats.TargetObject == null) return;
        aiData.target = _stats.TargetObject.transform;

        float distanceToTarget = Vector2.Distance(transform.position, aiData.target.position);


        //modify speed based on distance
        if (distanceToTarget < _stats.AttackRange + 2)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            -movDirSolver.GetDirectionToMove(steeringBehaviors, aiData) + (Vector2)transform.position,
            _stats.MoveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,
            -movDirSolver.GetDirectionToMove(steeringBehaviors, aiData) + (Vector2)transform.position,
            (_stats.MoveSpeed / 2) * Time.deltaTime);
        }
    }

    public void TargetedMovement()
    {
        aiData.freeMovement = false;

        if (_stats.TargetObject == null) return;
        aiData.target = _stats.TargetObject.transform;

        float distanceToTarget = Vector2.Distance(transform.position, aiData.target.position);

        //modify speed based on distance
        if (distanceToTarget < _stats.AttackRange + 2)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            movDirSolver.GetDirectionToMove(steeringBehaviors, aiData) + (Vector2)transform.position,
            (_stats.MoveSpeed / 2) * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,
            movDirSolver.GetDirectionToMove(steeringBehaviors, aiData) + (Vector2)transform.position,
            _stats.MoveSpeed * Time.deltaTime);
        }
    }

    public void GuardMovement()
    {
        aiData.freeMovement = true;

        /*facingAngle = Mathf.Atan2(_stats.TargetObject.transform.position.y, 
            _stats.TargetObject.transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, facingAngle));*/

        // set target = center between crystal and player clamped at Danger range of crystal
        if (_stats.TargetObject == null) return;
        aiData.targetPosition = Vector2.ClampMagnitude(Vector2.Lerp(homeCrystal.transform.position, _stats.TargetObject.transform.position, .5f), _stats.CrystalHome.GetComponent<DarkCrystal>().DangerRange + randomLimitsOffset);
        aiData.targetPosition += targetLocationOffset;

        Vector3 nextPosition;

        //modify speed based on distance
        float distanceToFreeTarget = Vector2.Distance(transform.position, aiData.targetPosition);
        if (distanceToFreeTarget < _stats.AttackRange + 2)
        {
            nextPosition = Vector2.MoveTowards(transform.position,
            movDirSolver.GetDirectionToMove(steeringBehaviors, aiData) + (Vector2)transform.position,
            (_stats.MoveSpeed / 3) * Time.deltaTime) - (Vector2)homeCrystal.transform.position;

            transform.position = homeCrystal.transform.position + nextPosition;
        }
        else
        {
            nextPosition = Vector2.MoveTowards(transform.position,
            movDirSolver.GetDirectionToMove(steeringBehaviors, aiData) + (Vector2)transform.position,
            (_stats.MoveSpeed) * Time.deltaTime) - (Vector2)homeCrystal.transform.position;

            transform.position = homeCrystal.transform.position + nextPosition;
        }
    }

    public void FindNewHome()
    {
        Collider2D newHomeCrystal = Physics2D.OverlapCircle(transform.position, _stats.HomeScanRadius, LayerMask.GetMask("Crystal"));
        if (newHomeCrystal != null)
        {
            _stats.CrystalHome = newHomeCrystal.gameObject;
        }
        else
        {
            Damage(99999);
        }

    }

    public bool CheckInLight()
    {

        Collider2D collision = Physics2D.OverlapCircle(transform.position, .3f, LayerMask.GetMask("Light"));
        if (collision != null)
        {
            _stats.AfflictingLight = collision.gameObject;
            return true;
        }
        return false;
    }

    public void Damage(int damage)
    {
        //Ignores all other effects
        _stats.Health -= damage;

        if (_stats.Health <= 0)
        {
            StartCoroutine(Destroyed());
        }
    }

    public void Damage(int damage, float knockBackStrength, Vector3 sourceLocation)
    {
        if (_stats.InLight)
        {
            _stats.Health -= damage;
        } else
        {
            _stats.Health -= Mathf.RoundToInt(damage / 2);
        }


        if (_stats.Health <= 0)
        {
            StartCoroutine(Destroyed());
        }


        KnockBack(knockBackStrength, sourceLocation);
        Debug.Log("Health of " + gameObject.name + ": " + _stats.Health);
    }

    void KnockBack(float knockBackStrength, Vector3 sourceLocation)
    {
        Vector3 dir = (sourceLocation - transform.position).normalized;
        float knockBack = knockBackStrength - _stats.KnockBackResistance;
        if (knockBack <= 0) { knockBack = 0; }
        GetComponent<Rigidbody2D>().AddForce(-dir * knockBack, ForceMode2D.Impulse);
        float knockBackDistance = knockBack / 4;
        StartCoroutine(KnockBackReset(knockBackDistance));
    }

    IEnumerator ChangeDestination()
    {
        
        movePaused = true;
        float pauseTime = Random.Range(_stats.IdleMovementPauseTimeMin, _stats.IdleMovementPauseTimeMax);
        yield return new WaitForSeconds(pauseTime);

        aiData.targetPosition = new Vector2(Random.Range(-(_stats.IdleMovementDistance), _stats.IdleMovementDistance), Random.Range(-(_stats.IdleMovementDistance), _stats.IdleMovementDistance)) + (Vector2)_stats.CrystalHome.transform.position;
        idleTargetReached = false;
        movePaused = false;

        /*facingAngle = Mathf.Atan2(aiData.targetPosition.y, aiData.targetPosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, facingAngle));*/
    }

    IEnumerator KnockBackReset(float knockBackDistance)
    {
        Vector3 lastLocation = transform.position;

        yield return new WaitWhile(() => Vector3.Distance(lastLocation, transform.position) < knockBackDistance);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    IEnumerator Destroyed()
    {
        Destroy(this.GetComponent<CircleCollider2D>());
        yield return new WaitForSeconds(.5f);
        int shardAmount = Mathf.RoundToInt(Random.Range(_stats.ShardSpawnMin, _stats.ShardSpawnMax));
        for (int amountSpawned = 0; amountSpawned <= shardAmount; amountSpawned++)
        {
            Instantiate(_crystalShardPrefab, transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), Quaternion.identity);
        }

        if (this.gameObject.GetComponent<TankLogic>())
        {
            this.gameObject.GetComponent<TankLogic>().Drop();
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(KnockBackReset(.05f));
        }
    }

    void PerformDetection()
    {
        foreach(AIMovementDetector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    void PerformTargetlessDetection()
    {
        foreach (AIMovementDetector detector in detectors)
        {
            if (detector.name == "ObstacleDetector")
            {
                detector.Detect(aiData);
            }
        }
    }
}
