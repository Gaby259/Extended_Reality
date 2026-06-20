using UnityEngine;

public class InstructionPageController : MonoBehaviour
{
    [Header("Pages")]
    public GameObject[] pages;

    private int currentPage = 0;

    void Start()
    {
        if (pages == null || pages.Length == 0)
            throw new System.Exception("InstructionPageController: no pages assigned in Inspector.");

        ShowPage(0);
    }

    // Assign this to every Next button and the final Start button
    public void NextPage()
    {
        currentPage++;

        if (currentPage >= pages.Length)
        {
            UIManager.Instance.ShowAR();
            return;
        }

        ShowPage(currentPage);
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == index);
    }
}