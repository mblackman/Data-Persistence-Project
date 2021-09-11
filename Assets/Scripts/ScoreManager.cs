using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<RoundData> rounds;
}

[System.Serializable]
public class RoundData
{
    public string playerName;
    public int score;
}

public class ScoreManager : MonoBehaviour
{
    private List<RoundData> rounds = new List<RoundData>();
    private string SavePath => Application.persistentDataPath + "/save.json";

    public static ScoreManager Instance { get; private set; }

    public IEnumerable<RoundData> Rounds => rounds;

    public string BestRoundLabel
    {
        get
        {
            var bestRound = Instance.rounds.OrderByDescending(r => r.score).FirstOrDefault();

            if (bestRound != null)
            {
                return $"Best Score: {bestRound.playerName} : {bestRound.score}";
            }

            return "Best Score:";
        }
    }

    public RoundData LastRound => rounds.LastOrDefault();

    public static string currentPlayer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    public void RecordScore(int score)
    {
        if (string.IsNullOrEmpty(currentPlayer))
        {
            Debug.Log("Current Player not set. Can't save score.");
        }
        {
            var newRound = new RoundData()
            {
                playerName = currentPlayer,
                score = score
            };
            rounds.Add(newRound);
            SaveData();
        }
    }

    private void SaveData()
    {
        var save = new SaveData();
        save.rounds = rounds;

        string json = JsonUtility.ToJson(save);
        File.WriteAllText(SavePath, json);
    }

    private void LoadData()
    {
        if (File.Exists(SavePath))
        {
            string content = File.ReadAllText(SavePath);
            var saveData = JsonUtility.FromJson<SaveData>(content);
            rounds = saveData.rounds;
        }
    }
}