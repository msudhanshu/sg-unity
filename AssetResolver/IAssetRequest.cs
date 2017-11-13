using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using SgUnity;

public enum AssetType {
	IMAGE,
	MUSIC,
	GIF,
	TEXT,
	SPRITE,
	DEFAULT
}

public enum CdnType {
	UNKNOWN,
	RESOURCE,
	APK,
	SDCARD,
	SERVER,
	CDN,
	FIREBASE_STORAGE
}


public interface IAssetRequest  {

	string getAssetUrl ();
	CdnType getCdnType();
	int getGifFrameCount();
	bool getIsGifSprite();
	AssetType getAssetType ();
}
