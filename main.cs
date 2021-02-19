using System;
using System.Collections.Generic;
using System.Text;

class MainClass {
        
public static void Main (string[] args) {
    string frame = "1H|1|P";
    string hexDelimiter = " ";

    ChecksumCal cc = new ChecksumCal(frame,hexDelimiter);
    Console.WriteLine("Complete Frame in Hex:");
    Console.WriteLine(cc.GetFrameHexString());
    Console.WriteLine("Checksum:");
    Console.WriteLine(cc.GetChecksumHex());
  }
}


class ChecksumCal {

  public static string StringFrame { get; internal set;}
  public static string Delimiter { get; internal set;}

  static byte byteETX = 0x03, byteCR = 0x0D;

  private static byte[] StringToBytes(string s) { return Encoding.ASCII.GetBytes(s); }
  private static string BytesToString(byte[] b) { return Encoding.UTF8.GetString(b); }
  private static string ChecksumFormat(int c) { return c.ToString("x").ToUpper(); }

  private static byte[] BuildCompleteFrame() {
    List<byte> bytesList = new List<byte>(); 
    bytesList.AddRange(StringToBytes(StringFrame));
    bytesList.Add(byteCR);
    bytesList.Add(byteETX);
    return bytesList.ToArray();
  }
    
  public string GetFrameHexString() {
    return BitConverter.ToString(BuildCompleteFrame()).Replace("-", Delimiter);
  }

  public string GetChecksumHex() {
    var completeFrame = BytesToString(BuildCompleteFrame());
    int cs = 0;
    for (int i = 0; i < completeFrame.Length; i++) cs += (int)completeFrame[i]; cs %= 256;
    string h = ChecksumFormat(cs);
    h = h.Length == 1 ? "0" + h : h;
    return h;
  }

  public ChecksumCal(string s, string d = "-") {
    StringFrame = s;
    Delimiter = d;
  }

}