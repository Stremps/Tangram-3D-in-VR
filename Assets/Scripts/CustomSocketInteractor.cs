using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class CustomSocketInteractor : XRSocketInteractor
{
    [Header("Snapping Settings")]
    [Tooltip("Define a porcentagem de sobreposição necessária para ativar o snap.")]
    [Range(0f, 1f)]
    public float requiredOverlapPercentage = 0.5f;

    private Collider socketCollider;
    private static Dictionary<XRBaseInteractable, CustomSocketInteractor> bestSockets = new Dictionary<XRBaseInteractable, CustomSocketInteractor>();

    protected override void Awake()
    {
        base.Awake();
        socketCollider = GetComponent<Collider>();

        if (socketCollider == null)
        {
            Debug.LogError("CustomSocketInteractor: Nenhum Collider encontrado no socket!");
        }
    }

    public override bool CanHover(XRBaseInteractable interactable)
    {
        // Calcula a sobreposição do objeto atual
        float overlapPercentage = CalculateOverlapPercentage(interactable);

        // Se não atinge o limite mínimo, não permite hover
        if (overlapPercentage < requiredOverlapPercentage)
        {
            return false;
        }

        // Verifica qual socket tem a maior sobreposição para este objeto
        if (!bestSockets.ContainsKey(interactable) || bestSockets[interactable] == null ||
            CalculateOverlapPercentage(interactable, bestSockets[interactable]) < overlapPercentage)
        {
            bestSockets[interactable] = this;
        }

        // Permite hover somente se este for o socket com maior sobreposição
        return bestSockets[interactable] == this;
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // Mantém o comportamento original do XRSocketInteractor
        if (!base.CanSelect(interactable)) return false;

        // Permite seleção apenas se este socket for o melhor para o objeto
        return bestSockets.ContainsKey(interactable) && bestSockets[interactable] == this;
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

    private static float CalculateOverlapPercentage(XRBaseInteractable interactable, CustomSocketInteractor socket)
    {
        if (socket == null) return 0f;
        return socket.CalculateOverlapPercentage(interactable);
    }
}
