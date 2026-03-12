using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TicTacToe
{
    public class TicTacToeUiController : MonoBehaviour, ITicTacToeUi
    {
        [Header("Root")]
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private Canvas gameCanvas;

        [Header("Status")]
        [SerializeField] private TMP_Text statusText;

        [Header("Cells")]
        [SerializeField] private Button[] cells = new Button[TicTacToeConstants.CellCount];
        [SerializeField] private TMP_Text[] cellTexts = new TMP_Text[TicTacToeConstants.CellCount];

        private Action<int> _onCellClicked;

        public void BindCellClicked(Action<int> onCellClicked)
        {
            _onCellClicked = onCellClicked;
        }

        public void SetGameCanvas(Canvas canvas)
        {
            gameCanvas = canvas;
            ValidateBindings();
        }

        public void Show(bool show)
        {
            ValidateBindings();
            if (uiRoot != null)
            {
                uiRoot.SetActive(show);
            }
        }

        public void ResetCells()
        {
            ValidateBindings();
            for (var i = 0; i < TicTacToeConstants.CellCount; i++)
            {
                SetCell(i, "", true);
            }
        }

        public void SetCell(int index, string text, bool interactable)
        {
            ValidateBindings();
            if (index < 0 || index >= TicTacToeConstants.CellCount)
            {
                return;
            }

            if (cellTexts[index] != null)
            {
                cellTexts[index].text = text;
            }

            if (cells[index] != null)
            {
                cells[index].interactable = interactable;
            }
        }

        public void SetAllInteractable(bool interactable)
        {
            ValidateBindings();
            for (var i = 0; i < TicTacToeConstants.CellCount; i++)
            {
                if (cells[i] != null)
                {
                    cells[i].interactable = interactable;
                }
            }
        }

        public void SetStatus(string text)
        {
            ValidateBindings();
            if (statusText != null)
            {
                statusText.text = text;
            }
        }

        private void Awake()
        {
            ValidateBindings();
            Show(false);
        }

        private void ValidateBindings()
        {
            if (gameCanvas == null)
            {
                gameCanvas = GetComponentInParent<Canvas>();
            }

            if (uiRoot == null)
            {
                Debug.LogError("TicTacToeUI root is not assigned.");
                return;
            }

            if (statusText == null)
            {
                Debug.LogError("StatusText is not assigned on TicTacToeUI.");
            }

            WireCellsIfNeeded();

            if (FindAnyObject<EventSystem>() == null)
            {
                Debug.LogWarning("EventSystem not found in scene. UI input will not work.");
            }
        }

        private void WireCellsIfNeeded()
        {
            var needsCells = cells == null || cells.Length != TicTacToeConstants.CellCount;
            if (needsCells)
            {
                cells = new Button[TicTacToeConstants.CellCount];
            }

            var needsTexts = cellTexts == null || cellTexts.Length != TicTacToeConstants.CellCount;
            if (needsTexts)
            {
                cellTexts = new TMP_Text[TicTacToeConstants.CellCount];
            }

            for (var i = 0; i < TicTacToeConstants.CellCount; i++)
            {
                if (cells[i] == null)
                {
                    Debug.LogError($"Cell button {i} is not assigned on TicTacToeUI.");
                    continue;
                }

                if (cellTexts[i] == null)
                {
                    Debug.LogError($"Cell text {i} is not assigned on TicTacToeUI.");
                }

                if (cells[i] != null)
                {
                    var index = i;
                    cells[i].onClick.RemoveAllListeners();
                    cells[i].onClick.AddListener(() => HandleCellClicked(index));
                }
            }
        }

        private void HandleCellClicked(int index)
        {
            _onCellClicked?.Invoke(index);
        }

        private static T FindAnyObject<T>() where T : UnityEngine.Object
        {
#if UNITY_2023_1_OR_NEWER
            return FindFirstObjectByType<T>();
#else
            return FindObjectOfType<T>();
#endif
        }
    }
}
