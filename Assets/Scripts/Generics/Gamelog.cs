using System.Collections;
using System.Collections.Generic;
using Crawler.Actors;
using UnityEngine;

public class Gamelog : MonoBehaviour
{
    private List<LogEntry> logs;
    private Queue<string> queue;

    public List<LogEntry> Logs { get => logs; }
    public Queue<string> Queue { get => queue; set => queue = value; }

    #region Delegates
    public delegate void AddEntry();
    public event AddEntry onAddEntry;
    #endregion

    public void Initialize()
    {
        logs = new List<LogEntry>();
        queue = new Queue<string>();
    }

    public void NewEntry(Actor initiator, Actor target, string log)
    {
        LogEntry entry = new LogEntry(initiator, target, log);
        logs.Add(entry);
        queue.Enqueue(entry.log);
        onAddEntry?.Invoke();
    }

    public string Pull()
    {
        if(queue.Count > 0)
            return queue.Dequeue();
        
        return "";
    }
}

public class LogEntry
{
    public Actor initiator;
    public Actor target;
    public string log;

    public LogEntry(Actor initiator, Actor target, string log)
    {
        this.initiator = initiator;
        this.target = target;
        this.log = log;
    }
}
