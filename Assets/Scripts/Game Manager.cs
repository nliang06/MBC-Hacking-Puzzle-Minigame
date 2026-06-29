using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NumberBoxesManager numberBoxesManager;

    // Start is called before the first frame update
    void Start()
    {
        numberBoxesManager.InitBoxes(Random.Range(1,5));
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
    /// Restart the game and generate a new hack
    /// </summary>
    public void StartNewGame()
    {
        Debug.Log("Restarting");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Handle player correctly solving the hack
    /// </summary>
    private void TriggerVictory()
    {
        AudioManager.instance.PlayVictorySound();
    }
}
