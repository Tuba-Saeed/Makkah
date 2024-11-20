using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetSequenceController : MonoBehaviour
{
    public GameObject safaMarwaInfoPanel;
    public GameObject safaPanel;
    public GameObject marwaPanel;
    public GameObject finalPanel;
    public GameObject indicator;
    public Transform safaTarget;
    public Transform marwaTarget;
    public Transform player;
    public float targetDistance = 2f;
    public Vector3 indicatorOffset = new Vector3(0, 2, 0);

    private bool safaVisited = false;
    private bool marwaVisited = false;
    private Transform currentTarget;

    void Start()
    {
        safaMarwaInfoPanel.SetActive(true);
        safaPanel.SetActive(false);
        marwaPanel.SetActive(false);
        finalPanel.SetActive(false);
        indicator.SetActive(false);

        currentTarget = marwaTarget;
        StartCoroutine(ClosePanelAfterDelay(safaMarwaInfoPanel, 3f, CloseSafaMarwaInfoPanel));
    }

    void Update()
    {
        if (indicator.activeSelf)
        {
            indicator.transform.position = player.position + indicatorOffset;
            Vector3 direction = currentTarget.position - player.position;
            if (direction != Vector3.zero)
            {
                indicator.transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // When Marwa target is reached
        if (!marwaVisited && Vector3.Distance(player.position, marwaTarget.position) < targetDistance)
        {
            marwaVisited = true;
            indicator.SetActive(false);
            marwaPanel.SetActive(true);

            StartCoroutine(ClosePanelAfterDelay(marwaPanel, 3f, CloseMarwaPanel));
        }

        // When Safa target is reached
        if (marwaVisited && !safaVisited && Vector3.Distance(player.position, safaTarget.position) < targetDistance)
        {
            safaVisited = true;
            indicator.SetActive(false);
            safaPanel.SetActive(true);

            StartCoroutine(ClosePanelAfterDelay(safaPanel, 3f, CloseSafaPanel));
        }
    }

    public void CloseSafaMarwaInfoPanel()
    {
        safaMarwaInfoPanel.SetActive(false);
        indicator.SetActive(true);
        currentTarget = marwaTarget;
    }

    public void CloseMarwaPanel()
    {
        marwaPanel.SetActive(false);
        currentTarget = safaTarget;
        indicator.SetActive(true);
    }

    public void CloseSafaPanel()
    {
        safaPanel.SetActive(false);
        indicator.SetActive(false);
        ShowFinalPanel();
    }

    public void ShowFinalPanel()
    {
        finalPanel.SetActive(true);
    }

    private IEnumerator ClosePanelAfterDelay(GameObject panel, float delay, System.Action onClose)
    {
        yield return new WaitForSeconds(delay);
        if (panel.activeSelf)
        {
            onClose?.Invoke();
        }
    }
}
