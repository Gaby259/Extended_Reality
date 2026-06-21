using UnityEngine;
using Vuforia;

public class ArtifactSpawner : MonoBehaviour
{
    [Header("Artifact")]
    public ArtifactData artifactData;

    [Header("World Info Panel")]
    public WorldInfoPanel sceneInfoPanel;

    private GameObject spawnedEffect;
    private ObserverBehaviour observer;

    void Awake()
    {
        observer = GetComponent<ObserverBehaviour>();

        if (observer == null)
            throw new System.Exception($"ArtifactSpawner on {gameObject.name}: missing ObserverBehaviour.");
    }

    void OnEnable()
    {
        if (observer != null)
            observer.OnTargetStatusChanged += OnTargetStatusChanged;

        if (sceneInfoPanel != null)
            sceneInfoPanel.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;

        if (sceneInfoPanel != null)
            sceneInfoPanel.gameObject.SetActive(false);
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
        if (sceneInfoPanel != null)
        {
            sceneInfoPanel.gameObject.SetActive(true);
            sceneInfoPanel.Setup(artifactData);
        }
    }

    private void OnMarkerLost()
    {
        if (spawnedEffect != null) { Destroy(spawnedEffect); spawnedEffect = null; }

        if (sceneInfoPanel != null)
            sceneInfoPanel.gameObject.SetActive(false);
    }
}