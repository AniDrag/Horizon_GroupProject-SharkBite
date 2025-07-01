using UnityEngine;

public class PlatoonShooting : MonoBehaviour
{


    private float _timeReset;
    private PlayerStats _playerStats;
    private Pooler _itemPooler;
    void Start()
    {
        _playerStats = PlayerManager.instance.playerStats;
        _itemPooler = Pooler.instance;
    }

    private void FixedUpdate()
    {
        _timeReset += Time.deltaTime;
        if (_timeReset >= _playerStats.GetFireRate())
        {
            _timeReset = 0;

            Shoot(_playerStats.GetFireRate());
        }
    }

    private void Shoot(float cd)
    {
        Vector3 direction = transform.position - GameManager.instance._playerPos;
        GameObject newBullet = _itemPooler.SpawnFromPool("Bullet", transform.position + direction.normalized, Quaternion.identity);
        newBullet.GetComponent<Damage>().SetDamage(_playerStats.GetBulletDamage());

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;

            rb.AddForce(direction * _playerStats.GetBulletSpeed() * 200, ForceMode.Force);
        }
    }


}
