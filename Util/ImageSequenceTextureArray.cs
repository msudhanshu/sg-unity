using UnityEngine;
using System.Collections;


//http://www.41post.com/4726/programming/unity-animated-texture-from-image-sequence-part-1
using System.Collections.Generic;


public class ImageSequenceTextureArray : MonoBehaviour {

    //An array of Objects that stores the results of the Resources.LoadAll() method  
    private Object[] objects;  
    //Each returned object is converted to a Texture and stored in this array  
    //public List<Texture> textures; 
    public Texture[] textures; 
    //With this Material object, a reference to the game object Material can be stored  
    private Material goMaterial;  
    private Renderer targetRender;
    public int FPS = 5;
    //An integer to advance frames  
    private int frameCounter = 0;     
    public bool initWithResDir = false;
    public string resDir = "gif1";
    [HideInInspector]
    public bool textureInitialized = false;

    void Awake()  
    {  
       // textures = new List<Texture>();
        //Get a reference to the Material of the game object this script is attached to  
        targetRender = gameObject.GetComponent<Renderer>();
        this.goMaterial = targetRender.material;  
        if(initWithResDir)
            StartCoroutine(CoroutineLoadFromResources(resDir));

    }  
        
//    public void clearTextures() {
//       // textures.Clear();
//    }
//
//    public void addTexture(Texture t) {
//       // textures.Add(t);
//    }
//
    void Update ()  
    {  
        if(textureInitialized) {
            //Call the 'PlayLoop' method as a coroutine with a 0.04 delay  
            StartCoroutine("PlayLoop",1/(float)FPS);  
            //Set the material's texture to the current value of the frameCounter variable  
            goMaterial.mainTexture = textures[frameCounter];  
        }

    }


    //The following methods return a IEnumerator so they can be yielded:  
    //A method to play the animation in a loop  
    IEnumerator PlayLoop(float delay)  
    {  
        //Wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);    

        //Advance one frame  
        frameCounter = (++frameCounter)%textures.Length;  

        //Stop this coroutine  
        StopCoroutine("PlayLoop");  
    }    

    //A method to play the animation just once  
    IEnumerator Play(float delay)  
    {  
        //Wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);    

        //If the frame counter isn't at the last frame  
        if(frameCounter < textures.Length-1)  
        {  
            //Advance one frame  
            ++frameCounter;  
        }  

        //Stop this coroutine  
        StopCoroutine("PlayLoop");  
    }   

    public IEnumerator CoroutineLoadFromResources(string seqdir) {
        this.objects = Resources.LoadAll(seqdir, typeof(Texture));  
        yield return 0;

        int gifFrameSize = objects.Length;

        //Initialize the array of textures with the same size as the objects array  
        this.textures = new Texture[gifFrameSize];  

        //Cast each Object to Texture and store the result inside the Textures array  
        for(int i=0; i <gifFrameSize; i++)  
        {  
            this.textures[i] = (Texture)this.objects[i];  
        }
        this.textureInitialized = true;
        yield return 0;
    }
}  