using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using System.Text;

public class MessageData2
{
    public float Distance;
}


public class SensorUDP2 : MonoBehaviour
{

    public GameObject QuadPortal;
    public Material onMaterial;
    public Material offMaterial;
    float lastshit;


    private Thread receiveThread;
    private bool isInitialized;
    private Queue receiveQueue;


    private UdpClient receiveClient;
    private int receivePort = 6666;

    // Start is called before the first frame update
    void Start()
    {

        Renderer renderer = QuadPortal.GetComponent<Renderer>();

        receiveClient = new UdpClient(receivePort);
        receiveQueue = Queue.Synchronized(new Queue());
        receiveThread = new Thread(new ThreadStart(ReceiveDataListener));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        isInitialized = true;
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
            MessageData2 msgData = JsonUtility.FromJson<MessageData2>(message);
            if (lastshit != msgData.Distance)
            {
                if (QuadPortal != null)
                {
                    Renderer renderer = QuadPortal.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        if (lastshit == 0)
                        {
                            renderer.material = offMaterial;
                        }
                        else
                        {
                            renderer.material = onMaterial;
                        }

                        lastshit = msgData.Distance;
                    }
                }
            }
        }
    }
    public void ResetMaterial()
    {
        Renderer renderer = QuadPortal.GetComponent<Renderer>();
        if (renderer != null) {
            renderer.material = onMaterial;
            lastshit = 0;
        }
    }
}
