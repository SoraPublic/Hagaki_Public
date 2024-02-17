using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class TopScoreView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] _easyScoreTexts;
    [SerializeField]
    private TextMeshProUGUI[] _normalScoreTexts;
    [SerializeField]
    private TextMeshProUGUI[] _hardScoreTexts;

    private void Start()
    {
        SaveData data = SaveManager.SaveData;
        int[] easyScores = data.EasyHighScore;
        int[] normalScores = data.NormalHighScore;
        int[] hardScores = data.HardHighScore;

        for(int i = 0; i < 3; i++)
        {
            _easyScoreTexts[i].text = easyScores[i].ToString();
            _normalScoreTexts[i].text= normalScores[i].ToString();
            _hardScoreTexts[i].text = hardScores[i].ToString();
        }
    }
}
