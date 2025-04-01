using UnityEngine;
using ScriptableObjectArchitecture;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuration")]
    public SceneSO sceneToLoad;
    public LevelEntranceSO levelEntrance;
    public bool loadingScreen;

    [Header("Player Path")]
    public PlayerPathSO playerPath;

    [Header("Broadcasting events")]
    public LoadSceneRequestGameEvent loadSceneEvent;

    public void LoadScene()
    {
        if (levelEntrance != null && playerPath != null)
            playerPath.levelEntrance = levelEntrance;

        var request = new LoadSceneRequest(
            scene: sceneToLoad,
            loadingScreen: loadingScreen
        );

        loadSceneEvent.Raise(request);
    }
}
