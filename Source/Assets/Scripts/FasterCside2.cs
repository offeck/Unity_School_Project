using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FasterCside2 : MonoBehaviour
{
    private string errormsg = "error";
    private int bytesRec;
    private int bytesSent;
    public GameObject bs;
    public GameObject rs;
    private Socket sender;
    private byte[] bytes = new byte[1024];
    public GameObject borb;
    public GameObject morb;
    public float spawnrange;
    private byte[] msgbytes;
    private string msg;
    private int por = 8000;
    private Queue<String> msgqueue;
    public GameObject Erightpos;
    public GameObject Eleftpos;
    public GameObject Eprojectile;
    public GameObject Eleftblade;
    public GameObject Erightblade;
    public GameObject meteor;
    //private Life bslife;
    //private Life rslife;

    public static Vector3 StringToVector3(string sVec)
    {
        // Remove the parentheses
        if (sVec.StartsWith("(") && sVec.EndsWith(")"))
        {
            sVec = sVec.Substring(1, sVec.Length - 2);
        }

        // split the items
        string[] sArray = sVec.Split(',');
        float yt = float.Parse(sArray[1]);
        yt *= -1;
        sArray[1] = yt.ToString("0.0");

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    // Start is called before the first frame update
    void Awake()
    {
        IPAddress clientip = IPAddress.Loopback;
        string serip = "127.0.0.1"; /// server ip address /// when online -> "85.65.244.189" when offline-> "127.0.0.1"
        IPAddress serverip = IPAddress.Parse(serip);
        Debug.Log(serverip);
        Debug.Log(IPAddress.IsLoopback(clientip));
        IPEndPoint remoteEP = new IPEndPoint(serverip, por);

        sender = new Socket(clientip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        sender.Connect(remoteEP);

        Debug.Log("Socket connected to " + sender.RemoteEndPoint.ToString());

        bytesRec = sender.Receive(bytes);

        ///bytesRec = sender.Receive(bytes); // recv
        string t = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        ///Debug.Log(Encoding.ASCII.GetString(bytes, 0, bytesRec));
        ///string temp = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        ///msgbytes = Encoding.ASCII.GetBytes(temp);
        bytesSent = sender.Send(Encoding.ASCII.GetBytes(t));
        ///Debug.Log(Encoding.ASCII.GetString(bytes, 0, bytesRec));
        ///yield return new WaitForSeconds(0.01f);
        wait(0.01f);
    }

    private void Start()
    {
         msgqueue = new Queue<string>();
        //bslife = bs.GetComponent<Life>();
        //rslife = rs.GetComponent<Life>();
    }

    public void putinqueue(string str)
    {
        //Debug.Log(str);
        msgqueue.Enqueue(str);
    }

    // Update is called once per frame

    public bool checkwincondition()
    {
        if (!bs.GetComponent<Life>().isalive())
        {
            gameresult("lost");
            return true;
        }
        if (!rs.GetComponent<Life>().isalive())
        {
            gameresult("won");
            return true;
        }
        return false;
    }

    public void gameresult(string result)
    {
        Debug.Log(result);
        if (result == "lost")
        {
            try
            {
                bytesSent = sender.Send(Encoding.ASCII.GetBytes(result));
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            try
            {
                bytesSent = sender.Send(Encoding.ASCII.GetBytes(result));
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            Debug.Log(SceneManager.GetActiveScene().buildIndex + 2);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        //if (result == "won")
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); will add later
        //else
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
        {
            bytesSent = sender.Send(Encoding.ASCII.GetBytes(errormsg));

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            Application.Quit();
        }
        else
        {
            try
            {
                msg = bs.transform.position.ToString();
                //Debug.Log(msg);
                while (msgqueue.Count != 0)
                {
                    msg += msgqueue.Dequeue();
                }
                msgbytes = Encoding.ASCII.GetBytes(msg);
                bytesSent = sender.Send(msgbytes);
                ///bytesRec = sender.Receive(bytes);
                ///if (Encoding.ASCII.GetString(bytes, 0, bytesRec) == msg)
                ///{
                Time.timeScale = 0;
                bytesRec = sender.Receive(bytes);
                Time.timeScale = 1;
                string strrec = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (strrec.Contains("lost"))
                {
                    Debug.Log("won really");
                    gameresult("won");
                }
                if (strrec.Contains("won"))
                {
                    Debug.Log("lost really");
                    gameresult("lost");
                }
                if (strrec.Contains("_"))
                {
                    string[] s = strrec.Split('_');
                    foreach (string pos in s)
                    {
                        //Debug.Log(pos);
                        ///Console.WriteLine(pos);
                        switch (pos[0])
                        {
                            case 'e':
                                if (pos[1] == 'r')
                                {
                                    string postemp = pos.Substring(3);
                                    int speed = (int)char.GetNumericValue(pos[2]);
                                    //Debug.Log("speed " + speed);
                                    Vector3 meteorpos = StringToVector3(postemp);
                                    GameObject meteortemp = Instantiate(meteor, meteorpos, Quaternion.identity);
                                    meteortemp.GetComponent<MeteorScript>().speed = speed;
                                    meteortemp.GetComponent<MeteorScript>().startmoving();
                                    //Instantiate(meteor, meteorpos, Quaternion.identity);
                                    continue;
                                }
                                else
                                {
                                    string postemp = pos.Substring(3);
                                    int speed = (int)char.GetNumericValue(pos[2]);
                                    //Debug.Log("speed " + speed);
                                    Vector3 meteorpos = StringToVector3(postemp);
                                    GameObject meteortemp = Instantiate(meteor, meteorpos, Quaternion.Euler(0, 180, 0));
                                    meteortemp.GetComponent<MeteorScript>().speed = speed;
                                    meteortemp.GetComponent<MeteorScript>().startmoving();
                                    //Instantiate(meteor, meteorpos, Quaternion.Euler(0, 180, 0));
                                    continue;
                                }
                            case 'p':
                                if (pos[1] == 'r')
                                {
                                    Soundmanagerscript.playsound("shot");// sound
                                    Instantiate(Eprojectile, Erightpos.transform.position, Quaternion.identity);
                                    continue;
                                }
                                else
                                {
                                    Soundmanagerscript.playsound("shot");// sound
                                    Instantiate(Eprojectile, Eleftpos.transform.position, Quaternion.identity);
                                    continue;
                                }
                            case 'o':
                                if (pos[1] == 'm')
                                {
                                    string postemp = pos.Substring(2);
                                    Vector3 orbpos = StringToVector3(postemp);
                                    Instantiate(morb, orbpos, Quaternion.identity);
                                    continue;
                                }
                                else
                                {
                                    string postemp = pos.Substring(2);
                                    Vector3 orbpos = StringToVector3(postemp);
                                    Instantiate(borb, orbpos, Quaternion.identity);
                                    continue;
                                }
                            case 'm':
                                if (pos[1] == 'l')
                                {
                                    if (pos[2] == 'a' && !Eleftblade.activeSelf)
                                    {
                                        Soundmanagerscript.playsound("on");// sound
                                        Eleftblade.SetActive(true);
                                        continue;
                                    }
                                    else if (pos[2] == 'd' && Eleftblade.activeSelf)
                                    {
                                        Soundmanagerscript.playsound("off");// sound
                                        Eleftblade.SetActive(false);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (pos[2] == 'a' && !Erightblade.activeSelf)
                                    {
                                        Soundmanagerscript.playsound("on");// sound
                                        Erightblade.SetActive(true);
                                        continue;
                                    }
                                    else if (pos[2] == 'd' && Erightblade.activeSelf)
                                    {
                                        Soundmanagerscript.playsound("off");// sound
                                        Erightblade.SetActive(false);
                                        continue;
                                    }
                                }
                                break;
                            default:
                                if (pos == "")
                                {
                                    if (checkwincondition()) { }
                                    else
                                    {
                                        //throw new SocketException(1);
                                    }
                                }
                                else
                                {
                                    Vector3 rspos = StringToVector3(pos);
                                    rs.transform.position = rspos;
                                }
                                break;
                        }
                    }
                }
                else
                {
                        if (strrec == "")
                        {
                            if (checkwincondition()) { }
                            else
                            {
                                //throw new SocketException(1);
                            }
                        }
                        else
                        {
                            //Debug.Log(strrec);
                            Vector3 rspos = StringToVector3(strrec);
                            rs.transform.position = rspos;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Debug.Log("errorshutdown");
                try
                {
                    bytesSent = sender.Send(Encoding.ASCII.GetBytes(errormsg));
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception x)
                {
                    Debug.Log(x);
                }
                Debug.Log("QUIT!!");
                Application.Quit();
            }
        }
        ///bytesSent = sender.Send(Encoding.ASCII.GetBytes(strrec));
        ///Vector3 rspos = StringToVector3(strrec);
        ///rs.transform.position = rspos;
        ///}

        ///else
        ///{
        ///bytesSent = sender.Send(Encoding.ASCII.GetBytes(errormsg));

        ///sender.Shutdown(SocketShutdown.Both);
        ///sender.Close();
        ///}
    }
}