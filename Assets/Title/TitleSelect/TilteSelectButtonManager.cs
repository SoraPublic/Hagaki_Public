using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TilteSelectButtonManager : MonoBehaviour
{
    [SerializeField]
    private FadeAnimation _fade;
    private GameObject _currentButton;

    private void Start()
    {
        _currentButton = EventSystem.current.currentSelectedGameObject;
        if (_currentButton.GetComponent<SelectButtonView>())
        {
            _currentButton.GetComponent<SelectButtonView>().Init();
        }
    }
    public void ChangeSelectPanel()
    {
        TitleSelectManager.instance.OpenPanel(SelectState.Select).Forget();
    }
    public void ChangeLevelPanel()
    {
        TitleSelectManager.instance.OpenPanel(SelectState.Level).Forget();
    }
    public void ChangeAudioPanel()
    {
        TitleSelectManager.instance.OpenPanel(SelectState.Audio).Forget();
    }
    public void ChangeScorePanel()
    {
        TitleSelectManager.instance.OpenPanel(SelectState.Score).Forget();
    }
    public void GoEasyGame()
    {
        AudioManager.instance.OnSubmitUI.Play();
        TenSceneManager.SetGameLevel(QuizManager.Level.easy);
        AudioManager.instance.FadeOutChangeBGM(BGMKind.Easy);
        LoadScene().Forget();
    }
    public void GoNormalGame()
    {
        AudioManager.instance.OnSubmitUI.Play();
        TenSceneManager.SetGameLevel(QuizManager.Level.normal);
        AudioManager.instance.FadeOutChangeBGM(BGMKind.Normal);
        LoadScene().Forget();
    }
    public void GoHardGame()
    {
        AudioManager.instance.OnSubmitUI.Play();
        TenSceneManager.SetGameLevel(QuizManager.Level.hard);
        AudioManager.instance.FadeOutChangeBGM(BGMKind.Hard);
        LoadScene().Forget();
    }

    private async UniTask LoadScene()
    {
        await _fade.DoFadeInAsync();
        await UniTask.WaitUntil(() => AudioManager.instance.State == BGMChangeState.FadeOut);
        await UniTask.WaitUntil(() =>
            !AudioManager.instance.OnSubmitUI.isPlaying
        );

        TenSceneManager.AddScene(Scene.Game);
        TenSceneManager.UnloadScene(Scene.Title);
    }
}
