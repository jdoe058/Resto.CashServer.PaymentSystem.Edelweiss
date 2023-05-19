// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Cheques.EdelweissBillCheque
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using Resto.CashServer.PaymentSystem.Edelweiss.Data;
using Resto.CashServer.PaymentSystem.Edelweiss.Settings;
using Resto.CashServer.Services.Data.Orders;
using Resto.Front.Views.Cheques;
using System;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Cheques
{
  public sealed class EdelweissBillCheque : CustomBillChequeBase
  {
    public EdelweissBillCheque(IBaseOrder order, EdelweissGuest epitomeGuest, string chequeNumber)
      : base(order, chequeNumber, epitomeGuest.RoomNumber, epitomeGuest.Name)
    {
    }

    protected override Type PaymentItemType => typeof (EdelweissPaymentItem);

    protected override bool PrintGuestSignature => EdelweissSettingsLocator.Instance.Settings.PrintSignaturePlace;
  }
}
