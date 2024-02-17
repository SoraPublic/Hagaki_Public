using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleSelectManager : MonoBehaviour
{
    public static TitleSelectManager instance;
    private bool _isGame = true;
    public bool IsGame => _isGame;
    private bool _isOpen = false;
    [SerializeField]
    private GameObject _titleUI;
    private SelectState _nowState;
    [SerializeField]
    private List<SelectPanelBase> _panelList;
    [SerializeField]
    private List<SelectState> _stateList = new List<SelectState>
    {
        SelectState.Select,
        SelectState.Level,
        SelectState.Audio,
        SelectState.Score
    };
    private Dictionary<SelectState, SelectPanelBase> _panelDict = new Dictionary<SelectState, SelectPanelBase>();

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        GetDictionary();
        foreach(SelectPanelBase panel in _panelList)
        {
            panel.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        SaveManager.Load();

        if (!TenSceneManager.IsLoaded(Scene.AudioManager))
        {
            TenSceneManager.AddScene(Scene.AudioManager);
        }        
    }

    private async void Update()
    {
        if(_isGame)
        {
            return;
        }

        if(!_isOpen && Input.GetKeyDown(KeyCode.Return))
        {
            AudioManager.instance.OnSubmitUI.Play();
            await OpenPanel(SelectState.Select);
        }
    }

    public void StartSelect()
    {
        _isGame=false;
        SaveManager.StartGame();
    }

    public async UniTask OpenPanel(SelectState state)
    {
        if(!_isOpen)
        {
            _titleUI.SetActive(false);
            _isOpen = true;
        }

        await _panelDict[_nowState].ClosePanel();
        await _panelDict[state].OpenPanel();
        _nowState = state;
    }

    public async UniTask ClosePanel(SelectState state)
    {
        await _panelDict[state].ClosePanel();
    }

    private void GetDictionary()
    {
        if(_stateList.Count != _panelList.Count)
        {
            Debug.Log("リストの要素数エラー");
        }

        for(int i=0; i< _stateList.Count; i++)
        {
            _panelDict.Add(_stateList[i], _panelList[i]);
        }
    }
}