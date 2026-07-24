using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject unitPrefab;
    private Stage stage;
    public Stage Stage { get { return stage; } }

    private int timer = 0;

    [SerializeField] EnemyDefinition enemyDefinition;
    private Enemy unit;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this); 
            return;
        }
        Instance = this;

        stage = new Stage(5, 5);
        if (tilePrefab)
        {
            for (int i = 0; i < stage.Size; i++)
            {
                GameObject tileView = Instantiate(tilePrefab);
                GridCoords position = stage.TryGetCoordsUnchecked(i);
                tileView.transform.position = new Vector3(position.X, 0, position.Y);

                TileView tileScript = tileView.GetComponent<TileView>();
                if (tileScript)
                {
                    tileScript.Init(i);
                }
            }
        }

        unit = new Enemy(enemyDefinition);
        if (unitPrefab)
        {
            GameObject unitView = Instantiate(unitPrefab);

            UnitView unitScript = unitView.GetComponent<UnitView>();
            if (unitScript)
            {
                unitScript.Init(unit);
            }
        }
        stage.ForceMoveUnit(unit, 0);
    }

    private void Update()
    {
        timer++;
        if (timer > 60)
        {
            int dest;
            do
            {
                dest = UnityEngine.Random.Range(0, stage.Size);
                Debug.Log($"Attempted to move unit to tile {dest}: {stage.TryGetCoordsUnchecked(dest).ToString()}");
            } while (stage.IsOccupied(dest));
            stage.ForceMoveUnit(unit, dest);
            timer = 0;
        }
    }
}