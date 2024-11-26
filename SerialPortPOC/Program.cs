using System.IO.Ports;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
public class PortChat
{
    static SerialPort _serialPort;
    static void Main(string[] args)
    {
        InitiatePrintProcess("message");
        //Connect();
    }

    public static void Connect()
    {
        string comPort = "COM4"; // Update with your COM port
        int baudRate = 9600;
        Parity parity = Parity.None;
        int dataBits = 8;
        StopBits stopBits = StopBits.One;

        using (SerialPort serialPort = new SerialPort(comPort, baudRate, parity, dataBits, stopBits))
        {
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 5000; // Increased timeout

            try
            {
                serialPort.Open();

                if (serialPort.IsOpen)
                {
                    Console.WriteLine($"Serial port {comPort} opened successfully.");
                }

                string message = "Hello, Serial Port!";
                serialPort.WriteLine(message);

                Console.WriteLine($"Message sent to {comPort}: {message}");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Write Timeout Error: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access Error: {ex.Message}. Is the port already in use?");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
            finally
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    Console.WriteLine("Serial port closed.");
                }
            }
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
}

    public static void InitiatePrintProcess(string message)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        FileStream fs = new FileStream(path + "\\result.txt", FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);
        sw.BaseStream.Seek(0, SeekOrigin.End);
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        try
        {
            _serialPort = new SerialPort();
            var ports = SerialPort.GetPortNames();
            _serialPort.PortName = ports[0];
            _serialPort.BaudRate = 38400;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataBits = 8;

            _serialPort.Open(); //opens the port
            _serialPort.ReadTimeout = 2000;
            _serialPort.WriteTimeout = 2000;
            Console.WriteLine("printing");
            _serialPort.NewLine = "\r\n";
            _serialPort.Write(buffer, 0, buffer.Length);
            Console.WriteLine("printing done");
            sw.WriteLine("Printing done");
            sw.WriteLine(message);
            
        }
        catch (Exception ex)
        {
            sw.WriteLine(ex.Message);
            sw.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            _serialPort.Close();
            sw.Flush();
            sw.Close();
        }
    }
}