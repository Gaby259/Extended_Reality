using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WorldInfoPanel : MonoBehaviour
{
    [Header("Glow")]
    public Image glowImage;
    public float glowSpeed = 2f;
    public float glowMinAlpha = 0.3f;
    public float glowMaxAlpha = 1f;

    [Header("Shake")]
    public float shakeAmount = 0.002f;
    public float shakeSpeed = 10f;

    [Header("Button")]
    public Button tapButton;

    private ArtifactData artifactData;
    private Vector3 originalLocalPosition;

    void Awake()
    {
        originalLocalPosition = transform.localPosition;

        if (tapButton == null)
            throw new System.Exception("WorldInfoPanel: tapButton is not assigned in Inspector.");

        tapButton.onClick.AddListener(OnTapped);
    }

    void OnDestroy()
    {
        if (tapButton != null)
            tapButton.onClick.RemoveListener(OnTapped);
    }

    public void Setup(ArtifactData data)
    {
        artifactData = data;
        StartCoroutine(GlowRoutine());
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator GlowRoutine()
    {
        while (true)
        {
            float t = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f;
            float alpha = Mathf.Lerp(glowMinAlpha, glowMaxAlpha, t);
            Color c = glowImage.color;
            c.a = alpha;
            glowImage.color = c;
            yield return null;
        }
    }

    private IEnumerator ShakeRoutine()
    {
        while (true)
        {
            float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            float offsetY = Mathf.Cos(Time.time * shakeSpeed * 0.7f) * shakeAmount;
            transform.localPosition = originalLocalPosition + new Vector3(offsetX, offsetY, 0f);
            yield return null;
        }
    }

    private void OnTapped()
    {
        if (UIManager.Instance == null || !UIManager.Instance.IsARActive)
        {
            Debug.LogWarning("WorldInfoPanel: tap ignored, AR is not active.");
            return;
        }

        if (artifactData == null)
        {
            Debug.LogError("WorldInfoPanel: artifactData is null. Was Setup() called?");
            return;
        }

        UIManager.Instance.ShowInfoPopup(artifactData);
    }
}