namespace DnDStoryWriterHalper.Models;
public interface IPage : IDirrectoryComponent
{
    public new string? Name { get; }
    public string Guid { get; }
}
