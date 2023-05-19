// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Data.EdelweissGuest
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using BLToolkit.Mapping;
using Resto.Front.Localization.Resources;
using System;
using System.ComponentModel;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Data
{
  public sealed class EdelweissGuest
  {
    private int accountId;
    private int roomId;
    private string roomNumber;
    private string name;
    private string cardNumber;
    private Decimal creditLimit;
    private DateTime beginDate;
    private DateTime endDate;
    private EdelweissGuestType type;

    [MapField("GuestAccountID")]
    public int AccountId
    {
      get => accountId;
      set => accountId = value;
    }

    public int RoomId
    {
      get => roomId;
      set => roomId = value;
    }

    public string RoomNumber
    {
      get => roomNumber;
      set => roomNumber = value;
    }

    [MapField("GuestName")]
    public string Name
    {
      get => name;
      set => name = value;
    }

    public string CardNumber
    {
      get => cardNumber;
      set => cardNumber = value;
    }

    public Decimal CreditLimit
    {
      get => creditLimit;
      set => creditLimit = value;
    }

    public DateTime BeginDate
    {
      get => beginDate;
      set => beginDate = value;
    }

    public DateTime EndDate
    {
      get => endDate;
      set => endDate = value;
    }

    public EdelweissGuestType Type
    {
      get => type;
      set => type = value;
    }

    public string TypeString
    {
      get
      {
        switch (Type)
        {
          case EdelweissGuestType.Guest:
            return EdelweissLocalResources.GuestTypeGuest;
          case EdelweissGuestType.Group:
            return EdelweissLocalResources.GuestTypeGroup;
          case EdelweissGuestType.Internal:
            return EdelweissLocalResources.GuestTypeInternal;
          default:
            throw new InvalidEnumArgumentException("Type");
        }
      }
    }

    public bool CanPayAmount(Decimal amount) => Type == EdelweissGuestType.Internal || amount <= CreditLimit;
  }
}
