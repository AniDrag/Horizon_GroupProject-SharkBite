using UnityEngine;

public class OrbOfXP : MonoBehaviour
{
    public int amountOfXP = 1;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.GetComponent<LevelUp_SYS>().AddXP(amountOfXP);
        }
    }
}
