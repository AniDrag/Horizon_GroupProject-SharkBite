using UnityEngine;

public class BulletLogic : MonoBehaviour,IPooledObject
{

    private void Awake()
    {
        RespawndObject();
    }
    public void RespawndObject()
    {
        DestroyMe(10);
    }

    public void DestroyMe(float time)
    {
        //Destroy(this.gameObject, time);
        this.gameObject.SetActive(false);
    }

}
