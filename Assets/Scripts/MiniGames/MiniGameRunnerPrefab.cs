using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace MiniGames
{
    public class MiniGameRunnerPrefab : MonoBehaviour, IMiniGameRunner
    {
        [SerializeField] private string miniGameKey = "minigame/tictactoe_prefab";

        private AsyncOperationHandle<GameObject>? _instanceHandle;
        private IMiniGame _activeMiniGame;

        public void StartMiniGame(string miniGameKeyOverride, Action<MiniGameResult> onFinished)
        {
            miniGameKey = string.IsNullOrWhiteSpace(miniGameKeyOverride) ? miniGameKey : miniGameKeyOverride;
            _ = StartMiniGameInternal(onFinished);
        }

        public void StopMiniGame()
        {
            _ = StopMiniGameInternal();
        }

        private async Task StartMiniGameInternal(Action<MiniGameResult> onFinished)
        {
            if (_instanceHandle.HasValue)
            {
                return;
            }

            var handle = Addressables.InstantiateAsync(miniGameKey);
            _instanceHandle = handle;

            var instance = await handle.Task;
            if (instance == null)
            {
                Debug.LogError($"MiniGame prefab not found for key '{miniGameKey}'.");
                _instanceHandle = null;
                return;
            }

            if (ProjectContext.HasInstance)
            {
                ProjectContext.Instance.Container.InjectGameObject(instance);
            }

            _activeMiniGame = instance.GetComponentInChildren<IMiniGame>();
            if (_activeMiniGame == null)
            {
                Debug.LogError("IMiniGame component not found on loaded prefab.");
                await ReleaseInstance();
                return;
            }

            _activeMiniGame.StartGame(result =>
            {
                onFinished?.Invoke(result);
            });
        }

        private async Task StopMiniGameInternal()
        {
            _activeMiniGame?.StopGame();
            await ReleaseInstance();
        }

        private async Task ReleaseInstance()
        {
            if (!_instanceHandle.HasValue)
            {
                return;
            }

            var handle = _instanceHandle.Value;
            _instanceHandle = null;
            _activeMiniGame = null;

            Addressables.ReleaseInstance(handle);
            await Task.CompletedTask;
        }
    }
}
