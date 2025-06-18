using UnityEngine;

public class OrbOfXP : MonoBehaviour
{
    public int amountOfXP = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<LevelUp_SYS>().AddXP(amountOfXP);
            Destroy(this.gameObject);
        }
    }
}
