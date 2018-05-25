using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using System;

[Serializable]
public class ChatData
{
    public string msg;
}

public class SocketIOScript : MonoBehaviour {

	public string serverURL = "http://localhost:3000";
	protected Socket socket = null;

	int chatCount = 0;
	public List<string> chatLog = new List<string>();
	public GameObject chatBox;
	public 	InputField uiInput;

	public Text text = null;

	void Destroy()
	{
		DoClose();
	}
	// Use this for initialization
	void Start () {
		DoOpen();
		chatCount = chatLog.Count;
	}
	
	void Update()
	{
		if(Input.GetKey(KeyCode.Escape))
			DoClose();

		if(Input.GetButtonDown("Submit"))
			SendChat();

		if(chatCount != chatLog.Count)
		{
			chatCount = chatLog.Count;
			CreateNewText(chatLog[chatLog.Count-1]);
		}
	}
	
	void DoOpen()
	{
		if(socket == null)
		{
			socket = IO.Socket(serverURL);
			socket.On (Socket.EVENT_CONNECT, () => {
				Debug.Log("Joined");
			});

			socket.On ("chat", (data) => {
				string str = data.ToString();
				ChatData chat = JsonUtility.FromJson<ChatData>(str);
				chatLog.Add(chat.msg);
			});
		}
	}

	void DoClose()
	{
		if(socket !=null)
		{
			socket.Disconnect();
			socket = null;
		}
	}

	public void SendChat() {
		if(uiInput.text != "")
		{
			if (socket != null) 
				socket.Emit ("chat", uiInput.text);
		}
		//Clear input
		uiInput.text = "";
	}

	public void CreateNewText(string msg)
	{
		Text newText = Instantiate(text,transform.position, transform.rotation,chatBox.transform);
		newText.text = msg;
	}
}
