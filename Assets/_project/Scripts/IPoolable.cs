using UnityEngine.Pool;

public interface IPoolable
{
    void SetPool(ObjectPool<IPoolable> pool);
}