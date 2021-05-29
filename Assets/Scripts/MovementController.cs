 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    //VARS
    [Min(0f)][SerializeField] private float movementSpeed;
    [Min(0f)][SerializeField] private float walkingSpeed;
    [Min(0f)][SerializeField] private float runningSpeed;
    [Min(0f)][SerializeField] private float jumpHeight;
    [Range(0,10)][SerializeField] private float stamina=10;
    [Range(0, 10)] [SerializeField] private float maxStamina = 10;
    [Min(0f)][SerializeField] private float cooldown=2;
    [Min(0f)][SerializeField] private float slideSpeed = 0.35f;
    [Min(0f)][SerializeField] private float honeySpeed = 2f;

    [SerializeField] private GameObject snowParticles;


    [SerializeField] private AudioClip jumpingSound;


    AudioSource SoundSource;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 lastMoveDirection = Vector3.zero;
    private float StaminaRegenTimer = 0.0f;
    private bool icy = false;
    private bool honey = false;
    private bool canJump = true;

    private const float staminaDecrease = 2.0f;
    private const float staminaIncrease = 5.0f;

    private bool isParticleRunning = false;
    

    [SerializeField] private bool isGrounded;
    [Min(0f)][SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    //REFS
    private CharacterController controller;
    private Animator anim;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        SoundSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }



    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        float inputMagnitude = Mathf.Min(new Vector3(moveX, 0, moveZ).sqrMagnitude, 1f);


        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);
        if(inputMagnitude > 0.225f)
        {
            lastMoveDirection = moveDirection;
        }
        if (isGrounded)
        {
            if (icy)
            {
                moveDirection = lastMoveDirection * slideSpeed;
                if (!isParticleRunning)
                {
                    Instantiate(snowParticles,transform.localPosition, Quaternion.identity);
                    isParticleRunning = true;
                }
                canJump = true;
            }
            else if(honey)
            {
                DestroyAll("Snow");
                isParticleRunning = false;
                moveDirection *= honeySpeed;
                canJump = false;
            }
            else
            {
                DestroyAll("Snow");
                isParticleRunning = false;
                canJump = true;
                if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
                {
                    Walk();
                }
                else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)){
                    Run();
                }
                else if(moveDirection == Vector3.zero)
                {
                    Idle();
                }
                moveDirection *= movementSpeed;
            }

            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump(canJump);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                jumpHeight = 10;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                jumpHeight = 1.25f;
            }
        }
        controller.Move(moveDirection * Time.deltaTime); //movement
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime); //gravity  
    }

    private void Walk()
    {
        movementSpeed = walkingSpeed;
        anim.SetFloat("Speed", 0.25f, 0.1f, Time.deltaTime);
        RegenerateStamina();
    }

    private void Run()
    {
        if (stamina > 0)
        {
            stamina = Mathf.Clamp(stamina - (staminaDecrease * Time.deltaTime), 0.0f, maxStamina);
            movementSpeed = runningSpeed;
            anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
            StaminaRegenTimer = 0.0f;
        }
        else
        {
            Walk();
        }

    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0,0.1f,Time.deltaTime);
        RegenerateStamina();
    }
    private void Jump(bool canJump)
    {
        if (canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            movementSpeed = walkingSpeed;
            SoundSource.PlayOneShot(jumpingSound);
            anim.SetFloat("Speed", 0.75f);
        }
    }

    private void RegenerateStamina()
    {
        if (StaminaRegenTimer >= cooldown)
        {
            stamina = Mathf.Clamp(stamina + (staminaIncrease * Time.deltaTime), 0.0f, maxStamina);
        }
        else
        {
            StaminaRegenTimer += Time.deltaTime;
        }
    }

    public float getMaxStamina()
    {
        return maxStamina;
    }
    public float getStamina()
    {
        return stamina;
    }
    public float getCooldown()
    {
        return StaminaRegenTimer - cooldown;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        icy = hit.collider.CompareTag("Ice");
        honey = hit.collider.CompareTag("Honey");
    }

    private void createSnowyParticles(Vector3 position)
    {
        var iceParticles = Instantiate(snowParticles, position, Quaternion.identity);
        iceParticles.SetActive(true);
    }
    void DestroyAll(string tag)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < items.Length; i++)
        {
            Destroy(items[i]);
        }
    }
}
