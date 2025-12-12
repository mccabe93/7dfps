using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Direction = Vector3.forward;
    public float Speed = 50f;
    public float Mass = 1.0f;
    public float LifeTime = 5.0f;
    public float Damage = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = Mass;
        rb.AddForce(Direction.normalized * Speed, ForceMode.VelocityChange);

        Destroy(gameObject, LifeTime);
    }
}
