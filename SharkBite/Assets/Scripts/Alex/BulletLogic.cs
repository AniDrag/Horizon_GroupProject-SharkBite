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
        Debug.Log("I'm in DAAANGER");
        this.gameObject.SetActive(false);
    }

}
