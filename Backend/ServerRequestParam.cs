using System;

namespace SgUnity
{
public class ServerRequestParam {
	public Action action;
	public string url;
	public ServerNotifier notifier;

	public ServerRequestParam(Action action, string url, ServerNotifier notifier = null){
		this.action = action;
		this.url = url;
		this.notifier = notifier;
	}

	public ServerRequestParam(){
	}
}
}
