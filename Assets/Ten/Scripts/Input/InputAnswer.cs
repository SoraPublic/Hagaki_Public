using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;

public class InputAnswer : MonoBehaviour
{
    private float _time;
    [SerializeField]
    private TMP_Text _timerText;
    [SerializeField]
    private TextMeshProUGUI _replyText;
    private TMP_InputField _input;
    private Vector3 _replyTextPos;
    [SerializeField]
    private AudioSource _inputSE, _canselSE;
    private IEnumerator Start()
    {
        _input = GetComponent<TMP_InputField>();
        _replyTextPos = _replyText.gameObject.transform.localPosition;
        yield return new WaitUntil(() => GameStateManager.instance.IsGame);
        GameStateManager.instance.InputableReactiveProperty.Subscribe(Value =>
        {
            if (!Value)
            {
                _time = 0;
            }
        });
        GameStateManager.instance.SetInputable(true);
    }

    private void Update()
    {
        if(GameStateManager.instance.IsInputable && _input.text.Length > 0 && GameStateManager.instance.IsGame && !GameStateManager.instance.IsOpenMenu())
        {
            _time += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(_replyText.gameObject.transform.DOLocalMoveY(_replyTextPos.y + 200, 0.2f))
                    .Join(_replyText.gameObject.transform.DOScale(0f, 0.2f)).OnComplete(() =>
                    {
                        SendAnswer(_replyText.text);
                        _input.text = string.Empty;
                        StartCoroutine(SetDirecting());
                        _replyText.transform.localPosition = _replyTextPos;
                        _replyText.gameObject.transform.localScale = Vector3.one;
                    });
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _inputSE.Play();
                _input.caretPosition = _input.text.Length;
            }

            if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
            {
                _canselSE.Play();
                _input.text = string.Empty;
            }
        }
    }

    public void SendAnswer(string value)
    {
        //Debug.Log(value);
        QuizManager.instance.CheckAnswer(value, _time);
        _inputSE.Play();
    }

    public void ShowText()
    {
        _replyText.text = JapaneseMap.CombineString(_input.text);
        _inputSE.Play();
    }

    private IEnumerator SetDirecting()
    {
        GameStateManager.instance.SetInputable(false);
        yield return GameAnimationManager.instance.DoWriteAnimation().ToCoroutine(); // アニメーション終了時とかに変更する
        if (GameStateManager.instance.IsGame)
        {
            GameStateManager.instance.SetInputable(true);
        }
    }

    public void SetIniTime(float value)
    {
        _time = value;
    }
/*
    private float GetAnswerTime()
    {
        float answerTime = _time - float.Parse(_timerText.text);
        _time -= answerTime;
        return answerTime;
    }*/
}

