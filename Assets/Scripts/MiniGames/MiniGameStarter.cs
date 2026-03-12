using System;
using MiniGames;
using UnityEngine;
using Zenject;

public class MiniGameStarter : MonoBehaviour
{
    [SerializeField] private KeyCode startKey = KeyCode.T;
    [SerializeField] private string miniGameKey = "minigame/tictactoe_prefab";
    [SerializeField] private bool autoStart;

    private IMiniGameRunner _runner;
    private bool _isRunning;

    [Inject]
    private void Construct(IMiniGameRunner runner)
    {
        _runner = runner;
    }

    private void Awake()
    {
        ResolveRunnerIfNeeded();
    }

    private void Start()
    {
        ResolveRunnerIfNeeded();

        if (autoStart)
        {
            TryStart();
        }
    }

    private void Update()
    {
        if (autoStart)
        {
            return;
        }

        if (Input.GetKeyDown(startKey))
        {
            TryStart();
        }
    }

    private void TryStart()
    {
        if (_isRunning)
        {
            return;
        }

        if (_runner == null)
        {
            ResolveRunnerIfNeeded();
        }

        if (_runner == null)
        {
            Debug.LogError("IMiniGameRunner is not available. Check ProjectContext setup.");
            return;
        }

        _isRunning = true;
        _runner.StartMiniGame(miniGameKey, OnFinished);
    }

    private void OnFinished(MiniGameResult result)
    {
        _isRunning = false;
        Debug.Log($"MiniGame result: {result}");
    }

    private void ResolveRunnerIfNeeded()
    {
        if (_runner != null)
        {
            return;
        }

        var context = ProjectContext.HasInstance ? ProjectContext.Instance : ProjectContext.Instance;
        if (context != null)
        {
            _runner = context.Container.Resolve<IMiniGameRunner>();
        }
    }
}
