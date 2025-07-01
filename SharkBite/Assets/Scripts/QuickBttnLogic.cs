using UnityEngine;
public class QuickBttnLogic : MonoBehaviour
{
    GameObject _target;
    Manager_Sound _audio;
    private void Start()
    {
        _audio = Manager_Sound.instance;
    }
    public void QBCloseChild()
    {
        QBNormalBTNSound();
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void QBCloseGrandChild()
    {
        QBNormalBTNSound();
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    public void QBCloseParent()
    {
        QBNormalBTNSound();
        transform.parent.gameObject.SetActive(false);
    }
    public void QBCloseGrandParent()
    {
        QBNormalBTNSound();
        transform.parent.parent.gameObject.SetActive(false);
    }

    public void QBDisableTransform()
    {
        QBNormalBTNSound();
        gameObject.SetActive(false);        
    }

    public void QBEnableDisableChild()
    {
        if (_target == null)_target = transform.GetChild(0).gameObject;

        _audio.StopSFX();
        _audio.PlaySFX(_audio.checkMarkBTNSound);

        _target.SetActive(!_target.activeSelf);

    }
    public void QBNormalBTNSound()
    {
        _audio.StopSFX();
        _audio.PlaySFX(_audio.normalButtonSound);
    }
}
