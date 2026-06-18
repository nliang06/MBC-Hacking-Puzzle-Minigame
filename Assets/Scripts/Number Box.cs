using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberBox : MonoBehaviour
{
    [field: SerializeField] public int Number { get; protected set; }
    [SerializeField] protected TextMeshProUGUI numberText;

    // Start is called before the first frame update
    void Start()
    {
        Number = 0;
        numberText.text = Number.ToString();
    }

    public void UpdateNumber(int num)
    {
        Number = num;
        numberText.text = Number.ToString();
    }
}
