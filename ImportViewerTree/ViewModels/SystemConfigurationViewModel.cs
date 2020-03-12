using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crestron.myCrestron.Configuration.Model;
using Newtonsoft.Json;

namespace ImportViewerTree.ViewModels
{
	public class SystemConfigurationViewModel: BaseViewModel
	{
		//public ObservableCollection<Room> RoomList { get; set; }
		public ObservableCollection<RoomTreeItemViewModel> RoomList { get; set; }

		public ObservableCollection<DeviceLayoutTreeItemViewModel> DeviceLayoutList { get; set; }

		public ObservableCollection<Device> DeviceList { get; set; }

		private SystemConfiguration _config;
		public SystemConfigurationViewModel()
		{
			string jsonFilePath = @"C:\_delete\smalltest.json";
			var jsonText = File.ReadAllText(jsonFilePath);
			_config = JsonConvert.DeserializeObject<SystemConfiguration>(jsonText);

			RoomList = new ObservableCollection<RoomTreeItemViewModel>(
				_config.RoomList.Select(room => new RoomTreeItemViewModel(room)));

			DeviceLayoutList = new ObservableCollection<DeviceLayoutTreeItemViewModel>(_config.DeviceLayoutList.Select(device => new DeviceLayoutTreeItemViewModel(device)));
		}
	}

	public interface ITreeItem
	{
		string Name { get; }
		bool CanExpand { get; }
		bool IsExpanded { get; set; }
		ObservableCollection<ITreeItem> Children { get; set; }
	}

	public class DeviceLayoutTreeItemViewModel : BaseViewModel, ITreeItem
	{
		public bool CanExpand { get { return true; } }

		public bool _isExpanded;
		public bool IsExpanded
		{
			get
			{
				return Children?.Count(f => f != null) > 0;
			}
			set
			{
				// If the UI tells us to expand...
				if (value == true)
					// Find all children
					Expand();
				// If the UI tells us to close
				else
					this.ClearChildren();
			}
		}

		private DeviceLayout _layout;
		public string Name
		{
			get { return $"Device {_layout.DeviceId}"; }
		}

		public ObservableCollection<ITreeItem> Children { get; set; }

		public DeviceLayoutTreeItemViewModel(DeviceLayout layout)
		{
			_layout = layout;
			_isExpanded = false;
		}

		private void Expand()
		{
			// We cannot expand a file
			//if (_isExpanded)
			//	return;

			// Find all children
			var children = _layout.DevicePartList;
			Children = new ObservableCollection<ITreeItem>(children.Select(part => new DevicePartTreeItemViewModel(part)));
			//_isExpanded = true;
		}

		private void ClearChildren()
		{
			Children.Clear();
		}

	}

	public class DevicePartTreeItemViewModel : BaseViewModel, ITreeItem
	{
		private DevicePart _part;

		public DevicePartTreeItemViewModel(DevicePart part)
		{
			_part = part;
		}

		public string Name => throw new NotImplementedException();

		public bool CanExpand => throw new NotImplementedException();

		public bool IsExpanded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public ObservableCollection<ITreeItem> Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}

	public class RoomTreeItemViewModel : BaseViewModel, ITreeItem
	{
		private Room RoomObject;

		public string Name
		{
			get { return RoomObject.RoomName; }
		}

		public bool CanExpand { get => false; }
		public bool IsExpanded
		{
			get => true;
			set
			{
				
			}
		}
		public ObservableCollection<ITreeItem> Children { get => null;
			set { }
		}

		public RoomTreeItemViewModel(Room room)
		{
			RoomObject = room;
		}
	}

	//public class RoomViewModel : BaseViewModel
	//{
	//	//public string Id { get; set; }
	//	//public string RoomName { get; set; }
	//	//public string PrimaryImageKey { get; set; }
	//	//public bool Hidden { get; set; }

	//	private string id;
	//	private string roomName;
	//	private string primaryImageKey;
	//	private bool hidden;

	//	public string displayName;
	//	public string DisplayName
	//	{
	//		get { return RoomName; }
	//	}

	//	public string Id
	//	{
	//		get { return id; }
	//		set
	//		{
	//			if (value != id)
	//			{
	//				id = value;
	//				NotifyPropertyChanged();
	//			}
	//		}
	//	}

	//	public string RoomName
	//	{
	//		get { return roomName; }
	//		set
	//		{
	//			if (value != roomName)
	//			{
	//				roomName = value;
	//				NotifyPropertyChanged();
	//			}
	//		}
	//	}
	//	public string PrimaryImageKey
	//	{
	//		get { return primaryImageKey; }
	//		set
	//		{
	//			if (value != primaryImageKey)
	//			{
	//				primaryImageKey = value;
	//				NotifyPropertyChanged();
	//			}
	//		}
	//	}
	//	public bool Hidden
	//	{
	//		get { return hidden; }
	//		set
	//		{
	//			if (value != hidden)
	//			{
	//				hidden = value;
	//				NotifyPropertyChanged();
	//			}
	//		}
	//	}
	//}
}
