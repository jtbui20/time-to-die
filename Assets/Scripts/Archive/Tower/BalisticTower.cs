using UnityEngine;

public class BalisticTower : Tower
{
    [SerializeField] protected BalisticLaunchConfig balisticConfig;
    [SerializeField] protected GameObject projectilePrefab;

    protected override void Track()
    {
        if (target == null) { return; }
        float distance = Vector3.Distance(transform.position, new Vector3(target.Position.x, transform.position.y, target.Position.z));
        if (distance > towerData.Vision)
        {
            target = null;
            return;
        }

        float velocity = Mathf.Lerp(balisticConfig.MinVelocity, balisticConfig.MaxVelocity, Mathf.Clamp01(distance / towerData.Range));
        float travelTime = distance / velocity;

        Vector3 predictedPosition = target.Position + target.Velocity * travelTime;

        transform.LookAt(new Vector3(predictedPosition.x, transform.position.y, predictedPosition.z));
    }

    protected override bool Shoot()
    {
        if (!base.Shoot()) { return false; }
        if (balisticConfig == null) { return false; }

        float distance = Vector3.Distance(transform.position, target.Position);
        float t = Mathf.Clamp01(distance / towerData.Range);

        float angle = Mathf.Lerp(balisticConfig.MinAngle, balisticConfig.MaxAngle, t);
        float velocity = Mathf.Lerp(balisticConfig.MinVelocity, balisticConfig.MaxVelocity, t);

        //Vector3 direction = (target.Position - transform.position).normalized;
        Vector3 direction = target.Position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        Vector3 launchDirection = Quaternion.AngleAxis(-angle, transform.right) * transform.forward;

        Vector3 launchVelocityVector = launchDirection * velocity;
        Vector3 launchPosition = launcherPoint != null ? launcherPoint.position : transform.position;

        IProjectile projectile = GameObject.Instantiate(projectilePrefab, launchPosition, Quaternion.LookRotation(launchDirection)).GetComponent<IProjectile>();
        ProjectileStats stats = new ProjectileStats(towerData.Damage, launchDirection * velocity, velocity, balisticConfig.MinProjectileHoming, balisticConfig.MaxProjectileHoming, balisticConfig.Lifetime, target.transform);
        projectile.Initialise(stats);

        return true;
    }
}