using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCounter : MonoBehaviour
{
    [SerializeField] private int maxHearts = 3;
    private int hearts;
    [SerializeField] private GameObject healthPanel;
    [SerializeField] private GameObject heartFilled;
    [SerializeField] private GameObject heartEmpty;
    [SerializeField] private float padding = 20f;
    private float heartHeight;
    private float heartWidth;
    private RectTransform healthPanelTransform;
    private GameObject instantiatedHeart;
    // Start is called before the first frame update
    void Start()
    {
        hearts = maxHearts;
        heartHeight = heartFilled.GetComponent<RectTransform>().rect.height;
        heartWidth = heartFilled.GetComponent<RectTransform>().rect.width;
        healthPanelTransform = healthPanel.GetComponent<RectTransform>();
        DrawHearts();
    }

    public void GainHeart()
    {
        hearts++;
        if (hearts > maxHearts)
        {
            hearts = maxHearts;
        }
        DrawHearts();
    }

    public void LoseHeart()
    {
        hearts--;
        DrawHearts();
        if (hearts <= 0)
        {
            //Game Over
        }
    }

    public int GetHearts()
    {
        return hearts;
    }

    private void DrawHearts()
    {
        foreach (Transform child in healthPanel.transform)
        {
            Destroy(child.gameObject);
        }

        healthPanelTransform.sizeDelta = new Vector2(maxHearts * heartWidth + padding * 2 + padding * (maxHearts - 1), heartHeight + padding * 2);

        for (int i = 0; i < maxHearts; i++)
        {
            if (i < hearts)
            {
                instantiatedHeart = Instantiate(heartFilled, healthPanel.transform);
                instantiatedHeart.GetComponent<RectTransform>().localPosition = new Vector3(i * heartWidth + padding + i * padding, -(healthPanelTransform.rect.height / 2), 0);
            }
            else
            {
                instantiatedHeart = Instantiate(heartEmpty, healthPanel.transform);
                instantiatedHeart.GetComponent<RectTransform>().localPosition = new Vector3(i * heartWidth + padding + i * padding, -(healthPanelTransform.rect.height / 2), 0);
            }
        }
    }


}
