using UnityEngine;

public class UnitView : MonoBehaviour, IDamageable
{
    public FreeUnit Unit { get; private set; }
    public virtual IDamageable Source { get { return Unit; } }

    public void Init(FreeUnit unit)
    {
        if (unit == null) { return; }

        Unit = unit;
        Unit.OnPositionChanged += UpdatePosition;
        Unit.OnStatusChanged += UpdateView;
        Unit.OnCleanup += Cleanup;

        UpdateView();

        Unit.Position = transform.position;
    }

    protected void OnDisable()
    {
        if (Unit != null)
        {
            Unit.OnPositionChanged -= UpdatePosition;
        }
    }

    protected void UpdatePosition()
    {
        transform.position = Unit.Position;
    }

    protected virtual void UpdateView()
    {
        // Healthbar and other visuals update here
    }

    protected void Cleanup()
    {
        Unit.OnCleanup -= Cleanup;
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        Unit.TakeDamage(damage);
    }
}