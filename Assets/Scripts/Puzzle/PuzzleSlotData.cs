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
        Piece expectedPiece = expectedPiecePrefab.GetComponent<Piece>();
        Piece actualPiece = piece.GetComponent<Piece>();

        if(actualPiece == null || expectedPiece == null){
            Debug.Log("[Error] - Some piece do not have the piece component...\n"+
                $"ActualPiece: {(actualPiece == null)} || ExpectedPiece: {(expectedPiece == null)}" );
            
            return false;
        }

        // Verifica se a peça encaixada corresponde ao prefab esperado
        if (expectedPiece.nameTag == actualPiece.nameTag) // Verifica a tag da peça
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
