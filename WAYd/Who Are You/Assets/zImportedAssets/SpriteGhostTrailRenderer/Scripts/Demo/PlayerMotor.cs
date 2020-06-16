using UnityEngine;

namespace ActionCode.Demo
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private string hInputParam = "hInput";
        [SerializeField] private string vSpeedParam = "vSpeed";
        [SerializeField] private string crouchingParam = "crouching";
        [SerializeField] private string jumpParam = "jump";
        [SerializeField] private string attackParam = "attack";
        [SerializeField] private string groundedParam = "grounded";

        [Header("Physics")]
        [Range(0f, 20f)] public float speed = 10f;
        [Range(0f, 10f)] public float jumpForce = 5f;
        public ContactFilter2D groundFilter;

        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private SpriteRenderer _renderer;
     
        private float lastInput;
        private float lastDirection = 1f;

        private int hInputId;
        private int vSpeedId;
        private int crouchingId;
        private int jumpId;
        private int attackId;
        private int groundedId;

        private float crouching;
        private bool isGrounded = false;

        private readonly float SKIN = 0.04F;

        private void Reset()
        {
            SetupComponents();
        }

        private void Start()
        {
            HashAnimatorParams();
        }

        private void Update()
        {            
            if (Time.timeScale < 0.1f) return;
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            UpdatePhysics();
            crouching = 0f;
        }

        public void Move(float horInput)
        {
            lastInput = horInput;
            if (lastInput * lastDirection < 0f) FlipHorizontally();
            if (lastInput < 0f) lastDirection = -1f;
            else if (lastInput > 0f) lastDirection = 1f;
        }

        public void Jump()
        {
            if (isGrounded)
            {
                animator.SetTrigger(jumpId);
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        public void Attack()
        {
            if(Mathf.Abs(lastInput) < 0.1f) animator.SetTrigger(attackId);
        }

        public void Crouch(float crounching)
        {
            this.crouching = crounching;
        }

        private void SetupComponents()
        {
            animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }        

        private void HashAnimatorParams()
        {
            hInputId = Animator.StringToHash(hInputParam);
            vSpeedId = Animator.StringToHash(vSpeedParam);
            crouchingId = Animator.StringToHash(crouchingParam);
            jumpId = Animator.StringToHash(jumpParam);
            attackId = Animator.StringToHash(attackParam);
            groundedId = Animator.StringToHash(groundedParam);
        }        

        private void UpdatePhysics()
        {
            bool pushingAgainstWall = lastInput * lastDirection > 0 && IsForwardCollision();
            isGrounded = _rigidbody.IsTouching(groundFilter);

            if (!pushingAgainstWall)
            {
                Vector3 horVelocity = Vector3.right * lastInput * speed * Time.deltaTime;
                transform.position += horVelocity;
            }
        }        

        private void UpdateAnimator()
        {
            animator.SetFloat(crouchingId, crouching);
            animator.SetFloat(hInputId, Mathf.Abs(lastInput));
            animator.SetFloat(vSpeedId, _rigidbody.velocity.y);
            animator.SetBool(groundedId, isGrounded);
        }

        private void FlipHorizontally()
        {
            _renderer.flipX = !_renderer.flipX;
        }

        private bool IsForwardCollision()
        {
            Vector2 size = _collider.bounds.size;
            Vector3 direction = Vector3.right * lastDirection;
            Vector3 bottomPos = transform.position + direction * (size.x / 2f);
            Vector3 topPos = bottomPos + Vector3.up * size.y;

            return
                Physics2D.Raycast(topPos, direction, SKIN, groundFilter.layerMask, 0f, 0f) ||
                Physics2D.Raycast(bottomPos, direction, SKIN, groundFilter.layerMask, 0f, 0f);
        }
    }
}