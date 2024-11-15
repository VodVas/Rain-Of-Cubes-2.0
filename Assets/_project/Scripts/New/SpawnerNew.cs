using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnerNew<T> : MonoBehaviour where T : MonoBehaviour, IDeathEvent
{
    [SerializeField] private T _objectPrefab;
    [SerializeField] private Transform _platformTransform;
    [SerializeField] private float _offsetY = -1f;
    [SerializeField] private float _spawnDelay;

    private ObjectPool<T> _objectPool;

    private void Start()
    {
        _objectPool = new ObjectPool<T>(CreateObject, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, true, 10, 15);

        StartCoroutine(SpawnRepeated());
    }

    private void OnDisable()
    {
        _objectPool.Clear();
    }

    private IEnumerator SpawnRepeated()
    {
        while (true)
        {
            var wait = new WaitForSeconds(_spawnDelay);

            yield return wait;

            SpawnCube();
        }
    }

    private T CreateObject()
    {
        float maxAngleRotate = 360f;

        float angleX = Random.Range(0f, maxAngleRotate);
        float angleY = Random.Range(0f, maxAngleRotate);
        float angleZ = Random.Range(0f, maxAngleRotate);

        Quaternion randomRotation = Quaternion.Euler(angleX, angleY, angleZ);

        T obj = Instantiate(_objectPrefab, GetRandomPosition(), randomRotation);

        return obj;
    }

    private void HandleObjectDeath(IDeathEvent deadObject)
    {
        _objectPool.Release((T)deadObject);
    }

    private void OnGetFromPool(T obj)
    {
        obj.Dead += HandleObjectDeath;
        obj.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(T obj)
    {
        obj.Dead -= HandleObjectDeath;
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(T obj)
    {
        if (obj != null)
        {
            Destroy(obj.gameObject);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Renderer platformRenderer = _platformTransform.GetComponent<Renderer>();
        Bounds bounds = platformRenderer.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y - _offsetY;
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    private void SpawnCube()
    {
        T obj = _objectPool.Get();
        obj.transform.position = GetRandomPosition();
    }
}