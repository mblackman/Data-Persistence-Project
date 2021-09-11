#if UNITY_EDITOR

using UnityEditor;

#endif

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

using System.Text;
using System.Linq;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInput;

    [SerializeField]
    private TMP_Text bestScore;

    [SerializeField]
    private TMP_Text topScores;

    // Start is called before the first frame update
    private void Start()
    {
        bestScore.text = ScoreManager.Instance.BestRoundLabel;

        var builder = new StringBuilder();
        builder.Append("Top 10 Scores:");

        foreach (var round in ScoreManager.Instance.Rounds.OrderByDescending(r => r.score).Take(10))
        {
            builder.AppendLine();
            builder.Append(round.playerName);
            builder.Append(" : ");
            builder.Append(round.score);
        }

        topScores.text = builder.ToString();
    }

    public void StartGame()
    {
        if (nameInput.text != null)
        {
            ScoreManager.currentPlayer = nameInput.text;
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Enter a player name.");
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    // Update is called once per frame
    private void Update()
    {
    }
}