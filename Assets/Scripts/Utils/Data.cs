/// <summary>
/// Data 전달용 클래스 어미
/// </summary>
public abstract class Data 
{
    public abstract void Initialize();
}

public class Data_UIMenu : Data
{
    public int value;

    public override void Initialize()
    {
        value = 0;
    }
}