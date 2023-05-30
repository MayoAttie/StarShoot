using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Controller : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image joystickBgImage;  // 조이스틱 배경 이미지
    private Image joystickImage;    // 조이스틱 이미지

    private Vector2 inputVector;    // 입력 벡터

    private void Start()
    {
        joystickBgImage = GetComponent<Image>();
        joystickImage = transform.GetChild(1).GetComponent<Image>();
    }

    // 드래그시 실행되는 함수
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBgImage.rectTransform,
            eventData.position, eventData.pressEventCamera, out pos))
        {
            // 조이스틱 배경의 반지름 계산
            pos.x = (pos.x / joystickBgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBgImage.rectTransform.sizeDelta.y);

            float x = (joystickBgImage.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (joystickBgImage.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            inputVector = new Vector2(x, y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // 조이스틱 이미지 이동
            joystickImage.rectTransform.anchoredPosition =
                new Vector2(inputVector.x * (joystickBgImage.rectTransform.sizeDelta.x / 3),
                            inputVector.y * (joystickBgImage.rectTransform.sizeDelta.y / 3));
        }
    }

    // 클릭시 실행되는 함수
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    // 클릭 해제시 실행되는 함수
    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    // 입력 벡터 값 반환 함수
    public float GetHorizontalValue()
    {
        return inputVector.x;
    }

    public float GetVerticalValue()
    {
        return inputVector.y;
    }
}