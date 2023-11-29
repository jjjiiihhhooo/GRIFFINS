using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace genshin
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private GameObject title_obj;
        [SerializeField] private TextMeshProUGUI[] quest_Texts;
        private QuestInfo currentQuest;

        [SerializeField] UnityEngine.UI.Outline outline;

        public bool checking_Quest;
        private void Awake()
        {
            StopAllCoroutines();
            //currentQuest = QuestTitle.instance.currentQuest;
        }

        private void Update()
        {

        }

        public void SetActiveQuest(int index, bool _bool)
        {
            for (int i = 0; i < index; i++)
                quest_Texts[i].gameObject.SetActive(_bool);
        }



        public void ReRoadUI(QuestInfo quest)
        {
            currentQuest = quest;
            SetActiveQuest(quest_Texts.Length, false);
            SetActiveQuest(currentQuest.questInfoDatas.Length, true);
            checking_Quest = false;
            UpdateUI();
        }

        public void UpdateUI()
        {
            for (int i = 0; i < currentQuest.questInfoDatas.Length; i++)
            {
                if (currentQuest.questInfoDatas[i].questType == QuestType.item)
                {
                    //quest_Texts[i].text = currentQuest.questInfoDatas[i].description + " ("
                    //                    + currentQuest.questInfoDatas[i].item.count.ToString() + "/"
                    //                    + currentQuest.questInfoDatas[i].itemCompleteCount.ToString() + ")";
                }
                else if(currentQuest.questInfoDatas[i].questType == QuestType.Monster)
                {
                    quest_Texts[i].text = currentQuest.questInfoDatas[i].description + " ("
                                        + currentQuest.questInfoDatas[i].monsterCurrentCount.ToString() + "/"
                                        + currentQuest.questInfoDatas[i].monsterCompleteCount.ToString() + ")";
                }
                else
                {
                    if (currentQuest.questInfoDatas[i].isClear) quest_Texts[i].text = currentQuest.questInfoDatas[i].description + " (1/1)";
                    else quest_Texts[i].text = currentQuest.questInfoDatas[i].description + " (0/1)";
                }
            }
        }

        public IEnumerator ViewOutLine()
        {
            while (!checking_Quest)
            {
                for (int i = 0; i < 3; i++)
                {
                    outline.enabled = true;
                    yield return new WaitForSeconds(0.3f);
                    outline.enabled = false;
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }
    }

}
