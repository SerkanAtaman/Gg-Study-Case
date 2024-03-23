namespace GG.ObjectPooling
{
    public interface IPoolItem<T>
    {
        void OnPoolInitialized(ObjectPool<T> pool);
        void PushBackToPool();
    }
}