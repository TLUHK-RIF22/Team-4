using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartCounter : MonoBehaviour
{
    [SerializeField] private int maxHearts = 3;
    [SerializeField] private float damageCooldown = 1f;
    private int hearts;
    private float remainingDamageCooldown;
    [SerializeField] private GameObject healthPanel;
    [SerializeField] private GameObject heartFilled;
    [SerializeField] private GameObject heartEmpty;
    [SerializeField] private float padding = 20f;
    [SerializeField] private SpriteRenderer playerSprite;
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
        remainingDamageCooldown = 0;
        DrawHearts();
    }

    public void GainHearts(int amount = 1)
    {
        hearts+= amount;
        if (hearts > maxHearts)
        {
            hearts = maxHearts;
        }
        DrawHearts();
    }

    public void LoseHearts(int amount = 1)
    {
        if (remainingDamageCooldown > 0)
        {
            return;
        }
        hearts-= amount;
        DrawHearts();
        StartCoroutine(DamageCooldown());
        if (hearts <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the level
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

    private IEnumerator DamageCooldown()
    {
        remainingDamageCooldown = damageCooldown;
        while (remainingDamageCooldown > 0)
        {
            remainingDamageCooldown -= Time.deltaTime;
            playerSprite.color = new Color(1, 1 - remainingDamageCooldown / damageCooldown, 1 - remainingDamageCooldown / damageCooldown);
            yield return null;
        }
        playerSprite.color = Color.white;
    }


}
