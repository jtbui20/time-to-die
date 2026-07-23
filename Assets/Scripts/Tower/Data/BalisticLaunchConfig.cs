using UnityEngine;

[CreateAssetMenu(fileName = "BalisticLaunchConfig", menuName = "Scriptable Objects/Tower/BalisticLaunchConfig")]
public class BalisticLaunchConfig : ScriptableObject
{
    // launch angle
    // launch velocity
    public float MinAngle;
    public float MaxAngle;
    public float MinVelocity;
    public float MaxVelocity;
    public float MinProjectileHoming;
    public float MaxProjectileHoming;
    public float Lifetime;
}
