// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Service.EdelweissFault
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using System.Runtime.Serialization;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Service
{
  [DataContract]
  public class EdelweissFault
  {
    [DataMember]
    public string Message { get; set; }

    [DataMember]
    public int ResultCode { get; set; }

    public EdelweissFault() => ResultCode = -1;
  }
}
