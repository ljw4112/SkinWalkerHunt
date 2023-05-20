namespace Options
{
    public enum UIOptionsKey
    {
        None,
        Data,
        Index,
    }

    public enum UIStatus
    {
        CompleteShow,
        BackEntry,
    }

    public static class ResourcePath
    {
        public static string UIPrefab = "Prefabs/UI";
        public static string Manager = "Prefabs/Manager";
    }

    public enum SceneType
    {
        Intro,
        Main,
    }

    /// <summary>
    /// ĳ���� ����
    /// </summary>
    public enum PlayerStatus
    {
        None = -1,
        Stand,
        Move,
        Run,
        Shoot,
    }
}