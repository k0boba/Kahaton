using UnityEngine;
using System.Xml.Serialization;     //запись и чтение xml файла
using System.IO;

[XmlRoot("dialogue")]
public class Dialogue
{

    //[XmlElement("text")]
    public string name;

    [XmlElement("node")]
    public Node[] nodes;

    public static Dialogue Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue));
        StringReader reader = new StringReader(_xml.text);
        Dialogue dial = serializer.Deserialize(reader) as Dialogue;        
        return dial;
    }

    public void Remove()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < nodes[i].answers.Length; j++)
            {
                nodes[i].answers[j].text = "";
            }
            nodes[i].Npctext = "";
        }
    }
}

[System.Serializable]
public class Node
{
    [XmlElement("npctext")]
    public string Npctext;

    [XmlArray("answers")]
    [XmlArrayItem("answer")]
    public Answer[] answers;   

}

public class Answer
{
    [XmlAttribute("tonode")]
    public int nextNode;
    [XmlElement("text")]
    public string text;
    [XmlElement("dialend")] // Конец диалога и переход на следующий
    public string end;

    [XmlArray("quests")]
    [XmlArrayItem("quest")]
    public Quests[] quests;

    [XmlElement("after")] // Конец диалога без перехода на следующий, т.е. данный диалог будет проигран еще раз
    public string after;
}

public class Quests
{
    [XmlElement("textQuest")] // Текст для создания квеста
    public string textQuest;
    [XmlElement("questDone")] // Текст квеста для проверки, что он выполнен (рядом статус "выполнен")
    public string questDone;
    [XmlElement("questEndAndDelete")] // Текст квеста, который сдается во время диалога
    public string questEndAndDelete;
    [XmlElement("questChangeStatus")] // Текст квеста, для которого меняется статус
    public string questChangeStatus;
    [XmlElement("textNewStatus")] // Для смены статуса
    public string textNewStatus;
    [XmlElement("gameObjectSetActiv")] // Для включения или выключения объекта
    public string gameObjectSetActiv;

    [XmlArray("items")]
    [XmlArrayItem("item")]
    public Items[] items;

    [XmlElement("motion")] // Опыт
    public string motion;
}

public class Items
{     
    [XmlElement("gameObjectTake")] // Какие вещи должны быть у игрока для сдачи квеста
    public string gameObjectTake;
    [XmlElement("gameObjectTakeCount")] // Количество вещей
    public int gameObjectTakeCount;
}