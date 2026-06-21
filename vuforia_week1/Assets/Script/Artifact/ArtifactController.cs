using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ArtifactController : MonoBehaviour
{
    [SerializeField] private float floatHeight = 0.05f;
    [SerializeField] private float floatSpeed = 2f;

    private string artifactId;
    private Animator animator;
    private Vector3 startPosition;
    private bool isInteractable = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError($"[ArtifactController] No Animator found on {name}.");

        startPosition = transform.localPosition;
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
            float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.localPosition = startPosition + new Vector3(0f, offsetY, 0f);
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
        transform.localPosition = startPosition;

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