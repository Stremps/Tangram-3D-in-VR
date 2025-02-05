using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [Header("Piece Settings")]
    [Tooltip("Lista de prefabs das peças que serão spawnadas.")]
    public List<GameObject> piecePrefabs = new List<GameObject>();

    [Tooltip("Lista de cores disponíveis para as peças.")]
    public List<Color> availableColors = new List<Color>();

    [Tooltip("Número de peças a serem spawnadas por puzzle.")]
    public int piecesPerPuzzle = 5;

    [Tooltip("Área onde as peças serão spawnadas.")]
    public Transform spawnArea;

    [Tooltip("Offset para espaçar as peças ao spawnar.")]
    public float spawnOffset = 1.5f;

    private List<GameObject> spawnedPieces = new List<GameObject>();

    public void SpawnPieces()
    {
        // Remove peças antigas antes de spawnar novas
        ClearSpawnedPieces();

        if (piecePrefabs.Count == 0 || availableColors.Count == 0)
        {
            Debug.LogError("PieceSpawner: Não há peças ou cores disponíveis para spawnar!");
            return;
        }

        for (int i = 0; i < piecesPerPuzzle; i++)
        {
            // Escolhe um prefab aleatório da lista
            GameObject piecePrefab = piecePrefabs[Random.Range(0, piecePrefabs.Count)];

            // Define a posição da peça dentro da área de spawn
            Vector3 spawnPosition = spawnArea.position + new Vector3(i * spawnOffset, 0, 0);

            // Instancia a peça
            GameObject spawnedPiece = Instantiate(piecePrefab, spawnPosition, Quaternion.identity);
            spawnedPieces.Add(spawnedPiece);

            // Escolhe uma cor aleatória e aplica no material da peça
            Renderer pieceRenderer = spawnedPiece.GetComponent<Renderer>();
            if (pieceRenderer != null)
            {
                pieceRenderer.material.color = availableColors[Random.Range(0, availableColors.Count)];
            }
        }
    }

    public void ClearSpawnedPieces()
    {
        foreach (var piece in spawnedPieces)
        {
            Destroy(piece);
        }
        spawnedPieces.Clear();
    }
}
