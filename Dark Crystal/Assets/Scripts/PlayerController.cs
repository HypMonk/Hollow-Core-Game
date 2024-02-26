using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerStats _stats;
    [SerializeField]
    GameObject _attackPoint, _point;
    float attackPointMaxRadius = .65f;


    [SerializeField]
    PlayerAnimationController _playerAnimationController;
    
    //PlayerControlsTest _pControls;
    PlayerInput playerInput;
    InputAction move;
    InputAction look;
    InputAction interact;
    InputAction lightMelee;
    InputAction lightRange;
    InputAction heavyMelee;
    InputAction heavyRange;
    InputAction slide;
    
    public PlayerStates playerState;

    AudioManager audioManager;

    DarkCrystalConversions darkCrystalConversions;

    [SerializeField]
    GameObject lightMeleeObject, heavyMeleeObject, lightRangeObject, heavyRangeObject;

    Vector3 _savedAttackPointDirection;
    Vector2 _lastMovementDirection;
    Vector3 _attackPointDirection;

    bool isSliding = false;
    Vector3 slideDir;
    float slideTimer;

    bool isAttacking = false;
    float attackTime = -1;

    bool isInteracting = false;

    bool isStunned = false;
    float stunnedTimer;

    [HideInInspector]
    public bool isAtRiskofFalling = false;
    float fallGracePeriodSetTime = .2f;
    float fallGracePeriodTimer;

    bool beingCarried = false;
    GameObject carrier;

    bool isCarrying = false;
    GameObject itemCarried;

    float recordedStamina = 100;
    float staminaRechargeTimer = 0;

    private void Awake()
    {
        //_pControls = new PlayerControlsTest();
    }
