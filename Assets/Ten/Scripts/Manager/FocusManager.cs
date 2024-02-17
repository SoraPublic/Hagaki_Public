using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 構造上、配列に空要素があるとエラーが発生するので、気をつけて下さい。
/// </summary>
public class FocusManager : MonoBehaviour
{
    /// <summary>
    /// <see cref="Selectable"/> をフックするクラスです。
    /// </summary>
    private class SelectionHooker : MonoBehaviour, IDeselectHandler
    {
        public FocusManager Manager;

        /// <summary>
        /// 選択解除時にそれまで選択されていたオブジェクトを覚えておく。
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDeselect(BaseEventData eventData)
        {
            Manager.PreviousSelection = eventData.selectedObject;
        }
    }

    [System.Serializable]
    private class SelectablesGroup
    {
        [Header("この組み合わせで選択させたいオブジェクト一覧")]
        public Selectable[] items;
        [HideInInspector]
        public List<GameObject> Selectables = new List<GameObject>();
        public void SetHooker(FocusManager manager)
        {
            foreach(Selectable target in items)
            {
                var hooker = target.gameObject.AddComponent<SelectionHooker>();
                hooker.Manager = manager;
                Selectables.Add(target.gameObject);
            }
        }
    }

    [SerializeField, Header("選択させたいオブジェクトを選択可能な組み合わせ毎にに分けて登録する")]
    private SelectablesGroup[] _selectablesGroups;
    private GameObject PreviousSelection = null;
    [SerializeField, Header("開始時点で選択される組み合わせ(先頭のオブジェクトが最初に選択される)")]
    private IntReactiveProperty _stateNum = new IntReactiveProperty();
    public IntReactiveProperty StateNum => _stateNum;
    public int GetStateNum()
    {
        return _stateNum.Value;
    }
    public void SetReactiveStateNum(int value)
    {
        _stateNum.SetValueAndForceNotify(value);
    }

    /// <summary>
    /// 選択組み合わせを変更する。変更後、組み合わせ内の先頭のオブジェクトにフォーカスする。
    /// </summary>
    /// <param name="newValue"></param>
    public void SetStateNum(int newValue)
    {
        if(newValue > _selectablesGroups.Length - 1)
        {
            Debug.LogAssertion("選択組み合わせ数超過");
            return;
        }
        SetReactiveStateNum(newValue);
        EventSystem.current.SetSelectedGameObject(_selectablesGroups[GetStateNum()].Selectables[0]);
    }

    public void GotoNextState()
    {
        if(GetStateNum() + 1 > _selectablesGroups.Length - 1)
        {
            Debug.LogAssertion("選択組み合わせ数超過");
            return;
        }
        SetStateNum(GetStateNum() + 1);
    }

    public void GotoPrevState()
    {
        if(GetStateNum() -1 < 0)
        {
            Debug.LogAssertion("選択組み合わせ数超過");
            return;
        }
        SetStateNum(GetStateNum() - 1);
    }

    private void Awake()
    {
        foreach(var group in _selectablesGroups)
        {
            group.SetHooker(this);
        }
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(_selectablesGroups[GetStateNum()].Selectables[0]);
        StartCoroutine(RestrictSelection());
    }

    private IEnumerator RestrictSelection()
    {
        while (true)
        {
            yield return new WaitUntil(
                () => (EventSystem.current != null) && (EventSystem.current.currentSelectedGameObject != PreviousSelection));

            if ((PreviousSelection == null) || _selectablesGroups[GetStateNum()].Selectables.Contains(EventSystem.current.currentSelectedGameObject))
            {
                continue;
            }

            EventSystem.current.SetSelectedGameObject(PreviousSelection);
        }
    }
}
