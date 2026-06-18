using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBoxesManager : MonoBehaviour
{
    public int difficultyLevel;

    [Header("Number Boxes")]
    [SerializeField] private List<InputNumberBox> inputs;
    [SerializeField] private List<OutputNumberBox> outputs;

    [Header("References")]
    [SerializeField] private GameObject inputsLayout;
    [SerializeField] private GameObject ouputsLayout;

    [Header("Prefabs")]
    [SerializeField] private InputNumberBox inputPrefab;
    [SerializeField] private OutputNumberBox outputPrefab;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new List<InputNumberBox>();
        outputs = new List<OutputNumberBox>();

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
            output1 += inputs[i].Number;
            output2 += inputs[i].Number;
        }

        outputs[0].UpdateOutput(output1);
        outputs[1].UpdateOutput(output2);
    }
}
