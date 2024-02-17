using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;
    private bool _doRondom;
    /// <summary>
    /// 難易度イージーのタイムリミット
    /// </summary>
    [Header("難易度イージーのタイムリミット"), Range(0, 600)]
    [SerializeField]
    private int easyTimeLimit = 30;
    /// <summary>
    /// 難易度ノーマルのタイムリミット
    /// </summary>
    [Header("難易度ノーマルのタイムリミット"), Range(0, 600)]
    [SerializeField]
    private int normalTimeLimit = 90;
    /// <summary>
    /// 難易度ハードのタイムリミット
    /// </summary>
    [Header("難易度ハードのタイムリミット"), Range(0, 600)]
    [SerializeField]
    private int hardTimeLimit = 120;
    /// <summary>
    /// 問題のデータのスクリプタブルオブジェクト
    /// </summary>
    [Header("問題のデータのスクリプタブルオブジェクト")]
    [SerializeField]
    private QuizDatas _quizDatas;
    [SerializeField]
    private int muchAnswer;
    private List<AskedQuiz> _askedQuizList = new List<AskedQuiz>();
    private List<QuizData> _quizDataList = new List<QuizData>();
    private List<QuizData> _normalQuizDataList = new List<QuizData>();
    private List<QuizData> _hardQuizDataList = new List<QuizData>();
    private int _randomIndex;
    private Level _quizLevel;
    [SerializeField]
    private float _firstTime;
    [SerializeField]
    private List<PhraseAnimation> _phraseList;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// 問題の初期設定
    /// </summary>
    /// <param name="level">問題の難易度</param>
    public void Init()
    {
        SetLevel();
        CreatQuizDataListByLevel();
        SetQuizDataList();
    }

    /// <summary>
    /// 問題文の取得
    /// </summary>
    public string GetQuizSentence()
    {
        if(!_doRondom)
        {
            SetRandomIndex(_quizDataList.Count);
        }
        
        return _quizDataList[_randomIndex].QuizSentence;
    }

    /// <summary>
    /// 解答の取得
    /// </summary>
    public string GetQuizAnswer()
    {
        if (!_doRondom)
        {
            SetRandomIndex(_quizDataList.Count);
        }

        return _quizDataList[_randomIndex].QuizAnswer;
    }

    /// <summary>
    /// 表か裏か取得
    /// </summary>
    public bool IsFront()
    {
        if (!_doRondom)
        {
            SetRandomIndex(_quizDataList.Count);
        }

        return _quizDataList[_randomIndex].IsFront;
    }

    public Level GetLevel()
    {
        return _quizLevel;
    }

    /// <summary>
    /// 全回答履歴の取得
    /// </summary>
    public List<AskedQuiz> GetResultList()
    {
        return _askedQuizList;
    }

    /// <summary>
    /// 時間制限の取得
    /// </summary>
    public int GetTimeLimit()
    {
        switch (_quizLevel)
        {
            case Level.easy:
                return easyTimeLimit;
            case Level.normal: 
                return normalTimeLimit;
            case Level.hard:
                return hardTimeLimit;
            default:
                Debug.Log("レベルが設定されていません");
                return -1;
        }
    }

    private void RemoveAskedQuiz()
    {
        _quizDataList.RemoveAt(_randomIndex);

        if(_quizDataList.Count == 0)
        {
            SetQuizDataList();
        }

        _doRondom = false;
    }

    /// <summary>
    /// 答え合わせ
    /// </summary>
    /// <param name="answer">入力された解答</param>
    public void CheckAnswer(string answer, float time)
    {
        AskedQuiz askedQuiz = new AskedQuiz(
            _quizDataList[_randomIndex].QuizSentence,
            _quizDataList[_randomIndex].QuizAnswer,
            answer,
            false,
            time);
        
        if (_quizDataList[_randomIndex].QuizAnswer == answer)
        {
            askedQuiz.SetCorect(true);
            ShowPhrase(time);
        }

        _askedQuizList.Add(askedQuiz);
        if(_askedQuizList.Count == muchAnswer)
        {
            GameAnimationManager.instance.ChangeInkstone();
        }
        RemoveAskedQuiz();
    }

    private void SetLevel()
    {
        _quizLevel = TenSceneManager.getGameLevel();
    }

    public void SetRandomIndex(int maxCount)
    {
        if(maxCount == 1)
        {
            _randomIndex = 0;
        }
        _randomIndex = Random.Range(0, maxCount);
        _doRondom = true;
    }

    private void SetQuizDataList()
    {

        switch (_quizLevel)
        {
            case Level.easy:
                _quizDataList.AddRange(_normalQuizDataList);
                break;
            case Level.normal:
                _quizDataList.AddRange(_quizDatas.Quizzes);
                break;
            case Level.hard:
                _quizDataList.AddRange(_hardQuizDataList);
                break;
            default:
                Debug.Log("レベルが設定されていません");
                break;
        }
    }

    private void CreatQuizDataListByLevel()
    {
        foreach (QuizData data in _quizDatas.Quizzes)
        {
            if (data.IsHighLevel)
            {
                _hardQuizDataList.Add(data);
            }
            else
            {
                _normalQuizDataList.Add(data);
            }
        }
    }

    private void ShowPhrase(float time)
    {
        if (time > _firstTime)
        {
            return;
        }

        int randomNumber = Random.Range(0, _phraseList.Count);

        _phraseList[randomNumber].ShowPhraseAsync().Forget();
    }

    public enum Level
    {
        easy,
        normal,
        hard,
    }

    /// <summary>
    /// クイズ回答履歴
    /// </summary>
    public class AskedQuiz
    {
        /// <summary>
        /// 問題文
        /// </summary>
        private string _quiz;

        public string Quiz { get { return _quiz; } }
        
        /// <summary>
        /// 問題の答え
        /// </summary>
        private string _correctAnswer;
        public string CorrectAnswer { get { return _correctAnswer; } }
        
        /// <summary>
        /// プレイヤーの回答
        /// </summary>
        private string _answer;
        public string Answer { get { return _answer; } }
        
        /// <summary>
        /// 正解かどうか
        /// </summary>
        private bool _isCorect;
        public bool IsCorect {  get { return _isCorect; } }


        /// <summary>
        /// 解答までの時間
        /// </summary>
        private float _clearTime;
        public float ClearTime => _clearTime;

        public AskedQuiz(string quiz, string correctAnswer, string answer,bool correct, float time)
        {
            this._quiz = quiz;
            this._correctAnswer = correctAnswer;
            this._answer = answer;
            this._isCorect = correct;
            this._clearTime = time;
        }


        /// <summary>
        /// 正解かどうかの設定
        /// </summary>
        /// <param name="corect">正解ならtrue</param>
        public void SetCorect(bool corect)
        {
            this._isCorect= corect;
        }
    }
}