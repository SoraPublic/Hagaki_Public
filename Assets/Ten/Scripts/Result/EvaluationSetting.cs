using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EvaluationSetting")]
public class EvaluationSetting : ScriptableObject
{
    [SerializeField]
    private Setting[] _settings;
    public Setting[] Settings => _settings;
}


[System.Serializable]
public struct Setting
{
    [Header("設定するレベル")]
    public QuizManager.Level Level;
    [Header("秀の基準（～ 100%で秀、100% ～ 80%で優、80% ～ 60%で良、60% ～ 40%で可、40% ～　で不可）")]
    public int Value;
}
