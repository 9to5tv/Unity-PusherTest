using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class PusherMessageReceiver : MonoBehaviour
{
    // isntance of pusher client
    PusherClient.Pusher pusherClient = null;
    PusherClient.Channel _channel;

    public string AuthURL = "http://ec2-18-220-122-172.us-east-2.compute.amazonaws.com:4000/pusher/auth";
    public string AppKey = "b8422ea96c48b1169412";

    public bool Encrypted = true;
    public bool Verbose = true;

    public string Cluster = "us2";


    public string ChannelName = "private-tools-demo";
    public string EventName = "client-tool-action";

    public UnityEvent ActionA;
    public UnityEvent ActionB;
    public UnityEvent ActionC;

    private Stack<UnityEvent> _events = new Stack<UnityEvent>();

    // Initialize
    void Start()
    {
        // TODO: Replace these with your app values
        PusherSettings.Verbose = Verbose;
        PusherSettings.AppKey = AppKey;
        PusherSettings.HttpAuthUrl = AuthURL;
        PusherSettings.Cluster = Cluster;


        PusherClient.PusherOptions opts = new PusherClient.PusherOptions();
        opts.Cluster = Cluster;
        opts.Encrypted = Encrypted;
        opts.Authorizer = new PusherClient.HttpAuthorizer(PusherSettings.HttpAuthUrl);

        pusherClient = new PusherClient.Pusher(PusherSettings.AppKey, opts);
        pusherClient.Connected += HandleConnected;
        pusherClient.ConnectionStateChanged += HandleConnectionStateChanged;



        pusherClient.Connect();
    }

    void Update()
    {
        lock (this)
        {
            while (_events.Count > 0)
            {
                var e = _events.Pop();

                e.Invoke();
            }
        }
    }


    void HandleConnected(object sender)
    {
        Debug.Log("Pusher client connected, now subscribing to private channel");
        _channel = pusherClient.Subscribe(ChannelName);
        _channel.BindAll(HandleChannelEvent);
    }

    void OnDestroy()
    {
        if (pusherClient != null)
            pusherClient.Disconnect();
    }

    void HandleChannelEvent(string eventName, object evData)
    {
        Debug.Log("Received event on channel, event name: " + eventName + ", data: " + JsonHelper.Serialize(evData));

        var dict = evData as Dictionary<string, object>;
        var action = dict["action"] as Dictionary<string, object>;

        var str = action["data"].ToString();

        lock (this)
        {
            if (str == "Action A")
                _events.Push(ActionA);
            else if (str == "Action B")
                _events.Push(ActionB);
            else if (str == "Action C")
                _events.Push(ActionC);
        }
    }

    void HandleConnectionStateChanged(object sender, PusherClient.ConnectionState state)
    {
        Debug.Log("Pusher connection state changed to: " + state);

        //if (state.Equals(PusherClient.ConnectionState.Connected))
        //{

        //    _channel = pusherClient.Subscribe(ChannelName);

        //    _channel.Subscribed += (s) =>
        //    {
        //        Debug.Log("SubscribedToChannel");
        //    };

        //    _channel.Bind(EventName, (object data) =>
        //    {
        //        //Debug.Log(JsonHelper.Serialize(data));
        //        var dict = data as Dictionary<string, object>;
        //        //Debug.Log(dict["data"] as string);
        //        var action = dict["action"] as Dictionary<string, object>;

        //        Debug.Log(action["data"]);

        //        ActionA.Invoke();
        //    });
        //}
    }
}
