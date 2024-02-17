using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FocusSupporter : MonoBehaviour
{
    private class SelectionHooker : MonoBehaviour, IDeselectHandler
    {
        public FocusSupporter Supporter;

        public void OnDeselect(BaseEventData eventData)
        {
            Supporter.PreviousSelection = eventData.selectedObject;
        }
    }

    [SerializeField]
    private Selectable _inputField;
    [SerializeField]
    private Selectable _directingSelectable;
    [SerializeField]
    private Selectable _menuUIFirstSelectable;
    [SerializeField, Header("ここに残りのメニューUIを全て入れる")]
    private Selectable[] _UISelectables;
    private GameObject PreviousSelection = null;
    private List<GameObject> _selectables = new List<GameObject>();

    private void Awake()
    {
        SetHooker(_inputField);
        SetHooker(_directingSelectable);
        SetHooker(_menuUIFirstSelectable);
        SetUI(_menuUIFirstSelectable);
        foreach(Selectable selectable in _UISelectables)
        {
            SetHooker(selectable);
            SetUI(selectable);
        }
    }

    private void Start()
    {
        GameStateManager.instance.InputableReactiveProperty.Subscribe(value =>
        {
            if (GameStateManager.instance.IsOpenMenu())
            {
                return;
            }

            if (value)
            {
                EventSystem.current.SetSelectedGameObject(_inputField.gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(_directingSelectable.gameObject);
            }
        }).AddTo(GameStateManager.instance.gameObject);

        StartCoroutine(RestrictSelection());
    }

    private void SetHooker(Selectable target)
    {
        var hooker = target.gameObject.AddComponent<SelectionHooker>();
        hooker.Supporter = this;
        _selectables.Add(target.gameObject);
    }

    private void SetUI(Selectable target)
    {
        var menuUI = target.gameObject.AddComponent<MenuItem>();
        menuUI._input = this._inputField;
        menuUI._directing = this._directingSelectable;
        menuUI._menuFirst = this._menuUIFirstSelectable;
    }

    private IEnumerator RestrictSelection()
    {
        while (true)
        {
            yield return new WaitUntil(
                () => (EventSystem.current != null) && (EventSystem.current.currentSelectedGameObject != PreviousSelection) && GameStateManager.instance.IsGame);

            if ((PreviousSelection == null) || _selectables.Contains(EventSystem.current.currentSelectedGameObject))
            {
                continue;
            }

            EventSystem.current.SetSelectedGameObject(PreviousSelection);
        }
    }
}
