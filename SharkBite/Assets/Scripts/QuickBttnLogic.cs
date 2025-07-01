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
        NormalBTNSound();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void QBCloseParent()
    {
        NormalBTNSound();
        transform.parent.gameObject.SetActive(false);
    }

    public void QBDisableTransform()
    {
        NormalBTNSound();
        gameObject.SetActive(false);        
    }

    public void QBEnableDisableChild()
    {
        if (_target == null)_target = transform.GetChild(0).gameObject;

        _audio.StopSFX();
        _audio.PlaySFX(_audio.checkMarkBTNSound);

        _target.SetActive(!_target.activeSelf);

    }
    void NormalBTNSound()
    {
        _audio.StopSFX();
        _audio.PlaySFX(_audio.normalButtonSound);
    }
}
