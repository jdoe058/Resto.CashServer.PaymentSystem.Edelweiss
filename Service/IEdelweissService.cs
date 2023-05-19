// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Service.IEdelweissService
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using Resto.CashServer.PaymentSystem.Edelweiss.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Service
{
  [ServiceContract]
  public interface IEdelweissService
  {
    [OperationContract]
    [FaultContract(typeof (EdelweissFault))]
    List<EdelweissGuest> GetGuests();

    [OperationContract]
    [FaultContract(typeof (EdelweissFault))]
    int Pay(
      Guid requestId,
      EdelweissGuest guest,
      Decimal sum,
      string cashier,
      int departmentCode);
  }
}
