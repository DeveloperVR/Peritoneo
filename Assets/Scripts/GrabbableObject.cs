
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabbableObject : MonoBehaviour
{
    [Header("Identificacion de manos")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField]private GameObject locomotion;

    [Header("Acciones del stick")]
    [SerializeField] private InputActionReference scaleLeft;
    [SerializeField] private InputActionReference scaleRight;

    [Header("Configuracion de escala")]
    [SerializeField] private float scaleSpeed = 1f;
    [SerializeField] private float minScale = 0.2f;
    [SerializeField] private float maxScale = 3f;

    private XRGrabInteractable grabInteractable;

    private InputActionReference currentScaleAction;

    private Vector3 initialScale;

    private bool isGrabbed;

    private float scaleMultiplier = 1f;

    private void Awake()
    {
        // Obtener automaticamente el componente
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Obtener la escala inicial del propio objeto
        initialScale = transform.localScale;

        if (grabInteractable == null)
        {
            Debug.LogError("Falta XRGrabInteractable en " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        if (grabInteractable == null)
            return;

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        if (grabInteractable == null)
            return;

        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void Update()
    {
        if (!isGrabbed || currentScaleAction == null)
            return;

        Vector2 stick = currentScaleAction.action.ReadValue<Vector2>();

        float change = stick.y * scaleSpeed * Time.deltaTime;

        scaleMultiplier += change;

        scaleMultiplier = Mathf.Clamp(
            scaleMultiplier, minScale, maxScale);

        // Modificar la escala del propio objeto
        transform.localScale = initialScale * scaleMultiplier;
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        locomotion.SetActive(false);
        Transform interactor = args.interactorObject.transform;

        if (interactor == leftHand || interactor.IsChildOf(leftHand))
        {
            currentScaleAction = scaleRight;
            Debug.Log("Agarrado con mano izquierda");
        }
        else if (interactor == rightHand || interactor.IsChildOf(rightHand))
        {
            currentScaleAction = scaleLeft;
            Debug.Log("Agarrado con mano derecha");
        }
        else
        {
            Debug.LogWarning("No se pudo identificar la mano");
            return;
        }

        isGrabbed = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        locomotion.SetActive(true);
        isGrabbed = false;
        currentScaleAction = null;

        Debug.Log("Objeto liberado");
    }
}
