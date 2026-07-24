using UnityEngine;
using TMPro;

public class BombView : MonoBehaviour, IDamageable
{
    [SerializeField] Vector3 countdownTextOffset;
    public FreeBomb Bomb { get; private set; }
    public IDamageable Source { get { return Bomb; } }
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
        Bomb.OnPositionChanged += UpdatePosition;
        Bomb.OnStatusChanged += UpdateView;
        Bomb.OnCleanup += Cleanup;

        countdownObject = new GameObject("Countdown Text");
        countdownObject.transform.parent = transform;
        countdownObject.transform.localPosition = countdownTextOffset;

        countdownText = countdownObject.AddComponent<TextMeshPro>();
        countdownText.fontSize = 12;
        countdownText.alignment = TextAlignmentOptions.Center;
        countdownText.color = Color.white;
        UpdateView();

        Bomb.Position = transform.position;
    }

    private void OnDisable()
    {
        if (Bomb != null)
        {
            Bomb.OnPositionChanged -= UpdatePosition;
        }
    }

    private void Update()
    {
        if (countdownObject != null && mainCam != null)
        {
            countdownObject.transform.rotation = Quaternion.LookRotation(countdownObject.transform.position - mainCam.transform.position);
        }
    }

    private void UpdatePosition()
    {
        transform.position = Bomb.Position;
    }

    private void UpdateView()
    {
        if (countdownText != null)
        {
            countdownText.text = Bomb.Health.ToString();
        }
    }

    private void Cleanup()
    {
        Bomb.OnCleanup -= Cleanup;
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        Bomb.TakeDamage(damage);
    }
}