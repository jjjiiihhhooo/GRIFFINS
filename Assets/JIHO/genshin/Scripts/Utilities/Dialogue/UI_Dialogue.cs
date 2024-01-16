using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



[System.Serializable]
public class DialogueSequence
{
    public string speecher;
    [TextArea]
    public string context;
    [FoldoutGroup("Images", false), PreviewField]
    public Sprite portrait_L;
    [FoldoutGroup("Images"), PreviewField]
    public Sprite portrait_R;
    [FoldoutGroup("Images"), PreviewField]
    public Sprite portrait_LFace;
    [FoldoutGroup("Images"), PreviewField]
    public Sprite portrait_RFace;
    [FoldoutGroup("Images"), PreviewField]
    public Sprite popup_Image;
    [FoldoutGroup("Actions", false)]
    public UnityEvent action;
}

public class UI_Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueGroup;
    [SerializeField] private TextMeshProUGUI speecherText;
    [SerializeField] private TextMeshProUGUI contextText;
    [SerializeField] private Image portrait_L;
    [SerializeField] private Image portrait_L_Face;
    [SerializeField] private Image portrait_R;
    [SerializeField] private Image portrait_R_Face;
    [SerializeField] private Image popup_Image_UI;
    [SerializeField] private GameObject triangle;

    private Player player;

    private float textInterval = .02f;

    public void PlayDialogue(DialogueSequence[] dialogues)
    {


        dialogueGroup.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(Cor_PlayDialogue(dialogues));
    }

    public IEnumerator Cor_PlayDialogue(DialogueSequence[] dialogues)
    {
        portrait_L.gameObject.SetActive(false);
        portrait_R.gameObject.SetActive(false);
        portrait_L_Face.gameObject.SetActive(false);
        portrait_R_Face.gameObject.SetActive(false);
        popup_Image_UI.gameObject.SetActive(false);

        for (int i = 0; i < dialogues.Length; i++)
        {
            speecherText.text = dialogues[i].speecher;

            if (dialogues[i].portrait_L != null)
            {
                portrait_L.gameObject.SetActive(true);
                portrait_L_Face.gameObject.SetActive(false);
                portrait_L.sprite = dialogues[i].portrait_L;
            }
            else
            {
                portrait_L.gameObject.SetActive(false);
            }

            if (dialogues[i].portrait_R != null)
            {
                portrait_R.gameObject.SetActive(true);
                portrait_R_Face.gameObject.SetActive(false);
                portrait_R.sprite = dialogues[i].portrait_R;
            }
            else
            {
                portrait_R.gameObject.SetActive(false);
            }

            if (dialogues[i].portrait_LFace != null)
            {
                portrait_L_Face.gameObject.SetActive(true);
                portrait_L_Face.sprite = dialogues[i].portrait_LFace;
            }
            else
            {
                portrait_L_Face.gameObject.SetActive(false);
            }

            if (dialogues[i].portrait_RFace != null)
            {
                portrait_R_Face.gameObject.SetActive(true);
                portrait_R_Face.sprite = dialogues[i].portrait_RFace;
            }
            else
            {
                portrait_R_Face.gameObject.SetActive(false);
            }

            if (dialogues[i].popup_Image != null)
            {
                popup_Image_UI.gameObject.SetActive(true);
                popup_Image_UI.sprite = dialogues[i].popup_Image;
            }

            if (dialogues[i].popup_Image == null)
            {
                popup_Image_UI.gameObject.SetActive(false);
            }

            yield return null;

            for (int j = 0; j < dialogues[i].context.Length; j++)
            {
                contextText.text = dialogues[i].context.Substring(0, j);
                bool skipFlag = false;

                for (float t = textInterval; t > 0; t -= Time.deltaTime)
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.U))
                    {
                        skipFlag = true;
                    }
                    else
                    {
                        yield return null;
                    }
                }

                if (skipFlag) break;

            }
            contextText.text = dialogues[i].context;

            yield return new WaitForSeconds(0.5f);
            triangle.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.U));
            triangle.SetActive(false);

            if (dialogues[i].action != null)
                dialogues[i].action.Invoke();

            yield return null;
        }

        if (player == null) player = FindObjectOfType<Player>();

        dialogueGroup.SetActive(false);
    }
}


