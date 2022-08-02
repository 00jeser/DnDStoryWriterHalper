namespace DnDStoryWriterHalper.Models;
public interface IPage : IDirrectoryComponent
{
    public string Name { get; }
    public string Guid { get; }
}
