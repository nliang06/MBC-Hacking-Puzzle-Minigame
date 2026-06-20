using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBoxesManager : MonoBehaviour
{
    public int difficultyLevel;

    [Header("Number Boxes")]
    [SerializeField] private List<InputNumberBox> inputs;
    [SerializeField] private List<OutputNumberBox> outputs;
    [SerializeField] private List<OutputNumberBox> answers;
    [SerializeField] private int answer1;
    [SerializeField] private int answer2;

    [Header("Formulas Info")]
    [SerializeField] private List<int> correctNums;
    private List<char> formula1List;
    private List<char> formula2List;
    [SerializeField] private List<char> operations1;
    [SerializeField] private List<char> operations2;
    [SerializeField] private string formula1;
    [SerializeField] private string formula2;

    [Header("Layout Groups")]
    [SerializeField] private GameObject inputsLayout;
    [SerializeField] private GameObject ouputsLayout;
    [SerializeField] private GameObject answersLayout;

    [Header("Prefabs")]
    [SerializeField] private InputNumberBox inputPrefab;
    [SerializeField] private OutputNumberBox outputPrefab;
    [SerializeField] private OutputNumberBox answerPrefab;

    [Header("SFX")]
    [SerializeField] private AudioClip victorySFX;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new List<InputNumberBox>();
        outputs = new List<OutputNumberBox>();
        answers = new List<OutputNumberBox>();
        correctNums = new List<int>();
        formula1List = new List<char>();
        formula2List = new List<char>();
        operations1 = new List<char>();
        operations2 = new List<char>();

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
            OutputNumberBox newAnswer = Instantiate(answerPrefab, answersLayout.transform);
            answers.Add(newAnswer);
        }

        GenerateFormulas(difficulty);
    }

    /// <summary>
    /// Randomly generates the 2 formulas that produce the 2 target outputs according to given difficulty
    /// </summary>
    /// <param name="difficulty"></param>
    private void GenerateFormulas(int difficulty)
    {
        // Generate random nums for the correct inputs
        for (int i = 0; i < difficulty + 1; i++)
        {
            int num = Random.Range(0, 9);
            correctNums.Add(num);
        }

        char[] operations = { '+', '-', '*' };

        for (int i = 0; i < difficulty + 1; i++)
        {
            // Add variable to formula string
            char variable = (char)((int)'A' + i);
            formula1List.Add(variable);
            formula2List.Add(variable);

            // Stops adding unnecessary operator after last variable
            if (i == difficulty) break;

            // Add random operation after variable and re-randomize for the 2nd formula
            char operation = operations[Random.Range(0, operations.Length)];
            formula1List.Add(operation);
            operations1.Add(operation);

            operation = operations[Random.Range(0, operations.Length)];
            formula2List.Add(operation);
            operations2.Add(operation);
        }

        // Concentate formulas into strings
        formula1 = string.Join(" ", formula1List);
        formula2 = string.Join(" ", formula2List);

        CalculateAnswers();
    }

    /// <summary>
    /// Private helper to calculate answers for the generated variables and formula
    /// </summary>
    private void CalculateAnswers()
    {
        // Use first variable (i.e A) as starting value
        answer1 = correctNums[0];
        answer2 = correctNums[0];

        // Applies operations left to right (so no BEDMAS) according to operations lists for the 2 formulas
        for (int i = 0; i < correctNums.Count - 1; i++)
        {
            answer1 = ApplyOperation(correctNums[i + 1], answer1, operations1[i]);
            answer2 = ApplyOperation(correctNums[i + 1], answer2, operations2[i]);
        }

        answers[0].UpdateNumber(answer1);
        answers[1].UpdateNumber(answer2);
    }

    /// <summary>
    /// Calculates the output and updates it according to the current set of inputs
    /// </summary>
    public void CalculateOuput()
    {
        AudioManager.instance.PlayButtonClickSound();
        // Use first variable (i.e A) as starting value
        int output1 = inputs[0].Number;
        int output2 = inputs[0].Number;

        // Applies operations left to right (so no BEDMAS) according to operations lists for the 2 formulas
        for (int i = 0; i < inputs.Count - 1; i++)
        {
            output1 = ApplyOperation(inputs[i + 1].Number, output1, operations1[i]);
            output2 = ApplyOperation(inputs[i + 1].Number, output2, operations2[i]);
        }

        // Update visuals
        outputs[0].UpdateNumber(output1);
        outputs[1].UpdateNumber(output2);

        // Check if outputs match the answers
        if (output1 == answer1 && output2 == answer2)
        {
            // Trigger win
            AudioManager.instance.PlaySoundFX(victorySFX, transform, 0.7f);
        }
    }

    /// <summary>
    /// Private helper to read the operations lists and apply them, then returns result
    /// </summary>
    /// <param name="variable">variable to apply operation to</param>
    /// <param name="resultSoFar">result from previous operations</param>
    /// <param name="operation">the operation to apply</param>
    /// <returns></returns>
    private int ApplyOperation(int variable, int resultSoFar, char operation)
    {
        switch(operation)
        {
            case '+':
                return resultSoFar + variable;
            case '-':
                return resultSoFar - variable;
            case '*':
                return resultSoFar * variable;
            default:
                Debug.Log("Invalid operation");
                return 0;
        }
    }
}
