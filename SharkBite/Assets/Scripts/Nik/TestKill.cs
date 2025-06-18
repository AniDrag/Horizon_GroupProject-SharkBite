using System.Collections.Generic;
using UnityEngine;

public class TestKill : MonoBehaviour
{
    [SerializeField] private float killTime = 3;
    private float timer;
    Spawner SP;
    private void Start()
    {
        SP = Spawner.instance;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > killTime)
        {
            SP.SPAWN_enemysInScene.Remove(gameObject);
            Destroy(gameObject);
        }

    }
}
