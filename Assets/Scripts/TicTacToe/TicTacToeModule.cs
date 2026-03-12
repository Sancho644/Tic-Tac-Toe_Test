using System;
using MiniGames;
using UnityEngine;
using Zenject;

namespace TicTacToe
{
    public class TicTacToeModule : MonoBehaviour, IMiniGame
    {
        private TicTacToeGame _game;
        private Economy.MiniGameRewardService _rewardService;

        [Inject]
        private void Construct(TicTacToeGame game, Economy.MiniGameRewardService rewardService)
        {
            _game = game;
            _rewardService = rewardService;
        }

        public bool IsRunning => _game != null && _game.IsRunning;

        public void StartGame(Action<TicTacToeResult> onFinished, bool playerStarts = true)
        {
            if (_game == null)
            {
                Debug.LogError("TicTacToeModule is not injected. Check SceneContext/ProjectContext setup.");
                return;
            }

            _game.StartGame(result =>
            {
                var mapped = result switch
                {
                    TicTacToeResult.PlayerWin => MiniGameResult.Win,
                    TicTacToeResult.PlayerLose => MiniGameResult.Lose,
                    TicTacToeResult.Draw => MiniGameResult.Draw,
                    _ => MiniGameResult.Draw
                };

                _rewardService?.Reward(mapped);
                onFinished?.Invoke(result);
            }, playerStarts);
        }

        void IMiniGame.StartGame(Action<MiniGameResult> onFinished)
        {
            StartGame(result =>
            {
                var mapped = result switch
                {
                    TicTacToeResult.PlayerWin => MiniGameResult.Win,
                    TicTacToeResult.PlayerLose => MiniGameResult.Lose,
                    TicTacToeResult.Draw => MiniGameResult.Draw,
                    _ => MiniGameResult.Draw
                };

                onFinished?.Invoke(mapped);
            }, true);
        }


        public void StopGame()
        {
            _game?.StopGame();
        }
    }
}
