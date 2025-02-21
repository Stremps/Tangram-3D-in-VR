using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneTransitionOnGrab : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Nome da cena para carregar após o grab.")]
    public string sceneToLoad;

    public FadeScreen fadeScreen;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        // Obtém o componente XRGrabInteractable no objeto
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("SceneTransitionOnGrab: Nenhum XRGrabInteractable encontrado no objeto!");
            return;
        }

        // Adiciona um evento para detectar o grab
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("Transição para a cena: " + sceneToLoad);
            StartCoroutine(GoToSceneRoutine());
        }
        else
        {
            Debug.LogError("SceneTransitionOnGrab: Nenhuma cena definida para carregar!");
        }
    }

    private IEnumerator GoToSceneRoutine(){
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        // Launch the new scene
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnDestroy()
    {
        // Remove o listener ao destruir o objeto
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }
}
