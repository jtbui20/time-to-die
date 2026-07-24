using UnityEngine;

public class TileView : MonoBehaviour
{
    public int TileIndex { get; private set; }

    public void Init(int index)
    {
        TileIndex = index;
    }
}