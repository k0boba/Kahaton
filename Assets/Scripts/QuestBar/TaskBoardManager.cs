using System.Collections.Generic;
using UnityEngine;

//Клас для работы с квестами
public class TaskBoardManager : MonoBehaviour
{
    public static TaskBoardManager instance;

    public GameObject taskBoardPrefab;
    public TaskBoard taskBoard;
    public List<Task> tasks = new List<Task>();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Переключаем состояние объекта
            taskBoardPrefab.SetActive(!taskBoardPrefab.activeSelf);
        }
    }
    // Метод для добавления нового задания на доску
    public void AddTask(string questText)
    {
        questText = questText.Trim();
        Task newTask = new Task(questText, "Не выполнен");
        tasks.Add(newTask);
        taskBoard.AddTaskToBoard(newTask);

    }

    // Метод для обновления статуса задания
    public void UpdateTaskStatus(string questText, string newStatus)
    {
        questText = questText.Trim();
        newStatus = newStatus.Trim();
        Task taskToUpdate = tasks.Find(task => task.textQuest == questText);
        if (taskToUpdate != null)
        {
            taskToUpdate.statusQuest = newStatus;
            taskBoard.UpdateTaskStatusOnBoard(taskToUpdate);
        }
        else
        {
            Debug.Log("Задание не найдено");
        }
    }
    public void TaskDone(string questText, string questStatus)
    {
        questText = questText.Trim();
        questStatus = questStatus.Trim();
        Task taskToUpdate = tasks.Find(task => task.textQuest == questText);
        if (taskToUpdate != null)
        {
            if (taskToUpdate.statusQuest == questStatus)
            {
                taskBoard.RemoveTaskFromBoard(taskToUpdate);
            }
            else Debug.Log("Статус не закончен");
        }
        else
        {
            Debug.Log("Задание не найдено");
        }
    }

    public void TaskEndAndDelete(string questText)
    {
        questText = questText.Trim();
        Task taskToUpdate = tasks.Find(task => task.textQuest == questText);
        if (taskToUpdate != null)
        {
            taskBoard.RemoveTaskFromBoard(taskToUpdate);            
        }
        else
        {
            Debug.Log("Задание не найдено");
        }
    }

    // Метод для поиска квеста
    public bool FindTaskFromBoard(string questText)
    {
        questText = questText.Trim();
        Task taskToUpdate = tasks.Find(task => task.textQuest == questText);
        if (taskToUpdate != null)
        {
            return true;
        }
        else
        {            
            Debug.Log("Задание не найдено");
            return false;
        }
    }

    public bool FindStatusTaskFromBoard(string questText, string questStatus)
    {
        questText = questText.Trim();
        Task taskToUpdate = tasks.Find(task => task.textQuest == questText);
        if (taskToUpdate != null)
        {
            if (taskToUpdate.statusQuest == questStatus)
                return true;
            else
                return false;
        }
        else
        {
            Debug.Log("Задание не найдено");
            return false;
        }
    }
    
}
