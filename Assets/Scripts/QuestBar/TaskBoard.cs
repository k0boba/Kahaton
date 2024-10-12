using UnityEngine;
using UnityEngine.UI;


//Клас для работы с панелью тасков
public class TaskBoard : MonoBehaviour
{
    public Text taskTextPrefab;
    public Transform taskListPanel;
    
    // Метод для добавления задания на доску
    public void AddTaskToBoard(Task task)
    {
        Text newTaskText = Instantiate(taskTextPrefab, taskListPanel);
        newTaskText.text = task.textQuest + " - " + task.statusQuest;
    }

    // Метод для обновления статуса задания на доске
    public void UpdateTaskStatusOnBoard(Task task)
    {
        foreach (Transform taskTextTransform in taskListPanel)
        {
            Text taskText = taskTextTransform.GetComponent<Text>();
            if (taskText.text.Contains(task.textQuest))
            {
                taskText.text = task.textQuest + " - " + task.statusQuest;
            }
        }
    }

    // Метод для удаления статуса задания на доске
    public void RemoveTaskFromBoard(Task task)
    {
        foreach (Transform taskTextTransform in taskListPanel)
        {
            Text taskText = taskTextTransform.GetComponent<Text>();
            if (taskText.text.Contains(task.textQuest))
            {
                Destroy(taskText.gameObject);
                GameObject go = taskTextTransform.gameObject;
                Destroy(go);
                break;
            }
        }
    }    
}
