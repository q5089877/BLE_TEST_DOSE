using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BLE_TEST_DOSE
{

    public partial class Form1 : Form
    {
        Windows.Devices.Bluetooth.Advertisement.BluetoothLEAdvertisementWatcher advertisementWatcher;



        private static List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
        private static BleCore bleCore = null;
      
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BleCore bleCore = new BleCore();
            bleCore.DeviceWatcherChanged += DeviceWatcherChanged;
            bleCore.CharacteristicAdded += CharacteristicAdded;
            bleCore.CharacteristicFinish += CharacteristicFinish;
            bleCore.Recdate += Recdata;
            bleCore.StartBleDeviceWatcher();
            //   Console.ReadKey(true);
            //   bleCore.Dispose();
        }    

     



     

        private void AdvertisementWatcher_Received(Windows.Devices.Bluetooth.Advertisement.BluetoothLEAdvertisementWatcher sender, Windows.Devices.Bluetooth.Advertisement.BluetoothLEAdvertisementReceivedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Address: {0:X}", args.BluetoothAddress);
            System.Diagnostics.Debug.WriteLine("ManufacturerData: {0:X}", args.Advertisement.ManufacturerData);
            System.Diagnostics.Debug.WriteLine("ServiceUuids: {0:X}", args.Advertisement.ServiceUuids);
            System.Diagnostics.Debug.WriteLine("***********DataSections: {0:X}", args.Advertisement.DataSections);
            System.Diagnostics.Debug.WriteLine("AdvertisementType--------: {0:X}", args.AdvertisementType.ToString());
        }




        private static void DeviceWatcherChanged(BluetoothLEDevice currentDevice)
        {
            byte[] _Bytes1 = BitConverter.GetBytes(currentDevice.BluetoothAddress);
            Array.Reverse(_Bytes1);
            string address = BitConverter.ToString(_Bytes1, 2, 6).Replace('-', ':').ToLower();
            Console.WriteLine("Found：<" + currentDevice.Name + ">  address:<" + address + ">");                     
            //指定一个对象，使用下面方法去连接设备
            ConnectDevice(currentDevice);
        }

        private static void ConnectDevice(BluetoothLEDevice Device)
        {
            characteristics.Clear();
            bleCore.StopBleDeviceWatcher();
            bleCore.StartMatching(Device);
            bleCore.FindService();
        }


        private static void CharacteristicAdded(GattCharacteristic gatt)
        {
            Console.WriteLine(
                "handle:[0x{0}]  char properties:[{1}]  UUID:[{2}]",
                gatt.AttributeHandle.ToString("X4"),
                gatt.CharacteristicProperties.ToString(),
                gatt.Uuid);
            characteristics.Add(gatt);
        }

        private static void CharacteristicFinish(int size)
        {
            if (size <= 0)
            {
                Console.WriteLine("设备未连上");
                return;
            }
        }

        private static void Recdata(GattCharacteristic sender, byte[] data)
        {
            string str = BitConverter.ToString(data);
            Console.WriteLine(sender.Uuid + "             " + str);
        }

    }
}
