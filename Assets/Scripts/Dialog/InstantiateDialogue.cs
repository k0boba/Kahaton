using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateDialogue : MonoBehaviour
{
    #region variables
    public static InstantiateDialogue instance = null;
    [HideInInspector] public Dialogue dialogue;    
    public TaskBoardManager questsManager;
    [HideInInspector] public InventoryManager itemsManager;

    [SerializeField] private GameObject Window;
        
    [SerializeField] private Text nodeText;
    [SerializeField] private Text firstAnswer;
    [SerializeField] private Text secondAnswer;
    [SerializeField] private Text thirdAnswer;
    [SerializeField] private Button firstButton;
    [SerializeField] private Button secondButton;
    [SerializeField] private Button thirdButton;

    [HideInInspector] public bool dialogueEnded = false; //Закончен ли диалог?
    private bool firstNodeShown = false; //1 нод был показан?
    private bool questFalse = false; //квест не выполнен?

    [HideInInspector]public TextAsset ta;

    private int currentNode = 0;    
    [HideInInspector] public int butClicked;
    [HideInInspector] public bool objectSetActiv = false;
    #endregion

    #region 
    void Start()
    {
        if (instance == null)
        { instance = this; }        

        secondButton.enabled = false;
        thirdButton.enabled = false;
       
        firstButton.onClick.AddListener(but1);
        secondButton.onClick.AddListener(but2);
        thirdButton.onClick.AddListener(but3);
        
    }

    private void Update()
    {
        if (ta!=null)
        {
            if (dialogueEnded == false)
            {
                if (!firstNodeShown)
                {
                    firstStart();
                }

            }
        }
                   
    }

    #region // Buttons
    private void but1()
    {
        butClicked = 0;
        AnswerClicked(0);
    }
    private void but2()
    {
        butClicked = 1;
        AnswerClicked(1);
    }
    private void but3()
    {
        butClicked = 2;
        AnswerClicked(2);
    }
    #endregion
        
    private void firstStart()
    {
        objectSetActiv = false;
        dialogue = null;       
        dialogue = Dialogue.Load(ta);
        currentNode = 0;
        firstNodeShown = true;
        WriteText();
    }    
   
    private void WriteText()
    {
        deleteDialogue(); //скидываем текст ответа каждый раз перед началом печати нового ответа НПС
        nodeText.text = dialogue.nodes[currentNode].Npctext;
      
        firstAnswer.text = dialogue.nodes[currentNode].answers[0].text;     //первый ответ будет всегда            
        if (dialogue.nodes[currentNode].answers.Length >= 2)                //если ответов два
        {
            secondButton.enabled = true;
            secondAnswer.text = dialogue.nodes[currentNode].answers[1].text;    //показываем 
        }
        else
        {
            secondButton.enabled = false;                                       //иначе скрываем
            secondAnswer.text = "";
        }

        if (dialogue.nodes[currentNode].answers.Length == 3)
        {
            thirdButton.enabled = true;
            thirdAnswer.text = dialogue.nodes[currentNode].answers[2].text;
        }
        else
        {
            thirdButton.enabled = false;
            thirdAnswer.text = "";
        }
    }

    private void AnswerClicked(int numberOfButton)
    {
        if (!DialogueManager.instance.dialogueClosed)
        {
            if (dialogue.nodes[currentNode].answers[numberOfButton].quests != null)
            {
                WorkWithQuests(numberOfButton);
            }

            if (dialogue.nodes[currentNode].answers[numberOfButton].after == "true")
            {
                DialogueManager.instance.EndDialogue();
                StartCoroutine(waitFor(2f));
            }
            else if (dialogue.nodes[currentNode].answers[numberOfButton].end == "true")
            {
                dialogueEnded = true;
                DialogueManager.instance.EndDialogue();
            }
            else
            {
                currentNode = dialogue.nodes[currentNode].answers[numberOfButton].nextNode;
                if (questFalse)
                    currentNode = currentNode - 1;
                questFalse = false;
                WriteText();
            }
        }
    }
    #endregion
   
    private void WorkWithQuests(int numberOfButton)
    {

        for (int questNumber = 0; questNumber < dialogue.nodes[currentNode].answers[numberOfButton].quests.Length; questNumber++)
        {
            // Создание квеста
            if (dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].textQuest != null)
            {
                if (!questsManager.FindTaskFromBoard(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].textQuest))
                    questsManager.AddTask(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].textQuest);
            }
            // Если цель квеста поговорить с НПС и на этом квест закончен
            if (dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questEndAndDelete != null)
            {                
                if (questsManager.FindTaskFromBoard(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questEndAndDelete))
                    questsManager.TaskEndAndDelete(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questEndAndDelete);
                
            }
            // Если необходимо сдать выполненный квест НПС со статусом "Выполнено"
            if (dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questDone != null)
            {
                if (questsManager.FindTaskFromBoard(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questDone))
                    if (questsManager.FindStatusTaskFromBoard(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questDone, "Выполнен"))
                    {
                        questsManager.TaskEndAndDelete(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questDone);                        
                    }
                    else
                    {
                        questFalse = true;
                    }

            }
            // Если нужно поменять статус квеста после диалога
            if (dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].textNewStatus != null)
            {
                if (questsManager.FindTaskFromBoard(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questChangeStatus))
                    questsManager.UpdateTaskStatus(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].questChangeStatus,
                        dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].textNewStatus);
            }
            // Создание квеста
            if (dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].gameObjectSetActiv == "true")
            {
                objectSetActiv = true;
            }
            // Если нужно поменять статус квеста после диалога
            if (dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].items != null)
            {
                WorkWithItems(numberOfButton, questNumber);
            }
        }
    }
    private void WorkWithItems(int numberOfButton, int questNumber)
    {
        Dictionary<string, int> items = new Dictionary<string, int>();
        for (int itemNumber = 0; itemNumber < dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].items.Length; itemNumber++)
        {
            if (dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].items[itemNumber].gameObjectTake != null &
                dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].items[itemNumber].gameObjectTakeCount > 0)
            {
                items.Add(dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].items[itemNumber].gameObjectTake,
               dialogue.nodes[currentNode].answers[numberOfButton].quests[questNumber].items[itemNumber].gameObjectTakeCount);
            }            
        }
        if (itemsManager.FindItems(items))
        {
            itemsManager.DeleteItems(items);
            currentNode = currentNode + 2;
            WriteText();

        }
        else
        {
            currentNode = currentNode + 1;
            WriteText();
        }
    }

    private IEnumerator waitFor(float time)
    {
        yield return new WaitForSeconds(time);     
    }  

    public void CloseDialogue()
    {

        if (dialogue != null)
        {            
            dialogue = null;
        }
        deleteDialogue();
        currentNode = 0; // Начинаем с первого узла
        firstNodeShown = false; // Сбрасываем флаг показа первого узла
    }

    private void deleteDialogue()
    {
        secondButton.enabled = false;
        thirdButton.enabled = false;
        nodeText.text = ""; 
        firstAnswer.text = "";
        secondAnswer.text = "";
        thirdAnswer.text = "";
    }    

}