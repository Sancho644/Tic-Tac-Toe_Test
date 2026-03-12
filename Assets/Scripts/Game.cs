using MiniGames;
using TicTacToe;
using UnityEngine;
using Zenject;

public class Game : MonoBehaviour
{
    [SerializeField] private KeyCode startKey = KeyCode.T;
    [SerializeField] private bool playerStarts = true;
    [SerializeField] private string miniGameKey = "minigame/tictactoe_prefab";

    private TicTacToeModule _module;
    private IMiniGameRunner _runner;

    [Inject]
    private void Construct(TicTacToeModule module, [InjectOptional] IMiniGameRunner runner)
    {
        _module = module;
        _runner = runner;
    }

    private void Awake()
    {
        if (_module == null)
        {
            Debug.LogError("TicTacToeModule is not injected. Check SceneContext/ProjectContext setup.");
        }
    }

    private void Update()
    {
        if (_module == null)
        {
            return;
        }

        if (Input.GetKeyDown(startKey) && !_module.IsRunning)
        {
            if (_runner != null)
            {
                _runner.StartMiniGame(miniGameKey, OnMiniGameFinishedMapped);
            }
            else
            {
                _module.StartGame(OnMiniGameFinished, playerStarts);
            }
        }
    }

    private void OnMiniGameFinished(TicTacToeResult result)
    {
        Debug.Log($"TicTacToe result: {result}");
    }

    private void OnMiniGameFinishedMapped(MiniGameResult result)
    {
        Debug.Log($"MiniGame result: {result}");
    }
}
