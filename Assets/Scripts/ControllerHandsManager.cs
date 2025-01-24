using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerHandsManager : MonoBehaviour
{
    public InputActionReference triggerActionReference;
    public InputActionReference gripActionReference;

    public Animator handAnimator;

    private void Awake() {
        handAnimator = GetComponent<Animator>();
        SetupInputAction();
    }

    public void SetupInputAction(){
        if(triggerActionReference != null && gripActionReference != null){
            triggerActionReference.action.performed += ctx => UpdateHandAnimator("Trigger", ctx.ReadValue<float>());
            triggerActionReference.action.canceled += ctx => UpdateHandAnimator("Trigger", 0);
            gripActionReference.action.performed += ctx => UpdateHandAnimator("Grip", ctx.ReadValue<float>());
            gripActionReference.action.canceled += ctx => UpdateHandAnimator("Grip", 0);

        }
        else {
            Debug.LogWarning("Input Action References are not set in the Inspector");
        }
    }

    private void UpdateHandAnimator(string parameterName, float value){
        if(handAnimator != null){
            handAnimator.SetFloat(parameterName, value);
        }
    }

    private void OnEnable() {
        triggerActionReference?.action.Enable();
        gripActionReference?.action.Enable();
    }

    private void OnDisable() {
        triggerActionReference?.action.Disable();
        gripActionReference?.action.Disable();
    }
}
