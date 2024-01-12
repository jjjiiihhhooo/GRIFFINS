using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[System.Serializable]
public class LargeDialogueData
{
    public string speecher;
    public Color SpeecherColor;
    [TextArea] public string context;
    [FoldoutGroup("Extras"), PreviewField]
    public Sprite fullImage;
    [FoldoutGroup("Extras")]
    public UnityEvent actions;
    [FoldoutGroup("Extras")]
    public bool disableCrossfade;
}

public class UI_LargeDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speecherText;
    [SerializeField] private TextMeshProUGUI contextText;
    [SerializeField] private Image fullImageUI_A;
    [SerializeField] private Image fullImageUI_B;
    [SerializeField] private GameObject DialogueGroup;
    [SerializeField] private GameObject triangle;

    [SerializeField] private LargeDialogueData[] test_dialogues;

    private float dialogueTextInterval = 0.05f;

    private void Start()
    {
        PlayDialogue(test_dialogues);
    }


    public void PlayDialogue(LargeDialogueData[] dialogues)
    {
        StopAllCoroutines();
        DialogueGroup.SetActive(true);
        StartCoroutine(Cor_PlayDialogue(dialogues));
    }

    private IEnumerator Cor_PlayDialogue(LargeDialogueData[] dialogues)
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            speecherText.color = dialogues[i].SpeecherColor;
            speecherText.text = dialogues[i].speecher;
            if (dialogues[i].fullImage != null)
            {
                SetFullImage(dialogues[i].fullImage, dialogues[i].disableCrossfade);
            }
            else
            {
                fullImageUI_A.gameObject.SetActive(false);
                fullImageUI_B.gameObject.SetActive(false);
            }


            yield return null;

            for (int j = 0; j < dialogues[i].context.Length; j++)
            {
                contextText.text = dialogues[i].context.Substring(0, j);
                bool skipFlag = false;

                for (float t = dialogueTextInterval; t > 0; t -= Time.deltaTime)
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                    {
                        contextText.text = dialogues[i].context;
                        skipFlag = true;
                    }
                    else
                    {
                        yield return null;
                    }
                }

                if (skipFlag) break;

            }
            yield return new WaitForSeconds(0.5f);
            triangle.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space));
            triangle.SetActive(false);

            if (dialogues[i].actions != null)
                dialogues[i].actions.Invoke();
        }

        DialogueGroup.SetActive(false);

    }

    private bool IsAside = true;

    private void SetFullImage(Sprite image, bool disableCrossfade)
    {
        if (IsAside)
        {
            fullImageUI_B.gameObject.SetActive(true);
            fullImageUI_B.sprite = image;
            fullImageUI_B.transform.SetAsLastSibling();
            if (!disableCrossfade)
            {
                fullImageUI_B.GetComponent<DOTweenAnimation>().DORestart();
            }
            else
            {
                fullImageUI_B.GetComponent<CanvasGroup>().alpha = 1.0f;
            }
            IsAside = false;
        }
        else
        {
            fullImageUI_A.gameObject.SetActive(true);
            fullImageUI_A.sprite = image;
            fullImageUI_A.transform.SetAsLastSibling();
            if (!disableCrossfade)
            {
                fullImageUI_A.GetComponent<DOTweenAnimation>().DORestart();
            }
            else
            {
                fullImageUI_A.GetComponent<CanvasGroup>().alpha = 1.0f;
            }
            IsAside = true;
        }
    }
}


