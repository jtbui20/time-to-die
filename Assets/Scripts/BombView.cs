using UnityEngine;

public class BombView : MonoBehaviour
{
    public FreeBomb Bomb { get; private set; }

    public void Init(FreeBomb bomb)
    {
        if (bomb == null) { return; }

        Bomb = bomb;
        Bomb.OnPositionChanged += UpdatePosition;
        Bomb.OnCleanup += Cleanup;
    }

    private void OnDisableI()
    {
        if (Bomb != null)
        {
            Bomb.OnPositionChanged -= UpdatePosition;
        }
    }

    private void UpdatePosition()
    {
        transform.position = Bomb.Position;
    }

    private void Cleanup()
    {
        Bomb.OnCleanup -= Cleanup;
        Destroy(gameObject);
    }
}