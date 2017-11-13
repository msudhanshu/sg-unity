using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using SgUnity;
//1. Resource  2. Sdcard   3.server   4.cdn   5.dropbox  6.googledrive 7.firebaseStorage
using System.IO;
using Firebase.Storage;
using System.Threading.Tasks;


public class QAssetDownloader<T> : MonoBehaviour  where T : UnityEngine.Object{
    protected T loadedAsset;
    //private IAssetLoadCallback<T> callback;
	protected IAssetRequest questionAsset;
  //  private AudioSource audioSource;
    protected MonoBehaviour mbObject;

    public QAssetDownloader() {
        this.mbObject = QAssetDownloadManager.GetInstance();
    }

    public QAssetDownloader(MonoBehaviour mbObject) {
        this.mbObject = mbObject;
    }

	public void SetQuestionAsset(IAssetRequest questionAsset, IAssetLoadCallback<T> callback) {
        this.questionAsset = questionAsset;
        mbObject.StartCoroutine(CoroutineLoadImage(callback));
    }

    IEnumerator CoroutineLoadImage(IAssetLoadCallback<T> callback) {
        T loadedAsset ;
		yield return  mbObject.StartCoroutine(CoroutineResolveLoad(questionAsset.getAssetUrl(),
            t =>  {
                loadedAsset = t;
                if(loadedAsset) callback.assetLoadSuccess(loadedAsset);
                else {
                    callback.assetLoadFailed();
//                    if(tryEveryWhere) {
//                        callback.assetLoadFailed();
//                    } else {
//                        //if not unknown then then try again with all posiblity
//                    }
                }
            }
        ));

    }

    virtual protected string getResourceUrl(string fileName) {
        return Path.GetFileNameWithoutExtension(fileName);
    }

    virtual protected string getApkAssetUrl(string fileName) {
        return "movieguess/"+ Path.GetFileNameWithoutExtension(fileName);   
    }

    virtual protected string getSdCardPath(string fileName) {
        return "file:///storage/emulated/0/movieguess/"+fileName;   
    }

    virtual protected string getServerUrl(string fileName) {
		return SgUnityConfig.ServerConfig.BASE_URL+"/externalAssets/"+fileName;
    }

    virtual protected string getCdnUrl(string fileName) {
		return SgUnityConfig.ServerConfig.CDN_URL+"/"+fileName;
    }
     
	virtual protected string getFirebaseStorageRefrenceUrl(string fileName) {
		return SgFirebase.MyStorageBucket+"/"+fileName;
	}

    //1,abstractoin at this level , for facebook dowloader api coroutine 
    //2. abstraction for downloading through , www, or resourceload, or facebookdownloadapi

    // UnityAction<Texture2D> textureCallback , textureCallback.Invoke(www.texture);   

