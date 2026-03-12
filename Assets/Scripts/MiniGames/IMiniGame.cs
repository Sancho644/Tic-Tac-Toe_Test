using System;

namespace MiniGames
{
    public interface IMiniGame
    {
        void StartGame(Action<MiniGameResult> onFinished);
        void StopGame();
    }
}
