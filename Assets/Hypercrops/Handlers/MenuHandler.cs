using Assets.Hypercrops.Scene;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuHandler : MonoBehaviour
{
    public GameSceneManager GameScene;
    public InputAction StartEmpty;
    public InputAction LoadTest;

    void Start()
    {
        StartEmpty = InputSystem.actions.FindActionMap("Menu").FindAction("StartEmpty");
        LoadTest = InputSystem.actions.FindActionMap("Menu").FindAction("LoadTest");

        if (GameScene == null)
        {
            GameScene = GameSceneManager.Instance;
        }
    }

    void Update()
    {
        if (StartEmpty.WasPressedThisFrame())
        {
            StartEmptyGame();
            return;
        }

        if (LoadTest.WasPressedThisFrame())
        {
            LoadTestScene();
            return;
        }
    }

    private void StartEmptyGame()
    {
        StartCoroutine(GameScene.StartMain());
    }

    private void LoadTestScene()
    {
        // TODO: Load saved scene (see data model)
    }
}