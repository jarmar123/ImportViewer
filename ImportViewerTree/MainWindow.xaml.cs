using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Crestron.myCrestron.Configuration.Model;
using Newtonsoft.Json;
using System.Linq;

namespace ImportViewerTree
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private TreeViewItem AddTreeView(string header, object tag)
		{
			TreeViewItem tvi = new TreeViewItem();
			tvi.Header = header;
			tvi.Tag = tag;

			tvi.Expanded += ListItem_Expanded;
			TreeView1.Items.Add(tvi);

			return tvi;
		}

		private void ListItem_Expanded(object sender, RoutedEventArgs e)
		{
			var tvi = (TreeViewItem) sender;

			if (tvi.Tag is List)
			{
				List<Object> list = (List<Object>)tvi.Tag;
			}
		}

		private SystemConfiguration config;
		public MainWindow()
		{
			string jsonFilePath = @"C:\_delete\smalltest.json";
			var jsonText = File.ReadAllText(jsonFilePath);
			config = JsonConvert.DeserializeObject<SystemConfiguration>(jsonText);
			InitializeComponent();

			//TreeViewItem roomListItem = new TreeViewItem();
			//roomListItem.Header = "Rooms";
			//roomListItem.Tag = config.RoomList;
			//TreeView1.Items.Add(roomListItem);

			var roomTvi = AddTreeView("Rooms", config.RoomList);
			foreach (var room in config.RoomList)
			{
				TreeViewItem childItem = new TreeViewItem();
				childItem.Header = room.RoomName;
				childItem.Tag = room;
				childItem.MouseUp += DisplayRoom;
				roomTvi.Items.Add(childItem);
			}

			var deviceTvi = AddTreeView("Devices", config.DeviceList);

			foreach (var device in config.DeviceList)
			{
				TreeViewItem childItem = new TreeViewItem();
				childItem.Header = device.DeviceName;
				childItem.Tag = device;
				childItem.MouseUp += DisplayDevice;
				deviceTvi.Items.Add(childItem);
			}

			#region Test stuff

			//TreeViewItem ParentItem = new TreeViewItem();
			//ParentItem.Header = "Parent";
			//TreeView1.Items.Add(ParentItem);

			////  
			//TreeViewItem Child1Item = new TreeViewItem();
			//Child1Item.Header = "Child One";
			//ParentItem.Items.Add(Child1Item);
			//Child1Item.MouseUp += Child1Item_MouseUp;
			//Child1Item.Expanded += OnExpanded;
			////  
			//TreeViewItem Child2Item = new TreeViewItem();
			//Child2Item.Header = "Child Two";
			//ParentItem.Items.Add(Child2Item);
			//TreeViewItem SubChild1Item = new TreeViewItem();
			//SubChild1Item.Header = "Sub Child One";
			//Child2Item.Items.Add(SubChild1Item);
			//TreeViewItem SubChild2Item = new TreeViewItem();
			//SubChild2Item.Header = "Sub Child Two";
			//Child2Item.Items.Add(SubChild2Item);
			//TreeViewItem SubChild3Item = new TreeViewItem();
			//SubChild3Item.Header = "Sub Child Three";
			//Child2Item.Items.Add(SubChild3Item);
			//Child2Item.Expanded += OnExpanded;
			////  
			//TreeViewItem Child3Item = new TreeViewItem();
			//Child3Item.Header = "Child Three";
			//ParentItem.Items.Add(Child3Item);
			//TreeViewItem Child4Item = new TreeViewItem();
			//Child4Item.Header = "Child Four";
			//ParentItem.Items.Add(Child4Item);

			//for (int j = 0; j < 50; j++)
			//{
			//	ParentItem.Items.Add(new TreeViewItem(){Header = $"autoChild {j}"});
			//}

			#endregion
		}

		private void DisplayRoom(object sender, MouseButtonEventArgs e)
		{
			var tvi = (TreeViewItem) sender;
			var room = (Room)tvi.Tag;
			var text = GetRoomString(room);
				//$"Id: {room.Id}{Environment.NewLine}RoomName: {room.RoomName}{Environment.NewLine}Hidden: {room.Hidden}{Environment.NewLine}PIK: {room.PrimaryImageKey}";
			Display1.Text = text;
			CommandsPanel.Children.Clear();
		}

		private string GetRoomString(Room room)
		{
			return $"Id: {room.Id}{Environment.NewLine}RoomName: {room.RoomName}{Environment.NewLine}Hidden: {room.Hidden}{Environment.NewLine}PIK: {room.PrimaryImageKey}";
		}

		private void DisplayDevice(object sender, MouseButtonEventArgs e)
		{
			var tvi = (TreeViewItem)sender;
			var device = (Device)tvi.Tag;
			var text = GetDeviceString(device);
				
			Display1.Text = text;

			Button GetRoomButton = new Button(){Content = "View RoomInfo"};
			GetRoomButton.Click += GetRoomButton_Click; ;
			CommandsPanel.Children.Clear();
			CommandsPanel.Children.Add(GetRoomButton);
		}

		private void GetRoomButton_Click(object sender, RoutedEventArgs e)
		{
			var tvi = (TreeViewItem)TreeView1.SelectedItem;
			var device = (Device)tvi.Tag;
			var room = config.RoomList.FirstOrDefault(r => r.Id == device.RoomId);
			Display1.Text = GetRoomString(room);
		}


		private string GetDeviceString(Device d)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"ControlPathParentId: {d.ControlPathParentId}");
			sb.AppendLine($"ControlPathSlotId: {d.ControlPathSlotId}");
			sb.AppendLine($"DeviceClass: {d.DeviceClass}");
			sb.AppendLine($"Id: {d.Id}");
			sb.AppendLine($"DeviceName: {d.DeviceName}");
			sb.AppendLine($"DeviceCategory: {d.DeviceCategory}");
			sb.AppendLine($"Type: {d.Type}");
			sb.AppendLine($"Model: {d.Model}");
			sb.AppendLine($"ControlPathType: {d.ControlPathType}");
			sb.AppendLine($"RoomId: {d.RoomId}");

			return sb.ToString();
		}

		private void OnExpanded(object sender, RoutedEventArgs e)
		{
			var tvi = (TreeViewItem)sender;
		}

		int i = 5;
		private void Child1Item_MouseUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem Child3Item = new TreeViewItem();
			var name = $"Child {i++}";
			Child3Item.Header = name;
			TreeView1.Items.Add(Child3Item);
			var text = name;
			text += $"{Environment.NewLine} extra info";
			text += $"{Environment.NewLine} even more info";
			Display1.Text = text;

		}
	}
}







