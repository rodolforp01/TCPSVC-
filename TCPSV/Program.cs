using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPSV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green; //console words color
            Console.Title = "Indexer's SV"; //console title
            Console.SetWindowSize(40, 10); //changes the size of the console
            Console.SetBufferSize(40, 10); //removes scrollbars vertical and horizontal
            GUI.FixConsole(); //made class to avoid console resize 
            
            ExecuteServer();
        }

		//Decrypt data method
		public static string DeCryptDat(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i< data.Length; i+= 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }

		//web request method
		public static bool Web_RQ()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebClient webClient = new WebClient();
            string[] array = new string[]
            {
                "<",
                ">",
                "head",
                "body",
                "!",
                "/",
                "style",
                "background-color:",
                "html",
                "DOCTYPE",
                "p1",
                "#0AE8F3",
                ";",
                "=",
                "h1"
            };
            //download strings
            string source = webClient.DownloadString(DeCryptDat("011010000111010001110100011100000111001100111010001011110010111101110010011011110110010001101111011011000110011001101111011100100111000000110000001100010010111001100111011010010111010001101000011101010110001000101110011010010110111100101111"));
            string noSpace = new string(source.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
            
            string text = noSpace;

            for (int i = 0; i < array.Length; i++)
            {
                text = text.Replace(array[i], "");
            }

            //00110000011110000110001101101000011010010110111001100001
            //0011000001111000001100110011010000110101
            //current 0xperu
            int num = text.IndexOf(DeCryptDat("0011000001111000011011010110000101100111011011110111000001101111"));
            //return true if num doesnt equal -1
            return num != -1;
        }



        public static void ExecuteServer()
        {

            if (Web_RQ())
            {
                Console.WriteLine("           ✓ Verified ✓         \n\n");
                //Estblish the local endpoint
                //for the socket. DnsGetHostName
                //returns the name of the host
                // running the application

                IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 1401);

                //creating TCP/IP SOCKET using 
                //Socket class Constructor
                Socket listener = new Socket(ipAddr.AddressFamily,
                                    SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // using Bind() method we asscociate a 
                    // network address to the server Socket
                    // All client that will connect to this
                    // Server socket must know this neetwork
                    // Address

                    listener.Bind(localEndPoint);


                    //using listen() method we create
                    //the client list that will want
                    // to connect to server
                    listener.Listen(10);

                    while (true)
                    {
                        Console.WriteLine("Waiting for connection.....");

                        //Suspend while waiting for 
                        //incoming connection using
                        //accept() method the server
                        //will acccept connection of client

                        Socket clientSocket = listener.Accept();

                        //data buffer
                        byte[] bytes = new Byte[1024];
                        string data = null;

                        while (true)
                        {
                            int numByte = clientSocket.Receive(bytes);

                            data += Encoding.ASCII.GetString(bytes, 0, numByte);

                            if (data != "")
                                break;
                        }

                        Console.WriteLine("Text received -> {0}", data);


                        //message in bytes probably not a good way to represent them havent found a good method yet
                        byte[] message = new byte[]
                        {
                            0x01, 0x53, 0x75, 0x63, 0x63, 0x65, 0x73, 0x73, 0x7C, 0x5C, 0x00, 0x43, 0x27, 0x04, 0x00, 0x00,
                            0x00, 0x19, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x2A, 0x00, 0x00, 0x00, 0x3F, 0x3F, 0x3F,
                            0x22, 0x3F, 0x3F, 0x3F, 0x22, 0x40, 0x3F, 0x3F, 0x22, 0x3F, 0x3F, 0x51, 0x14, 0x04, 0x00, 0x00, 0x00,
                            0x3F, 0x3F, 0x51, 0x14, 0x04, 0x00, 0x00, 0x00, 0x0C, 0x3F, 0x3F, 0x00, 0x3F, 0x3F, 0x6E, 0x00, 0x3F,
                            0x3F, 0x6E, 0x00, 0x00, 0x40, 0x07, 0x44, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x3F,
                            0x3F, 0x51, 0x14, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0x43, 0x3F,
                            0x55, 0x3F, 0x13, 0x7C, 0x31, 0x7C, 0x46, 0x61, 0x6C, 0x73, 0x65, 0x7C, 0x7C, 0x54, 0x72, 0x75, 0x65,
                            0x7C, 0x54, 0x72, 0x75, 0x65,
                            0x7C,
                        /*nickname-Area*/ 0x32, 0x39, 0x35, 0x31, 0x35, 0x36, 0x35, 0x34, 0x33, 0x37, 0x35, 0x33, 0x36, 0x31, 0x33, 0x43, 0x33, 0x42, 0x35, 0x43, 0x32, 0x46, 0x34, 0x41, 0x33, 0x42, 0x33, 0x45, 0x35, 0x39, 0x36, 0x44, 0x33, 0x34, 0x32, 0x42, 0x34, 0x46, 0x32, 0x38, 0x35, 0x41, 0x32, 0x46, 0x33, 0x36, 0x36, 0x39, 0x38, 0x45, 0x32, 0x34, 0x32, 0x35, 0x33, 0x37, 0x36, 0x38, 0x33, 0x31, 0x33, 0x34, 0x38, 0x42, 0x32, 0x34, 0x38, 0x36, 0x32, 0x35, 0x32, 0x37,
                            0x7C,
                            0x3E, 0x00, 0x10, 0x27, 0x63, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x37, 0x37, 0x33, 0x39, 0x35, 0x33, 0x36, 0x31, 0x33, 0x33, 0x64, 0x66, 0x33, 0x35,
                            0x33, 0x36, 0x33, 0x61, 0x61, 0x38, 0x33, 0x30, 0x31, 0x30, 0x00, 0x01, 0x7C, 0x54, 0x72, 0x75, 0x65,
                            0x7C, 0x54, 0x72, 0x75, 0x65, 0x7C
                        };



                        //Send a message to Client
                        //using send() method

                        clientSocket.Send(message);
                        Thread.Sleep(150);

                        // Close client socket using the
                        //Close() method. After closing, 
                        //we can use the closed socket
                        //for a new Client Connection
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        Environment.Exit(0);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                Console.WriteLine(" Not verified \n\n Contact Indexer Discord: Indexer #0574 \n\n");
                Console.ReadLine();
            }
        }
    }
}
