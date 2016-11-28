using System;

namespace SgUnity
{
[System.Serializable]
public class GameResponse {
	public Action action;
	public string response;
	public bool failed;

	public GameResponse(Action action, string response, bool fail) {
		this.action = action;
		this.response = response;
		this.failed = fail;
	}
}
}