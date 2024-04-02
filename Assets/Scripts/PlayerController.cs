using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Animator animator;
    private Vector3 respawnPoint;
    public UnityEvent<int, int> healthChanged;
    public UnityEvent<int, int> manaChanged;
    public UnityEvent<int, int> expChanged;

    [Header("Horizontal Movement Settings")]
    [SerializeField] private float walkSpeed = 1;
    [Space(5)]

    //guardar
    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45f;
    private int jumpBufferCounter;
    [SerializeField] private int jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    private int airJumpCounter = 0;
    [SerializeField] public int maxAirJumps;
    [SerializeField] private int maxFallSpeed = 20;
    [Space(5)]

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;
    [Space(5)]

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    [Space(5)]
    //guardar
    [Header("Attack Settings")]
    [SerializeField] Transform SideAttackTransform;
    [SerializeField] Transform UpAttackTransform;
    [SerializeField] Transform DownAttackTransform;
    [SerializeField] Vector2 SideAttackArea, UpAttackArea, DownAttackArea;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] public float damage;
    [SerializeField] public float damageMultiplier;
    [SerializeField] GameObject slashEffect;
    bool attack = false;
    public float timeBetweenAttack;
    float timeSinceAttack;
    [Space(5)]

    [Header("Recoil Settings")]
    [SerializeField] int recoilXSteps = 5;
    [SerializeField] int recoilYSteps = 5;
    [SerializeField] float recoilXSpeed = 10;
    [SerializeField] float recoilYSpeed = 10;
    int stepsXRecoiled, stepsYRecoiled;
    //guardar
    [Header("Stat Settings")]
    [SerializeField] private int _health = 10;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, maxHealth);
        }
    }

    [SerializeField] public int maxHealth = 10;
    [SerializeField] private int _mana = 10;

    public int Mana
    {
        get
        {
            return _mana;
        }
        set
        {
            _mana = value;
            manaChanged?.Invoke(_mana, maxMana);
        }
    }

    private float tempMana;

    [SerializeField] public int maxMana = 10;
    [Space(5)]

    [Header("Wall Jump Settings")]
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallJumpingDuration;
    [SerializeField] private Vector2 wallJumpingPower;
    float wallJumpingDirection;
    bool isWallSliding;
    bool isWallJumping;
    [Space(5)]
    //guardar
    [Header("Level Up settings")]
    public int _exp = 0;

    public int Exp
    {
        get
        {
            return _exp;
        }
        set
        {
            _exp = value;
            expChanged?.Invoke(_exp, expNextLevel);
        }
    }

    public int currentLevel = 1; //guardar
    [SerializeField] public int maxLevel = 10;
    [SerializeField] public int skillPoints = 0; //guardar
    [SerializeField] public int expNextLevel = 100;
    [Space(5)]

    [HideInInspector] public PlayerStateList pState;
    public Rigidbody2D rb;
    private float xAxis, yAxis;
    public float gravity;
    Animator anim;

    public BoxCollider2D colli;
    private bool canDash = true;
    private bool dashed;

    //Unlock variables
    [Header("Unlock Settings")]
    public bool unlockedDash;
    public bool unlockedDoubleJump;
    public bool unlockedWallJump;
    public bool unlockedLong;
    public bool unlockedStaff;
    public bool unlockedBow;
    public bool unlockedGauntlet;
    public bool unlockedSecret;
    [Space(5)]

    //Weapon variables
    [SerializeField] public int currentWeapon = 0;
    [SerializeField] public int arrowAmount = 0;
    [SerializeField] public int healthPotions = 0;
    [SerializeField] public int manaPotions = 0;
    public bool[] weaponList;

    //Special variables
    [Header("Special Attack Settings")]
    public GameObject swordWave;
    public GameObject spiritSlash;
    public GameObject spellSpam;
    public GameObject tripleArrow;
    public GameObject goku;
    public GameObject riderKick;
    public GameObject secretSpecial;

    //Interface variables
    public bool skillTreeActive = false;

    public static PlayerController Instance;

    public void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        Health = maxHealth;
        weaponList = new bool[6];
        weaponList[0] = true;
        weaponList[1] = unlockedLong;
        weaponList[2] = unlockedStaff;
        weaponList[3] = unlockedBow;
        weaponList[4] = unlockedGauntlet;
        weaponList[5] = unlockedSecret;

    }


    // Start is called before the first frame update
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colli = GetComponent<BoxCollider2D>();
        gravity = rb.gravityScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    public ProjectileBehaviour projectilePrefab;
    public Transform launchOffset;

    void Update()
    {
        if (!pState.inventoryActive && !DialogueManager.isActive && !skillTreeActive)
        {
            GetInputs();
            UpdateJumpVariables();
            CheckLevelUp();
            if (pState.dashing) return;

            if (!isWallJumping && !pState.specialActive)
            {
                Recoil();
                Flip();
                Move();
                Jump();
                fallSpeedLimit();
            }

            if (unlockedWallJump)
            {
                WallSlide();
                WallJump();
            }

            Attack();
            SpecialAttack();
            if (unlockedDash)
            {
                StartDash();
            }

            if (skillTreeActive)
            {
                upgradeAvaible();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Carga la escena del menú principal
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
                gameObject.active = false;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Instantiate(projectilePrefab, launchOffset.position, transform.rotation);
            }
        }
    }

    public void MenuStart()
    {
        gameObject.active = true;
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetMouseButtonDown(0);
    }

    private void Flip()
    {
        if (xAxis < 0)
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 180);
            pState.lookingRight = false;

        }
        else if (xAxis > 0)
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0);
            pState.lookingRight = true;
        }

    }

    private void Move()
    {
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
        anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
    }

    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        if (transform.eulerAngles.y == 0)
        {
            rb.velocity = new Vector2(dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-dashSpeed, 0);
        }
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if(Mana < maxMana){
            tempMana += Time.deltaTime;
        }
        if(tempMana >= 2f && Mana < maxMana){
            Mana += 1;
            tempMana = 0;
        }

        if ((attack || Input.GetKeyDown(KeyCode.J)) && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;

            if (currentWeapon == 3)
            {

                if (arrowAmount > 0)
                {
                    arrowAmount--;
                    anim.SetTrigger("rangedattack");
                    Instantiate(projectilePrefab, launchOffset.position, transform.rotation);
                }

            }
            else
            {

                if (yAxis == 0 || yAxis < 0 && Grounded())
                {
                    Hit(SideAttackTransform, SideAttackArea, ref pState.recoilingX, recoilXSpeed);
                    Instantiate(slashEffect, SideAttackTransform);
                    anim.SetTrigger("Attacking");

                }
                else if (yAxis > 0)
                {
                    Hit(UpAttackTransform, UpAttackArea, ref pState.recoilingY, recoilYSpeed);
                    SlashEffectAtAngle(slashEffect, 90, UpAttackTransform);
                    anim.SetTrigger("UpAttacking");

                }
                else if (yAxis < 0 && !Grounded())
                {
                    Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingY, recoilYSpeed);
                    SlashEffectAtAngle(slashEffect, -90, DownAttackTransform);
                    anim.SetTrigger("DownAttacking");
                }

            }
        }
    }

    private void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {

        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }

        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyHit(damage * damageMultiplier, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
            }
            if (objectsToHit[i].GetComponent<Goblin>() != null)
            {
                objectsToHit[i].GetComponent<Goblin>().TakeDamage(damage * damageMultiplier);
            }
            if (objectsToHit[i].GetComponent<Ghost>() != null)
            {
                objectsToHit[i].GetComponent<Ghost>().TakeDamage(damage * damageMultiplier);
            }
            if (objectsToHit[i].GetComponent<Skeleton>() != null)
            {
                objectsToHit[i].GetComponent<Skeleton>().TakeDamage(damage * damageMultiplier);
            }
            if (objectsToHit[i].GetComponent<Eye_Boss>() != null)
            {
                objectsToHit[i].GetComponent<Eye_Boss>().TakeDamage(damage * damageMultiplier);
            }
        }

    }

    void Recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.velocity = new Vector2(-recoilXSpeed, 0);

            }
            else
            {
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pState.recoilingY)
        {
            rb.gravityScale = 0;
            if (yAxis < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, recoilYSpeed);

            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;

        }
        else
        {
            rb.gravityScale = gravity;
        }

        //Stop recoil X
        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;

        }
        else
        {
            StopRecoilX();
        }

        //Stop recoil Y
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;

        }
        else
        {
            StopRecoilY();
        }

        if (Grounded())
        {
            StopRecoilY();
        }
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }

    public void TakeDamage(float _damage)
    {
        Health -= Mathf.RoundToInt(_damage);
        if (Health <= 0)
        {
            anim.SetTrigger("Die");
            StartCoroutine("DieMenu");
        }
        StartCoroutine(StopTakingDamage());
    }

    IEnumerator DieMenu()
    {
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Death Menu");
    }

    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        ClampHealth();
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
    }

    void ClampHealth()
    {
        Health = Mathf.Clamp(Health, 0, maxHealth);
    }

    public bool Grounded()
    {

        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    void Jump()
    {

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            pState.jumping = false;

        }

        if (!pState.jumping)
        {

            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                pState.jumping = true;

            }
            else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;

                airJumpCounter++;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            }
        }

        anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;

        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;

        }
        else
        {
            jumpBufferCounter--;
        }
    }

    private bool Walled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    }

    void WallSlide()
    {
        if (Walled() && !Grounded() && xAxis != 0)
        {
            isWallSliding = true;

            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }
        else
        {
            isWallSliding = false;
        }
    }

    void WallJump()
    {

        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = !pState.lookingRight ? 1 : -1;

            CancelInvoke(nameof(StopWallJumping));
        }

        if (Input.GetButtonDown("Jump") && isWallSliding)
        {

            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);

            dashed = false;
            airJumpCounter = 0;

            //if((pState.lookingRight && transform.eulerAngles.y == 0) || (!pState.lookingRight && transform.eulerAngles.y != 0)){
            pState.lookingRight = !pState.lookingRight;
            int _yRotation = 180;

            transform.eulerAngles = new Vector2(transform.eulerAngles.x, _yRotation);
            //}

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

    void fallSpeedLimit()
    {
        if (rb.velocity.y < -maxFallSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, -maxFallSpeed);
        }
    }

    void CheckLevelUp()
    {
        if (Exp >= expNextLevel && currentLevel < maxLevel)
        {
            currentLevel++;
            Exp -= expNextLevel;
            skillPoints++;
            Debug.Log("Sube de nivel");
            Debug.Log(currentLevel);
        }
    }

    void showSkillTree()
    {
        skillTreeActive = true;
        Debug.Log("Mejoras activas");
    }

    void upgradeAvaible()
    {
        if (Input.GetButtonDown("UpgradeHealth"))
        {
            skillTreeActive = false;
            skillPoints--;
            maxHealth += 10;
            Health += 10;
            Debug.Log(maxHealth);
            Debug.Log("Mejora: Vida");

        }
        else if (Input.GetButtonDown("UpgradeDamage"))
        {
            skillTreeActive = false;
            skillPoints--;
            damage += 1;
            Debug.Log(damage);
            Debug.Log("Mejora: Daño");

        }
        else if (Input.GetButtonDown("UpgradeMana"))
        {
            skillTreeActive = false;
            skillPoints--;
            maxMana += 10;
            Mana += 10;
            Debug.Log(maxMana);
            Debug.Log("Mejora: Mana");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position;
        }
        else if (collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            respawnPoint = transform.position;
        }
        else if (collision.tag == "PreviousLevelTwo")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
            respawnPoint = transform.position;
        }
        else if (collision.tag == "NextLevelTwo")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            respawnPoint = transform.position;
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void ChangeWeapon(int _weaponNum)
    {

        int weaponDirection;
        if (pState.lookingRight)
        {
            weaponDirection = 1;
        }
        else
        {
            weaponDirection = -1;
        }

        switch (_weaponNum)
        {

            //Basic Sword
            case 0:
                currentWeapon = 0;

                SideAttackArea = new Vector2(4.5f, 3.5f);
                UpAttackArea = new Vector2(3.5f, 4.5f);
                DownAttackArea = new Vector2(3.5f, 4.5f);

                SideAttackTransform.position = new Vector2(transform.position.x + (2.75f * weaponDirection), transform.position.y + -0.7f);
                UpAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + 2.5f);
                DownAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + -3.45f);

                timeBetweenAttack = 0.4f;
                damageMultiplier = 1f;
                break;

            //Long Sword
            case 1:
                currentWeapon = 1;

                SideAttackArea = new Vector2(7.5f, 4f);
                UpAttackArea = new Vector2(4f, 7.5f);
                DownAttackArea = new Vector2(4f, 7.5f);

                SideAttackTransform.position = new Vector2(transform.position.x + (4.7f * weaponDirection), transform.position.y + -0.7f);
                UpAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + 4.85f);
                DownAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + -5.3f);

                timeBetweenAttack = 1f;
                damageMultiplier = 1.7f;
                break;

            //Staff
            case 2:
                currentWeapon = 2;

                SideAttackArea = new Vector2(4.5f, 5.5f);
                UpAttackArea = new Vector2(5.5f, 4.5f);
                DownAttackArea = new Vector2(5.5f, 4.5f);

                SideAttackTransform.position = new Vector2(transform.position.x + (3.25f * weaponDirection), transform.position.y + -0.7f);
                UpAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + 3.3f);
                DownAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + -3.95f);

                timeBetweenAttack = 0.7f;
                damageMultiplier = 1.3f;
                break;

            //Bow
            case 3:
                currentWeapon = 3;
                damageMultiplier = 1.1f;
                timeBetweenAttack = 0.2f;
                break;

            //Gauntlet
            case 4:
                currentWeapon = 4;

                SideAttackArea = new Vector2(1.5f, 2.5f);
                UpAttackArea = new Vector2(2.5f, 1.5f);
                DownAttackArea = new Vector2(2.5f, 1.5f);

                SideAttackTransform.position = new Vector2(transform.position.x + (1.25f * weaponDirection), transform.position.y + -0.352f);
                UpAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + 1.7f);
                DownAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + -2.4f);

                timeBetweenAttack = 0.05f;
                damageMultiplier = 1.4f;
                break;

            //Secret
            case 5:
                currentWeapon = 5;

                SideAttackArea = new Vector2(5.5f, 4.5f);
                UpAttackArea = new Vector2(4.5f, 5.5f);
                DownAttackArea = new Vector2(4.5f, 5.5f);

                SideAttackTransform.position = new Vector2(transform.position.x + (3.75f * weaponDirection), transform.position.y + -0.5f);
                UpAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + 3.8f);
                DownAttackTransform.position = new Vector2(transform.position.x + 0, transform.position.y + -4.5f);

                timeBetweenAttack = 0.3f;
                damageMultiplier = 1.2f;
                break;
        }
    }

    void SpecialAttack()
    {
        if (Input.GetKeyDown(KeyCode.K) && timeSinceAttack >= timeBetweenAttack)
        {
            int offsetDirection;
            if (pState.lookingRight)
            {
                offsetDirection = 1;

            }
            else
            {
                offsetDirection = -1;
            }

            switch (currentWeapon)
            {

                case 0:
                    if (Mana >= 5)
                    {
                        Instantiate(swordWave, launchOffset.position, transform.rotation);
                        anim.SetTrigger("Attacking");
                        Mana -= 5;
                        timeSinceAttack = 0;
                    }
                    break;

                case 1:
                    if (Mana >= 10)
                    {
                        Instantiate(spiritSlash, new Vector3(launchOffset.position.x + (0.5f * offsetDirection), launchOffset.position.y), transform.rotation);
                        anim.SetTrigger("Attacking");
                        Mana -= 10;
                        timeSinceAttack = 0;
                    }
                    break;

                case 2:
                    if (Mana >= 2)
                    {
                        Instantiate(spellSpam, launchOffset.position, transform.rotation);
                        Mana -= 2;
                        timeSinceAttack = 0;
                    }
                    break;

                case 3:
                    if (Mana >= 3 && arrowAmount > 0)
                    {
                        Quaternion tempQua1 = transform.rotation;
                        Quaternion tempQua2 = transform.rotation;


                        tempQua1.eulerAngles = new Vector3(tempQua1.eulerAngles.x, tempQua1.eulerAngles.y, 30f);
                        tempQua2.eulerAngles = new Vector3(tempQua1.eulerAngles.x, tempQua1.eulerAngles.y, -30f);

                        Instantiate(projectilePrefab, new Vector2(launchOffset.position.x, launchOffset.position.y + 0.4f), tempQua1);
                        Instantiate(projectilePrefab, new Vector2(launchOffset.position.x, launchOffset.position.y - 0.4f), tempQua2);
                        Instantiate(projectilePrefab, launchOffset.position, transform.rotation);

                        anim.SetTrigger("rangedattack");

                        Mana -= 3;
                        arrowAmount--;
                        timeSinceAttack = 0;

                    }
                    break;

                case 4:
                    if (Mana >= 10)
                    {
                        if (Grounded())
                        {
                            Instantiate(goku, new Vector3(launchOffset.position.x + (18f * offsetDirection), launchOffset.position.y), transform.rotation);
                            StartCoroutine(DeactivateSpecialAttack());
                            anim.SetTrigger("Attacking");

                        }
                        else
                        {
                            Instantiate(riderKick, new Vector3(transform.position.x + (0.7f * offsetDirection), transform.position.y - 0.8f), transform.rotation, transform);
                        }

                        Mana -= 10;
                        timeSinceAttack = 0;
                    }
                    break;

                case 5:
                    if (Mana >= 10)
                    {
                        Instantiate(secretSpecial, new Vector3(launchOffset.position.x, launchOffset.position.y + 0.7f), transform.rotation);
                        anim.SetTrigger("Attacking");
                        timeSinceAttack = 0;
                    }
                    break;

            }
        }
    }

    IEnumerator DeactivateSpecialAttack()
    {
        yield return new WaitForSeconds(0.20f);
        pState.specialActive = false;
    }

    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {

        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(_attackTransform.localScale.x, _attackTransform.localScale.y);
    }

}
