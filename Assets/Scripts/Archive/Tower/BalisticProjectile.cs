using UnityEngine;

public class BalisticProjectile : MonoBehaviour, IProjectile
{
    private Rigidbody rb;
    private Collider collider;
    
    private float damage = 0f;
    private Transform target;
    private float minHoming = 0f;
    private float maxHoming = 0f;
    private float speed = 0f;
    private float maxDistance = 0f;
    public float lifetime = 3f;
    private bool isActive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void Initialise(ProjectileStats stats)
    {
        damage = stats.Damage;
        rb.linearVelocity = stats.Direction;
        speed = stats.Speed;
        minHoming = stats.MinHoming;
        maxHoming = stats.MaxHoming;
        lifetime = stats.Lifetime;
        target = stats.Target;

        maxDistance = Helper.FlattenedDistance(transform.position, target.position);
        isActive = true;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0f) { Destroy(gameObject); }
    }

    private void FixedUpdate()
    {
        if (target == null) { return; }
        if (!isActive) { return; }

        float distance = Helper.FlattenedDistance(transform.position, target.position);
        float t = Mathf.Clamp01(distance / maxDistance);

        float homingStrength = Mathf.Lerp(maxHoming, minHoming, t);

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newDirection = Vector3.RotateTowards( rb.linearVelocity.normalized, direction, homingStrength * Time.fixedDeltaTime, 0f );

        rb.linearVelocity = newDirection * speed;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log($"{this.name} collied with {col.gameObject.name}");

        IDamageable target = col.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage((int)damage);
            Destroy(gameObject);
        }

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        collider.enabled = false;
        isActive = false;
    }
}