using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MenuButton : Selectable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _UI;
    [SerializeField]
    private Sprite _select, _deselect;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && GameStateManager.instance.IsGame)
        {
            GameStateManager.instance.ReverseMenu();
            gameObject.transform.DOComplete();
            gameObject.transform.DOScale(0.75f, 0.05f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
            AudioManager.instance.OnSubmitUI.Play();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.DOKill();
        gameObject.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.DOKill();
        gameObject.transform.DOScale(1, 0.1f).SetEase(Ease.OutQuad);
    }

    public void SetUI()
    {
        if (GameStateManager.instance.IsOpenMenu())
        {
            _UI.sprite = _select;
        }
        else
        {
            _UI.sprite = _deselect;
        }
    }
}
