//
//  DummyServer.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

public class DummyServer : IWWWWrapper,IServerResponseListner {
	string url;
	string className;
	string methodName;
	Dictionary<string,string> param;
	string serverName;

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
	
	public DummyServer(string url) {
		this.url = url;
		ParseUrlForReflection();
		ReflectAndCallServerController();
	}

	public DummyServer() {

	}

	public void Init(string url) {
		this.url = url;
		ParseUrlForReflection();
		ReflectAndCallServerController();
	}

	private void ParseUrlForReflection() {
		DebugDummyServer.Log("url to parse :"+url);
		string[] domainNparam = url.Split('?');
		if(domainNparam.Length>1) {
			ParseParam(domainNparam[1]);
		}
		string[] a = domainNparam[0].Split('/');

	    int length = a.Length;
		int domainIndex=-length;
		for(int i=0 ; i< length; i ++) {
			if(a[i].Contains(".com")) {
			  domainIndex = i;
				break;
			}
		}
		serverName = a[domainIndex+1];
		if(domainIndex+3 < length) {
			className = a[domainIndex+2];
			methodName = a[domainIndex+3];
		} else {
			className = "index";
			methodName = a[domainIndex+2];
		}
		className = "Server_"+ToTitleCase(className);
		methodName = ToTitleCase(methodName);

		DebugDummyServer.Log("ClassName="+className+",MethodName="+methodName);
	  //  Uri siteUri = new Uri(url);
		//NameValueCollection allParams = HttpUtility.ParseQueryString(siteUri.Query);
	}

	private string ToTitleCase(string name) {
		TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
		name = textInfo.ToTitleCase(name);
		return name;
	}

	private void ParseParam(string query) {
		if(query==null) return;
		if(param == null) param  = new Dictionary<string,string>(5);
		string[] allParams = query.Split('&');
		foreach(string s in allParams) {
			string[] keyvalue = s.Split('=');
			string key = keyvalue[0];
			string value = keyvalue.Length>1?keyvalue[1]:"";
			DebugDummyServer.Log("Param added: key="+key+", Value="+value);
			param.Add(key,value);
		}
	}

	private string GetValue(string key) {
		if(param==null) return null;
		return param.ContainsKey(key)?param[key]:null;
	}

	private void ReflectAndCallServerController() {
	/*	DummyServerManager.GetInstance().StartCoroutine(CallServerController());
	}
	
	IEnumerator CallServerController() {
		yield return null;
		*/
		try {
		// Get the type contained in the name string
		Type type = Type.GetType(className, true);
		// create an instance of that type
		object instance = Activator.CreateInstance(type);

			(instance as Server_Controller).serverResponseListner =  this;
			//FieldInfo field = type.GetField("serverResponseListner", BindingFlags.Public | BindingFlags.Instance);
			//field.SetValue(instance,this);
			//	typeof(type).SetField("serverResponseListner", this);
			//	instance.GetType().GetProperty(propName).SetValue()  GetValue(instance, null);

            type.InvokeMember(methodName,
		                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
		                null, instance, new object[] { param});
		//text = new Server_Users().New();
		} catch (Exception e) {
			error = e.Message;
			isDone=true;
			DebugDummyServer.LogError("reflection failed "+error);
		}
		DebugDummyServer.Log("server has been called and waiting for callback ");
		//yield break;
	}

	public void OnComplete(string serverResponse)
	{
		DebugDummyServer.Log("server Response completed ="+serverResponse);
		text = serverResponse;
		isDone = true;
	}
	
}



/*
 * http://www.dotnetperls.com/reflection-field
 * http://msdn.microsoft.com/en-us/library/kyaxdd3x(v=vs.110).aspx
 * http://www.techrepublic.com/article/applied-reflection-dynamically-accessing-properties-of-a-class-at-runtime/
 * 
        static void Main(string[] args)
        {
            Type testType = typeof(TestClass);
            ConstructorInfo ctor = testType.GetConstructor(System.Type.EmptyTypes);
            if(ctor != null)
            {
                object instance = ctor.Invoke(null);
                MethodInfo methodInfo = testType.GetMethod("TestMethod");
                Console.WriteLine(methodInfo.Invoke(instance, new object[] { 10 }));
            }
            Console.ReadKey();
        }
    }

    public class TestClass
    {
        private int testValue = 42;

        public int TestMethod(int numberToAdd)
        {
            return this.testValue + numberToAdd;
        }
    }

*/
