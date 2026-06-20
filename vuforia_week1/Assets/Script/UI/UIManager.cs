using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvases")]
    public GameObject startCanvas;
    public GameObject instructionsCanvas;
    public GameObject infoPopupCanvas;

    [Header("Info Popup")]
    public ArtifactInfoPopup artifactInfoPopup;

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

    void Start()
    {
        ShowStart();
    }

    public void ShowStart()
    {
        startCanvas.SetActive(true);
        instructionsCanvas.SetActive(false);
        infoPopupCanvas.SetActive(false);
    }

    public void ShowInstructions()
    {
        startCanvas.SetActive(false);
        instructionsCanvas.SetActive(true);
        infoPopupCanvas.SetActive(false);
    }

    public void ShowAR()
    {
        startCanvas.SetActive(false);
        instructionsCanvas.SetActive(false);
        infoPopupCanvas.SetActive(false);
    }

    public void ShowInfoPopup(ArtifactData data)
    {
        if (artifactInfoPopup == null)
        {
            Debug.LogError("UIManager: artifactInfoPopup is not assigned in Inspector.");
            return;
        }

        artifactInfoPopup.Populate(data);
        infoPopupCanvas.SetActive(true);
    }

    public void HideInfoPopup()
    {
        infoPopupCanvas.SetActive(false);
    }
}