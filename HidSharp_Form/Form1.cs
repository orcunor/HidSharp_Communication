using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HidSharp;

namespace HidSharp_Form
{
    public partial class Form1 : Form
    {
        HidDevice MyDevice;
        DeviceList List;
        HidStream Stream;
        IAsyncResult Result;
        private byte[] Buffer;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                List = DeviceList.Local;
                MyDevice = List.GetHidDevices(0x1781, 0x7D1).FirstOrDefault();
                if (MyDevice != null && MyDevice.TryOpen(out Stream))
                {
                    Stream.ReadTimeout = 32767;
                    textBox1.Text = MyDevice.DevicePath;
                    btnConnect.Text = "Connected!";
                    Buffer = new byte[9]; // 65
                    Result = Stream.BeginRead(Buffer, 0, 9, new AsyncCallback(BytesReady), null); // 64
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            Stream.EndRead(Result);
            Stream.Dispose();
            btnClose.Enabled = false;
            btnOpen.Enabled = false;

        }

        private void BytesReady(IAsyncResult result)
        {
            try
            {

                label3.Text = ($"Bytes Read = {Stream.EndRead(result)} ");
                //MessageBox.Show($"Bytes Read = {stream.EndRead(result)}");

                if (listBox1.InvokeRequired)
                    listBox1.BeginInvoke((MethodInvoker)delegate ()
                    {
                        for (int i = 0; i < Buffer.Length; i++)
                        {
                            listBox1.Items.Add($"{i}.bytes: {Buffer[i]}");
                        }
                         
                    }); 
                else
                    for (int i = 0; i < Buffer.Length; i++)
                    {
                        listBox1.Items.Add(Buffer[i]);
                    }; // BitConverter.ToString(buffer)

                
                result = Stream.BeginRead(Buffer, 0, 9, new AsyncCallback(BytesReady), null); // 64

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                //byte[] BytesToSend = new byte[9];
                byte[] testBytes = new byte[] { 0x00, 0x15, 0x00, 0x00, 0x01, 0x19, 0x00, 0x00, 0x00 }; // byte 2=1, byte 3 = 3
                //Array.Copy(testBytes, 0, BytesToSend, 0, testBytes.Length);
                //BytesToSend[0] = 0;
                Stream.Write(testBytes); // ,0,65
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.Message;
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                //byte[] BytesToSend = new byte[9];
                byte[] testBytes = new byte[] { 0x00, 0x15, 0x00, 0x00, 0x02, 0x19, 0x00, 0x00, 0x00 }; // byte 2=1, byte 3 = 3

                //Array.Copy(testBytes, 0, BytesToSend, 0, testBytes.Length);
                //BytesToSend[0] = 0;
                Stream.Write(testBytes); // ,0,65
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.Message;
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

       
        private HidDevice[] GetHidDevices()
        {
            var list = DeviceList.Local;
            //list.Changed += (sender, e) => Console.WriteLine("Device list changed.");

            var allHidDeviceList = list.GetHidDevices().ToArray();
            return allHidDeviceList;

        }

        private void btnClearDevices_Click(object sender, EventArgs e)
        {
            try
            {
                //listView1.Items.Remove(listView1.SelectedItems[0]);
                if (listView1.Items == null)
                {
                    btnClearDevices.Enabled = false;
                }
                else
                {
                    listView1.Items.Clear();
                    btnGetAllDevices.Enabled = true;
                    btnGetAllDevices.Text = "Get All Devices!";
                }
            }
            catch (Exception ex )
            {

                label2.Text = ex.Message;
            }
            

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listView1.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                textBox1.Text = listView1.Items[intselectedindex].Text;

            }
        }

        private void btnGetAllDevices_Click(object sender, EventArgs e)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var hidDevices = GetHidDevices();

                long deviceListTotalTime = stopwatch.ElapsedMilliseconds;

                foreach (var device in hidDevices)
                {
                    listView1.Items.Add(device.ToString());

                    stopwatch.Stop();
                    listView1.Sorting = SortOrder.Ascending;
                    label2.Text = ($"Complete device list took {deviceListTotalTime} ms to get {hidDevices.Length} devices").ToString();
                    //listView1.Items.Add(string.Format("Max Lengths: Input {0}, Output {1}, Feature {2}",
                    //    device.GetMaxInputReportLength(),
                    //    device.GetMaxOutputReportLength(),
                    //    device.GetMaxFeatureReportLength()));
                }

                //for (int i = 0; i < x.Length; i++)
                //{

                //    lstView.Items.Add(x[i].ToString());
                //    lstView.Sorting = SortOrder.Ascending;
                //    lblResult.Text = ($"Complete device list took {deviceListTotalTime} ms to get {x.Length} devices").ToString();

                //}
                btnGetAllDevices.Text = "Listed All Devices!";
                btnGetAllDevices.Enabled = false;

                if (listView1.Items == null)
                {
                    btnGetAllDevices.Enabled = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
