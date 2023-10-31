using UnityEngine;

using System.Collections;

using System.Text;

using System.Runtime.InteropServices;

using System;

//using System.Diagnostics;

public class WczytywanieSciezki : MonoBehaviour
{
    OpenFileName ofn;
    public class LibWrap
    {
        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    }

    public void WczytajSciezke()
    {
        //Process.Start(@"c:\");
        ofn = new OpenFileName();

            ofn.structSize = Marshal.SizeOf(ofn);

            ofn.filter = "Text files\0*.txt*\0\0";

            //ofn.filter = "Text files\0*folder*\0\0";

            ofn.file = new string(new char[256]);

            ofn.maxFile = ofn.file.Length;

            ofn.fileTitle = new string(new char[64]);

            ofn.maxFileTitle = ofn.fileTitle.Length;

            ofn.initialDir =UnityEngine.Application.dataPath;

            ofn.title = "Wczytaj mapę";

            ofn.defExt = "TXT";

            ofn.flags=0x00080000|0x00001000|0x00000800|0x00000200|0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

            if(LibWrap.GetOpenFileName(ofn))
            {
                DaneDoMap.uzywanaSciezka = ofn.file;
                //UnityEngine.Debug.Log(DaneDoMap.uzywanaSciezka);
            }

            //StartCoroutine(Cos());
    }

    /*IEnumerator Cos()
    {
        if(LibWrap.GetOpenFileName( ofn ))
            {
                yield return null;
                DaneDoMap.uzywanaSciezka = ofn.file;
                UnityEngine.Debug.Log(DaneDoMap.uzywanaSciezka);
            }
    }*/
}
