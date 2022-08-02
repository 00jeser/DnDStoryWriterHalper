using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DnDStoryWriterHalper.Models;
interface IDirrectory : IDirrectoryComponent
{
	public string Name { get; }
	public ObservableCollection<object> Content { get; }
}