using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using SgUnity;


// Any asset: try to load in this order
// if cdn value is set then will dowload from there, in not present or fails will try at other places
//1. Resource  2. Sdcard   3.server   4.cdn   5.dropbox  6.googledrive

public class QAssetDownloadManager : Manager<QAssetDownloadManager>  {

    override public void StartInit() {
      
    }

    override public void PopulateDependencies() {
       
    }

	public void SetQuestionMusic(IAssetRequest questionAsset, IAssetLoadCallback<AudioClip> callback) {
       // gameObject.AddComponent<QMusicDownloader<AudioClip>>().SetQuestionMusic(questionAsset,callback) ;
        (new QAssetDownloader<AudioClip>(this)).SetQuestionAsset(questionAsset,callback) ;

    }

	public void SetQuestionImage(IAssetRequest questionAsset, IAssetLoadCallback<Texture2D> callback) {
        // gameObject.AddComponent<QMusicDownloader<AudioClip>>().SetQuestionMusic(questionAsset,callback) ;
        (new QAssetDownloader<Texture2D>(this)).SetQuestionAsset(questionAsset,callback) ;
    }

	public void SetQuestionGifSprite(IAssetRequest questionAsset, IAssetLoadCallback<Texture2D> callback) {
        // gameObject.AddComponent<QMusicDownloader<AudioClip>>().SetQuestionMusic(questionAsset,callback) ;
        (new QAssetDownloader<Texture2D>(this)).SetQuestionAsset(questionAsset,callback) ;
    }

	public void SetQuestionGif(IAssetRequest questionAsset, IAssetLoadCallback<List<Texture2D>> callback) {
        (new QAssetGifDownloader(this)).SetQuestionGifImages(questionAsset,callback) ;
    }
}