using OlibKey.Structures;
using OlibKey.Views.Controls;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace OlibKey.ViewModels.Controls
{
	public class BorderCustomElementViewModel : ReactiveObject
	{
		private ObservableCollection<CustomElementListItem> customElements;
		public ObservableCollection<CustomElementListItem> CustomElements
		{
			get => customElements;
			set => this.RaiseAndSetIfChanged(ref customElements, value);
		}

		public ReactiveCommand<Unit, Unit> AddCustomElementCommand { get; }

		public int Type { get; set; }

		public BorderCustomElementViewModel(bool isActive)
		{
			AddCustomElementCommand = ReactiveCommand.Create(AddCustomElement);
		}

		private void AddCustomElement()
		{
			CustomElements.Add(new CustomElementListItem 
			{ 
				ID = Guid.NewGuid().ToString("N"),
				DeleteCustomElement = DeleteCustomElement
			});
		}

		private void DeleteCustomElement(string id)
		{
			foreach (var item in CustomElements)
			{
				if (item.ID == id)
				{
					CustomElements.Remove(item);
					break;
				}
			}
		}
	}
}
