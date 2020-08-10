using UnityEngine;
using TMPro;

public class WordMaker : MonoBehaviour
{
    public TextAsset glossaryText;
    public HardLevel hardLevel;

    private Glossary glossary;

    public void EnableBoxes(string word)
    {
        for (int i = 0; i < word.Length - 1; i++)
        {
            word.ToUpper();
            int pos = Mathf.FloorToInt((glossary.MaxWordLength - word.Length) / 2f);
            Transform child = transform.GetChild(pos + i);

            child.gameObject.SetActive(true);
            child.GetChild(0).GetComponent<TextMesh>().text = word[i].ToString().ToUpper();
        }
    }

    public void GetWord()
    {
        glossary = new Glossary(glossaryText.text);
        DisableBoxes();
        glossary.SetHardLevel(hardLevel);
        EnableBoxes(glossary.GetWord());
    }

    public void DisableBoxes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
