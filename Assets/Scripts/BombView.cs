using UnityEngine;
using TMPro;

public class BombView : UnitView
{
    [SerializeField] Vector3 countdownTextOffset;
    public FreeBomb Bomb { get; private set; }
    public override IDamageable Source { get { return Bomb; } }
    private TextMeshPro countdownText;
    private GameObject countdownObject;
    private Camera mainCam;

    public void Awake()
    {
        mainCam = Camera.main;
    }

    public void Init(FreeBomb bomb)
    {
        if (bomb == null) { return; }

        Bomb = bomb;

        countdownObject = new GameObject("Countdown Text");
        countdownObject.transform.parent = transform;
        countdownObject.transform.localPosition = countdownTextOffset;

        countdownText = countdownObject.AddComponent<TextMeshPro>();
        countdownText.fontSize = 12;
        countdownText.alignment = TextAlignmentOptions.Center;
        countdownText.color = Color.white;

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
    }
}