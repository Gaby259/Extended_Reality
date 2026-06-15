using System;
using System.Collections.Generic;
using UnityEngine;

public class ScanProgressManager : MonoBehaviour
{
    public static ScanProgressManager Instance { get; private set; }

    public event Action<string> OnArtifactScanned;

    private readonly HashSet<string> scannedArtifacts = new HashSet<string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MarkArtifactScanned(string artifactId)
    {
        if (string.IsNullOrEmpty(artifactId))
        {
            Debug.LogError("[ScanProgressManager] Cannot mark scanned: artifactId is empty.");
            return;
        }

        if (scannedArtifacts.Contains(artifactId))
        {
            Debug.LogWarning($"[ScanProgressManager] {artifactId} already marked scanned.");
            return;
        }

        scannedArtifacts.Add(artifactId);
        OnArtifactScanned?.Invoke(artifactId);
    }

    public bool IsArtifactScanned(string artifactId)
    {
        return scannedArtifacts.Contains(artifactId);
    }
}