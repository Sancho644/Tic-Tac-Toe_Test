using Economy;
using MiniGames;
using UnityEngine;
using Zenject;

namespace TicTacToe
{
    public class TicTacToeProjectInstaller : MonoInstaller
    {
        [Header("MiniGame Rewards")]
        [SerializeField] private int winCoins = 10;
        [SerializeField] private int loseCoins = 2;
        [SerializeField] private int drawCoins = 5;

        public override void InstallBindings()
        {
            Container.Bind<CoinWallet>().AsSingle();

            var settings = new MiniGameRewardSettings
            {
                winCoins = winCoins,
                loseCoins = loseCoins,
                drawCoins = drawCoins
            };

            Container.BindInstance(settings).AsSingle();
            Container.Bind<MiniGameRewardService>().AsSingle();

            Container.Bind<IMiniGameRunner>()
                .To<MiniGameRunnerPrefab>()
                .FromNewComponentOnNewGameObject()
                .UnderTransform(this.transform)
                .AsSingle()
                .NonLazy();
        }
    }
}
