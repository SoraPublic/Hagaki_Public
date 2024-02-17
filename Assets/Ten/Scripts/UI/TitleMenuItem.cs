using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleMenuItem :　MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, ISubmitHandler
{
    private Image _line;

    private void Awake()
    {
        _line = gameObject.transform.Find("UnderLine").GetComponent<Image>();
    }

    private void OnDisable()
    {
        gameObject.transform.DOKill();
        gameObject.transform.DOScale(0.75f, 0.1f).SetEase(Ease.OutQuart);
        _line.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameObject.transform.DOKill();
        gameObject.transform.DOScale(1, 0.1f).SetEase(Ease.OutQuart);
        _line.gameObject.SetActive(true);
        AudioManager.instance.OnSelectUI.Play();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.transform.DOKill();
        gameObject.transform.DOScale(0.75f, 0.1f).SetEase(Ease.OutQuart);
        _line.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.instance.OnSubmitUI.Play();
    }
}
