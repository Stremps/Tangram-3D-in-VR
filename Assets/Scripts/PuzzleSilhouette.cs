using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PuzzleSilhouette : MonoBehaviour
{
    public event Action OnPuzzleCompleted;

    [Tooltip("Lista de sockets deste quebra-cabeça.")]
    public List<PuzzleSlotData> puzzleSlots = new List<PuzzleSlotData>();

    private bool isCompleted = false;

    private void Start()
    {
        // Inscreve cada socket para eventos de interação
        foreach (var slot in puzzleSlots)
        {
            slot.socket.selectEntered.AddListener((args) => OnPieceInserted(slot, args.interactable.gameObject));
            slot.socket.selectExited.AddListener((args) => OnPieceRemoved(slot));
        }
    }

    private void OnPieceInserted(PuzzleSlotData slot, GameObject insertedPiece)
    {
        if (slot.CheckCorrectPiece(insertedPiece))
        {
            Debug.Log($"Peça correta encaixada em {slot.socket.name}!");
            CheckPuzzleCompletion();
        }
        else
        {
            Debug.LogWarning("Peça errada encaixada!");
        }
    }

    private void OnPieceRemoved(PuzzleSlotData slot)
    {
        Debug.Log($"Peça removida de {slot.socket.name}.");
        slot.ResetSlot();
    }

    private void CheckPuzzleCompletion()
    {
        foreach (var slot in puzzleSlots)
        {
            if (!slot.IsFilled)
            {
                return;
            }
        }

        if (!isCompleted)
        {
            isCompleted = true;
            Debug.Log("Quebra-cabeça concluído!");
            OnPuzzleCompleted?.Invoke(); // Notifica o `PuzzleManager`
        }
    }

    public List<GameObject> GetPlacedPieces()
    {
        List<GameObject> pieces = new List<GameObject>();
        foreach (var slot in puzzleSlots)
        {
            GameObject piece = slot.GetCurrentPiece();
            if (piece != null)
            {
                pieces.Add(piece);
            }
        }
        return pieces;
    }
}
