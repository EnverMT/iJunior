using UnityEngine;
using UnityEngine.Pool;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Target _target;
    [SerializeField] private Color _color;
    [SerializeField] private Enemy _enemy;
    [SerializeField, Range(1, 10)] private float _speed;

    private ObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(OnEnemyCreate, OnEnemyGet, OnEnemyRelease, OnEnemyDestroy);
    }

    public Enemy Spawn()
    {
        Enemy enemy = _enemyPool.Get();

        enemy.Init(gameObject.transform.position, _target, _speed, _color);
        enemy.gameObject.transform.SetParent(gameObject.transform, true);
        enemy.Died += OnEnemyDied;

        return enemy;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        _enemyPool.Release(enemy);
    }

    #region EnemyObjectPoolMethods 
    private Enemy OnEnemyCreate()
    {
        return Instantiate(_enemy);
    }

    private void OnEnemyGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnEnemyRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnEnemyDestroy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
    #endregion
}
