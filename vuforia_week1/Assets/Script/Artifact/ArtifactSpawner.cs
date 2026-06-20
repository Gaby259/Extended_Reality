using UnityEngine;
using Vuforia;

public class ArtifactSpawner : MonoBehaviour
{
    [Header("Artifact")]
    public ArtifactData artifactData;

    [Header("World Info Panel")]
    public GameObject worldInfoPanelPrefab;
    public Vector3 infoPanelOffset = new Vector3(0.15f, 0f, 0f);

    private GameObject spawnedArtifact;
    private GameObject spawnedEffect;
    private GameObject spawnedInfoPanel;
    private ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();

        if (observer == null)
            throw new System.Exception($"ArtifactSpawner on {gameObject.name}: missing ObserverBehaviour.");

        observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;

        if (isTracked)
            OnMarkerFound();
        else
            OnMarkerLost();
    }

    private void OnMarkerFound()
    {
        if (artifactData == null)
        {
            Debug.LogError($"ArtifactSpawner on {gameObject.name}: ArtifactData is not assigned.");
            return;
        }

        if (artifactData.revealEffectPrefab != null)
        {
            spawnedEffect = Instantiate(artifactData.revealEffectPrefab, transform.position, transform.rotation, transform);

            var reveal = spawnedEffect.GetComponent<ArtifactRevealEffect>();

            if (reveal == null)
                throw new System.Exception($"{artifactData.name} revealEffectPrefab is missing ArtifactRevealEffect component.");

            reveal.Play(OnRevealComplete);
        }
        else
        {
            OnRevealComplete();
        }
    }

    private void OnRevealComplete()
    {
        if (artifactData.artifactPrefab != null)
        {
            spawnedArtifact = Instantiate(artifactData.artifactPrefab, transform.position, transform.rotation, transform);

            var controller = spawnedArtifact.GetComponent<ArtifactController>();

            if (controller != null)
                controller.Activate(artifactData);
        }

        if (worldInfoPanelPrefab != null)
        {
            Vector3 panelPos = transform.position + transform.TransformDirection(infoPanelOffset);
            spawnedInfoPanel = Instantiate(worldInfoPanelPrefab, panelPos, transform.rotation, transform);

            var infoPanel = spawnedInfoPanel.GetComponent<WorldInfoPanel>();

            if (infoPanel == null)
                throw new System.Exception("worldInfoPanelPrefab is missing WorldInfoPanel component.");

            infoPanel.Setup(artifactData);
        }
    }

    private void OnMarkerLost()
    {
        if (spawnedArtifact != null) { Destroy(spawnedArtifact); spawnedArtifact = null; }
        if (spawnedEffect != null) { Destroy(spawnedEffect); spawnedEffect = null; }
        if (spawnedInfoPanel != null) { Destroy(spawnedInfoPanel); spawnedInfoPanel = null; }
    }
}