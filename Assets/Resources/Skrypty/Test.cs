using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;

    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;

    public String file = null;
    public int maxFile = 0;

    public String fileTitle = null;
    public int maxFileTitle = 0;

    public String initialDir = null;

    public String title = null;

    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;

    public String defExt = null;

    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;

    public String templateName = null;

    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class LibWrap
{
    //BOOL GetOpenFileName(LPOPENFILENAME lpofn);

    [DllImport("Comdlg32.dll", CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
}

public class Test : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Czekaj());
    }

    public IEnumerator Czekaj()
    {
        yield return new WaitForSeconds(5);
        Wczytuj();
    }

    void Wczytuj()
    {
        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = "Text files\0*.txt\0";//\0Batch files\0*.bat\0";

        ofn.file = new String(new char[256]);
        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new String(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = "C:\\";
        ofn.title = "Open file called using platform invoke...";
        ofn.defExt = "txt";

        if (LibWrap.GetOpenFileName(ofn))
        {
            Debug.Log(ofn.file);
            //Console.WriteLine("Selected file name: {0}", ofn.fileTitle);
            //Console.WriteLine("Offset from file name: {0}", ofn.fileOffset);
            //Console.WriteLine("Offset from file extension: {0}", ofn.fileExtension);
        }
    }
}
