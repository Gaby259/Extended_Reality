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

    public void Cancel()
    {
        StopAllCoroutines();
        onComplete = null;
    }

    private IEnumerator PlayRoutine()
    {
        yield return new WaitForSeconds(effectDuration);

        if (this != null && gameObject.activeInHierarchy)
            onComplete?.Invoke();
    }
}