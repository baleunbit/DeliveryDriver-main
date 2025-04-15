using UnityEngine;

public class Drift : MonoBehaviour
{
    [SerializeField] float acceleration = 60f;
    [SerializeField] float steering = 10f;
    [SerializeField] float maxSpeed = 100f;
    [SerializeField] float driftFactor = 0.95f;

    [SerializeField] float slowAcclerationRatio = 0.5f;
    [SerializeField] float boostAcclerationRatio = 1.5f;

    [SerializeField] ParticleSystem smokeLeft;
    [SerializeField] ParticleSystem smokeRight;
    [SerializeField] TrailRenderer LeftTrail;
    [SerializeField] TrailRenderer RightTrail;

    private SpriteRenderer spriteRenderer;

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite boostSprite;


    Rigidbody2D rb;
    AudioSource audioSource;

    float defaultAcceleration;
    float slowAcceleration;
    float boostAcceleration;

    float previousSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;

        defaultAcceleration = acceleration;
        slowAcceleration = acceleration * slowAcclerationRatio;
        boostAcceleration = acceleration * boostAcclerationRatio;
    }

    void FixedUpdate()
    {
        float speed = Vector2.Dot(rb.linearVelocity, transform.up);
        if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * Input.GetAxis("Vertical") * acceleration);
        }

        float turnAmount = Input.GetAxis("Horizontal") * steering * Mathf.Clamp(speed / maxSpeed, 0.4f, 1);
        rb.MoveRotation(rb.rotation - turnAmount);

        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);

            float currentSpeed = rb.linearVelocity.magnitude;
            if (currentSpeed < previousSpeed - 0.1f)
            {
                Debug.Log("속도 감소");
            }
            previousSpeed = currentSpeed;
    }

    void Update()
    {
        float sideWayVelocity = Vector2.Dot(rb.linearVelocity, transform.right);

        bool isDrifting = rb.linearVelocity.magnitude > 2f && Mathf.Abs(sideWayVelocity) > 1f;
        if (isDrifting)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            if (!smokeLeft.isPlaying) smokeLeft.Play();
            if (!smokeRight.isPlaying) smokeRight.Play();
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
            if (smokeLeft.isPlaying) smokeLeft.Stop();
            if (smokeRight.isPlaying) smokeRight.Stop();
        }

        LeftTrail.emitting = isDrifting;
        RightTrail.emitting = isDrifting;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            acceleration = boostAcceleration;
            spriteRenderer.sprite = boostSprite;
            Debug.Log("Boost!!!!!");

            Invoke("ResetAcceleration", 2f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        acceleration = slowAcceleration;
        Invoke("ResetAcceleration", 2f);
    }

    void ResetAcceleration()
    {
        acceleration = defaultAcceleration;
        spriteRenderer.sprite = normalSprite;
    }
}
