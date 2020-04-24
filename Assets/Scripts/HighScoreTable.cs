using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighScoreEntry> highScoresEntryList;
    private List<Transform> highScoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        highScoresEntryList = new List<HighScoreEntry>()
        {
            new HighScoreEntry{ score = 34566, name = "AAA"},
            new HighScoreEntry{ score = 1223, name = "POPA"},
            new HighScoreEntry{ score = 122345, name = "ASM"},
            new HighScoreEntry{ score = 123, name = "PAL"},
            new HighScoreEntry{ score = 2345, name = "ASN"},
            new HighScoreEntry{ score = 124, name = "AAA"},
            new HighScoreEntry{ score = 34566, name = "ALAS"},
            new HighScoreEntry{ score = 2345, name = "AMS"},
            new HighScoreEntry{ score = 3451266, name = "ASD"},
            new HighScoreEntry{ score = 234, name = "BAB"}
        };

        for (int i = 0; i < highScoresEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScoresEntryList.Count; j++)
            {
                if (highScoresEntryList[j].score > highScoresEntryList[i].score)
                {
                    HighScoreEntry temp = highScoresEntryList[i];
                    highScoresEntryList[i] = highScoresEntryList[j];
                    highScoresEntryList[j] = temp;
                }
            }

        }

        highScoreEntryTransformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScoresEntryList)
        {
            CreateHighScoresEntryTemplate(highScoreEntry, entryContainer, highScoreEntryTransformList);
        }

       

    }

    private void  CreateHighScoresEntryTemplate(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1:
                rankString = "1ST"; break;
            case 2:
                rankString = "2ND"; break;
            case 3:
                rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highScoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highScoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }


    private class HighScoreEntry {
        public int score;
        public string name;
    }
   
}
