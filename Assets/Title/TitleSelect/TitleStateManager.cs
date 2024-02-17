using System.Collections;
using UniRx;
using UnityEngine;

public class TitleStateManager : MonoBehaviour
{
    public static TitleStateManager instance;
    private TitleMenuStateReactiveProperty _titleMenuState = new TitleMenuStateReactiveProperty();
    public TitleMenuStateReactiveProperty TitleMenuState => _titleMenuState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetState(global::SelectMenuState.TitleGame);
    }

    public bool isOpen()
    {
        switch(_titleMenuState.Value)
        {
            case global::SelectMenuState.Select:
            case global::SelectMenuState.Level:
            case global::SelectMenuState.Audio:
            case global::SelectMenuState.Score:
                return true;
            default:
                return false;
        }
    }

    public bool isGame()
    {
        switch (_titleMenuState.Value)
        {
            case global::SelectMenuState.TitleGame:
                return true;

            default:
                return false;
        }
    }

    public IEnumerator SetState(SelectMenuState state)
    {
        yield return null;
        _titleMenuState.SetValueAndForceNotify(state);
    }

    private void Update()
    {
        
    }

}

public enum SelectMenuState
{
    Idle,
    TitleGame,
    Select,
    Level,
    Audio,
    Score
}

public class TitleMenuStateReactiveProperty : ReactiveProperty<SelectMenuState>
{
    public TitleMenuStateReactiveProperty()
    {
    }
    public TitleMenuStateReactiveProperty(SelectMenuState menuState) : base(menuState)
    {
    }
}
