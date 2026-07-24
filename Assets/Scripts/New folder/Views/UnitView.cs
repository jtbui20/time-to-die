using UnityEngine;

public class UnitView : MonoBehaviour
{
    public Unit Unit { get; private set; }

    public void Init(Unit unit)
    {
        if (unit == null) { return; }

        Unit = unit;
        Unit.OnPositionChanged += UpdatePosition;
    }

    private void OnDisableI()
    {
        if (Unit != null)
        {
            Unit.OnPositionChanged -= UpdatePosition;
        }
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(Unit.Position.X, 0.5f, Unit.Position.Y);
    }
}