using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class RandomScript : NetworkBehaviour
{
    private CharacterControls1 pc;
    //private bool pcAssigned;

    [SerializeField] TextMeshProUGUI ipAddressText;
    [SerializeField] TMP_InputField ip;

    [SerializeField] string ipAddress;
    [SerializeField] UnityTransport transport;



    public string playerName;

    void Start()
    {
        ipAddress = "0.0.0.0";
        SetIpAddress(); 
        //pcAssigned = false;
        InvokeRepeating("assignPlayerController", 0.1f, 0.1f);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        var status = NetworkManager.SceneManager.LoadScene("CharacterSelector", LoadSceneMode.Single);
        SetIpAddress();
    }

    public void StartClient()
    {
        ipAddress = ip.text;
        GetLocalIPAddress();
        Debug.Log(ipAddress);
        NetworkManager.Singleton.StartClient();
    }

    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressText.text = ip.ToString();
                ipAddress = ip.ToString();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    public void SetIpAddress()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
    }

    private void assignPlayerController()
    {
        if (pc == null)
        {
            pc = FindObjectOfType<CharacterControls1>();
        }
        else if (pc == FindObjectOfType<CharacterControls1>())
        {
            //pcAssigned = true;
            CancelInvoke();
        }
    }
}
