# Communicate With HID and USB Devices 
Communicate with USB and HID with hidsharp library this is basic app for communicate with HID
you have to implement hidsharp.dll in your app to references 
İt's simple to do that you can do that like this write this command on Package Nuget Console:
 Install-Package HidSharp
 
 In this project, there is an example of reading from buffer in a synchronous and asynchronous way.
 If you want to reach the documentation about the Hidsharp library, you can look at the HidSharp Documentation in TR.pdf file.

Here is the sample code:
 
      using Hidsharp;

      var devices = DeviceList.Local;
      var hidDeviceCollection = devices.GetHidDevices(VendorId, ProductId).ToList();
      var device = hidDeviceCollection.FirstOrDefault();
      var stream = device.Open();


            if (device != null && stream != null)
  
              {
                 
                 var buffer = new byte[9];
                 buffer[0] = 0x00;
                 buffer[1] = 0x21;
                 buffer[2] = 0x19;
                 
                 stream.Write(buffer); // It's easy to write buffer
                 buffer = stream.Read(); // See It's easy to read from buffer
                 return buffer;
               }
