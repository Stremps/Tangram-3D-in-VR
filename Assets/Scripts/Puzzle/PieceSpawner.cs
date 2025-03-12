using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [Header("Piece Sounds")]
    public string[] spawnSounds;
    public bool mixTones;

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

    private void playSpawnSound(Vector3 position)
    {
        if(AudioManager.Instance != null)
        {
            int randomSound = Random.Range(0, spawnSounds.Length);
            float randomPitch = (mixTones == true) ? Random.Range(0.9f, 1.1f) : 1;
            AudioManager.Instance.SFX_PlayAtSource(spawnSounds[randomSound], position, randomPitch);
        }
        else
        {
            Debug.Log("Audio manager is not instantiate!");
        }
    }

    private IEnumerator SpawnPiecesWithDelay(PieceSet selectedSet, BoxCollider spawnCollider)
    {
        Vector3 spawnCenter = spawnCollider.bounds.center;
        Vector3 spawnSize = spawnCollider.bounds.extents; // Metade do tamanho

        for (int i = 0; i < selectedSet.pieces.Count; i++)
        {
            // Escolhe um prefab do conjunto de peças do puzzle atual
            GameObject piecePrefab = selectedSet.pieces[i];

            // Gera uma posição aleatória dentro dos limites do spawnCollider
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnCenter.x - spawnSize.x, spawnCenter.x + spawnSize.x),
                Random.Range(spawnCenter.y - spawnSize.y, spawnCenter.y + spawnSize.y),
                Random.Range(spawnCenter.z - spawnSize.z, spawnCenter.z + spawnSize.z)
            );

            // Gera uma rotação aleatória
            Quaternion randomRotation = Random.rotation;

            // Instancia o prefab normalmente (o objeto pai que contém o "Model" como filho)
            GameObject spawnedPieceParent = Instantiate(piecePrefab, randomPosition, randomRotation);

            // Procura o filho "Model" que contém os componentes e o Renderer
            Transform modelTransform = spawnedPieceParent.transform.Find("Model");
            GameObject mainPiece;
            if (modelTransform != null)
            {
                // Desanexa o "Model" do objeto pai para que ele seja o objeto principal
                modelTransform.parent = null;
                mainPiece = modelTransform.gameObject;

                // Destrói o objeto pai, pois não será mais necessário
                Destroy(spawnedPieceParent);
            }
            else
            {
                Debug.LogWarning($"PieceSpawner: Nenhum objeto 'Model' encontrado em {spawnedPieceParent.name}. A peça principal será o próprio objeto instanciado.");
                mainPiece = spawnedPieceParent;
            }

            // Adiciona a peça principal na lista de peças instanciadas
            spawnedPieces.Add(mainPiece);

            playSpawnSound(randomPosition);

            // Aplica o material no Renderer do objeto principal
            Renderer pieceRenderer = mainPiece.GetComponent<Renderer>();
            if (pieceRenderer != null && availableMaterials.Count > 0)
            {
                Material randomMaterial = availableMaterials[Random.Range(0, availableMaterials.Count)];
                pieceRenderer.material = new Material(randomMaterial); // Nova instância para garantir a mudança
            }
            else
            {
                Debug.LogWarning($"PieceSpawner: Nenhum Renderer encontrado em {mainPiece.name}. O material não foi aplicado.");
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
