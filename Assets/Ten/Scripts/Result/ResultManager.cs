using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using unityroom.Api;

public class ResultManager : MonoBehaviour
{
    private (List<QuizManager.AskedQuiz> quizData, QuizManager.Level level) _result;
    [SerializeField]
    private Selectable _firstSelectable;
    [SerializeField]
    private ResultPanel _firstPanel;
    [SerializeField]
    private ScorePanel _secondPanel;
    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private Button _prevButton;
    [SerializeField]
    private TMP_Text _nextButtonText;
    private FocusManager _focusManager;
    private bool _isGotoNext;

    private IEnumerator Start()
    {
        _focusManager = GetComponent<FocusManager>();

        _result.quizData = QuizManager.instance.GetResultList();
        _result.level = QuizManager.instance.GetLevel();
        SaveManager.SaveHighScore(_result.level, _result.quizData.FindAll(I => I.IsCorect).Count);
        SaveData data = SaveManager.Load();
        UnityroomApiClient.Instance.SendScore(1, data.HardHighScore[0] + data.NormalHighScore[0] + data.EasyHighScore[0], ScoreboardWriteMode.HighScoreDesc);
        StartCoroutine(_firstPanel.CreateResultPanel(_result.quizData));
        yield return new WaitUntil(() => _firstPanel.IsFinish);

        _focusManager.StateNum.Subscribe(Value =>
        {
            _firstPanel.transform.DOComplete();
            _secondPanel.transform.DOComplete();
            _nextButton.transform.DOComplete();
            _prevButton.transform.DOComplete();
            _nextButton.GetComponent<CanvasGroup>().DOComplete();
            _prevButton.GetComponent<CanvasGroup>().DOComplete();

            if (Value == 0)
            {
                _nextButton.transform.DOLocalMoveY(-450, 0.5f).SetEase(Ease.OutQuad);
                _nextButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    _nextButton.interactable = true;
                });
                _prevButton.transform.DOLocalMoveY(-450, 0.5f).SetEase(Ease.OutQuad);
                _prevButton.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetEase(Ease.OutQuad).OnStart(() =>
                {
                    _prevButton.interactable = false;
                });
                _firstPanel.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f).SetEase(Ease.OutQuad);
                _secondPanel.GetComponent<RectTransform>().DOLocalMoveX(1920, 0.5f).SetEase(Ease.OutQuad);
                _nextButtonText.text = "次へ";
                _isGotoNext = false;
            }
            else if(Value == 1)
            {
                StartCoroutine(_secondPanel.SetScorePanel(_result.quizData.FindAll(I => I.IsCorect).Count, _result.level));
                _prevButton.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    _prevButton.interactable = true;
                    _nextButton.interactable = true;
                });
                _firstPanel.GetComponent<RectTransform>().DOLocalMoveX(-1920, 0.5f).SetEase(Ease.OutQuad);
                _secondPanel.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f).SetEase(Ease.OutQuad);
                _nextButtonText.text = "タイトルへ";
                _isGotoNext = true;
            }
        }).AddTo(this);
    }

    public void GotoNextPanel()
    {
        if (_isGotoNext)
        {
            StartCoroutine(GotoTitle());
            return;
        }
        _focusManager.GotoNextState();
        _nextButton.interactable = false;
    }

    private IEnumerator GotoTitle()
    {
        Scene[] exceptScene = new Scene[1]
        {
            Scene.AudioManager
        };

        EndPanelFade.instance.FadeIn();
        AudioManager.instance.FadeOutChangeBGM(BGMKind.Title);
        yield return new WaitUntil(() => AudioManager.instance.State == BGMChangeState.FadeOut && EndPanelFade.instance.IsFade);
        TenSceneManager.AddScene(Scene.Title);
        TenSceneManager.UnloadSceneExcept(exceptScene);
    }

    public void GotoPrevPanel()
    {
        _focusManager.GotoPrevState();
        _prevButton.interactable = false;
    }
}
