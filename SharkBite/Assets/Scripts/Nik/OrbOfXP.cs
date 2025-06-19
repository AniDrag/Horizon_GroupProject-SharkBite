using UnityEngine;

public class OrbOfXP : MonoBehaviour
{
    public int amountOfXP = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerManager>().AddXP(amountOfXP);
            Destroy(this.gameObject);
        }
    }

    public void SetXPamount(int amount) => amountOfXP = amount;
}
