using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [Tooltip("Lista de silhuetas a serem montadas.")]
    public List<GameObject> puzzleSilhouettes = new List<GameObject>();

    [Tooltip("Lista de prefabs finais que substituirão as silhuetas após conclusão.")]
    public List<GameObject> finalPrefabs = new List<GameObject>();

    [Tooltip("Tempo de espera antes de substituir a silhueta pelo objeto final.")]
    public float transformDelay = 2f;

    [Tooltip("Tempo de transição entre um puzzle e outro.")]
    public float transitionDelay = 2f;

    [Header("Final Scene Settings")]

    [Tooltip("Habilitar ou não a transição de cena no final.")]
    public bool enableSceneTransition = true;

    [Tooltip("Nome da cena para carregar após concluir todas as silhuetas.")]
    public string sceneToLoad;

    [Tooltip("Tempo de espera antes de carregar a cena final.")]
    public float sceneTransitionDelay = 3f; // Novo temporizador para a transição de cena

    public GameObject particleEffectPrefab;


    private int currentPuzzleIndex = 0;
    private GameObject activePuzzle;
    private GameObject activeFinalPrefab;
    public FadeScreen fadeScreen;
    private List<GameObject> activePieces = new List<GameObject>();
    public PieceSpawner pieceSpawner;


    private void Start()
    {
        if (puzzleSilhouettes.Count > 0 && puzzleSilhouettes.Count == finalPrefabs.Count)
        {
            LoadNextPuzzle(); // Inicia o primeiro puzzle
        }
        else
        {
            Debug.LogError("Certifique-se de que todas as silhuetas possuem um prefab final correspondente!");
        }
    }

    private void LoadNextPuzzle()
    {
        if (currentPuzzleIndex >= puzzleSilhouettes.Count)
        {
            Debug.Log("Todos os quebra-cabeças foram concluídos!");
            
            if (enableSceneTransition)
                StartCoroutine(GoToSceneRoutine());
            return;
        }


        // Se houver um FinalPrefab ativo na cena, ele será destruído antes de carregar o próximo puzzle
        if (activeFinalPrefab != null)
        {
            Destroy(activeFinalPrefab);
            activeFinalPrefab = null;
        }

        // Instancia a nova silhueta na posição e rotação do PuzzleManager
        activePuzzle = Instantiate(puzzleSilhouettes[currentPuzzleIndex], transform.position, transform.rotation);
        PuzzleSilhouette puzzleScript = activePuzzle.GetComponent<PuzzleSilhouette>();

        if (puzzleScript != null)
        {
            puzzleScript.OnPuzzleCompleted += HandlePuzzleCompletion;
        }
        else
        {
            Debug.LogError("A silhueta não contém o script PuzzleSilhouette!");
        }

        // Chama o PieceSpawner para spawnar novas peças
        if (pieceSpawner != null)
        {
            pieceSpawner.SpawnPieces(currentPuzzleIndex);
        }
    }


    private void HandlePuzzleCompletion()
    {
        if (activePuzzle != null)
        {
            // Obtém as peças encaixadas antes de destruir a silhueta
            PuzzleSilhouette puzzleScript = activePuzzle.GetComponent<PuzzleSilhouette>();
            if (puzzleScript != null)
            {
                activePieces = puzzleScript.GetPlacedPieces();
            }

            StartCoroutine(TransformPuzzle());
        }
    }

    private IEnumerator TransformPuzzle()
    {
        
        // ADD MORE CONTROL OF THIS TRANSITION
        if (particleEffectPrefab != null)
        {
            yield return new WaitForSeconds(transformDelay/2);
            // ADD NAME CHOICE IN INSPECTOR
            AudioManager.Instance.SFX_PlayAtSource("magicTransition", transform.position);
            GameObject particles = Instantiate(particleEffectPrefab, activePuzzle.transform.position, Quaternion.identity);
            Destroy(particles, particleEffectPrefab.GetComponent<ParticleSystem>().main.duration); // 🔹 Destroi as partículas após sua duração
            
        }
        yield return new WaitForSeconds(transformDelay/2);

        // Remove a silhueta e todas as peças encaixadas
        Destroy(activePuzzle);
        foreach (var piece in activePieces)
        {
            Destroy(piece);
        }
        activePieces.Clear();

        // Instancia o prefab final na posição e rotação do PuzzleManager
        activeFinalPrefab = Instantiate(finalPrefabs[currentPuzzleIndex], transform.position, transform.rotation);

        currentPuzzleIndex++;

        if (currentPuzzleIndex < puzzleSilhouettes.Count)
        {
            StartCoroutine(TransitionToNextPuzzle());
        }
        else
        {
            Debug.Log("Todos os objetos foram transformados. Fim do desafio!");
            yield return new WaitForSeconds(transformDelay/2);
            AudioManager.Instance.SFX_PlayAtSource("PuzzleConclusion", transform.position);
            if(enableSceneTransition) 
                StartCoroutine(GoToSceneRoutine());
        }
    }

    private IEnumerator TransitionToNextPuzzle()
    {
        yield return new WaitForSeconds(transitionDelay);
        LoadNextPuzzle();
    }

    public IEnumerator GoToSceneRoutine(){
        yield return new WaitForSeconds(sceneTransitionDelay);
        fadeScreen.FadeOut();
        AudioManager.Instance.Music_FadeOut(fadeScreen.fadeDuration);
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        // Launch the new scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