    protected IEnumerator CoroutineResolveLoad(string url, System.Action<T> resultCallback /*,IAssetLoadCallback<T> callback*/ ) {
        //string url = questionAsset.assetUrl;
        T loadedAsset = null;
        Debug.Log("File being loaded: "+url);

        //questionAsset.cdn = "";
        bool retry = true;
        bool tryEveryWhere = false;

		CdnType cdnType = questionAsset.getCdnType ();

        while(retry) {
            
        if(tryEveryWhere || cdnType == CdnType.RESOURCE) {
        //Try on resources
            Debug.Log("Try downloading in res "+url );
            loadedAsset = Resources.Load<T>(getResourceUrl(url));
            if(loadedAsset) {
                resultCallback(loadedAsset);
                //callback.loadedAsset(loadedAsset);
                yield break;
            }
        }

        if(tryEveryWhere || cdnType == CdnType.APK) {
            //try on apk but not resource

                #if UNITY_EDITOR
                Debug.Log("loading failed  and try again in apk "+ getApkAssetUrl(url) );

            loadedAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(getApkAssetUrl(url));
            if(loadedAsset) {
                resultCallback(loadedAsset);
                //callback.loadedAsset(loadedAsset);
                yield break;
            }
                #endif
        }
               
        if(tryEveryWhere || cdnType == CdnType.SDCARD) {
            //Try on sdcard
            Debug.Log("loading failed  and try again in sdcard "+getSdCardPath(url) );
            yield return mbObject.StartCoroutine(CoroutineLoadUrl(getSdCardPath(url),  t => loadedAsset = t ));
            if(loadedAsset) {
                resultCallback(loadedAsset);
                //callback.loadedAsset(loadedAsset);
                yield break;
            }
        }

        if(tryEveryWhere || cdnType == CdnType.SERVER) {
            Debug.Log("loading failed  and try again in server "+ getServerUrl(url) );
            yield  return mbObject.StartCoroutine(CoroutineLoadUrl(getServerUrl(url),  t => loadedAsset = t ));
            if(loadedAsset) {
                resultCallback(loadedAsset);
                //callback.loadedAsset(loadedAsset);
                yield break;
            }
        }

        if(tryEveryWhere || cdnType == CdnType.CDN) {
            Debug.Log("loading failed  and try again in cdn "+ getCdnUrl(url) );
            yield  return mbObject.StartCoroutine(CoroutineLoadUrl(getCdnUrl(url), t => loadedAsset = t ));
            if(loadedAsset) {
                resultCallback(loadedAsset);
                //callback.loadedAsset(loadedAsset);
                yield break;
            }
        }

		if(tryEveryWhere || cdnType == CdnType.FIREBASE_STORAGE) {
				Debug.Log("loading failed  and try again in firebase url "+ getFirebaseStorageRefrenceUrl(url) );
				//yield  return mbObject.StartCoroutine(CoroutineLoadUrl(getCdnUrl(url), t => loadedAsset = t ));
				//yield  return mbObject.StartCoroutine(SgFirebase.GetInstance().DownloadFromFirebaseStorageURL(getCdnUrl(url), t => loadedAsset = t ));
			
				/*
				StorageReference reference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl(getFirebaseStorageRefrenceUrl(url));
				reference.GetDownloadUrlAsync().ContinueWith(task => {
					if (!task.IsFaulted && !task.IsCanceled) {
						string firebaseStorageDownloadurl = task.Result;
						Debug.Log("Firebase storage Download URL: " + firebaseStorageDownloadurl);
						//yield return mbObject.StartCoroutine(CoroutineLoadUrl(firebaseStorageDownloadurl, t => loadedAsset = t ));
						// ... now download the file via WWW or UnityWebRequest.

						WWW www = null;
						//T loadedAsset = null;
						www = new WWW(url);

						while(!www.isDone) {
							yield return 0;
						}
						try {
							if(www!=null && www.error == null) {
								
								UnityEngine.Object t = null;
								if(typeof(T) == typeof(AudioClip))
									loadedAsset = (T)(UnityEngine.Object)www.GetAudioClip();
								else if(typeof(T) == typeof(Texture2D) )
									loadedAsset = (T)(UnityEngine.Object)www.texture;   
								resultCallback(loadedAsset);
							}
						} catch (Exception e) {
							Debug.Log(e.ToString());
						}

					}
				});
				*/
			if(loadedAsset) {
				resultCallback(loadedAsset);
				//callback.loadedAsset(loadedAsset);
				yield break;
			}
		}

        if(tryEveryWhere ) { // it means it has already done a pass of trying everywhere, so start retry now.
            retry = false;
            tryEveryWhere = false;
        } else {
				Debug.Log("Asset Load retrying everywhere now since couldn't found in "+questionAsset.getCdnType());
            tryEveryWhere = true;
        }

        }
        //callback.assetLoadFailed();
        resultCallback(null);
        yield break;
    }

    protected IEnumerator CoroutineLoadUrl(string url, System.Action<T> resultCallback)  {
        WWW www = null;
        T loadedAsset = null;
        www = new WWW(url);
        while(!www.isDone) {
            yield return 0;
        }
        try {
            if(www!=null && www.error == null) {
                UnityEngine.Object t = null;
                if(typeof(T) == typeof(AudioClip))
                    loadedAsset = (T)(UnityEngine.Object)www.GetAudioClip();
                else if(typeof(T) == typeof(Texture2D) )
                    loadedAsset = (T)(UnityEngine.Object)www.texture;   
                resultCallback(loadedAsset);
            }
        } catch (Exception e) {
            Debug.Log(e.ToString());
        }
        yield break;
    }
      
}
