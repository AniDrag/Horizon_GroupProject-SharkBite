using UnityEngine;
using UnityEngine.InputSystem;
public class MovableJoistick : MonoBehaviour
{
    RectTransform _l;
    [SerializeField] RectTransform _r;
    [SerializeField] Camera _c;
    [SerializeField] RectTransform _joistick;
    [SerializeField] PlayerInput _playerInput;

    private void Start()
    {
        _l = GetComponent<RectTransform>();
        _r = transform.parent.GetComponent<RectTransform>();
        int targetVal = (int)(Screen.width * 0.23);
       _l.sizeDelta = new Vector2(targetVal, _l.sizeDelta.y);
    }
    public void MoveJoistick()
    {
        Vector2 _moveDirection = Camera.main.ScreenToViewportPoint(_playerInput.actions["Press"].ReadValue<Vector2>());
        Debug.Log("click was at" + _moveDirection);
        Vector2 returnVector;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_r, _moveDirection, _c,out returnVector);
        Debug.Log("Press location =" + returnVector.x + " / " + returnVector.y);
        _joistick.anchoredPosition = returnVector;
    }
}
