using System.Runtime.CompilerServices;

namespace FredBot.Extensions;

public static class TasksExtensions
{
    public static TaskAwaiter GetAwaiter(this TaskList wrapper)
    => Task.WhenAll(wrapper.Tasks).GetAwaiter();
    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>) tasks)
        => Task.WhenAll(tasks.Item1, tasks.Item2).GetAwaiter();
}
public class TaskList
{

    public List<Task> Tasks { get; private set; } = new();

    public static TaskList operator | (TaskList list, Task t1)
    {
        list.Tasks.Add(t1);
        return list;
    }

    public static TaskList operator |(TaskList list, IEnumerable<Task> t1)
    {
        list.Tasks.AddRange(t1);
        return list;
    }

    public static explicit operator TaskList(List<Task> tasks)
    {
        return new TaskList() 
        {
            Tasks = new List<Task>(tasks)
        };
    }
}
