using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIPanelManager : MonoBehaviour
{
    [SerializeField]
    private PanelSet _menu, _audio, _cansel;
    [SerializeField]
    private MenuButton _menuButton;

    private void Start()
    {
        if (!TenSceneManager.IsLoaded(Scene.Animation))
        {
            TenSceneManager.AddScene(Scene.Animation);
        }
        if (!TenSceneManager.IsLoaded(Scene.AudioManager))
        {
            TenSceneManager.AddScene(Scene.AudioManager);
        }

        GameStateManager.instance.MenuState.Subscribe(Value =>
        {
            _menuButton.SetUI();
            switch (Value)
            {
                case MenuState.Idle:
                    _menu.ClosePanel();
                    _audio.ClosePanel();
                    _cansel.ClosePanel();
                    break;

                case MenuState.Open:
                    _menu.OpenPanel();
                    _audio.ClosePanel();
                    _cansel.ClosePanel();
                    break;

                case MenuState.Audio:
                    _audio.OpenPanel();
                    break;

                case MenuState.Cansel:
                    _cansel.OpenPanel();
                    break;
            }
        }).AddTo(GameStateManager.instance.gameObject);
    }
}

public class MenuItem : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, ISubmitHandler
{
    public Selectable _input;
    public Selectable _directing;
    public Selectable _menuFirst;
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

        if (!GameStateManager.instance.IsGame)
        {
            return;
        }

        if (GameStateManager.instance.IsOpenMenu())
        {
            EventSystem.current.SetSelectedGameObject(_menuFirst.gameObject);
        }
        else if (GameStateManager.instance.IsInputable)
        {
            EventSystem.current.SetSelectedGameObject(_input.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_directing.gameObject);
        }
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

[System.Serializable]
public class PanelSet
{
    public RectTransform Panel;
    public Selectable FirstSelected;

    public void OpenPanel()
    {
        Panel.gameObject.transform.DOComplete();
        if (Panel.sizeDelta.x > Panel.sizeDelta.y)
        {
            Panel.gameObject.transform.DOScaleX(1, 0.1f).SetEase(Ease.OutQuad).OnStart(() =>
            {
                Panel.gameObject.SetActive(true);
            }).OnComplete(() =>
            {
                EventSystem.current.SetSelectedGameObject(FirstSelected.gameObject);
            });
        }
        else
        {
            Panel.gameObject.transform.DOScaleY(1, 0.1f).SetEase(Ease.OutQuad).OnStart(() =>
            {
                Panel.gameObject.SetActive(true);
            }).OnComplete(() =>
            {
                EventSystem.current.SetSelectedGameObject(FirstSelected.gameObject);
            });
        }
    }

    public void ClosePanel()
    {
        Panel.gameObject.transform.DOComplete();
        if(Panel.sizeDelta.x > Panel.sizeDelta.y)
        {
            Panel.gameObject.transform.DOScaleX(0, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Panel.gameObject.SetActive(false);
            });
        }
        else
        {
            Panel.gameObject.transform.DOScaleY(0, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Panel.gameObject.SetActive(false);
            });
        }
    }
}
