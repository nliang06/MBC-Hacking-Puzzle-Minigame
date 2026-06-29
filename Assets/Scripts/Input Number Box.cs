using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputNumberBox : NumberBox, IPointerClickHandler
{
    public delegate void OnChangeNumber();
    public static event OnChangeNumber onChangeNumber;

    private bool isSolved;

    // Start is called before the first frame update
    void Start()
    {
        isSolved = false;
    }

    private void OnEnable()
    {
        // Observe for correct solve
        NumberBoxesManager.onSolve += DisableInput;
    }

    private void OnDisable()
    {
        // Observe for correct solve
        NumberBoxesManager.onSolve -= DisableInput;
    }

    /// <summary>
    /// Stop player from changing inputs
    /// </summary>
    private void DisableInput()
    {
        gameObject.GetComponent<Button>().enabled = false;
        isSolved = true;
    }

    /// <summary>
    /// Increment number in input box and update output numbers
    /// </summary>
    public void IncrementNumber()
    {
        AudioManager.instance.PlayButtonClickSound();

        // Overflow back to 0
        if (Number == 9)
        {
            Number = 0;
        }
        else
        {
            Number++;
        }
        numberText.text = Number.ToString();
        onChangeNumber?.Invoke();
    }

    /// <summary>
    /// Decrement number in input box and update output numbers
    /// </summary>
    public void DecrementNumber()
    {
        AudioManager.instance.PlayButtonClickSound();

        // Underflow up to 9
        if (Number == 0)
        {
            Number = 9;
        }
        else
        {
            Number--;
        }
        numberText.text = Number.ToString();
        onChangeNumber?.Invoke();
    }

    /// <summary>
    /// Method for detecting right clicks because apparently the buttons don't have built-in right click detection
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !isSolved)
            DecrementNumber();
    }
}
