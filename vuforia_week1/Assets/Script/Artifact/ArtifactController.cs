using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ArtifactController : MonoBehaviour
{
    [SerializeField] private float wiggleAngle = 10f;
    [SerializeField] private float wiggleSpeed = 2f;

    private string artifactId;
    private Animator animator;
    private Quaternion startRotation;
    private bool isInteractable = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[ArtifactController] No Animator found on {name}.");
        }
        startRotation = transform.localRotation;
    }
    public void Activate(ArtifactData data)
    {
        artifactId = data.artifactName;
    }
    public void SetArtifactId(string id)
    {
        artifactId = id;
    }

    void Update()
    {
        if (isInteractable)
        {
            float angle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAngle;
            transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, angle);
        }
    }

    void OnMouseDown()
    {
        Interact();
    }

    public void Interact()
    {
        if (!isInteractable)
        {
            Debug.LogWarning($"[ArtifactController] {artifactId} already interacted with.");
            return;
        }

        isInteractable = false;
        transform.localRotation = startRotation;

        if (animator != null)
        {
            animator.SetTrigger("Activate");
        }
        else
        {
            Debug.LogError($"[ArtifactController] Cannot play animation, Animator missing on {artifactId}.");
        }

        if (ScanProgressManager.Instance != null)
        {
            ScanProgressManager.Instance.MarkArtifactScanned(artifactId);
        }
        else
        {
            Debug.LogError("[ArtifactController] ScanProgressManager instance not found.");
        }
       
    }
}