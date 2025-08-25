using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public GameObject dialogBox;
    public TMP_Text DialogText;
    public Button continueButton;

    private string[] Lines;
    private int currentLine;

    void Start()
    {
        if (dialogBox == null || DialogText == null || continueButton == null)
        {
            Debug.LogError("DialogManager tidak diatur dengan benar. Pastikan semua referensi diisi.");
            return;
        }
    }

    void Awake()
    {
        Instance = this;
        dialogBox.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(NextLine);
    }

    public void ShowDialog(string[] dialoglines)
    {
        if (dialoglines == null || dialoglines.Length == 0)
        {
            Debug.LogWarning("Dialog lines are empty or null.");
            return;
        }
        Lines = dialoglines;
        currentLine = 0;
        DialogText.text = Lines[currentLine];
        dialogBox.SetActive(true);
        continueButton.gameObject.SetActive(true);
    }

    void NextLine()
    {
        if (Lines == null || Lines.Length == 0)
        {
            Debug.LogWarning("Tidak ada dialog yang tersedia untuk dilanjutkan.");
            return;
        }

        currentLine++;
        Debug.Log("NextLine dipanggil CurrentLine: " + currentLine);

        if (currentLine < Lines.Length && Lines != null)
        {
            DialogText.text = Lines[currentLine];
        }
        else
        {
            Debug.Log("Dialog selesai");
            // Tamatkan dialog
            dialogBox.SetActive(false);
            continueButton.gameObject.SetActive(false);
            currentLine = 0;
            DialogText.text = "";
            Lines = null;
        }
    }   
}
