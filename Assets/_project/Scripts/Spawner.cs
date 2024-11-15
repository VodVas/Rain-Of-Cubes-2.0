using System.Collections;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private float _spawnDelay = 1f;

    private WaitForSeconds _wait;
    private PositionSetter _positionSetter;

    private void Awake()
    {
        _wait = new WaitForSeconds(_spawnDelay);
        _positionSetter = GetComponent<PositionSetter>();
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnRepeatedly());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnRepeatedly()
    {
        while (true)
        {
            yield return _wait;

            Vector3 spawnPosition = _positionSetter.GetSpawnPosition<T>();
            Spawn(spawnPosition);
        }
    }

    public void Spawn(Vector3 position)
    {
        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(0f, 360f),
            Random.Range(0f, 360f),
            Random.Range(0f, 360f));

        //T instance = Instantiate(_prefab, position, randomRotation);
        Instantiate(_prefab, position, randomRotation);
    }
}