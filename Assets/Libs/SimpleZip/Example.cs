using Giab.Common.Utils;
using Ionic.Zlib;
using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleZip
{
    public class Example : MonoBehaviour
    {
        public Text Text;

        /// <summary>
        /// Usage example
        /// </summary>
        public IEnumerator Start()
        {
            foreach (string file in System.IO.Directory.GetFiles("E:\\UnityProjectTemp\\Testconvert\\Assets\\LevelZip"))
            {
                if (!file.Contains(".meta"))
                {
                   yield return StartCoroutine(ReadStringEncode(file));

                }

            }
        }

        

        
        static void WriteString(string path, string data)
        {
            path = path.Replace("E:\\UnityProjectTemp\\Testconvert\\Assets\\Level", "E:\\UnityProjectTemp\\Testconvert\\Assets\\LevelZip"); 
            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(data);
            writer.Close();

        }

        
        static IEnumerator ReadString(string path)
        {

            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);
            string ss = reader.ReadToEnd();
            reader.Close(); 
            ss = StringToolbox.Base64ZipEncodeString(ss);

           
            WriteString(path, ss);

            yield return true;
        }


        
        static IEnumerator ReadStringEncode(string path)
        {

            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);
            string ss = reader.ReadToEnd();
            reader.Close();
            ss = StringToolbox.Base64ZipDecodeString(ss);


            WriteStringEndoce(path, ss);

            yield return true;
        }

        static void WriteStringEndoce(string path, string data)
        {
            path = path.Replace("E:\\UnityProjectTemp\\Testconvert\\Assets\\LevelZip", "E:\\UnityProjectTemp\\Testconvert\\Assets\\LevelUnZip");
            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(data);
            writer.Close();

        }
    }
}