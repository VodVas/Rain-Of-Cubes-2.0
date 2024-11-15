using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IDeathEvent
{
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 5f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 500f;
    [SerializeField] private Material _initialMaterial;
    [SerializeField, Range(0, 1)] private float _finalTransparent = 0;

    private Renderer _renderer;
    private Material _material;

    public event Action<IDeathEvent> Dead;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeAndExplode());
    }

    private IEnumerator FadeAndExplode()
    {
        float lifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);
        float timer = 0f;

        Color startColor = _initialMaterial.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, _finalTransparent);

        while (timer < lifeTime)
        {
            float t = timer / lifeTime;
            _material.color = Color.Lerp(startColor, endColor, t);

            timer += Time.deltaTime;
            yield return null;
        }

        Explode();
        Dead?.Invoke(this);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }
}