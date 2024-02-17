using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SimpleSelectAction : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerClickHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        gameObject.transform.DOComplete();
        gameObject.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutQuad);
        AudioManager.instance.OnSelectUI.Play();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.transform.DOComplete();
        gameObject.transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        gameObject.transform.DOComplete();
        gameObject.transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        AudioManager.instance.OnSubmitUI.Play();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        gameObject.transform.DOComplete();
        gameObject.transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
    }
}
