using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using SgUnity;
//1. Resource  2. Sdcard   3.server   4.cdn   5.dropbox  6.googledrive
using System.IO;


public class QAssetGifDownloader : QAssetDownloader<Texture2D> {
    private string gifResFolder;
    public QAssetGifDownloader(MonoBehaviour mbObject) {
        this.mbObject = mbObject;
    }

	public void SetQuestionGifImages(IAssetRequest questionAsset, IAssetLoadCallback<List<Texture2D>> imagesLoadCallback)
    {
        this.questionAsset = questionAsset;
		gifResFolder = Path.GetFileNameWithoutExtension(questionAsset.getAssetUrl());
        mbObject.StartCoroutine(CoroutineLoadGifImages(imagesLoadCallback));
    }

    override protected string getResourceUrl(string fileName) {
        return Path.GetFileNameWithoutExtension(fileName);
    }

    override protected string getApkAssetUrl(string fileName) {
        return "movieguess/"+gifResFolder+"/"+fileName;   
    }

    override protected string getSdCardPath(string fileName) {
        return "file:///storage/emulated/0/movieguess/"+gifResFolder+"/"+fileName;   
    }

    override protected string getServerUrl(string fileName) {
		return SgUnityConfig.ServerConfig.BASE_URL+"/externalAssets/"+gifResFolder+"/"+fileName;
    }

    override protected string getCdnUrl(string fileName) {
		return SgUnityConfig.ServerConfig.CDN_URL+"/"+gifResFolder+"/"+fileName;
    }

    IEnumerator CoroutineLoadGifImages(IAssetLoadCallback<List<Texture2D>> imagesLoadCallback) {
        List<Texture2D> textures = new List<Texture2D>();
        bool retry = true;
        bool tryEveryWhere = false;
        while(retry) {
            
			if(questionAsset.getAssetType() == AssetType.GIF && !tryEveryWhere && (questionAsset.getCdnType()==CdnType.RESOURCE || questionAsset.getCdnType()==CdnType.UNKNOWN) && !questionAsset.getIsGifSprite()) {
           
                Debug.Log("Gif images is being downloaded from Resources Folder");
                UnityEngine.Object[] texobjects;  
                texobjects = Resources.LoadAll( gifResFolder, typeof(Texture));  
                yield return 0;
                int gifFrameSize = texobjects.Length;
                //Cast each Object to Texture and store the result inside the Textures array  
                for(int i=0; i <gifFrameSize; i++)  
                {  
                    textures.Add( (Texture2D)texobjects[i] );  
                }
                if(textures!=null && textures.Count > 0) {
                    retry = false;
                }else {
					Debug.Log("Gif Asset " + questionAsset.getAssetUrl()  +" Load retrying everywhere now since couldn't found in "+questionAsset.getCdnType());
                    tryEveryWhere = true;
                }
                
          } else {
            
				for (int i=1; i<=questionAsset.getGifFrameCount(); i++ ) {
                    Texture2D loadedAsset ;
					string extention = Path.GetExtension(questionAsset.getAssetUrl());
                    string url = gifResFolder + "_" + i + extention;
                    yield return  mbObject.StartCoroutine(CoroutineResolveLoad(url,
                        t => {
                            loadedAsset = t;
                            if(loadedAsset) 
                                textures.Add(loadedAsset);
                            else {
                                //imagesLoadCallback.assetLoadFailed();
                            }
                        }
                    ));
                }
                retry = false;
                tryEveryWhere = false;

        }

        }
        if(textures!=null && textures.Count > 0)
             imagesLoadCallback.assetLoadSuccess(textures);
        else
            imagesLoadCallback.assetLoadFailed();
    }

}
