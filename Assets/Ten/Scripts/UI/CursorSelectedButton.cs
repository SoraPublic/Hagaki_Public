using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorSelectedButton : Button, IPointerEnterHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            return;
        }

        base.OnPointerUp(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            return;
        }

        base.OnPointerDown(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            return;
        }

        base.OnPointerClick(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
