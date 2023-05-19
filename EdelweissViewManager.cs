// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.EdelweissViewManager
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using Resto.CashServer.PaymentSystem.Edelweiss.Data;
using Resto.CashServer.PaymentSystem.Edelweiss.Views;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;
using Resto.Framework.Common.ObjectPaths;
using Resto.Framework.Common.Search;
using Resto.Front.Common.MVCFramework.ModelInterfaces.Search;
using Resto.Front.Controllers.Core;
using Resto.Front.Localization.Resources;
using Resto.Front.Main.MVC;
using Resto.Front.Main.MVC.States;
using System;
using System.Collections.Generic;

namespace Resto.CashServer.PaymentSystem.Edelweiss
{
  internal sealed class EdelweissViewManager
  {
    private static EdelweissViewManager instance;
    private readonly SearchAlgorithm<EdelweissGuest> guestSearchAlgorithm = new SearchAlgorithm<EdelweissGuest>()
            .AddProperty(o => o.Name, PropertySearchMode.StringInclude)
            .AddProperty(o => o.RoomNumber, PropertySearchMode.StringInclude)
            .AddProperty(o => o.CardNumber, PropertySearchMode.StringInclude)
            .Freeze();
    
    public static EdelweissViewManager Instance => instance ?? (instance = new EdelweissViewManager());

    private static ViewManager ViewManager => (ViewManager) Controller.ViewManager;

    public EdelweissGuest ShowGuestSelectPopup(IEnumerable<EdelweissGuest> guests) => ViewManager.ShowPopupWithResult(
        new GuestsSearchScreenState(guests, guestSearchAlgorithm, g => GetDetailInfo(g.Matches.Object)))
            .GetValueOrDefault()?.Object;

    private static string GetDetailInfo(EdelweissGuest guest) => string.Format(
        EdelweissLocalResources.GuestInfoFormat, 
        guest.Name, guest.RoomNumber, 
        CurrencyHelper.MoneyToString(guest.CreditLimit), 
        guest.BeginDate, 
        guest.EndDate, 
        guest.TypeString);
  }
}
