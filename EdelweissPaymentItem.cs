// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.EdelweissPaymentItem
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using Resto.CashServer.Data;
using Resto.CashServer.PaymentSystem.Edelweiss.Cards;
using Resto.CashServer.PaymentSystem.Edelweiss.Cheques;
using Resto.CashServer.PaymentSystem.Edelweiss.Data;
using Resto.CashServer.PaymentSystem.Edelweiss.Exceptions;
using Resto.CashServer.Services;
using Resto.CashServer.Services.Data;
using Resto.CashServer.Services.Data.LockInfo;
using Resto.CashServer.Services.Data.Orders;
using Resto.CashServer.Services.EntityBuilders.Orders;
using Resto.CashServer.Services.Extensions;
using Resto.CashServer.Services.Helpers;
using Resto.Framework.Common;
using Resto.Front.Localization.Resources;
using Resto.Front.Main.MVC;
using System;
using System.Linq;

namespace Resto.CashServer.PaymentSystem.Edelweiss
{
  internal sealed class EdelweissPaymentItem : HotelPaymentItem<EdelweissCardTransaction>
  {
    private static readonly string ErrorGetGuests = EdelweissLocalResources.GuestListRetrievingError;
    private EdelweissCard card;
    private EdelweissGuest guest;
    private readonly int deparmentCode;

    public override bool AsyncProcessSupported => false;

    public EdelweissCard Card
    {
      get => card;
      set => card = value;
    }

    public EdelweissGuest Guest
    {
      get => guest;
      set => guest = value;
    }

    public EdelweissPaymentItem()
    {
    }

    public EdelweissPaymentItem(Guid id, INonCashPaymentType nonCashPaymentType, decimal amount)
      : base(id, nonCashPaymentType, amount)
    {
      deparmentCode = nonCashPaymentType.DepartmentCode;
    }

    protected override void ExecuteProcessingOperation(
      IBaseOrderBuilder orderBuilder,
      ILockedObjectsToken token,
      ICafeSession cafeSession,
      IUser cashier,
      IOrderMultiSaver orderSaver,
      Action<string> changeProgressBarMessage = null)
    {
      string str;
      EdelweissGuest edelweissGuest = ShowGuestSelectPopup(out str);
      if (edelweissGuest != null)
      {
        EdelweissPaymentItem.PrintHotelCheque(cafeSession.CashRegister, orderBuilder, edelweissGuest);
        if (!ShowSignatureConfirmation(cafeSession.CashRegister, orderBuilder, edelweissGuest, out str))
          edelweissGuest = null;
      }
      if (edelweissGuest == null)
      {
        Status = (Resto.Data.PaymentItemStatus) 10;
        FrontEntityExtensions.Save(orderBuilder.ToBaseOrder(), token);
        throw new RestoException(string.IsNullOrEmpty(str) ? EdelweissLocalResources.CancelMessage : str);
      }
      Card = new EdelweissCard(edelweissGuest.RoomNumber);
      Guest = edelweissGuest;
      base.ExecuteProcessingOperation(orderBuilder, token, cafeSession, cashier, orderSaver, changeProgressBarMessage);
    }

    private EdelweissGuest ShowGuestSelectPopup(out string errorMessage)
    {
      EdelweissGuest selectedGuest = null;
      IOrderedEnumerable<EdelweissGuest> guests = null;
      try
      {
        guests = EdelweissPluginContext.ServiceClient.GetGuests().OrderBy(i => i.RoomNumber).ThenBy(i => i.Name);
        errorMessage = null;
      }
      catch (EdelweissException ex)
      {
        Log.Error(ErrorGetGuests, ex);
        errorMessage = ex.Message;
      }
      if (guests != null)
        errorMessage = UiDispatcher.Execute<string>(() =>
        {
          try
          {
            selectedGuest = EdelweissViewManager.Instance.ShowGuestSelectPopup(guests);
            return null;
          }
          catch (EdelweissException ex)
          {
            Log.Error(ErrorGetGuests, ex);
            return ex.Message;
          }
        });
      if (selectedGuest != null && !selectedGuest.CanPayAmount(Sum))
      {
        errorMessage = EdelweissLocalResources.ResultCode5NotEnoughLimit;
        selectedGuest = null;
      }
      return selectedGuest;
    }

    private bool ShowSignatureConfirmation(
      ICashRegister cashRegister,
      IBaseOrder baseOrder,
      EdelweissGuest selectedGuest,
      out string message)
    {
      message = null;
      bool? nullable;
      bool flag;
      do
      {
        nullable = ServiceFactory.Instance.UserInteraction.SendConfirmRequest();
        flag = nullable.HasValue && !nullable.Value;
        if (flag)
          PrintHotelCheque(cashRegister, baseOrder, selectedGuest);
      }
      while (flag);
      return nullable.HasValue;
    }

    protected override EdelweissCardTransaction CreateTransaction(
      Resto.Data.CardTransactionType type,
      Guid sessionId)
    {
      return new EdelweissCardTransaction(DateTime.Now, NonCashType.PaymentSystem, type, sessionId, new decimal?(Sum), Card);
    }

    protected override void PerformTransaction(
      IBaseOrderBuilder orderBuilder,
      ILockedObjectsToken token,
      ICafeSession cafeSession,
      IUser cashier,
      Action<string> changeProgressBarMessage = null)
    {
      EdelweissPluginContext.ServiceClient.Pay(Transaction.Id, Guest, Sum, Transaction, deparmentCode);
    }

    protected override void PerformCancelTransaction(
      IBaseOrder order,
      EdelweissCardTransaction cancelTransaction,
      ICafeSession cafeSession,
      IUser cashier,
      Action<string> changeProgressBarMessage = null)
    {
      EdelweissPluginContext.ServiceClient.VoidTransaction(Transaction);
    }

    protected override void PerformStorningTransaction(
      IBaseOrder order,
      EdelweissCardTransaction stornoTransaction,
      ICafeSession cafeSession,
      IUser cashier,
      Action<string> changeProgressBarMessage = null)
    {
      try
      {
        EdelweissPluginContext.ServiceClient.VoidTransaction(Transaction);
      }
      finally
      {
        ServiceFactory.Instance.UserInteraction.SendWarningMessage(EdelweissLocalResources.InfoRemovePaymentManually, new TimeSpan?());
      }
    }

    public override void CompleteOrderTransaction(IOrderPaymentTransaction orderPaymentTransaction)
    {
      CardOrderTransaction orderTransaction = (CardOrderTransaction) orderPaymentTransaction;
      orderTransaction.CardNumber = card.RoomNumber;
      orderTransaction.CardTypeName = "edelweiss";
    }

    private static void PrintHotelCheque(
      ICashRegister register,
      IBaseOrder order,
      EdelweissGuest epitomeGuest)
    {
      UiDispatcher.Execute(() => PrintManager.PrintHotelCheque(register, new EdelweissBillCheque(order, epitomeGuest, GenerateChequeNumber(order))));
    }

    private static string GenerateChequeNumber(IBaseOrder order) => ServiceFactory.Instance.LocalDevices.DefaultCashRegister.Number.ToString() + "_" + ServiceFactory.Instance.CashServer.ServerInfo.CurrentCafeSessionNumber.ToString() + "_" + order.Number.ToString();

    protected override string CancelMessage => string.Format("{0}\r\n{1}",  CancelMessage, EdelweissLocalResources.InfoRemovePaymentManually);
  }
}
