using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TutorialLevelScript : MonoBehaviour
{

    [SerializeField] private Color successTextColor = Color.green;
    [SerializeField] private Canvas tutorialTextCanvas;
    private List<TextMeshProUGUI> tutorialTexts;
    private GameObject player;
    private Camera mainCamera;
    private PlayerMovement playerMovement;
    private GameObject nugis;
    private NugisAI nugisAI;
    [SerializeField] private GameObject fadeToBlackPanel;
    [SerializeField] private GameObject glidingTrigger;
    [HideInInspector] public bool passedGlidingTrigger = false;
    [HideInInspector] public bool passedEndTrigger = false;
    [SerializeField] private GameObject tutorialStar;
    [SerializeField] private GameObject tutorialLeaf;
    [SerializeField] private GameObject tutorialAcorn;
    // Start is called before the first frame update
    void Start()
    {
        tutorialTexts = new List<TextMeshProUGUI>(tutorialTextCanvas.GetComponentsInChildren<TextMeshProUGUI>());
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        nugis = GameObject.FindGameObjectWithTag("Nugis");
        nugisAI = nugis.GetComponent<NugisAI>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTutorialTaskCompletion();
    }

    void CheckTutorialTaskCompletion()
    {
        List<TextMeshProUGUI> textsOnScreen = GetTutorialTextsOnScreen();

        if (textsOnScreen.Count == 0)
        {
            return;
        }

        foreach (TextMeshProUGUI text in textsOnScreen)
        {
            string[] taskTitleWords = text.gameObject.name.Split(' ');
            int taskNumber = int.Parse(taskTitleWords[taskTitleWords.Length - 1]);
            switch (taskNumber)
            {
                case 1:
                    if (playerMovement.movementState == PlayerMovement.MovementState.Grounded && playerMovement.dirX != 0)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 2:
                    if (playerMovement.movementState == PlayerMovement.MovementState.Jumping)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 3:
                    if (playerMovement.movementState == PlayerMovement.MovementState.Climbing && playerMovement.dirY != 0)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 4:
                    if (playerMovement.movementState == PlayerMovement.MovementState.Climbing && playerMovement.dirX != 0)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 5:
                    if (passedGlidingTrigger)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 6:
                    if (tutorialStar == null)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 7:
                    if (tutorialLeaf == null)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 8:
                    if (tutorialAcorn == null)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 9:
                    if (nugisAI.isStunned)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 10:
                    if (nugisAI.isStunned)
                    {
                        text.color = successTextColor;
                    }
                    break;
                case 11:
                    if (passedEndTrigger)
                    {
                        text.color = successTextColor;
                    }
                    break;
            }
        }
    }

    List<TextMeshProUGUI> GetTutorialTextsOnScreen()
    {
        List<TextMeshProUGUI> textsOnScreen = new List<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in tutorialTexts)
        {
            Vector3[] corners = new Vector3[4];
            text.GetComponent<RectTransform>().GetWorldCorners(corners);
            if (mainCamera.WorldToScreenPoint(corners[0]).x < Screen.width && mainCamera.WorldToScreenPoint(corners[0]).x > 0 || mainCamera.WorldToScreenPoint(corners[3]).x < Screen.width && mainCamera.WorldToScreenPoint(corners[3]).x > 0) {
                textsOnScreen.Add(text);
            }

        }
        return textsOnScreen;
    }

    public void EndTutorial()
    {
        StartCoroutine(EndTutorialCoroutine());
    }

    private IEnumerator EndTutorialCoroutine()
    {
        float levelEndFadeDuration = 2f;
        float timePassed = 0;
        while (timePassed < levelEndFadeDuration)
        {
            fadeToBlackPanel.GetComponent<Image>().color = new Color(0, 0, 0, timePassed / levelEndFadeDuration);
            timePassed += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("MenuScene");
    }
}
