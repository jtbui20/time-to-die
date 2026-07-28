using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BombView : UnitView, IBombReference
{
    [SerializeField] Vector3 countdownTextOffset;
    public FreeBomb Bomb { get; private set; }
    public override IDamageable Source { get { return Bomb; } }
    [SerializeField]
    private TextMeshPro countdownText;
    [SerializeField]
    private GameObject countdownObject;
    [SerializeField]
    private GameObject AOEArea;
    private Camera mainCam;

    public void Awake()
    {
        mainCam = Camera.main;
    }

    public void Init(FreeBomb bomb)
    {
        if (bomb == null) { return; }

        Bomb = bomb;

        base.Init(bomb);
    }

    private void Update()
    {
        if (countdownObject != null && mainCam != null)
        {
            countdownObject.transform.rotation = Quaternion.LookRotation(countdownObject.transform.position - mainCam.transform.position);
        }
    }

    protected override void UpdateView()
    {
        if (countdownText != null)
        {
            countdownText.text = Bomb.Health.ToString();
        }

        float rootScale = 1f;
        AOEArea.transform.localScale = new Vector3(Bomb.Range * 1 * rootScale, 0.1f, Bomb.Range * 1 * rootScale);
    }
}