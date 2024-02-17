using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGame : MonoBehaviour
{
    private int _pushedKeyIndex = 0;
    [SerializeField]
    private GameObject _titleGameObject;
    [SerializeField]
    private GameObject _fadeObject;
    private TitleFadeAnimation _fadeAnimation;
    [SerializeField]
    private GameObject _questionObject;
    private TitleGameTextAnimation _questionTextAnimation;
    [SerializeField]
    private Transform _keyImagesTransform;
    private List<GameObject> _keyObjectList = new List<GameObject>();
    private List<KeyCode> _keyList = new List<KeyCode>();
    [SerializeField]
    private Material _skybox;
    
    private IEnumerator Start()
    {
        _titleGameObject.SetActive(true);
        _fadeObject.SetActive(true);
        _fadeAnimation = _fadeObject.GetComponent<TitleFadeAnimation>();
        _questionObject.SetActive(true);
        _questionTextAnimation = _questionObject.GetComponent<TitleGameTextAnimation>();

        for(int i=0; i< _keyImagesTransform.childCount; i++)
        {
            var keyObject = _keyImagesTransform.GetChild(i).gameObject;
            keyObject.SetActive(true);
            _keyObjectList.Add(keyObject);
            _keyList.Add(keyObject.GetComponent<TitleGameKeyAnimation>().Key);
        }

        if (SaveManager.IsLoaded)
        {
            _titleGameObject.SetActive(false);
            _fadeObject.SetActive(false);
            _questionObject.SetActive(false);
            TitleSelectManager.instance.StartSelect();
        }

        yield return new WaitUntil(() => AudioManager.instance != null);
        RenderSettings.skybox = _skybox;
        AudioManager.instance.FadeInBGM();
    }

    private async void Update()
    {
        if (_keyObjectList.Count != _keyList.Count)
        {
            Debug.Log("キーが設定されていないオブジェクトがあります");
            return;
        }

        if (!TitleSelectManager.instance.IsGame)
        {
            return;
        }

        if (_pushedKeyIndex >= _keyObjectList.Count)
        {
            await EndGameAsync();
            return;
        }

        if (Input.GetKeyDown(_keyList[_pushedKeyIndex]))
        {
            _keyObjectList[_pushedKeyIndex].GetComponent<TitleGameKeyAnimation>().doAnimetion();
            _fadeAnimation.GoFadeAsync().Forget();
            if (_keyList[_pushedKeyIndex] is (KeyCode.A or KeyCode.I or KeyCode.U or KeyCode.E or KeyCode.O))
            {
                if (AudioManager.instance.KeyInput.isPlaying)
                {
                    AudioManager.instance.KeyInput.Stop();
                }
                AudioManager.instance.KeyInput.Play();
                _questionTextAnimation.doAnimation();
            }
            _pushedKeyIndex++;
        }
    }

    private async UniTask EndGameAsync()
    {
        _questionObject.SetActive(false);
        _keyImagesTransform.gameObject.SetActive(false);
        await _fadeAnimation.DoFadeOutAsync();
        TitleSelectManager.instance.StartSelect();
        if (_titleGameObject != null)
        {
            _titleGameObject.SetActive(false);
        }
    }
}
