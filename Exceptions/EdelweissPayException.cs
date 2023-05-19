// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Exceptions.EdelweissPayException
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using Resto.Front.Localization.Resources;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Exceptions
{
  public class EdelweissPayException : EdelweissException
  {
    public const int CODE_NONE = -1;
    public const int CODE_SUCCESS = 0;

    public int ResultCode { get; set; }

    public EdelweissPayException(int errorCode)
      : base(ErrorCodeToString(errorCode))
    {
      ResultCode = errorCode;
    }

    public static string ErrorCodeToString(int errorCode)
    {
      switch (errorCode)
      {
        case 0:
          return EdelweissLocalResources.ResultCode0Success;
        case 1:
          return EdelweissLocalResources.ResultCode1NotUnique;
        case 2:
          return EdelweissLocalResources.ResultCode2DayClosing;
        case 3:
          return EdelweissLocalResources.ResultCode3NoService;
        case 4:
          return EdelweissLocalResources.ResultCode4WrongDepartment;
        case 5:
          return EdelweissLocalResources.ResultCode5NotEnoughLimit;
        default:
          return EdelweissLocalResources.ResultCodeUnknown;
      }
    }
  }
}
