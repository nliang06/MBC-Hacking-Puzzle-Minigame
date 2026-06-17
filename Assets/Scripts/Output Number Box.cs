using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutputNumberBox : MonoBehaviour
{
    [field: SerializeField] public int Number { get; private set; }
    [SerializeField] private TextMeshProUGUI numberText;

    // Start is called before the first frame update
    void Start()
    {
        Number = 0;
        numberText.text = Number.ToString();
    }

    public void UpdateOutput(int num)
    {
        Number = num;
        numberText.text = Number.ToString();
    }
}
