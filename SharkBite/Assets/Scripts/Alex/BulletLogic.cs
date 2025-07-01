using UnityEngine;

public class BulletLogic : MonoBehaviour,IPooledObject
{
    [SerializeField] [Range(1,10)]float time;
    private void Start()
    {
        RespawndObject();
    }
    public void RespawndObject()
    {
        Invoke("DestroyMe", time);
    }

    public void DestroyMe()
    {
        //Destroy(this.gameObject, time);
        Debug.Log("Bullet was invoke destroyed");
        this.gameObject.SetActive(false);
    }

}
