using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    private const int ScoreAddition = 50;
    [SerializeField] private GameObject _FreeTile;
    [SerializeField] private int _FreeTilesCount = 3;
    [SerializeField] private GameObject[] _GameTiles;
    [SerializeField] private int _SameTilesCount = 3;
    [SerializeField] private int _TileOffset = 10;
    [SerializeField] private float _StartLevelSpeed = 5;
    [SerializeField] private float _SpeedAcceleration = 0.5f;
    [SerializeField] private int _VisibleTilesCount = 10;
    [SerializeField] private Player _Player;
    
    private List<GameObject> _GameTilesPool;
    private Queue<GameObject> _ActiveTiles;

    private float _CurrentSpeed;
    private int _Score;
    private Camera _Camera;

    private void Awake()
    {
        _Camera = Camera.main;
    }

    private void Start()
    {
        _ActiveTiles = new Queue<GameObject>();
        _GameTilesPool = new List<GameObject>();
        _CurrentSpeed = _StartLevelSpeed;
        _Score = 0;
        for (int i = 0; i < _FreeTilesCount; i++)
        {
            var freeTile = Instantiate(_FreeTile, transform);
            _ActiveTiles.Enqueue(freeTile);
            freeTile.SetActive(true);
            freeTile.transform.position =  _TileOffset * i * Vector3.forward;
        }
        foreach (var gameTile in _GameTiles)
        {
            for (int i = 0; i < _SameTilesCount; i++)
            {
                _GameTilesPool.Add(Instantiate(gameTile, transform));
                _GameTilesPool[^1].SetActive(false);
            }
        }
        var currentActiveTiles = _FreeTilesCount;
        while (currentActiveTiles < _VisibleTilesCount)
        {
            var tile = GetRandomInactiveTile();
            _ActiveTiles.Enqueue(tile);
            tile.SetActive(true);
            tile.transform.position = currentActiveTiles * _TileOffset * Vector3.forward;
            currentActiveTiles++;
        }
    }

    private void Update()
    {
        transform.position -= _CurrentSpeed * Time.deltaTime * Vector3.forward;
        if (transform.position.z <= -_TileOffset)
        {
            var levelTransform = transform;
            var pos = levelTransform.position;
            pos.z = pos.z % _TileOffset;
            levelTransform.position = pos;
            UpdateTiles();
        }
    }

    private GameObject GetRandomInactiveTile()
    {
        var inactiveCount  = _GameTilesPool.Count(x => !x.activeSelf);
        var inactiveTiles  = _GameTilesPool.Where(x => !x.activeSelf);
        return inactiveTiles.ElementAt(Random.Range(0, inactiveCount));
    }
    private void UpdateTiles()
    {
        var tile = _ActiveTiles.Dequeue();
        tile.gameObject.SetActive(false);
        tile = GetRandomInactiveTile();
        tile.SetActive(true);
        _ActiveTiles.Enqueue(tile);
        int pos = 0;
        _CurrentSpeed += _SpeedAcceleration;
        _Score += ScoreAddition;
        WebglBridge.UpdateScore(_Score);
        var playerPos = _Camera.WorldToViewportPoint(_Player.transform.position);
        WebglBridge.SetFloatingText(playerPos.x, playerPos.y, $"+{ScoreAddition}");
        
        foreach (var activeTile in _ActiveTiles)
        {
            activeTile.transform.position = pos * _TileOffset * Vector3.forward;
            pos++;
        }
    }
}
