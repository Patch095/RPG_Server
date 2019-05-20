using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class GameClient : MonoBehaviour
{
    [SerializeField]
    private string address;

    [SerializeField]
    private int port;

    [System.Serializable]
    struct NetPrefab
    {
        public uint Id;
        public GameObject Prefab;
    }

    private Socket socket;
    private IPEndPoint endPoint;

    [SerializeField]
    private NetPrefab[] netPrefabs;

    private Dictionary<uint, GameObject> netPrefabsCache;
    private Dictionary<uint, GameObject> netGameObject;

    private uint myNetId;
    private GameObject myGameObj;

    void Awake()
    {
        netPrefabsCache = new Dictionary<uint, GameObject>();
        foreach (NetPrefab netPrefab in netPrefabs)
        {
            netPrefabsCache[netPrefab.Id] = netPrefab.Prefab;
        }
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Blocking = false;
        endPoint = new IPEndPoint(IPAddress.Parse(address), port);

        netGameObject = new Dictionary<uint, GameObject>();
    }

    void Start()
    {
        Packet join = new Packet(0);
        socket.SendTo(join.GetData(), endPoint);
    }
}
