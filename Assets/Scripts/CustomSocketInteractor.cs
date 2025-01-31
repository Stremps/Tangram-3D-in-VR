using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomSocketInteractor : XRSocketInteractor
{
    [Header("Snapping Settings")]
    [Tooltip("Define a porcentagem de sobreposição necessária para ativar o snap.")]
    [Range(0f, 1f)]
    public float requiredOverlapPercentage = 0.5f; // Adicionamos apenas essa nova propriedade

    private Collider socketCollider;

    protected override void Awake()
    {
        base.Awake(); // Mantém a inicialização original
        socketCollider = GetComponent<Collider>();

        if (socketCollider == null)
        {
            Debug.LogError("CustomSocketInteractor: Nenhum Collider encontrado no socket!");
        }
    } 

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // Mantém o comportamento original
        if (!base.CanSelect(interactable)) return false;

        // Aplica a nova lógica de sobreposição
        float overlapPercentage = CalculateOverlapPercentage(interactable);
        return overlapPercentage >= requiredOverlapPercentage;
    }

    private float CalculateOverlapPercentage(XRBaseInteractable interactable)
    {
        Collider interactableCollider = interactable.GetComponent<Collider>();
        if (interactableCollider == null || socketCollider == null) return 0f;

        Bounds socketBounds = socketCollider.bounds;
        Bounds objectBounds = interactableCollider.bounds;

        Bounds intersection = new Bounds();
        intersection.SetMinMax(
            Vector3.Max(socketBounds.min, objectBounds.min),
            Vector3.Min(socketBounds.max, objectBounds.max)
        );

        if (intersection.size.x <= 0 || intersection.size.y <= 0 || intersection.size.z <= 0)
            return 0f;

        float intersectionVolume = intersection.size.x * intersection.size.y * intersection.size.z;
        float objectVolume = objectBounds.size.x * objectBounds.size.y * objectBounds.size.z;

        return intersectionVolume / objectVolume;
    }
}
