using System.Collections;
using UnityEngine;

public class InterruptionSystem : MonoBehaviour
{
    public void Interruption()
    {
        StartCoroutine(InterruptionCoroutine());
    }

    private IEnumerator InterruptionCoroutine()
    {
        GameStateManager.instance.Interrupt();
        GameStateManager.instance.EndGame();
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
}
