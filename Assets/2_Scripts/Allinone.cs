using UnityEngine;

public class Allinone : MonoBehaviour
{
    enum CarState { Normal, Carrying, Boosting }
    CarState currentState = CarState.Normal;

    [Header("�̵� �� �帮��Ʈ")]
    [SerializeField] float acceleration = 50;
    [SerializeField] float steering = 11;
    [SerializeField] float maxSpeed = 80;
    [SerializeField] float driftFactor = 0.95f;

    [SerializeField] float slowAccelerationRatio = 0.5f;
    [SerializeField] float boostAccelerationRatio = 1.5f;

    [Header("����Ʈ")]
    [SerializeField] ParticleSystem smokeLeft;
    [SerializeField] ParticleSystem smokeRight;
    [SerializeField] TrailRenderer LeftTrail;
    [SerializeField] TrailRenderer RightTrail;

    [Header("��������Ʈ")]
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite carryingSprite;
    [SerializeField] Sprite boostSprite;
    [SerializeField] Sprite carryingBoostSprite;

    [Header("�ӵ� ���� ����")]
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color fastColor = Color.red;
    [SerializeField] Color slowColor = Color.blue;


    Rigidbody2D rb;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;

    float defaultAcceleration;
    float slowAcceleration;
    float boostAcceleration;

    float previousSpeed = 0;
    bool hasItem = false;
    bool canLogSpeedChange = true;
    int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        defaultAcceleration = acceleration;
        slowAcceleration = acceleration * slowAccelerationRatio;
        boostAcceleration = acceleration * boostAccelerationRatio;

        spriteRenderer.sprite = normalSprite;
        spriteRenderer.color = normalColor;

        Debug.Log("����: 0");
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float speed = Vector2.Dot(rb.linearVelocity, transform.up);
        if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * verticalInput * acceleration);
        }

        float turnAmount = horizontalInput * steering * Mathf.Clamp(speed / maxSpeed, 0.4f, 1);
        rb.MoveRotation(rb.rotation - turnAmount);

        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);

        float currentSpeed = rb.linearVelocity.magnitude;

        if (canLogSpeedChange)
        {
            if (currentSpeed < previousSpeed - 0.1f)
            {
                Debug.Log("�ӵ� ����");
                canLogSpeedChange = false;
                Invoke("EnableSpeedLog", 1);
            }
            else if (currentSpeed > previousSpeed + 0.1f)
            {
                Debug.Log("�ӵ� ����");
                canLogSpeedChange = false;
                Invoke("EnableSpeedLog", 1);
            }
        }

        previousSpeed = currentSpeed;

        if (currentSpeed > 40)
            spriteRenderer.color = fastColor;
        else if (currentSpeed < 5)
            spriteRenderer.color = slowColor;
        else
            spriteRenderer.color = normalColor;
    }

    void EnableSpeedLog()
    {
        canLogSpeedChange = true;
    }

    void Update()
    {
        float sideWayVelocity = Vector2.Dot(rb.linearVelocity, transform.right);
        bool isDrifting = rb.linearVelocity.magnitude > 2 && Mathf.Abs(sideWayVelocity) > 1;

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
            if (!smokeRight.isPlaying) smokeRight.Stop();
        }

        LeftTrail.emitting = isDrifting;
        RightTrail.emitting = isDrifting;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boost"))
        {
            acceleration = boostAcceleration;

            if (hasItem)
                spriteRenderer.sprite = carryingBoostSprite;
            else
                spriteRenderer.sprite = boostSprite;

            currentState = CarState.Boosting;
            Debug.Log("�ν�Ʈ ����!");
            Invoke("EndBoost", 2);
        }

        if (other.CompareTag("Chicken") && !hasItem)
        {
            hasItem = true;

            if (currentState == CarState.Boosting)
                spriteRenderer.sprite = carryingBoostSprite;
            else
                spriteRenderer.sprite = carryingSprite;

            currentState = CarState.Carrying;

            // �� ���: ���ǿ��� ������ ������� ��û
            PickupItem item = other.GetComponent<PickupItem>();
            if (item != null)
            {
                item.OnPickup(0.1f); // �Ǵ� 0f ��� ����
            }

            Debug.Log("ġŲ �Ⱦ���");
        }


        if (other.CompareTag("Customer") && hasItem)
        {
            hasItem = false;

            if (currentState == CarState.Boosting)
                spriteRenderer.sprite = boostSprite;
            else
                spriteRenderer.sprite = normalSprite;

            currentState = CarState.Normal;
            score += 10;
            Debug.Log("ġŲ ��޵�! ���� ����: " + score);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        acceleration = slowAcceleration;
        Invoke("ResetAcceleration", 2);
    }

    void ResetAcceleration()
    {
        acceleration = defaultAcceleration;
    }

    void EndBoost()
    {
        acceleration = defaultAcceleration;

        if (hasItem)
            spriteRenderer.sprite = carryingSprite;
        else
            spriteRenderer.sprite = normalSprite;

        currentState = hasItem ? CarState.Carrying : CarState.Normal;
        Debug.Log("�ν�Ʈ ����");
    }
}
