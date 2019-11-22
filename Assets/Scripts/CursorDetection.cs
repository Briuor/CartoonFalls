using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CursorDetection : MonoBehaviour
{

    private GraphicRaycaster gr;
    private PointerEventData pointerEventData = new PointerEventData(null);

    public Transform currentCharacterPlayer1;
    public Transform currentCharacterPlayer2;

    public Transform tokenPlayer1;
    public Transform tokenPlayer2;
    public bool hasTokenPlayer1;
    public bool hasTokenPlayer2;
    private TextMeshProUGUI cursorTextMesh;

    void Start()
    {
        gr = GetComponentInParent<GraphicRaycaster>();
        cursorTextMesh = GetComponentInChildren<TextMeshProUGUI>();
        SmashCSS.instance.ShowCharacterInSlot(0, null);
        hasTokenPlayer1 = true;
        hasTokenPlayer2 = false;

    }

    void Update()
    {
        //CONFIRM
        if (Keyboard.current.enterKey.wasReleasedThisFrame)
        {
            if (hasTokenPlayer2)
            {
                SmashCSS.instance.ConfirmCharacter(1, SmashCSS.instance.characters[currentCharacterPlayer2.GetSiblingIndex()]);
                // MUDA PRA PROXIMA CENA
            }
            else
            {
                hasTokenPlayer1 = false;
                hasTokenPlayer2 = true;
                SmashCSS.instance.ConfirmCharacter(0, SmashCSS.instance.characters[currentCharacterPlayer1.GetSiblingIndex()]);
                cursorTextMesh.text = "P2";
                cursorTextMesh.color = new Color32(82, 218, 255, 255);
            }
        }

        if (hasTokenPlayer1)
        {
            tokenPlayer1.position = transform.position;
        }

        if (hasTokenPlayer2)
        {
            tokenPlayer2.position = transform.position;
        }

        pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);

        if (hasTokenPlayer1)
        {
            if (results.Count > 0)
            {
                Transform raycastCharacter = results[0].gameObject.transform;

                if (raycastCharacter != currentCharacterPlayer1)
                {
                    if (currentCharacterPlayer1 != null)
                    {
                        currentCharacterPlayer1.Find("selectedBorder").GetComponent<Image>().DOKill();
                        currentCharacterPlayer1.Find("selectedBorder").GetComponent<Image>().color = Color.clear;
                    }
                    SetCurrentCharacter(raycastCharacter);
                }
            }
            else
            {
                if (currentCharacterPlayer1 != null)
                {
                    currentCharacterPlayer1.Find("selectedBorder").GetComponent<Image>().DOKill();
                    currentCharacterPlayer1.Find("selectedBorder").GetComponent<Image>().color = Color.clear;
                    SetCurrentCharacter(null);
                }
            }
        }

        else if (hasTokenPlayer2)
        {
            if (results.Count > 0)
            {
                Transform raycastCharacter = results[0].gameObject.transform;

                if (raycastCharacter != currentCharacterPlayer2)
                {
                    if (currentCharacterPlayer2 != null)
                    {
                        currentCharacterPlayer2.Find("selectedBorder").GetComponent<Image>().DOKill();
                        currentCharacterPlayer2.Find("selectedBorder").GetComponent<Image>().color = Color.clear;
                    }
                    SetCurrentCharacter(raycastCharacter);
                }
            }
            else
            {
                if (currentCharacterPlayer2 != null)
                {
                    currentCharacterPlayer2.Find("selectedBorder").GetComponent<Image>().DOKill();
                    currentCharacterPlayer2.Find("selectedBorder").GetComponent<Image>().color = Color.clear;
                }
            }
        }


    }

    void SetCurrentCharacter(Transform t)
    {

        if (t != null)
        {
            t.Find("selectedBorder").GetComponent<Image>().color = Color.white;
            t.Find("selectedBorder").GetComponent<Image>().DOColor(Color.red, .7f).SetLoops(-1);
        }

        if (hasTokenPlayer1)
            currentCharacterPlayer1 = t;
        else
            currentCharacterPlayer2 = t;

        if (t != null)
        {
            int index = t.GetSiblingIndex();
            Character character = SmashCSS.instance.characters[index];
            if (hasTokenPlayer1)
                SmashCSS.instance.ShowCharacterInSlot(0, character);
            else
                SmashCSS.instance.ShowCharacterInSlot(1, character);
        }
        else
        {
            SmashCSS.instance.ShowCharacterInSlot(0, null);
        }
    }

}
