// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Views.GuestSearchModel
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using Resto.CashServer.PaymentSystem.Edelweiss.Data;
using Resto.Framework.Common.Currency;
using Resto.Framework.Common.Search;
using Resto.Front.Common.MVCFramework.ModelInterfaces.Search;
using Resto.Front.Localization.Resources;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Views
{
  public sealed class GuestSearchModel : StringSearchModel<EdelweissGuest>
  {
    private readonly string additionalInfo;

    public GuestSearchModel(SearchMatches<EdelweissGuest> matches)
      : base(matches)
    {
      EdelweissGuest edelweissGuest = matches.Object;
      additionalInfo = string.Format(EdelweissLocalResources.AdditionalInfoFormat, edelweissGuest.RoomNumber, 
          CurrencyHelper.MoneyToString(edelweissGuest.CreditLimit), 
          edelweissGuest.BeginDate, edelweissGuest.EndDate, edelweissGuest.TypeString);
    }

    public override string AdditionalInfo => additionalInfo;

    public override string SearchDisplayName => Matches.Object.Name;

    public override bool IsHighlighted => false;

    public override string AdditionalBottomInfo => null;
  }
}
