using UnityEngine;

public class TestKill : MonoBehaviour
{
    [Header("Settigns")]
    [SerializeField] private float killTime_sec = 3;
    private float timer;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > killTime_sec)
        {
            Destroy(gameObject);
        }
    }
}
