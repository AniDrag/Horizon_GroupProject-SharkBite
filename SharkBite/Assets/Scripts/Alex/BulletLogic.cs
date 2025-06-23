using UnityEngine;

public class BulletLogic : MonoBehaviour
{

    private void Awake()
    {
        DestroyMe(10);
    }

    public void DestroyMe(float time)
    {
        //Destroy(this.gameObject, time);
        this.gameObject.SetActive(false);
    }

}
