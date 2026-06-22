using UnityEngine;

[CreateAssetMenu(fileName = "ArtifactData", menuName = "AR/Artifact Data")]
public class ArtifactData : ScriptableObject
{
  [Header("3D Content")]
    public GameObject artifactPrefab;
    public GameObject revealEffectPrefab;
 
    [Header("Info Panel")]
    public string artifactName;
    [TextArea(3, 6)]
    public string artifactDescription;
    public Sprite artifactImage;
    
    [Header("Audio")]
    public AudioClip revealSound;
    public AudioClip ambientSound;
}