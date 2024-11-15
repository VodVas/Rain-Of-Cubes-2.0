using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, IDeathEvent
{
    [SerializeField] private float _minLifeTime = 0.1f;
    [SerializeField] private float _maxLifeTime = 5f;

    private bool _hasCollided = false;
    private Renderer _renderer;

    public event Action<IDeathEvent> Dead;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.gameObject.TryGetComponent(out Platform _))
        {
            _hasCollided = true;
            SetRandomColor();
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);

        var wait = new WaitForSeconds(lifeTime);

        yield return wait;

        Dead?.Invoke(this);
    }

    private void SetRandomColor()
    {
        float maxValue = 1f;

        _renderer.material.color = Random.ColorHSV();
        _renderer.material.SetFloat("Metallic", maxValue);
        _renderer.material.SetFloat("_Smoothness", maxValue);
    }
}