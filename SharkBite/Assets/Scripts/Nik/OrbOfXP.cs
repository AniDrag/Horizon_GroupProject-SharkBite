using UnityEngine;

public class OrbOfXP : MonoBehaviour
{
    [SerializeField] int amountOfXP = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerManager>().AddXP(amountOfXP);
            this.gameObject.SetActive(false);
        }
    }

    public void SetXPamount(int amount) => amountOfXP = amount;
}
