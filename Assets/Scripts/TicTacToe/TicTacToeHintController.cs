using UnityEngine;
using Zenject;

namespace TicTacToe
{
    public class TicTacToeHintController : MonoBehaviour
    {
        [SerializeField] private GameObject hintRoot;

        private TicTacToeGame _game;

        [Inject]
        private void Construct(TicTacToeGame game)
        {
            _game = game;
        }

        private void Awake()
        {
            if (hintRoot == null)
            {
                hintRoot = gameObject;
            }
        }

        private void OnEnable()
        {
            if (_game == null)
            {
                return;
            }

            _game.GameRunningChanged += OnGameRunningChanged;
            OnGameRunningChanged(_game.IsRunning);
        }

        private void OnDisable()
        {
            if (_game == null)
            {
                return;
            }

            _game.GameRunningChanged -= OnGameRunningChanged;
        }

        private void OnGameRunningChanged(bool isRunning)
        {
            if (hintRoot != null)
            {
                hintRoot.SetActive(!isRunning);
            }
        }
    }
}
