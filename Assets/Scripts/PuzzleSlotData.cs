using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class PuzzleSlotData
{
    [Tooltip("Socket onde a peça será encaixada.")]
    public XRSocketInteractor socket;

    [Tooltip("Prefab da peça correta para este socket.")]
    public GameObject expectedPiecePrefab;

    private GameObject currentPiece = null; // Peça atualmente encaixada
    private bool isFilled = false;

    public bool IsFilled => isFilled;

    public bool CheckCorrectPiece(GameObject piece)
    {
        // Verifica se a peça encaixada corresponde ao prefab esperado
        if (piece.CompareTag(expectedPiecePrefab.tag)) // Verifica a tag da peça
        {
            isFilled = true;
            currentPiece = piece;
            return true;
        }
        return false;
    }

    public void ResetSlot()
    {
        isFilled = false;
        currentPiece = null;
    }

    public GameObject GetCurrentPiece()
    {
        return currentPiece;
    }
}
