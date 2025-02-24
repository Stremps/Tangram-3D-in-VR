using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [Header("Piece Settings")]
    [Tooltip("Lista de grupos de peças associadas a cada puzzle.")]
    public List<PieceSet> puzzlePieceSets = new List<PieceSet>();

    [Tooltip("Lista de materiais disponíveis para aplicar às peças.")]
    public List<Material> availableMaterials = new List<Material>();

    [Tooltip("Área onde as peças serão spawnadas.")]
    public Transform spawnArea;

    [Tooltip("Tempo de espera entre o spawn de cada peça.")]
    public float spawnDelay = 0.2f;  // Delay entre spawns (editável no Inspector)

    private List<GameObject> spawnedPieces = new List<GameObject>();

    public void SpawnPieces(int puzzleIndex)
    {
        // Remove peças antigas antes de spawnar novas
        ClearSpawnedPieces();

        if (puzzleIndex >= puzzlePieceSets.Count)
        {
            Debug.LogError("PieceSpawner: O índice do puzzle está fora do alcance dos conjuntos de peças.");
            return;
        }

        PieceSet selectedSet = puzzlePieceSets[puzzleIndex];

        if (selectedSet.pieces.Count == 0 || availableMaterials.Count == 0)
        {
            Debug.LogError("PieceSpawner: Não há peças ou materiais disponíveis para spawnar!");
            return;
        }

        // Obtém os limites da área de spawn
        BoxCollider spawnCollider = spawnArea.GetComponent<BoxCollider>();
        if (spawnCollider == null)
        {
            Debug.LogError("PieceSpawner: O Spawn Area precisa ter um BoxCollider para definir os limites!");
            return;
        }

        StartCoroutine(SpawnPiecesWithDelay(selectedSet, spawnCollider));
    }

    private IEnumerator SpawnPiecesWithDelay(PieceSet selectedSet, BoxCollider spawnCollider)
    {
        Vector3 spawnCenter = spawnCollider.bounds.center;
        Vector3 spawnSize = spawnCollider.bounds.extents; // Metade do tamanho

        for (int i = 0; i < selectedSet.pieces.Count; i++)
        {
            // Escolhe um prefab do conjunto de peças do puzzle atual
            GameObject piecePrefab = selectedSet.pieces[i];

            // Gera uma posição aleatória dentro dos limites do spawnArea
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnCenter.x - spawnSize.x, spawnCenter.x + spawnSize.x),
                Random.Range(spawnCenter.y - spawnSize.y, spawnCenter.y + spawnSize.y),
                Random.Range(spawnCenter.z - spawnSize.z, spawnCenter.z + spawnSize.z)
            );

            // Gera uma rotação aleatória
            Quaternion randomRotation = Random.rotation;

            // Instancia a peça
            GameObject spawnedPiece = Instantiate(piecePrefab, randomPosition, randomRotation);
            spawnedPieces.Add(spawnedPiece);

            float randomPitch = Random.Range(0.9f, 1.1f);
            AudioManager.Instance.SFX_PlayAtSource("SpawnSound2", randomPosition, randomPitch);

            // Encontra o filho "Model"
            Transform modelTransform = spawnedPiece.transform.Find("Model");
            if (modelTransform != null)
            {
                Renderer pieceRenderer = modelTransform.GetComponent<Renderer>();
                if (pieceRenderer != null && availableMaterials.Count > 0)
                {
                    Material randomMaterial = availableMaterials[Random.Range(0, availableMaterials.Count)];
                    pieceRenderer.material = new Material(randomMaterial); // Criamos uma nova instância para garantir a mudança
                }
            }
            else
            {
                Debug.LogWarning($"PieceSpawner: Nenhum objeto 'Model' encontrado dentro de {spawnedPiece.name}. O material não foi aplicado.");
            }

            // Aguarda um tempo antes de spawnar a próxima peça
            yield return new WaitForSeconds(spawnDelay);
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

[System.Serializable]
public class PieceSet
{
    [Tooltip("Lista de peças pertencentes a este puzzle.")]
    public List<GameObject> pieces = new List<GameObject>();
}
