using MiniGames;

namespace Economy
{
    public class MiniGameRewardService
    {
        private readonly CoinWallet _wallet;
        private readonly MiniGameRewardSettings _settings;

        public MiniGameRewardService(CoinWallet wallet, MiniGameRewardSettings settings)
        {
            _wallet = wallet;
            _settings = settings;
        }

        public void Reward(MiniGameResult result)
        {
            var reward = result switch
            {
                MiniGameResult.Win => _settings.winCoins,
                MiniGameResult.Lose => _settings.loseCoins,
                MiniGameResult.Draw => _settings.drawCoins,
                _ => 0
            };

            _wallet.AddCoins(reward);
        }
    }
}
