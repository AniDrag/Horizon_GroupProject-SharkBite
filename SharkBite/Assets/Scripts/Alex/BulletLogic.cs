using UnityEngine;

public class BulletLogic : MonoBehaviour
{

    private void Awake()
    {
        DestroyMe();
    }

    private void DestroyMe()
    {
        Destroy(this.gameObject, 10f);
    }
}
