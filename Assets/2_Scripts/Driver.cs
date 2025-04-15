using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] float turnSpeed = 60;
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float slowSpeedRatio = 1.5f;
    [SerializeField] float boostSpeedRatio = 5;
    float slowSpeed;
    float boostSpeed;

    void Start()
    {
        slowSpeed = moveSpeed * slowSpeedRatio;
        boostSpeed = moveSpeed * boostSpeedRatio;
    }

    private void Update()
    {
        float turnamount = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        float moveamount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Rotate(0, 0, -turnamount);
        transform.Translate(0, moveamount, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            moveSpeed = boostSpeed;
            Debug.Log("Boost!!!!!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        moveSpeed = slowSpeed;
        
    }
}
