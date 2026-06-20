using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactInfoPopup : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image artifactImage;

    public void Populate(ArtifactData data)
    {
        if (data == null)
        {
            Debug.LogError("ArtifactInfoPopup: received null ArtifactData.");
            return;
        }

        nameText.text = data.artifactName;
        descriptionText.text = data.artifactDescription;

        if (data.artifactImage != null)
        {
            artifactImage.sprite = data.artifactImage;
            artifactImage.gameObject.SetActive(true);
        }
        else
        {
            artifactImage.gameObject.SetActive(false);
        }
    }

    // Assign this to the Go Back button
    public void GoBack()
    {
        UIManager.Instance.HideInfoPopup();
    }
}