using UnityEngine;
using System.Collections;

namespace SgUnity
{
public interface ServerNotifier {

	void OnSuccess(GameResponse response);
	void OnFailure(string error);
}

}