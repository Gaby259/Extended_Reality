using UnityEngine;

public class ArtifactSpawner : MonoBehaviour
{
    [SerializeField] private ArtifactData artifactData;

    private GameObject spawnedArtifact;
    private GameObject spawnedEffect;

    public void SpawnArtifact()
    {
        if (artifactData == null)
        {
            Debug.LogError($"[ArtifactSpawner] No ArtifactData assigned on {name}.");
            return;
        }

        if (spawnedArtifact != null) return;

        if (artifactData.artifactPrefab != null)
        {
            spawnedArtifact = Instantiate(artifactData.artifactPrefab, transform.position, transform.rotation, transform);
            var controller = spawnedArtifact.GetComponent<ArtifactController >();
            if (controller != null)
            {
                controller.SetArtifactId(artifactData.artifactId);
            }
            else
            {
                Debug.LogError($"[ArtifactSpawner] {artifactData.artifactPrefab.name} is missing ArtifactController.");
            }
        }
        else
        {
            Debug.LogError($"[ArtifactSpawner] artifactPrefab not set on {artifactData.name}.");
        }

        if (artifactData.spawnEffectPrefab != null)
        {
            spawnedEffect = Instantiate(artifactData.spawnEffectPrefab, transform.position, transform.rotation, transform);
        }
    }

    public void RemoveArtifact()
    {
        if (spawnedArtifact != null)
        {
            Destroy(spawnedArtifact);
            spawnedArtifact = null;
        }

        if (spawnedEffect != null)
        {
            Destroy(spawnedEffect);
            spawnedEffect = null;
        }
    }
}