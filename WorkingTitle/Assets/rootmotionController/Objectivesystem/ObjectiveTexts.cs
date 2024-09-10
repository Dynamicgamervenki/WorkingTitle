using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveTexts : MonoBehaviour
{
    [SerializeField] string value;

    [SerializeField] TextMeshProUGUI ObjectiveText;

    [SerializeField] float timeToDissaperText;

    public void PlayerTheText()
    {
       StartCoroutine(nameof(TextVisibility));
    }

    IEnumerator TextVisibility()
    {
        ObjectiveText.text = value;
        yield return new WaitForSeconds(timeToDissaperText);
        ObjectiveText.text=string.Empty;
    }
}
