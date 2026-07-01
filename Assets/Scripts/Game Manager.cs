using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int difficultyLevel = 1;

    [SerializeField] private NumberBoxesManager numberBoxesManager;

    [Header("Line Colours")]
    [SerializeField] private Color unsolvedColour;
    [SerializeField] private Color solvedColour;

    [Header("UI References")]
    [SerializeField] private Image line1;
    [SerializeField] private Image line2;

    // Start is called before the first frame update
    void Start()
    {
        numberBoxesManager.InitBoxes(difficultyLevel);
        line1.color = unsolvedColour;
        line2.color = unsolvedColour;
    }

    private void OnEnable()
    {
        // Observe for correct solve
        NumberBoxesManager.onSolve += TriggerVictory;
    }

    private void OnDisable()
    {
        // Observe for correct solve
        NumberBoxesManager.onSolve -= TriggerVictory;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Restart the game and generate a new hack with given difficulty level
    /// </summary>
    public void StartNewGame(int difficulty)
    {
        AudioManager.instance.PlayButtonClickSound();
        Debug.Log("Restarting");
        difficultyLevel = difficulty;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Handle player correctly solving the hack
    /// </summary>
    private void TriggerVictory()
    {
        AudioManager.instance.PlayVictorySound();
        line1.color = solvedColour;
        line2.color = solvedColour;
    }
}
