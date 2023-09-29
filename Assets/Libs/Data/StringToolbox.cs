
using Ionic.Zlib;
using System.Collections;
using System.IO;
using UnityEngine;

namespace /*<com>*/Giab.Common.Utils
{
    public class StringToolbox
    {
        private static string BASE64CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        private static string ROT13CHARS = "abcdefghijklmnopqrstuvwxyzabcdefghijklmABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLM";
        public StringToolbox()
        {

        }
        public static string Base64EncodeString(string pData)
        {
            byte[] vBa = System.Text.Encoding.UTF8.GetBytes(pData);

            return Base64EncodeByteArray(vBa);
        }
        public static string Base64ZipEncodeString(string pData)
        {
            byte[] vBa = System.Text.Encoding.UTF8.GetBytes(pData);

            vBa = ZlibStream.CompressBuffer(vBa); 

            return Base64EncodeByteArray(vBa);
        }
        public static string Base64EncodeByteArray(byte[] pData)
        {
            int i = 0;
            int[] vDataBuffer = null;
            string vStr = "";
            int[] vOutputBuffer = new int[4];

            using (MemoryStream m = new MemoryStream(pData))
            {
                m.Position = 0;
                using (BinaryReader w = new BinaryReader(m))
                {
                    while (w.BaseStream.Position != w.BaseStream.Length)
                    {
                        vDataBuffer = new int[3];
                        i = 0;
                        while (i < 3 && w.BaseStream.Position != w.BaseStream.Length)
                        {
                            vDataBuffer[i] = w.ReadByte();
                            i++;
                        }
                        vOutputBuffer[0] = (vDataBuffer[0] & 252) >> 2;
                        vOutputBuffer[1] = (vDataBuffer[0] & 3) << 4 | vDataBuffer[1] >> 4;
                        vOutputBuffer[2] = (vDataBuffer[1] & 15) << 2 | vDataBuffer[2] >> 6;
                        vOutputBuffer[3] = vDataBuffer[2] & 63;
                        for (i = vDataBuffer.Length; i < 3; i++)
                        {
                            vOutputBuffer[i + 1] = 64;
                        }
                        for (i = 0; i < vOutputBuffer.Length; i++)
                        {
                            vStr = vStr + BASE64CHARS[(vOutputBuffer[i])];
                        }
                    }
                }
            }
            return vStr;
        }
        public static string Base64DecodeString(string pData)
        {
            byte[] vBa = Base64DecodeByteArray(pData);
            return System.Text.Encoding.UTF8.GetString(vBa);
        }
        public static string Base64ZipDecodeString(string pData)
        {
            byte[] vBa = Base64DecodeByteArray(pData);
             
            vBa = Decompress(vBa);

            return System.Text.Encoding.UTF8.GetString(vBa);
        }


        public static byte[] Decompress(byte[] gzip)
        {
            using (var stream = new Ionic.Zlib.ZlibStream(new MemoryStream(gzip), Ionic.Zlib.CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public static byte[] Compress(byte[] gzip)
        {
            using (var stream = new Ionic.Zlib.ZlibStream(new MemoryStream(gzip), Ionic.Zlib.CompressionMode.Compress))
            {

                using (MemoryStream memory = new MemoryStream())
                {

                    memory.Write(gzip, 0, gzip.Length);

                    return memory.ToArray();
                }
            }
        }

        public static byte[] Base64DecodeByteArray(string pData)
        {
            int i = 0;
            int j = 0;
            byte[] vBa;
            int[] vDataBuffer = new int[4];
            int[] vOutputBuffer = new int[3];
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(m))
                {
                    int iLim = pData.Length;
                    for (i = 0; i < iLim; i = i + 4)
                    {
                        j = 0;
                        while (j < 4 && i + j < iLim)
                        {
                            vDataBuffer[j] = BASE64CHARS.IndexOf(pData[(i + j)]);
                            j++;
                        }
                        vOutputBuffer[0] = (vDataBuffer[0] << 2) + ((vDataBuffer[1] & 48) >> 4);
                        vOutputBuffer[1] = ((vDataBuffer[1] & 15) << 4) + ((vDataBuffer[2] & 60) >> 2);
                        vOutputBuffer[2] = ((vDataBuffer[2] & 3) << 6) + vDataBuffer[3];
                        for (j = 0; j < vOutputBuffer.Length; j++)
                        {
                            if (vDataBuffer[j + 1] == 64)
                            {
                                break;
                            }
                            w.Write((byte)vOutputBuffer[j]);
                        }
                    }
                }
                vBa = m.ToArray();
            }

            return vBa;
        }

    }
}