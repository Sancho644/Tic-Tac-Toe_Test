using UnityEngine;
using Zenject;

namespace TicTacToe
{
    public class TicTacToeSceneInstaller : MonoInstaller
    {
        [SerializeField] private TicTacToeUiController uiController;
        [SerializeField] private TicTacToeModule module;
        [SerializeField] private Canvas gameCanvas;

        public override void InstallBindings()
        {
            if (uiController == null)
            {
                uiController = FindAnyObject<TicTacToeUiController>();
            }

            if (module == null)
            {
                module = FindAnyObject<TicTacToeModule>();
            }

            if (gameCanvas == null)
            {
                var canvases = GetComponentsInChildren<Canvas>(true);
                if (canvases.Length > 0)
                {
                    gameCanvas = canvases[0];
                }
            }

            if (gameCanvas == null)
            {
                gameCanvas = FindAnyObject<Canvas>();
            }

            Container.BindInstance(uiController).AsSingle();
            Container.BindInstance(gameCanvas).AsSingle();
            uiController.SetGameCanvas(gameCanvas);
            Container.Bind<ITicTacToeUi>().To<TicTacToeUiController>().FromInstance(uiController);
            Container.BindInstance(module).AsSingle();

            Container.Bind<TicTacToeBoard>().AsSingle();
            Container.Bind<TicTacToeAi>().AsSingle();
            Container.Bind<TicTacToeGame>().AsSingle().NonLazy();
        }

        private static T FindAnyObject<T>() where T : Object
        {
#if UNITY_2023_1_OR_NEWER
            return FindFirstObjectByType<T>();
#else
            return FindObjectOfType<T>();
#endif
        }
    }
}
