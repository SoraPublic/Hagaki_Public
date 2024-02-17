using UnityEngine;

public static class SaveManager
{
    private static SaveData saveData;
    public static SaveData SaveData => saveData;

    private static bool _isLoaded = false;
    public static bool IsLoaded => _isLoaded;
    public static void StartGame()
    {
        _isLoaded = true;
    }

    /// <summary>
    /// 現在のセーブデータを保存する
    /// </summary>
    public static void Save()
    {
        // セーブデータをJSON形式の文字列に変換
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("SaveData", json);
    }

    /// <summary>
    /// 保存されているデータをロードする
    /// </summary>
    public static SaveData Load()
    {
        string json = PlayerPrefs.GetString("SaveData");

        if(json != string.Empty)
        {
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            saveData = new SaveData();
            saveData.BGMVolume = 2;
            saveData.SEVolume = 2;
        }

        return SaveData;
    }

    /// <summary>
    /// BGMの音量を保存する
    /// </summary>
    /// <param name="value"></param>
    public static void SaveBGMVolume(int value)
    {
        value = Mathf.Clamp(value, 0, 5);
        saveData.BGMVolume = value;
        Save();
    }

    /// <summary>
    /// SEの音量を保存する
    /// </summary>
    /// <param name="value"></param>
    public static void SaveSEVolume(int value)
    {
        value = Mathf.Clamp(value, 0, 5);
        saveData.SEVolume = value;
        Save();
    }

    /// <summary>
    /// ハイスコアのデータを保存する
    /// </summary>
    /// <param name="level"></param>
    /// <param name="value"></param>
    public static void SaveHighScore(QuizManager.Level level, int value)
    {
        int temp;
        switch (level)
        {
            case QuizManager.Level.easy:
                for(int i = 0; i < saveData.EasyHighScore.Length; i++)
                {
                    if (saveData.EasyHighScore[i] < value)
                    {
                        temp = saveData.EasyHighScore[i];
                        saveData.EasyHighScore[i] = value;
                        value = temp;
                    }
                }
                break;

            case QuizManager.Level.normal:
                for (int i = 0; i < saveData.NormalHighScore.Length; i++)
                {
                    if (saveData.NormalHighScore[i] < value)
                    {
                        temp = saveData.NormalHighScore[i];
                        saveData.NormalHighScore[i] = value;
                        value = temp;
                    }
                }
                break;

            case QuizManager.Level.hard:
                for (int i = 0; i < saveData.HardHighScore.Length; i++)
                {
                    if (saveData.HardHighScore[i] < value)
                    {
                        temp = saveData.HardHighScore[i];
                        saveData.HardHighScore[i] = value;
                        value = temp;
                    }
                }
                break;
        }

        Save();
    }
}
