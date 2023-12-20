namespace FredBot.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class InstallerAttribute : Attribute
{
    public InstallerAttribute(bool enabled, int priority = 0)
    {
        Enabled = enabled;
        Priority = priority;
    }

    public bool Enabled { get; }
    public int Priority { get; }
}