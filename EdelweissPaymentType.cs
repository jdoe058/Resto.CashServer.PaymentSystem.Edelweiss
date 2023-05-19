// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.EdelweissPaymentType
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using Resto.CashServer.Services.Data;
using Resto.CashServer.Services.Data.Payment;
using Resto.Data;
using System;

namespace Resto.CashServer.PaymentSystem.Edelweiss
{
  internal sealed class EdelweissPaymentType : IExternalNonCashPaymentType
  {
    public TransactionType TransactionType => TransactionType.CREDIT;

    public IPaymentItem CreatePaymentItem(Guid id, INonCashPaymentType basePaymentType) => CreatePaymentItem(id, basePaymentType, null);

    public IPaymentItem CreatePaymentItem(
      Guid id,
      INonCashPaymentType basePaymentType,
      IExternalPaymentItemAdditionalData additionalData)
    {
      return new EdelweissPaymentItem(id, basePaymentType, 0M);
    }

    public IExternalPaymentItemAdditionalData GetAdditionalData(IPaymentItem paymentItem) => null;

    public bool CanProcessReturnWithoutOrder() => false;

    public void ProcessReturnWithoutOrder(
      INonCashPaymentType basePaymentType,
      Decimal sum,
      ICashRegister cashRegister,
      IUser cashier,
      Action<string> changeProgressBarMessage)
    {
      throw new NotSupportedException();
    }
  }
}
