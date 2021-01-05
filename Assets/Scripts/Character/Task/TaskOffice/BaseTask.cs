
public abstract class BaseTask
{
    public string Name { get; private set; }
    protected BaseTask(string name)
    {
        Name = name;
    }
}


