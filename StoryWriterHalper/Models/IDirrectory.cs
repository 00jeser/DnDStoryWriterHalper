using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryWriterHalper.Models;
interface IDirrectory : IDirrectoryComponent
{
	public string Name { get; }
	public ObservableCollection<object> Content { get; }
}