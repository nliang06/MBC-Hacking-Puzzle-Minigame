using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputNumberBox : MonoBehaviour, IPointerClickHandler
{
    public int Number { get; private set; }
    [SerializeField] private TextMeshProUGUI numberText;

    // Start is called before the first frame update
    void Start()
    {
        Number = 0;
        numberText.text = Number.ToString();
    }

    /// <summary>
    /// Increment number in input box and update output numbers
    /// </summary>
    public void IncrementNumber()
    {
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
    }

    /// <summary>
    /// Decrement number in input box and update output numbers
    /// </summary>
    public void DecrementNumber()
    {
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
    }

    /// <summary>
    /// Method for detecting right clicks because apparently the buttons don't have built-in right click detection
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            DecrementNumber();
    }
}
