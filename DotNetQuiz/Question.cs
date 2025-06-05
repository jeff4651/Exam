public class Question
{
    public int Id { get; set; }
    public int Chapter { get; set; }
    public string Text { get; set; } = "";
    public List<string> Answers { get; set; } = new();
    public int CorrectIndex { get; set; }
}
