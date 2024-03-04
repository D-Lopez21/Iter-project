using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

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
    bool attack = false;
    float timeBetweenAttack, timeSinceAttack;
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
    [SerializeField]public int maxLevel = 10;
    [SerializeField]public int skillPoints = 0; //guardar
    [SerializeField]public int expNextLevel = 100;
    public bool skillTreeActive = false;
    [Space(5)]

    [HideInInspector] public PlayerStateList pState;
    private Rigidbody2D rb;
    private float xAxis, yAxis;
    private float gravity;
    Animator anim;

    public BoxCollider2D colli;
    private bool canDash = true;
    private bool dashed;


    //Unlock variables
    public bool unlockedDash;
    public bool unlockedDoubleJump;
    public bool unlockedWallJump;


    public static PlayerController Instance;

    public void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);

        }else{
            Instance = this;
        }

        Health = maxHealth;
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

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();
        CheckLevelUp();
        if(pState.dashing) return;

        if(!isWallJumping){
            Recoil();
            Flip();
            Move();
            Jump();
            fallSpeedLimit();
        }

        if(unlockedWallJump){
            WallSlide();
            WallJump();
        }

        Attack();
        if(unlockedDash){
            StartDash();
        }

        if(skillTreeActive){
            upgradeAvaible();
        }
        
    }

    void GetInputs(){
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetMouseButtonDown(0);
    }

    private void Flip(){
        if(xAxis < 0){
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 180);
            pState.lookingRight = false;

        }else if(xAxis > 0){
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0);
            pState.lookingRight = true;
        }

    }

    private void Move(){
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
        anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
    }

    void StartDash(){
        if(Input.GetButtonDown("Dash") && canDash && !dashed){
            StartCoroutine(Dash());
            dashed = true;
        }

        if(Grounded()){
            dashed = false;
        }
    }

    IEnumerator Dash(){
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        if(transform.eulerAngles.y == 0){
            rb.velocity = new Vector2(dashSpeed, 0);
        }else{
            rb.velocity = new Vector2(-dashSpeed, 0);
        }
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Attack(){
        timeSinceAttack += Time.deltaTime;
        if(attack && timeSinceAttack >= timeBetweenAttack){
            timeSinceAttack = 0;

            if(yAxis == 0 || yAxis < 0 && Grounded()){
                Hit(SideAttackTransform, SideAttackArea, ref pState.recoilingX, recoilXSpeed);
                anim.SetTrigger("Attacking");

            }else if(yAxis > 0){
                Hit(UpAttackTransform, UpAttackArea, ref pState.recoilingY, recoilYSpeed);
                anim.SetTrigger("UpAttacking");

            }else if(yAxis < 0 && !Grounded()){
                Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingY, recoilYSpeed);
                anim.SetTrigger("DownAttacking");
            }
        }
    }

    private void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength){

        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        if(objectsToHit.Length > 0){
            _recoilDir = true;
        }

        for(int i = 0; i < objectsToHit.Length; i++){
            if(objectsToHit[i].GetComponent<Enemy>() != null){
                objectsToHit[i].GetComponent<Enemy>().EnemyHit(damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
            }
        }

    }

    void Recoil(){
        if(pState.recoilingX){
            if(pState.lookingRight){
                rb.velocity = new Vector2(-recoilXSpeed, 0);

            }else{
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if(pState.recoilingY){
            rb.gravityScale = 0;
            if(yAxis < 0){
                rb.velocity = new Vector2(rb.velocity.x, recoilYSpeed);

            }else{
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;

        }else{
            rb.gravityScale = gravity;
        }

        //Stop recoil X
        if(pState.recoilingX && stepsXRecoiled < recoilXSteps){
            stepsXRecoiled++;

        }else{
            StopRecoilX();
        }

        //Stop recoil Y
        if(pState.recoilingY && stepsYRecoiled < recoilYSteps){
            stepsYRecoiled++;

        }else{
            StopRecoilY();
        }

        if(Grounded()){
            StopRecoilY();
        }
    }

    void StopRecoilX(){
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY(){
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }

    public void TakeDamage(float _damage)
    {
        Health -= Mathf.RoundToInt(_damage);
        StartCoroutine(StopTakingDamage());
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

    public bool Grounded(){

        if(Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) 
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) 
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)){
            return true;

        }else{
            return false;
        }
    }

    void Jump(){

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0){
            rb.velocity = new Vector2(rb.velocity.x, 0);
            pState.jumping = false;

        }

        if(!pState.jumping){

            if(jumpBufferCounter > 0 && coyoteTimeCounter > 0){
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                pState.jumping = true;

            }else if(!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump")){
                pState.jumping = true;

                airJumpCounter++;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            }
        }

        anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables(){
        if(Grounded()){
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;

        }else{
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump")){
            jumpBufferCounter = jumpBufferFrames;

        }else{
            jumpBufferCounter--;
        }
    }

    private bool Walled(){
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    }

    void WallSlide(){
        if(Walled() && !Grounded() && xAxis != 0){
            isWallSliding = true;

            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }else{
            isWallSliding = false;
        }
    }

    void WallJump(){

        if(isWallSliding){
            isWallJumping = false;
            wallJumpingDirection = !pState.lookingRight ? 1 : -1;

            CancelInvoke(nameof(StopWallJumping));
        }

        if(Input.GetButtonDown("Jump") && isWallSliding){

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

    void StopWallJumping(){
        isWallJumping = false;
    }

    void fallSpeedLimit(){
        if(rb.velocity.y < -maxFallSpeed){
            rb.velocity = new Vector3(rb.velocity.x, -maxFallSpeed);
        }
    }

    void CheckLevelUp(){
        if(Exp >= expNextLevel && currentLevel < maxLevel){
            currentLevel++;
            Exp -= expNextLevel;
            skillPoints++;
            Debug.Log("Sube de nivel");
            Debug.Log(currentLevel);
        }
    }

    void showSkillTree(){
        skillTreeActive = true;
        Debug.Log("Mejoras activas");
    }

    void upgradeAvaible(){
        if(Input.GetButtonDown("UpgradeHealth")){
            skillTreeActive = false;
            skillPoints--;
            maxHealth += 10;
            Health += 10;
            Debug.Log(maxHealth);
            Debug.Log("Mejora: Vida");

        }else if(Input.GetButtonDown("UpgradeDamage")){
            skillTreeActive = false;
            skillPoints--;
            damage += 1;
            Debug.Log(damage);
            Debug.Log("Mejora: Daño");

        }else if(Input.GetButtonDown("UpgradeMana")){
            skillTreeActive = false;
            skillPoints--;
            maxMana += 10;
            Mana += 10;
            Debug.Log(maxMana);
            Debug.Log("Mejora: Mana");
        }
    }
}