/*
    private void OnEnable()
    {
        move = _pControls.Player.Move;
        move.Enable();

        look = _pControls.Player.Look;
        look.Enable();
        look.performed += LookInput;

        interact = _pControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;

        slide = _pControls.Player.Slide;
        slide.Enable();
        slide.performed += SlideTrigger;

        lightMelee = _pControls.Player.LightMelee;
        lightMelee.Enable();
        lightMelee.performed += LightMeleeAttack;

        lightRange = _pControls.Player.LightRange;
        lightRange.Enable();
        lightRange.performed += LightRangeAttack;

        heavyMelee = _pControls.Player.HeavyMelee;
        heavyMelee.Enable();
        heavyMelee.performed += HeavyMeleeAttack;

        heavyRange = _pControls.Player.HeavyRange;
        heavyRange.Enable();
        heavyRange.performed += HeavyRangeAttack;
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        interact.Disable();
        slide.Disable();
        lightMelee.Disable();
        lightRange.Disable();
        heavyMelee.Disable();
        heavyRange.Disable();
        
    }*/

    public bool BeingCarried { get { return beingCarried; } }

    private void Start()
    {
        darkCrystalConversions = new DarkCrystalConversions();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions["Move"];
        look = playerInput.actions["Look"];
        interact = playerInput.actions["Interact"];
        lightMelee = playerInput.actions["LightMelee"];
        lightRange = playerInput.actions["LightRange"];
        heavyMelee = playerInput.actions["HeavyMelee"];
        heavyRange = playerInput.actions["HeavyRange"];
        slide = playerInput.actions["Slide"];
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.isPaused) return;

        //Debug.Log("PlayerState: " + playerState);

        if (isAtRiskofFalling && playerState != PlayerStates.Falling)
        {
            fallGracePeriodTimer += Time.deltaTime;

            if (fallGracePeriodTimer >= fallGracePeriodSetTime)
            {
                playerState = PlayerStates.Falling;
                StartCoroutine(PitfallFalling());
            }

        } else
        {
            fallGracePeriodTimer = 0;
        }

        //prevent actions if stunned
        if (isStunned)
        {
            stunnedTimer += Time.deltaTime;
        }

        if (isStunned || beingCarried || playerState == PlayerStates.Falling) return;


        //Player Movement input
        Vector2 input = move.ReadValue<Vector2>();
        if (input != Vector2.zero) _lastMovementDirection = input.normalized;
        //if(move.ReadValue<Vector2>() != Vector2.zero) _lastMovementDirection = move.ReadValue<Vector2>().normalized;

        
        StaminaRecharge();

        //Prevent Movement if sliding
        if (isSliding) 
        {
            slideTimer += Time.deltaTime;
            return; 
        }
        
        //Prevent Stacking Attacks and movement during attack animations
        if (isAttacking)
        {
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }
            else
            {
                attackTime = -1;
                isAttacking = false;
            }
            return;
        }

        //Prevent Movement if Interacting
        if (isInteracting) { return; }

        //Player Looking
        if (playerInput.currentControlScheme != "Keyboard&Mouse")
        {
            // Using gamepad input
            Vector2 lookDirection = look.ReadValue<Vector2>();
            if (lookDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 90f;
                _attackPoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        else
        {
            // Using mouse input
            Vector3 screenMousePosition = Camera.main.ScreenToWorldPoint(look.ReadValue<Vector2>());
            Vector2 direction = (screenMousePosition - transform.position);

            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
                _attackPoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        if (interact.triggered) Interact();

        if (lightMelee.triggered)
        {
            if (_stats.Stamina - _stats.LightMeleeAttackStaminaCost < 0 || _stats.LightLevel - _stats.LightMeleeAttackLightLevelCost < 0) { return; }

            isAttacking = true;

            attackTime = darkCrystalConversions.AttackSpeed(_stats.Speed, _stats.LightMeleeAttackSpeed);
            LoseStamina(_stats.LightMeleeAttackStaminaCost);
            LoseLight(_stats.LightMeleeAttackLightLevelCost);

            audioManager.Play("Player Light Melee Swing", 0, Random.Range(1f, 1.2f));

            StartCoroutine(meleeAttack(lightMeleeObject, attackTime, _stats.LightMeleeAttackAoESize, _stats.LightMeleeAttackAoERadius,
                _stats.LightMeleeAttackDamage, _stats.LightMeleeAttackKnockbackStrength));
        }

        if (lightRange.triggered)
        {
            if (_stats.Stamina - _stats.LightRangedAttackStaminaCost < 0 || _stats.LightLevel - _stats.LightRangedAttackLightLevelCost < 0) { return; }

            isAttacking = true;

            attackTime = darkCrystalConversions.AttackSpeed(_stats.Speed, _stats.LightRangedAttackSpeed);
            LoseStamina(_stats.LightRangedAttackStaminaCost);
            LoseLight(_stats.LightRangedAttackLightLevelCost);

            StartCoroutine(RangeAttack(lightRangeObject, attackTime, _stats.LightRangedAttackProjectileSpeed, _stats.LightRangedAttackDamage,
                _stats.LightRangedAttackLightLevelCost, _stats.LightRangedAttackKnockbackStrength));
        }

        if (heavyMelee.triggered)
        {
            if (_stats.Stamina - _stats.HeavyMeleeAttackStaminaCost < 0 || _stats.LightLevel - _stats.HeavyMeleeAttackLightLevelCost < 0) { return; }

            isAttacking = true;

            attackTime = darkCrystalConversions.AttackSpeed(_stats.Speed, _stats.HeavyMeleeAttackSpeed);
            LoseStamina(_stats.HeavyMeleeAttackStaminaCost);
            LoseLight(_stats.HeavyMeleeAttackLightLevelCost);
            StartCoroutine(meleeAttack(heavyMeleeObject, attackTime, _stats.HeavyMeleeAttackAoESize, _stats.HeavyMeleeAttackAoERadius,
                _stats.HeavyMeleeAttackDamage, _stats.HeavyMeleeAttackKnockbackStrength));
        }

        if (heavyRange.triggered)
        {
            if (_stats.Stamina - _stats.HeavyRangedAttackStaminaCost < 0 || _stats.LightLevel - _stats.HeavyRangedAttackLightLevelCost < 0) { return; }

            isAttacking = true;

            attackTime = darkCrystalConversions.AttackSpeed(_stats.Speed, _stats.HeavyRangedAttackSpeed);
            LoseStamina(_stats.HeavyRangedAttackStaminaCost);
            LoseLight(_stats.HeavyRangedAttackLightLevelCost);
            StartCoroutine(RangeAttack(heavyRangeObject, attackTime, _stats.HeavyRangedAttackProjectileSpeed, _stats.HeavyRangedAttackDamage,
                _stats.HeavyRangedAttackLightLevelCost, _stats.HeavyRangedAttackKnockbackStrength));
        }

        if (slide.triggered)
        {
            if (_stats.Stamina - _stats.SlideStaminaCost < 0) { return; }

            isSliding = true;
            LoseStamina(_stats.SlideStaminaCost);
            Vector3 slideMovement = (playerInput.actions["Move"].ReadValue<Vector2>() * 100).normalized;
            //Vector3 slideMovement = (move.ReadValue<Vector2>() * 100).normalized;
            slideDir = slideMovement;
            _playerAnimationController.state = PlayerStates.Sliding;
            StartCoroutine(Slide());
        }

        transform.position = new Vector2(transform.position.x, transform.position.y) + input * (_stats.Speed - _stats.SpeedDebuff) * Time.deltaTime;
        //transform.position = new Vector2(transform.position.x, transform.position.y) + move.ReadValue<Vector2>() * (_stats.Speed - _stats.SpeedDebuff) * Time.deltaTime;
        
        UpdateState(input);
        //UpdateState(move.ReadValue<Vector2>());
        UpdateDirection(_attackPoint.transform.rotation.eulerAngles);
        
    }

    private void LateUpdate()
    {
        if (beingCarried)
        {
            transform.position = carrier.transform.position;
        }
    }

    public void PickedUp(GameObject Carrier)
    {
        carrier = Carrier;
        beingCarried = true;
    }

    public void Dropped()
    {
        beingCarried = false;
        carrier = null;
    }

    void Interact()
    {
        isInteracting = true;

        if (isCarrying)
        {
            if (itemCarried.tag == "Torch")
            {
                StartCoroutine(DropTorch());
            }
        }
        else
        {
            Collider2D interactable = Physics2D.OverlapCircle((Vector2)transform.position, 1, LayerMask.GetMask("Torch"));
            if (interactable != null)
            {
                if (interactable.tag == "Torch")
                {
                    if (interactable.GetComponent<TorchController>().torch.isStationary)
                    {
                        StartCoroutine(LightTorch(interactable.gameObject));
                    }
                    else
                    {
                        StartCoroutine(PickUpTorch(interactable.gameObject));
                    }
                }
            }
            else
            {
                isInteracting = false;
            }
        }

    }

    /*
    void LookInput(InputAction.CallbackContext context)
    {
        if (GameManager.isPaused) return;
        if (isInteracting || isAttacking || isSliding || isStunned || beingCarried || playerState == PlayerStates.Falling) return;


        //Player Looking
        if (context.control.device.name != "Mouse")
        {
            _attackPointDirection = look.ReadValue<Vector2>().normalized;
        }
        else
        {
            Vector3 screenMousePosition = Camera.main.ScreenToWorldPoint(look.ReadValue<Vector2>());

            Vector2 direction = screenMousePosition - transform.position;
            _attackPointDirection = direction.normalized;
        }
    }

    void LightMeleeAttack(InputAction.CallbackContext context)
    {

        if (GameManager.isPaused) return;
        if (isCarrying || isInteracting || isAttacking || isSliding || isStunned || beingCarried || playerState == PlayerStates.Falling) return;
        if (_stats.Stamina - _stats.LightMeleeAttackStaminaCost < 0 || _stats.LightLevel - _stats.LightMeleeAttackLightLevelCost < 0) { return; }

        isAttacking = true;
        
        attackTime = darkCrystalConversions.AttackSpeed(_stats.Speed, _stats.LightMeleeAttackSpeed);
        LoseStamina(_stats.LightMeleeAttackStaminaCost);
        LoseLight(_stats.LightMeleeAttackLightLevelCost);

        audioManager.Play("Player Light Melee Swing", 0, Random.Range(1f, 1.2f));

        StartCoroutine(meleeAttack(lightMeleeObject, attackTime, _stats.LightMeleeAttackAoESize, _stats.LightMeleeAttackAoERadius,
            _stats.LightMeleeAttackDamage, _stats.LightMeleeAttackKnockbackStrength));
    }

    void HeavyMeleeAttack(InputAction.CallbackContext context)
    {
        if (GameManager.isPaused) return;
        if (isCarrying || isInteracting || isAttacking || isSliding || isStunned || beingCarried || playerState == PlayerStates.Falling) return;

        if (_stats.Stamina - _stats.HeavyMeleeAttackStaminaCost < 0 || _stats.LightLevel - _stats.HeavyMeleeAttackLightLevelCost < 0) { return; }

        isAttacking = true;
        
        attackTime = darkCrystalConversions.AttackSpeed(_stats.Speed, _stats.HeavyMeleeAttackSpeed);
        LoseStamina(_stats.HeavyMeleeAttackStaminaCost);
        LoseLight(_stats.HeavyMeleeAttackLightLevelCost);
        StartCoroutine(meleeAttack(heavyMeleeObject, attackTime, _stats.HeavyMeleeAttackAoESize, _stats.HeavyMeleeAttackAoERadius,
            _stats.HeavyMeleeAttackDamage, _stats.HeavyMeleeAttackKnockbackStrength));
    }

    void LightRangeAttack(InputAction.CallbackContext context)
    {
        if (GameManager.isPaused) return;
        if (isCarrying || isInteracting || isAttacking || isSliding || isStunned || beingCarried || playerState == PlayerStates.Falling) return;

        if (_stats.Stamina - _stats.LightRangedAttackStaminaCost < 0 || _stats.LightLevel - _stats.LightRangedAttackLightLevelCost < 0) { return; }

        isAttacking = true;
        
        attackTime = darkCrystalConversions.AttackSpeed(_stats.Speed, _stats.LightRangedAttackSpeed);
        LoseStamina(_stats.LightRangedAttackStaminaCost);
        LoseLight(_stats.LightRangedAttackLightLevelCost);
        StartCoroutine(RangeAttack(lightRangeObject, attackTime, _stats.LightRangedAttackProjectileSpeed, _stats.LightRangedAttackDamage,
            _stats.LightRangedAttackLightLevelCost, _stats.LightRangedAttackKnockbackStrength));
    }

    void HeavyRangeAttack(InputAction.CallbackContext context) 
    {
        if (GameManager.isPaused) return;
        if (isCarrying || isInteracting || isAttacking || isSliding || isStunned || beingCarried || playerState == PlayerStates.Falling) return;

        if (_stats.Stamina - _stats.HeavyRangedAttackStaminaCost < 0 || _stats.LightLevel - _stats.HeavyRangedAttackLightLevelCost < 0) { return; }

        isAttacking = true;
        
        attackTime = darkCrystalConversions.AttackSpeed(_stats.Speed, _stats.HeavyRangedAttackSpeed);
        LoseStamina(_stats.HeavyRangedAttackStaminaCost);
        LoseLight(_stats.HeavyRangedAttackLightLevelCost);
        StartCoroutine(RangeAttack(heavyRangeObject, attackTime, _stats.HeavyRangedAttackProjectileSpeed, _stats.HeavyRangedAttackDamage,
            _stats.HeavyRangedAttackLightLevelCost, _stats.HeavyRangedAttackKnockbackStrength));
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (GameManager.isPaused) return;
        if (isInteracting || isAttacking || isSliding || isStunned || beingCarried || playerState == PlayerStates.Falling) return;
        isInteracting = true;

        if (isCarrying)
        {
            if (itemCarried.tag == "Torch")
            {
                StartCoroutine(DropTorch());
            }
        } else
        {
            Collider2D interactable = Physics2D.OverlapCircle((Vector2)transform.position, 1, LayerMask.GetMask("Torch"));
            if (interactable != null)
            {
                if (interactable.tag == "Torch")
                {
                    if (interactable.GetComponent<TorchController>().torch.isStationary)
                    {
                        StartCoroutine(LightTorch(interactable.gameObject));
                    }
                    else
                    {
                        StartCoroutine(PickUpTorch(interactable.gameObject));
                    }
                }
            }
            else
            {
                isInteracting = false;
            }
        }

    }
    */
    void UpdateState(Vector2 _movement)
    {
        if (_movement != Vector2.zero)
        {
            _playerAnimationController.state = PlayerStates.Walking;
        }
        else
        {
            _playerAnimationController.state = PlayerStates.Idle;
        }
    }

    void UpdateDirection(Vector3 _APD)
    {

        if (_APD.z >= 0 && _APD.z <= 180) //right
        {
            if (_APD.z < 85)
            {
                _playerAnimationController.direction = PlayerDirection.RDown;
            } else if (_APD.z >= 85 && _APD.z <= 95)
            {
                _playerAnimationController.direction = PlayerDirection.Right;
            } else
            {
                _playerAnimationController.direction = PlayerDirection.RUp;
            }
        } else
        {
            if (_APD.z > 280)
            {
                _playerAnimationController.direction = PlayerDirection.LDown;
            }
            else if (_APD.z <= 280 && _APD.z >= 270)
            {
                _playerAnimationController.direction = PlayerDirection.Left;
            }
            else
            {
                _playerAnimationController.direction = PlayerDirection.LUp;
            }
        }

        /*if (_APD.x > 0) //right
        {
            if (_APD.y > 0) //Up
            {
                _playerAnimationController.direction = PlayerDirection.RUp;
            } else if (_APD.y < 0)//Down
            {
                _playerAnimationController.direction = PlayerDirection.RDown;
            } else
            {
                _playerAnimationController.direction = PlayerDirection.Right;
            }
        } else if (_APD.x < 0)//Left
        {
            if (_APD.y > 0) //Up
            {
                _playerAnimationController.direction = PlayerDirection.LUp;
            }
            else if (_APD.y < 0) //Down
            {
                _playerAnimationController.direction = PlayerDirection.LDown;
            } else 
            {
                _playerAnimationController.direction = PlayerDirection.Left;
            }
        } else if (_APD.x == 0)
        {
            if (_APD.y > 0) //Up
            {
                _playerAnimationController.direction = PlayerDirection.LUp;
            }
            else if (_APD.y <= 0) //Down
            {
                _playerAnimationController.direction = PlayerDirection.LDown;
            }
        }*/
    }
    /*
    void SlideTrigger(InputAction.CallbackContext context)
    {
        if (GameManager.isPaused) return;
        if (isCarrying || isInteracting || isAttacking || isSliding || isStunned || beingCarried || playerState == PlayerStates.Falling) return;

        if (_stats.Stamina - _stats.SlideStaminaCost < 0) { return; }

        isSliding = true;
        LoseStamina(_stats.SlideStaminaCost);
        Vector3 slideMovement = (playerInput.actions["Move"].ReadValue<Vector2>() * 100).normalized;
        //Vector3 slideMovement = (move.ReadValue<Vector2>() * 100).normalized;
        slideDir = slideMovement;
        _playerAnimationController.state = PlayerStates.Sliding;
        StartCoroutine(Slide());
    }*/

    void StaminaRecharge()
    {
        if (recordedStamina != _stats.Stamina)
        {
            staminaRechargeTimer = 0;
        }

        if (_stats.Stamina != _stats.MaxStamina)
        {
            staminaRechargeTimer += Time.deltaTime;
            if(staminaRechargeTimer > _stats.StaminaRechargeDelay)
            {
                _stats.Stamina += _stats.StaminaRechargeRate;
                if (_stats.Stamina > _stats.MaxStamina)
                {
                    _stats.Stamina = _stats.MaxStamina;
                }
            }
        }

        recordedStamina = _stats.Stamina;
    }

    public void LoseLight(float value)
    {
        _stats.LightLevel -= value;
    }

    void LoseStamina(float value)
    {
        _stats.Stamina -= value;
    }

    public void Damage(float damage, float knockBackStrength, Vector3 sourceLocation)
    {
        if (isSliding) return;

        _stats.Health -= damage;
        Debug.Log("Player Health: " + _stats.Health);
        if (_stats.Health <= 0)
        {
            _stats.Health = 0;
            StartCoroutine(Destroyed());
        }

        isStunned = true;
        KnockBack(knockBackStrength, sourceLocation);
    }

    public void Damage(float damage)
    {
        _stats.Health -= damage;
        Debug.Log("Player Health: " + _stats.Health);
        if (_stats.Health <= 0)
        {
            _stats.Health = 0;
            StartCoroutine(Destroyed());
        }

        isStunned = true;
        StartCoroutine(StunRecovery());
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

    //Thrown
    public void Thrown(float throwStrength, Vector2 direction)
    {
        float knockBack = throwStrength - _stats.KnockBackResistance;
        if (knockBack <= 0) { knockBack = 0; }
        GetComponent<Rigidbody2D>().AddForce(-direction * knockBack, ForceMode2D.Impulse);
        float throwDistance = knockBack / 4;
        StartCoroutine(KnockBackReset(throwDistance));
    }

    IEnumerator PitfallFalling()
    {
        Vector2 testPoint = -(_lastMovementDirection) * 2f;
        Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + testPoint, .3f, 1 << 7);
        if (hit == null)
        {
            transform.position = (Vector2)transform.position + testPoint;
        } else
        {
            Vector2 newTestPoint = -(_lastMovementDirection) * 3f;
            Collider2D newHit = Physics2D.OverlapCircle((Vector2)transform.position + newTestPoint, .3f, 1 << 7);
            if (newHit == null)
            {
                transform.position = (Vector2)transform.position + newTestPoint;
            }
        }

        Damage(25);

        yield return new WaitForSeconds(.5f);
        isAtRiskofFalling = false;
        playerState = PlayerStates.Idle;
    }

    IEnumerator KnockBackReset(float knockBackDistance)
    {
        Vector3 lastLocation = transform.position;

        yield return new WaitWhile(() => Vector3.Distance(lastLocation, transform.position) < knockBackDistance && stunnedTimer <= .5f);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isStunned = false;
        stunnedTimer = 0;
    }

    IEnumerator StunRecovery()
    {
        yield return new WaitForSeconds(.1f);

        isStunned = false;
    }

    IEnumerator Destroyed()
    {
        Debug.Log("Death Animation");
        yield return new WaitForSeconds(.1f);
        Destroy(this.gameObject);
    }

    IEnumerator Slide()
    {
        Physics2D.IgnoreLayerCollision(13, 10, true);
        Vector3 lastLocation = transform.position;
        gameObject.GetComponent<Rigidbody2D>().AddForce(slideDir * _stats.SlideSpeed, ForceMode2D.Impulse);
        yield return new WaitWhile(() => Vector3.Distance(lastLocation, transform.position) < _stats.SlideDistance && slideTimer <= .5f);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Physics2D.IgnoreLayerCollision(13, 10, false);
        _playerAnimationController.state = PlayerStates.Idle;
        isSliding = false;
        slideTimer = 0;
    }

    IEnumerator meleeAttack(GameObject swingObject, float attackTime, float AoESize, float AoERadius, float attackDamage, float attackKnockbackStrength)
    {
        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll((Vector2)transform.position, AoERadius);
        List<GameObject> targets = new List<GameObject>();

        foreach (Collider2D target in potentialTargets)
        {

            //Verify no duplicates are added to hit targets
            bool skip = false;
            foreach (GameObject addedTarget in targets)
            {
                if (target.gameObject == addedTarget)
                {
                    skip = true;
                }
            }
            if (skip) { continue; }


            // Verify they can be hit
            float angleFromTargetAngle = Vector2.Angle(target.gameObject.transform.position - transform.position, _point.transform.position - transform.position);
            
            if (angleFromTargetAngle <= AoESize)
            {
                targets.Add(target.gameObject);
            }
        }

        GameObject tempAttackAnimation = Instantiate(swingObject, _point.transform.position, Quaternion.identity);
        tempAttackAnimation.transform.rotation = _attackPoint.transform.rotation * Quaternion.Euler(0,0,180);
        yield return new WaitForSeconds(attackTime);

        foreach (GameObject target in targets)
        {
            if (target == null)
            {
                continue;
            }
            if (target.tag == "DarkCrystal")
            {
                target.GetComponent<DarkCrystal>().Damage((int)attackDamage, transform);
            }
            if (target.tag == "Enemy")
            {
                target.GetComponent<BaseEnemyController>().Damage((int)attackDamage, attackKnockbackStrength, transform.position);
            }
        }
        
        Destroy(tempAttackAnimation);
    }

    IEnumerator RangeAttack(GameObject projectileObject, float attackTime, float projectileSpeed, float attackDamage, float lightLevelCost, float knockbackStrength)
    {

        if (projectileObject == heavyRangeObject)
        {
            audioManager.Play("Player Heavy Range Charge", 0, Random.Range(1f, 1.2f));
        }

        yield return new WaitForSeconds(attackTime);

        if (projectileObject == heavyRangeObject)
        {
            audioManager.Play("Player Heavy Range Shot", 0, Random.Range(1f, 1.2f));
        } else if (projectileObject == lightRangeObject)
        {
            audioManager.Play("Player Light Range Shot", 0, Random.Range(1f, 1.2f));
        }

        GameObject projectile = Instantiate(projectileObject, _point.transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileController>().sourceTransform = transform;
        projectile.GetComponent<ProjectileController>().speed = projectileSpeed;
        projectile.GetComponent<ProjectileController>().damage = attackDamage;
        projectile.GetComponent<ProjectileController>().power = lightLevelCost;
        projectile.GetComponent<ProjectileController>().knockback = knockbackStrength;
        projectile.transform.rotation = _attackPoint.transform.rotation * Quaternion.Euler(0, 0, 180);

    }

    IEnumerator LightTorch(GameObject torch)
    {
        
        yield return new WaitForSeconds(_stats.TorchLightAnimationTime);

        int lightCost = 40;
        if (_stats.LightLevel - lightCost > 0)
        {
            torch.GetComponent<TorchController>().ToggleLight(lightCost);
            LoseLight(lightCost);
        }

        isInteracting = false;

    }

    IEnumerator PickUpTorch(GameObject torch)
    {
        yield return new WaitForSeconds(_stats.TorchPickUpAnimationTime);

        itemCarried = torch;

        isCarrying = true;
        _stats.SpeedDebuff += _stats.CarryingSpeedDebuff;
        torch.GetComponent<TorchController>().PickedUp(_point);
        isInteracting = false;
    }

    IEnumerator DropTorch()
    {
        yield return new WaitForSeconds(_stats.ItemDropAnimationTime);

        isCarrying = false;
        _stats.SpeedDebuff -= _stats.CarryingSpeedDebuff;
        itemCarried.GetComponent<TorchController>().Dropped();
        isInteracting = false;
    }
}

public enum PlayerStates { Idle, Walking, Sliding, Attacking, Interacting, BeingCarried, Thrown, Falling}

public enum PlayerDirection { RDown, RUp, Right, LDown, LUp, Left}
