using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBoxesManager : MonoBehaviour
{
    public int difficultyLevel;

    [Header("Number Boxes")]
    [SerializeField] private List<InputNumberBox> inputs;
    [SerializeField] private List<OutputNumberBox> outputs;
    [SerializeField] private int answer1;
    [SerializeField] private int answer2;

    [Header("Formulas Info")]
    [SerializeField] private List<int> correctNums;
    private List<char> formula1List;
    private List<char> formula2List;
    [SerializeField] private string formula1;
    [SerializeField] private string formula2;


    [Header("References")]
    [SerializeField] private GameObject inputsLayout;
    [SerializeField] private GameObject ouputsLayout;
    [SerializeField] private GameObject answersLayout;

    [Header("Prefabs")]
    [SerializeField] private InputNumberBox inputPrefab;
    [SerializeField] private OutputNumberBox outputPrefab;
    [SerializeField] private NumberBox answerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new List<InputNumberBox>();
        outputs = new List<OutputNumberBox>();
        correctNums = new List<int>();
        formula1List = new List<char>();
        formula2List = new List<char>();

        answer1 = 0;
        answer2 = 0;

        // test set up of number boxes
        InitBoxes(difficultyLevel);
    }

    private void OnEnable()
    {
        // Observe input number boxes for when to update the ouputs
        InputNumberBox.onChangeNumber += CalculateOuput;
    }

    /// <summary>
    /// Initialize layout for the given difficulty level
    /// </summary>
    public void InitBoxes(int difficulty)
    {
        // Num of inputs is hacking level + 1
        for (int i = 0; i < difficulty + 1; i++)
        {
            InputNumberBox newInput = Instantiate(inputPrefab, inputsLayout.transform);
            inputs.Add(newInput);
        }

        for (int i = 0; i < 2; i++)
        {
            OutputNumberBox newOutput = Instantiate(outputPrefab, ouputsLayout.transform);
            outputs.Add(newOutput);
        }

        for (int i = 0; i < 2; i++)
        {
            NumberBox newAnswer = Instantiate(answerPrefab, answersLayout.transform);
        }

        GenerateFormulas(difficulty);
    }

    /// <summary>
    /// Randomly generates the 2 formulas that produce the 2 target outputs according to given difficulty
    /// </summary>
    /// <param name="difficulty"></param>
    private void GenerateFormulas(int difficulty)
    {
        char[] operations = { '+', '-', '*' };

        // Generate random nums for the correct inputs
        for (int i = 0; i < difficulty + 1; i++)
        {
            int num = Random.Range(0, 9);
            correctNums.Add(num);
        }

        for (int i = 0; i < difficulty; i++)
        {
            // Add variable to formula string
            char variable = (char)((int)'A' + i);
            formula1List.Add(variable);
            formula2List.Add(variable);

            // Stops adding unnecessary operator after last variable
            if (i == difficulty - 1) break;

            // Add random operation after variable and re-randomize for the 2nd formula
            char operation = operations[Random.Range(0, operations.Length)];
            formula1List.Add(operation);
            operation = operations[Random.Range(0, operations.Length)];
            formula2List.Add(operation);
        }

        // Concentate formulas into strings
        formula1 = string.Join(" ", formula1List);
        formula2 = string.Join(" ", formula2List);
    }

    /// <summary>
    /// Calculates the output and updates it according to the current set of inputs
    /// </summary>
    public void CalculateOuput()
    {
        // Calculate and update output from the current set of inputs
        int output1 = 0;
        int output2 = 0;

        for (int i = 0; i < inputs.Count; i++)
        {
            output1 += inputs[i].Number; // stub
            output2 += inputs[i].Number; // stub
        }

        outputs[0].UpdateOutput(output1);
        outputs[1].UpdateOutput(output2);

        // Check if outputs match the answers
        if (output1 == answer1 && output2 == answer2)
        {
            // Trigger win
            Debug.Log("Solved");
        }
    }
}
