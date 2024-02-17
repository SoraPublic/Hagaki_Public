using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class TenSceneManager
{
    private static QuizManager.Level _level;
    private static Dictionary<Scene, string> sceneNameList = new Dictionary<Scene, string>()
    {
        {Scene.Title, "Title"},
        {Scene.AudioManager, "Audio"},
        {Scene.Game, "MainGame"},
        {Scene.Result, "Result"},
        {Scene.Animation, "HandAnimation" }
    };

    public static bool IsLoaded(Scene scene)
    {
        var sceneCount = SceneManager.sceneCount;

        for (var i = 0; i < sceneCount; i++)
        {
            var currentScene = SceneManager.GetSceneAt(i);

            if (currentScene.name == sceneNameList[scene] && currentScene.isLoaded)
            {
                return true;
            }
        }

        return false;
    }
    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(sceneNameList[scene]);
    }
    public static void AddScene(Scene scene)
    {
        SceneManager.LoadSceneAsync(sceneNameList[scene], LoadSceneMode.Additive);
    }
    public static void UnloadScene(Scene scene)
    {
        SceneManager.UnloadSceneAsync(sceneNameList[scene]);
    }
    public static void UnloadSceneExcept(Scene[] scenes)
    {
        var sceneCount = SceneManager.sceneCount;
        List<string> exceptSceneNames = new List<string>();
        foreach(Scene scene in scenes)
        {
            exceptSceneNames.Add(sceneNameList[scene]);
        }

        for (var i = 0; i < sceneCount; i++)
        {
            var currentScene = SceneManager.GetSceneAt(i);

            if (!exceptSceneNames.Contains(currentScene.name) && currentScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(currentScene);
            }
        }
    }
    public static void SetGameLevel(QuizManager.Level level)
    {
        _level = level;
    }
    public static QuizManager.Level getGameLevel()
    {
        return _level;
    }
}

public enum Scene
{
    Title,
    AudioManager,
    Game,
    Result,
    Animation
}
