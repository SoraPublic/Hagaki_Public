using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SelectPanelBase : MonoBehaviour
{
    private bool _isOpen;
    private GameObject _currentButton;
    private RectTransform _myTransform;
    [SerializeField]
    private SelectState _myState;
    [SerializeField]
    private FocusManager _focusManager;

    private void Awake()
    {
        _myTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _currentButton = EventSystem.current.currentSelectedGameObject;
        ClosePanel().Forget();
    }

    private void Update()
    {
        if(_currentButton == null)
        {
            _currentButton = EventSystem.current.currentSelectedGameObject;
            if (_currentButton != null && _currentButton.GetComponent<SelectButtonView>())
            {
                _currentButton.GetComponent<SelectButtonView>().Highlighted();
            }
            return;
        }

        if (_isOpen && _currentButton != EventSystem.current.currentSelectedGameObject)
        {
            if (_currentButton.GetComponent<SelectButtonView>())
            {
                _currentButton.GetComponent <SelectButtonView>().Unhighlighted();
            }
            _currentButton = EventSystem.current.currentSelectedGameObject;
            if(_currentButton!=null && _currentButton.GetComponent<SelectButtonView>())
            {
                _currentButton.GetComponent<SelectButtonView>().Highlighted();
            }
        }
    }

    public async UniTask OpenPanel()
    {
        _myTransform.gameObject.transform.DOComplete();
        if (_myTransform.sizeDelta.x > _myTransform.sizeDelta.y)
        {
           await this.gameObject.transform.DOScaleX(1, 0.1f).SetEase(Ease.OutQuad).OnStart(() =>
            {
                this.gameObject.SetActive(true);
                _isOpen = true;
            }).OnComplete(() =>
            {
                _focusManager.SetStateNum((int)_myState);
            });
        }
        else
        {
            await _myTransform.gameObject.transform.DOScaleY(1, 0.1f).SetEase(Ease.OutQuad).OnStart(() =>
            {
                _myTransform.gameObject.SetActive(true);
                _isOpen = true;
            }).OnComplete(() =>
            {
                _focusManager.SetStateNum((int)_myState);
            });
        }
    }

    public async UniTask ClosePanel()
    {
        this.gameObject.transform.DOComplete();
        if (_myTransform.sizeDelta.x > _myTransform.sizeDelta.y)
        {
            await this.gameObject.transform.DOScaleX(0, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _isOpen = false;
                gameObject.SetActive(false);
            });
        }
        else
        {
            await this.gameObject.transform.DOScaleY(0, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _isOpen = false;
                gameObject.SetActive(false);
            });
        }
    }
}

public enum SelectState
{
    Select = 0,
    Level = 1,
    Audio = 2,
    Score = 3,
}