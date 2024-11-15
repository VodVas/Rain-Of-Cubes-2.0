using System.Collections;
using UnityEngine;

public class PositionSetter : MonoBehaviour
{
    [SerializeField] private Transform _spawnArea;
    [SerializeField] protected float _spawnOffsetY = 5f;

    //private Vector3 _cubeSpawnPosition;
    private Vector3 _bombSpawnPosition;
    private bool _hasNewBombPosition = false;

    private void OnEnable()
    {
        //Cube.OnCubeDestroyed += OnCubeDestroyed;
    }

    private void OnDisable()
    {
        //Cube.OnCubeDestroyed -= OnCubeDestroyed;
    }

    public Vector3 GetSpawnPosition<U>() where U : MonoBehaviour
    {
        if (typeof(U) == typeof(Cube))
        {
            return GetRandomPosition();
        }
        else if (typeof(U) == typeof(Bomb) && _hasNewBombPosition)
        {
            _hasNewBombPosition = false;
            return _bombSpawnPosition;
        }
        else
        {
            return transform.position;
        }
    }

    private void OnCubeDestroyed(Vector3 position)
    {
        _bombSpawnPosition = position;
        _hasNewBombPosition = true;

        Spawner<Bomb> bombSpawner = GetComponent<Spawner<Bomb>>();
        if (bombSpawner != null)
        {
            bombSpawner.StartCoroutine(SpawnBomb());
        }
    }

    private Vector3 GetRandomPosition()
    {
        Renderer platformRenderer = _spawnArea.GetComponent<Renderer>();
        Bounds bounds = platformRenderer.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y + _spawnOffsetY;
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    private IEnumerator SpawnBomb()
    {
        yield return null;

        Spawner<Bomb> bombSpawner = GetComponent<Spawner<Bomb>>();

        if (bombSpawner != null)
        {
            Vector3 spawnPosition = GetSpawnPosition<Bomb>();
            bombSpawner.Spawn(spawnPosition);
        }
    }
}