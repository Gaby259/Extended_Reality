using System;
using System.Collections;
using UnityEngine;
 
public class ArtifactRevealEffect : MonoBehaviour
{
    [Header("Settings")]
    public float effectDuration = 2f;
 
    private Action onComplete;
 
    public void Play(Action onComplete)
    {
        this.onComplete = onComplete;
        StartCoroutine(PlayRoutine());
    }
 
    private IEnumerator PlayRoutine()
    {
        // ParticleSystem or VFX plays automatically on spawn
        yield return new WaitForSeconds(effectDuration);
        onComplete?.Invoke();
    }
}