using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberBoxesManager : MonoBehaviour
{
    #region Fields
    public delegate void OnSolve();
    public static event OnSolve onSolve;

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
    [SerializeField] private List<int> coefficients1;
    [SerializeField] private List<int> coefficients2;
    [SerializeField] private string formula1 = "";
    [SerializeField] private string formula2 = "";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI hackLevelText;

    [Header("Layout Groups")]
    [SerializeField] private GameObject leftInputs;
    [SerializeField] private GameObject bottomInputs;
    [SerializeField] private GameObject leftLines;
    [SerializeField] private GameObject bottomLines;
    [SerializeField] private GameObject ouputsLayout;
    [SerializeField] private GameObject answersLayout;

    [Header("Prefabs")]
    [SerializeField] private InputNumberBox inputPrefab;
    [SerializeField] private OutputNumberBox outputPrefab;
    [SerializeField] private OutputNumberBox answerPrefab;
    [SerializeField] private GameObject verticalLinePrefab;
    [SerializeField] private GameObject horizontalLinePrefab;
    #endregion

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
        coefficients1 = new List<int>();
        coefficients2 = new List<int>();
    }

    private void OnEnable()
    {
        // Observe input number boxes for when to update the ouputs
        InputNumberBox.onChangeNumber += CalculateOuput;
    }

    private void OnDisable()
    {
        // Observe input number boxes for when to update the ouputs
        InputNumberBox.onChangeNumber -= CalculateOuput;
    }

    /// <summary>
    /// Initialize layout for the given difficulty level
    /// </summary>
    public void InitBoxes(int difficulty)
    {
        hackLevelText.text = $"Level {difficulty} Hack";

        InitInputBoxes(difficulty);

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
        CalculateAnswers();
        CalculateOuput();
    }

    /// <summary>
    /// Private helper to handle initializing input boxes
    /// </summary>
    /// <param name="difficulty"></param>
    private void InitInputBoxes(int difficulty)
    {
        int leftInputCount = 0;
        int bottomInputCount = 0;

        // Num of inputs is hacking level + 1
        for (int i = 0; i < difficulty + 1; i++)
        {
            InputNumberBox newInput;

            // If one layout is already full of inputs, add the remaining input boxes to the other one
            if (leftInputCount > bottomInputCount)
            {
                newInput = Instantiate(inputPrefab, bottomInputs.transform);
                Instantiate(verticalLinePrefab, bottomLines.transform);
                bottomInputCount++;
            }
            else if (leftInputCount <= bottomInputCount)
            {
                newInput = Instantiate(inputPrefab, leftInputs.transform);
                Instantiate(horizontalLinePrefab, leftLines.transform);
                leftInputCount++;
            }
            // If both have space, randomly add input box to one of them
            else if (Random.Range(0, 2) == 1)
            {
                newInput = Instantiate(inputPrefab, bottomInputs.transform);
                Instantiate(verticalLinePrefab, bottomLines.transform);
                bottomInputCount++;
            }
            else
            {
                newInput = Instantiate(inputPrefab, leftInputs.transform);
                Instantiate(horizontalLinePrefab, leftLines.transform);
                leftInputCount++;
            }

            inputs.Add(newInput);
        }
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
            int num = Random.Range(0, 10);
            correctNums.Add(num);
        }

        char[] operations = { '+', '-', '*' };

        // Hack difficulty determines range of coefficients each variable can have
        int coefficientsMaxRange = 2 * difficulty;

        for (int i = 0; i < difficulty + 1; i++)
        {
            // Add random coefficient to each variable
            int coefficient1 = Random.Range(1, coefficientsMaxRange);
            int coefficient2 = Random.Range(1, coefficientsMaxRange);
            coefficients1.Add(coefficient1);
            coefficients2.Add(coefficient2);
            formula1 += coefficient1.ToString();
            formula2 += coefficient2.ToString();

            // Add variable to formula string
            char variable = (char)((int)'A' + i);
            formula1 += variable.ToString();
            formula2 += variable.ToString();
            //formula1List.Add(variable);
            //formula2List.Add(variable);

            // Stops adding unnecessary operator after last variable
            if (i == difficulty) break;

            // Add random operation after variable and re-randomize for the 2nd formula
            char operation = operations[Random.Range(0, operations.Length)];
            formula1 += $" {operation} ";
            operations1.Add(operation);

            operation = operations[Random.Range(0, operations.Length)];
            formula2 += $" {operation} ";
            operations2.Add(operation);
        }

        // Concentate formulas into strings
        //formula1 = string.Join(" ", formula1List);
        //formula2 = string.Join(" ", formula2List);
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
            answer1 = ApplyOperation(correctNums[i + 1], answer1, operations1[i], coefficients1[i + 1]);
            answer2 = ApplyOperation(correctNums[i + 1], answer2, operations2[i], coefficients2[i + 1]);
        }

        answers[0].UpdateNumber(answer1);
        answers[1].UpdateNumber(answer2);
    }

    /// <summary>
    /// Calculates the output and updates it according to the current set of inputs
    /// </summary>
    public void CalculateOuput()
    {
        Debug.Log("Calculated output");

        // Use first variable (i.e A) as starting value
        int output1 = inputs[0].Number * coefficients1[0];
        int output2 = inputs[0].Number * coefficients2[0];

        // Applies operations left to right (so no BEDMAS) according to operations lists for the 2 formulas
        for (int i = 0; i < inputs.Count - 1; i++)
        {
            output1 = ApplyOperation(inputs[i + 1].Number, output1, operations1[i], coefficients1[i + 1]);
            output2 = ApplyOperation(inputs[i + 1].Number, output2, operations2[i], coefficients2[i + 1]);
        }

        // Update visuals
        outputs[0].UpdateNumber(output1);
        outputs[1].UpdateNumber(output2);

        // Check if outputs match the answers
        if (outputs[0].Number == answer1 && outputs[1].Number == answer2)
        {
            // Trigger win
            onSolve?.Invoke();
            Debug.Log("Solved!");
        }
    }

    /// <summary>
    /// Private helper to read the operations lists and apply them, then returns result
    /// </summary>
    /// <param name="variable">variable to apply operation to</param>
    /// <param name="resultSoFar">result from previous operations</param>
    /// <param name="operation">the operation to apply</param>
    /// <param name="coefficient">coefficient of variable</param>
    /// <returns></returns>
    private int ApplyOperation(int variable, int resultSoFar, char operation, int coefficient)
    {
        switch(operation)
        {
            case '+':
                return resultSoFar + (coefficient * variable);
            case '-':
                return resultSoFar - (coefficient * variable);
            case '*':
                return resultSoFar * (coefficient * variable);
            default:
                Debug.Log("Invalid operation");
                return 0;
        }
    }
}
