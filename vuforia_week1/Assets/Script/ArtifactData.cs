using UnityEngine;

[CreateAssetMenu(fileName = "ArtifactData", menuName = "AR/Artifact Data")]
public class ArtifactData : ScriptableObject
{
    public string artifactId;
    public GameObject artifactPrefab;
    public GameObject spawnEffectPrefab;
}
