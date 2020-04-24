using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highScoreEntryTransformList;
    [SerializeField] private HighScores _HighScores;


    private void Awake()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        //AddHighScoreEntry(1000, "DAS");

        if (System.IO.File.Exists(Application.persistentDataPath + "/TableData.json"))
        {
            string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/TableData.json");
            HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

            Debug.Log(highScores);
            for (int i = 0; i < highScores.highScoresEntryList.Count; i++)
            {
                for (int j = i + 1; j < highScores.highScoresEntryList.Count; j++)
                {
                    if (highScores.highScoresEntryList[j].score > highScores.highScoresEntryList[i].score)
                    {
                        HighScoreEntry temp = highScores.highScoresEntryList[i];
                        highScores.highScoresEntryList[i] = highScores.highScoresEntryList[j];
                        highScores.highScoresEntryList[j] = temp;
                    }
                }

            }
            highScoreEntryTransformList = new List<Transform>();
            var index = 0;
            foreach (HighScoreEntry highScoreEntry in highScores.highScoresEntryList)
            {
                index += 1;
                if (index <= 10)
                {
                    CreateHighScoresEntryTemplate(highScoreEntry, entryContainer, highScoreEntryTransformList);
                }
            }


        }

      

        /*HighScores highScores = new HighScores { highScoresEntryList = highScoresEntryList };
        _HighScores = new HighScores { highScoresEntryList = highScoresEntryList };
        
        string potion = JsonUtility.ToJson(_HighScores);
        Debug.Log(potion);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/TableData.json", potion);
        var elem = System.IO.File.ReadAllText(Application.persistentDataPath + "/TableData.json");
        var exista = System.IO.File.Exists(Application.persistentDataPath + "/TableData.json");
        Debug.Log(elem);
        Debug.Log(exista);



       
        string json = JsonUtility.ToJson(highScores);

        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highScoreTable"));*/


       

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

        //string name = highScoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = "Player" + rank;

        //Set background visible odds and evens
        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);
        
        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        switch (rank)
        {
            default:
                entryTransform.Find("troph").gameObject.SetActive(false);
                break;
            case 1:
                entryTransform.Find("troph").GetComponent<Image>().color = Color.yellow;
                break;
            case 2:
                entryTransform.Find("troph").GetComponent<Image>().color = Color.gray;
                break;
            case 3:
                entryTransform.Find("troph").GetComponent<Image>().color = Color.red;
                break;
        }
        transformList.Add(entryTransform);
    }

    private void AddHighScoreEntry(int score, string name)
    {
        //Create HighScoreEntry
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };

        //Load saved HighScores
        string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/TableData.json");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        //Add new entry
        highScores.highScoresEntryList.Add(highScoreEntry);


        _HighScores = new HighScores { highScoresEntryList = highScores.highScoresEntryList };

        string list = JsonUtility.ToJson(_HighScores);
        Debug.Log(list);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/TableData.json", list);

    }

    [System.Serializable]
    private class HighScores
    {
        public List<HighScoreEntry> highScoresEntryList;
    }

    [System.Serializable]
    private class HighScoreEntry {
        public int score;
        public string name;
    }
   
}
