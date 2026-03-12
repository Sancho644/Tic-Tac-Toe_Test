using System;

namespace MiniGames
{
    public interface IMiniGameRunner
    {
        void StartMiniGame(string miniGameKey, Action<MiniGameResult> onFinished);
        void StopMiniGame();
    }
}
