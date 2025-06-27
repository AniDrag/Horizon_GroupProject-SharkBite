using UnityEngine;

public class BulletLogic : MonoBehaviour,IPooledObject
{

    private void Awake()
    {
    }
    public void RespawndObject()
    {
        Invoke(nameof(DestroyMe),10);
    }

    public void DestroyMe()
    {
        //Destroy(this.gameObject, time);
        this.gameObject.SetActive(false);
    }

}
