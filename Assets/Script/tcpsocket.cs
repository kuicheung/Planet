using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class tcpsocket : MonoBehaviour {
	
	/* NetworkStream that will be used */
        private static NetworkStream myStream;
        /* TcpClient that will connect for us */
        private static TcpClient myClient;
        /* Storage space */
        private static byte[] myBuffer;
        /* Application running flag */
        private static bool bActive = true;

        Thread tidListen;

        /* Thread responsible for "remote input" */
        private static void ListenThread()
        {
            Debug.Log("Listening...");
            while (bActive)
            {
                /* Reading data from socket (stores the length of data) */
                int lData = myStream.Read(myBuffer, 0,
                      myClient.ReceiveBufferSize);
                /* String conversion (to be displayed on console) */
                string myString = Encoding.ASCII.GetString(myBuffer);
                /* Trimming data to needed length, 
                   because TcpClient buffer is 8kb long */
                /* and we don't need that load of data 
                   to be displayed at all times */
                /* (this could be done better for sure) */
                myString = myString.Substring(0, lData);
                /* Display message */
                Debug.Log(myString);
            }
        }
	// Use this for initialization
	void Start () {
		
            string strServer = "127.0.0.1";

            string strPort = "3000";

            /* Connecting to server (will crash if address/name is incorrect) */
            myClient = new TcpClient(strServer, int.Parse(strPort));
            Debug.Log("Connected...");
            /* Store the NetworkStream */
            myStream = myClient.GetStream();
            /* Create data buffer */
            myBuffer = new byte[myClient.ReceiveBufferSize];

            /* Vital: Create listening thread and assign it to ListenThread() */
            tidListen = new Thread(new ThreadStart(ListenThread));



            /* Start listening thread */
            tidListen.Start();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnApplicationQuit() {
        // Make sure prefs are saved before quitting.
		myClient.Close();
		tidListen.Abort();
        Debug.Log("quit");
		
    }
	
	void sendMessage(string message){
    	myStream.Write(Encoding.ASCII.GetBytes(
			message.ToCharArray()), 0, message.Length);	
	}
}
