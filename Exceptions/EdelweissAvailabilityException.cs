﻿// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Exceptions.EdelweissAvailabilityException
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using System;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Exceptions
{
  public class EdelweissAvailabilityException : EdelweissException
  {
    public EdelweissAvailabilityException(string message)
      : base(message)
    {
    }

    public EdelweissAvailabilityException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
