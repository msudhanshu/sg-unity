//
//  WWWWrapper.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;
using CielaSpike;
using System.Threading;

public class WWWWrapper : IWWWWrapper
{
	MonoBehaviour monObject;
	WWW www;
	DummyServer dummyServer;
	string url;

	private bool _isDone = false;
	public bool isDone {
		get {
			return _isDone;
		}
		private set {
			_isDone = value;
		}
	}

	private string _error = null;
	public string error {
		get {
			return _error;
		}
		private set {
			_error = value;
        }
    }

	private string _text = null;
	public string text {
		get {
			return _text;
		}
		private set {
			_text = value;
        }
    }
    
    public WWWWrapper(string url) {
		this.url = url;
		monObject = DummyServerManager.GetInstance();
		if(DummyServerManager.DUMMY_SERVER_ENABLED) {
			monObject.StartCoroutine(StartDummyServer());
		} else
			monObject.StartCoroutine(StartWWWDownload());
	}

	IEnumerator StartWWWDownload() {
		www = new WWW(url);
        yield return www;
		error = www.error;
		text = www.text;
		isDone = true;
    }

	IEnumerator StartDummyServer() {
		CielaSpike.Task task;
		DebugDummyServer.Log("Blocking Thread");
		monObject.StartCoroutineAsync(ExecuteDummyServerInThread(), out task);
		yield return monObject.StartCoroutine(task.Wait());
		DebugDummyServer.Log("Blocking Thread execution done");
    }

	IEnumerator ExecuteDummyServerInThread() {
		dummyServer = new DummyServer(url);
		while(!dummyServer.isDone)
			yield return 0;
		error = dummyServer.error;
		text = dummyServer.text;
		isDone = dummyServer.isDone;
	}
}