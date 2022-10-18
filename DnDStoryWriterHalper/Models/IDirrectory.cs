using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DnDStoryWriterHalper.Models;
interface IDirrectory : IDirrectoryComponent
{
	public new string? Name { get; }
	public ObservableCollection<object> Content { get; }
}