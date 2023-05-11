using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using System.Text;

public class MessageData
{
    public float Distance;
    public string Estado;
}

public class SensorUDP : MonoBehaviour
{

    private Renderer portalRenderer;
    public Material onMaterial;
    public Material offMaterial;
    public GameObject Portal;
    public float lastshit;
    public float currentDistance;


    private Thread receiveThread;
    private bool isInitialized;
    private Queue receiveQueue;


    private UdpClient receiveClient;
    private int receivePort = 6666;

    // Start is called before the first frame update
    void Start()
    {
        
        portalRenderer = Portal.GetComponent<Renderer>();
        portalRenderer.material = offMaterial;
        Transform transform = Portal.GetComponent<Transform>();

        receiveClient = new UdpClient(receivePort);
        receiveQueue = Queue.Synchronized(new Queue());
        receiveThread = new Thread(new ThreadStart(ReceiveDataListener));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        isInitialized = true;

        currentDistance = 5f;
        lastshit = 5f;

    }
    private void ReceiveDataListener()
    {
        IPEndPoint receiveEndPoint = new IPEndPoint(0, 0);

        while (true)
        {
            try
            {
                byte[] data = receiveClient.Receive(ref receiveEndPoint);
                string text = Encoding.UTF8.GetString(data);
                Debug.Log("Data received from " + receiveEndPoint + ": " + text);
                SerializeMessage(text);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }
    private void SerializeMessage(string message)
    {
        try
        {
            receiveQueue.Enqueue(message);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void OnDestroy()
    {
        TryKillThread();
    }

    private void OnApplicationQuit()
    {
        TryKillThread();
    }

    private void TryKillThread()
    {
        if (isInitialized)
        {
            receiveThread.Abort();
            receiveThread = null;
            receiveClient.Close();
            receiveClient = null;
            Debug.Log("Thread killed");
            isInitialized = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (receiveQueue.Count != 0)
        {
            string message = (string)receiveQueue.Dequeue();
            //MessageData msgData = JsonUtility.FromJson<MessageData>(message);
            
            //Debug.Log(msgData);


            string[] strings = message.Split(':');
            strings = strings[1].Split('}');
            
            //Debug.Log(strings[0]);
            //if(strings[0] == "\"ON\"") Debug.Log("Lo lgramos");

             if (strings[0] == "\"ON\"")
             {
                    if (lastshit != currentDistance)
                    {
                        transform.Rotate(0f,180f,0f);
                        portalRenderer.material = onMaterial;
                        lastshit = currentDistance;
                    }    
             }            
            else
            {
                currentDistance = System.Convert.ToSingle(strings[0]);
            }
        }
    }
}
