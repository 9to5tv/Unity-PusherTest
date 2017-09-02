﻿using UnityEngine;
using System.Collections;

public class PusherSettings
{
	// if true, Pusher will output debug info to console
	public static bool Verbose = true;

	// App Key from pusher.com app settings
	public static string AppKey = "";

	// if specified, then this will be used as callback url for authorizing connections to private channels
	public static string HttpAuthUrl = "";

	// client name & version for identifying client library
	public static string ClientName = "pusher-unity";
	public static string ClientVersion = "1.0";

    public static string Cluster = "";
}
