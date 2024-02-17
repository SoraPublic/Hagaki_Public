using Cysharp.Threading.Tasks;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "QuizDatas", menuName = "Create/ScriptableObjects/QuizDatas", order = 1)]
public class QuizDatas:ScriptableObject
{
    private const string APIURL = "https://script.google.com/macros/s/AKfycbw632H_ifuA-kKbh5HTkIoJKDXmw7zarTy4bRh4BfUXscyuupaz_kw7LTH7JdOU0DKVQQ/exec";
    private const string ID_KEY = "id";
    private const string QUESTION_KEY = "question";
    private const string LEVEL_KEY = "level";
    private const string ANSWER_KEY = "answer";
    private const string FRONT_KEY = "front";

    /// <summary>
    /// 問題のデータ
    /// </summary>
    [Header("問題のデータ")]
    [SerializeField]
    private List<QuizData> _quizzes;
    public List<QuizData> Quizzes
    {
        get
        {
            return _quizzes;
        }
#if UNITY_EDITOR
        set
        {
            _quizzes = value;
        }
#endif
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(QuizDatas))]
public class QuizDatasEditor : Editor
{
    public override async void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        QuizDatas quizDatas = target as QuizDatas;

        if (GUILayout.Button("取得"))
        {
            SetQuizData setData = new SetQuizData();
            List<QuizData> fetchedData = await setData.GetQuestDataList();
            quizDatas.Quizzes = fetchedData;
            EditorUtility.SetDirty(quizDatas);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif

[System.Serializable]
public class QuizData
{
    /// <summary>
    /// 問題文
    /// </summary>
    [Header("問題文")]
    [SerializeField]
    private string _quizSentence;
    public string QuizSentence
    {
        get 
        {
            return _quizSentence;
        }
#if UNITY_EDITOR
            set
            {
                _quizSentence = value;
            }
#endif
    }

    /// <summary>
    /// 問題ID
    /// </summary>
    [Header("問題ID")]
    [SerializeField]
    private int _quizId;
    public int QuizID
    {
        get
        {
            return _quizId;
        }
#if UNITY_EDITOR
        set
        {
            _quizId = value;
        }
#endif
    }

    /// <summary>
    /// 問題が難読か
    /// </summary>
    [Header("問題が難読か")]
    [SerializeField]
    private bool _isHighLevel;
    public bool IsHighLevel
    {
        get
        {
            return _isHighLevel;
        }
#if UNITY_EDITOR
            set
            {
                _isHighLevel = value;
            }
#endif
    }

    /// <summary>
    /// 解答
    /// </summary>
    [Header("解答")]
    [SerializeField]
    private string _quizAnswer;
    public string QuizAnswer
    {
        get
        {
            return _quizAnswer;
        }
#if UNITY_EDITOR
        set
        {
            _quizAnswer = value;
        }
#endif
    }

    /// <summary>
    /// 年賀状の表に書く文字か
    /// </summary>
    [Header("年賀状の表に書く文字か")]
    [SerializeField]
    private bool _isFront;
    public bool IsFront
    {
        get
        {
            return _isFront;
        }
#if UNITY_EDITOR
        set
        {
            _isFront = value;
        }
#endif
    }
}

#if UNITY_EDITOR
[System.Serializable]
public class SetQuizData
{
    private const string APIURL = "https://script.google.com/macros/s/AKfycbw632H_ifuA-kKbh5HTkIoJKDXmw7zarTy4bRh4BfUXscyuupaz_kw7LTH7JdOU0DKVQQ/exec";
    private const string ID_KEY = "id";
    private const string QUESTION_KEY = "question";
    private const string LEVEL_KEY = "level";
    private const string ANSWER_KEY = "answer";
    private const string FRONT_KEY = "front";

    public async UniTask<List<QuizData>> GetQuestDataList()
    {
        Dictionary<string, object> datas = await GetRequestAsync();
        return CreateQuestDataList(datas);
    }

    private async UniTask<Dictionary<string, object>> GetRequestAsync()
    {
        Dictionary<string, object> response = new Dictionary<string, object>();
        using (UnityWebRequest request = UnityWebRequest.Get(APIURL))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("GET Request Failure");
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("GET Request Success");
                Debug.Log(request.downloadHandler.text);
                response = Json.Deserialize(request.downloadHandler.text) as Dictionary<string, object>;
            }
        }

        return response;
    }

    private List<QuizData> CreateQuestDataList(Dictionary<string, object> datas)
    {
        List<int> ids = GetIntDatas(datas, ID_KEY);
        List<string> sentences = GetStringDatas(datas, QUESTION_KEY);
        List<bool> levels = GetBoolDatas(datas, LEVEL_KEY);
        List<string> answers = GetStringDatas(datas, ANSWER_KEY);
        List<bool> fronts = GetBoolDatas(datas, FRONT_KEY);
        List<QuizData> questDataList = new List<QuizData>();

        for (int i = 0; i < ids.Count; i++)
        {
            QuizData _questData = new QuizData();
            _questData.QuizID = ids[i];
            _questData.QuizSentence = sentences[i];
            _questData.IsHighLevel = levels[i];
            _questData.QuizAnswer = answers[i];
            _questData.IsFront = fronts[i];
            questDataList.Add(_questData);
        }

        return questDataList;
    }

    private List<int> GetIntDatas(Dictionary<string, object> datas, string key)
    {
        string[] questDatas = datas[key].ToString().Split(",");
        return StrsToInts(questDatas);
    }

    private List<string> GetStringDatas(Dictionary<string, object> datas, string key)
    {
        return datas[key].ToString().Split(",").ToList();
    }

    private List<bool> GetBoolDatas(Dictionary<string, object> datas, string key)
    {
        string[] quizDatas = datas[key].ToString().Split(",");
        return StrsToBools(quizDatas);
    }


    private List<int> StrsToInts(string[] str)
    {
        List<int> ints = new List<int>();
        for (int i = 0; i < str.Length; i++)
        {
            ints.Add(int.Parse(str[i]));
        }

        return ints;
    }

    private List<bool> StrsToBools(string[] str)
    {
        List<bool> bools = new List<bool>();
        for (int i = 0; i < str.Length; i++)
        {
            bools.Add(bool.Parse(str[i]));
        }

        return bools;
    }
}
#endif