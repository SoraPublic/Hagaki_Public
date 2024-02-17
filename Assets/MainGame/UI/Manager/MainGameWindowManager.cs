using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using DG.Tweening;

public class MainGameWindowManager : MonoBehaviour
{
    [SerializeField]
    private QuestionPresenter _questionTextUI;
    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private InputAnswer _answer;
    [SerializeField]
    private FadeAnimation _fade;
    [SerializeField]
    private PopUpAnimation _popUpAnimation;
    [SerializeField]
    private SkyboxSet[] skyboxSet;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => QuizManager.instance != null &&
            GameAnimationManager.instance != null &&
            AudioManager.instance != null);

        //クイズマネージャの初期化
        QuizManager.instance.Init();
        yield return null;

        RenderSettings.skybox = Array.Find(skyboxSet, I => I.Level == QuizManager.instance.GetLevel()).Skybox;

        GameStateManager.instance.InputableReactiveProperty.Skip(1).Subscribe(Value =>
        {
            if (Value)
            {
                _questionTextUI.SetQuestionText();
                GameAnimationManager.instance.DoIdleAnimationAsync(QuizManager.instance.IsFront()).Forget();
            }
        }).AddTo(GameStateManager.instance.gameObject);

        int iniTime = QuizManager.instance.GetTimeLimit();

        StartCoroutine(_timer.UpdateTimeAsync(iniTime));
        yield return _fade.DoFadeOutAsync().ToCoroutine();
        Tweener tweener = CameraMover.instance.gameObject.transform.DORotate(new Vector3(90, 0, 0), 1.5f).SetEase(Ease.OutQuad).SetDelay(1.0f);
        yield return new WaitUntil(() => !tweener.IsActive());
        yield return _popUpAnimation.ShowAnimation().ToCoroutine();
        _popUpAnimation.gameObject.SetActive(false);

        //タイマースタート
        _timer.IsStart = true;
        //_answer.SetIniTime(iniTime);
    }

    [System.Serializable]
    private struct SkyboxSet
    {
        public QuizManager.Level Level;
        public Material Skybox;
    }
}
