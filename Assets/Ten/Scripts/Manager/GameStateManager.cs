using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    private bool _isGame; // タイマー開始から終了までTrue
    public bool IsGame => _isGame;
    public void StartGame()
    {
        _isGame = true;
    }
    public void EndGame()
    {
        _isGame = false;
    }

    private bool _isInterrupt = false;
    public bool IsInterrupt => _isInterrupt;
    public void Interrupt()
    {
        _isInterrupt = true;
    }

    private BoolReactiveProperty _isInputable = new BoolReactiveProperty();
    public BoolReactiveProperty InputableReactiveProperty => _isInputable;
    public bool IsInputable
    {
        get { return _isInputable.Value; }
    }
    public void SetInputable(bool value)
    {
        _isInputable.SetValueAndForceNotify(value);
    }

    private MenuStateReactiveProperty _menuState = new MenuStateReactiveProperty();
    public MenuStateReactiveProperty MenuState => _menuState;
    public bool IsOpenMenu()
    {
        switch (_menuState.Value)
        {
            case global::MenuState.Open:
                return true;

            case global::MenuState.Audio:
                return true;

            case global::MenuState.Cansel:
                return true;

            default:
                return false;
        }
    }
    public bool IsOpenAudioMenu()
    {
        switch (_menuState.Value)
        {
            case global::MenuState.Audio:
                return true;

            default:
                return false;
        }
    }
    public bool IsOpenCanselMenu()
    {
        switch (_menuState.Value)
        {
            case global::MenuState.Cansel:
                return true;

            default:
                return false;
        }
    }
    public void SetMenuState(MenuState state)
    {
        StartCoroutine(SetMenuAsync(state));
    }
    public void SetMenu()
    {
        StartCoroutine(SetMenuAsync(global::MenuState.Open));
    }
    public void SetAudio()
    {
        StartCoroutine(SetMenuAsync(global::MenuState.Audio));
    }
    public void SetCansel()
    {
        StartCoroutine(SetMenuAsync(global::MenuState.Cansel));
    }
    public void SetIdle()
    {
        StartCoroutine(SetMenuAsync(global::MenuState.Idle));
    }
    public void ReverseMenu()
    {
        switch (_menuState.Value)
        {
            case global::MenuState.Open:
            case global::MenuState.Audio:
            case global::MenuState.Cansel:
                StartCoroutine(SetMenuAsync(global::MenuState.Idle));
                break;

            default:
                StartCoroutine(SetMenuAsync(global::MenuState.Open));
                break;
        }
    }

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
        _menuState.SetValueAndForceNotify(global::MenuState.Idle);
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && IsGame)
        {
            AudioManager.instance.OnSubmitUI.Play();
            ReverseMenu(); // メニュー画面の表示切り替え
        }
    }

    private IEnumerator SetMenuAsync(MenuState state)
    {
        yield return null;
        _menuState.SetValueAndForceNotify(state);
    }
}

public enum MenuState
{
    Idle,
    Open,
    Audio,
    Cansel
}

public class MenuStateReactiveProperty : ReactiveProperty<MenuState>
{
    public MenuStateReactiveProperty()
    {
    }
    public MenuStateReactiveProperty(MenuState menuState) : base(menuState)
    {
    }
}