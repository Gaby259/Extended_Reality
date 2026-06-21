using System.Collections;
using UnityEngine;
using Vuforia;

public class ArtifactSpawner : MonoBehaviour
{
    [Header("Artifact")]
    public ArtifactData artifactData;
    public GameObject artifactObject;

    [Header("World Info Panel")]
    public WorldInfoPanel sceneInfoPanel;

    [Header("Timing")]
    [SerializeField] private float artifactHideDelay = 0f;

    private GameObject spawnedEffect;
    private ObserverBehaviour observer;
    private Coroutine markerFoundCoroutine;
    private bool isTracked = false;

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

        if (artifactObject != null)
            artifactObject.SetActive(false);
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
        bool tracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;

        if (tracked)
        {
            isTracked = true;

            if (markerFoundCoroutine != null)
                StopCoroutine(markerFoundCoroutine);

            // Clean up any existing effect before starting fresh
            CancelAndDestroyEffect();

            markerFoundCoroutine = StartCoroutine(MarkerFoundRoutine());
        }
        else
        {
            isTracked = false;

            if (markerFoundCoroutine != null)
            {
                StopCoroutine(markerFoundCoroutine);
                markerFoundCoroutine = null;
            }

            OnMarkerLost();
        }
    }

    private IEnumerator MarkerFoundRoutine()
    {
        if (artifactData == null)
        {
            Debug.LogError($"ArtifactSpawner on {gameObject.name}: ArtifactData is not assigned.");
            yield break;
        }

        yield return new WaitForSeconds(artifactHideDelay);

        if (!isTracked) yield break;

        if (artifactObject != null)
            artifactObject.SetActive(false);

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
        if (!isTracked) return;

        if (artifactObject != null)
            artifactObject.SetActive(true);

        if (sceneInfoPanel != null)
        {
            sceneInfoPanel.gameObject.SetActive(true);
            sceneInfoPanel.Setup(artifactData);
        }
    }

    private void OnMarkerLost()
    {
        CancelAndDestroyEffect();

        if (artifactObject != null)
            artifactObject.SetActive(false);

        if (sceneInfoPanel != null)
            sceneInfoPanel.gameObject.SetActive(false);
    }

    private void CancelAndDestroyEffect()
    {
        if (spawnedEffect != null)
        {
            var reveal = spawnedEffect.GetComponent<ArtifactRevealEffect>();
            if (reveal != null) reveal.Cancel();

            Destroy(spawnedEffect);
            spawnedEffect = null;
        }
    }
}