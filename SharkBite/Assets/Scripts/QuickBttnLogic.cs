using UnityEngine;
public class QuickBttnLogic : MonoBehaviour
{
    GameObject _target;
   public void QBCloseChild()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void QBCloseParent()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void QBDisableTransform()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void QBEnableDisableChild()
    {
        if (_target == null)_target = transform.GetChild(0).gameObject;

        _target.SetActive(!_target.activeSelf);
    }
}
