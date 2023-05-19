// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Views.GuestsSearchScreenState
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using Resto.CashServer.PaymentSystem.Edelweiss.Data;
using Resto.CashServer.Services.Search;
using Resto.Framework.Common.Search;
using Resto.Front.Common.MVCFramework.ModelInterfaces.Search;
using Resto.Front.Controllers.Core.Interfaces;
using Resto.Front.Controllers.Interfaces;
using Resto.Front.Localization.Resources;
using Resto.Front.Main.MVC.States.SearchStates;
using System;
using System.Collections.Generic;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Views
{
  internal sealed class GuestsSearchScreenState : SearchState<EdelweissGuest, GuestSearchModel>
  {
    public GuestsSearchScreenState(
      IEnumerable<EdelweissGuest> itemsSource,
      SearchAlgorithm<EdelweissGuest> searchAlgorithmFreezed,
      Func<StringSearchModel<EdelweissGuest>, string> getStringSearchDetailedInfo)
      : base(itemsSource, searchAlgorithmFreezed, getStringSearchDetailedInfo, false, string.Empty, (SearchCustomersType) 4)
    {
    }

    protected override IStringSearchModel<EdelweissGuest, GuestSearchModel> CreateSearchModel(
      IEnumerable<EdelweissGuest> itemsSource,
      SearchAlgorithm<EdelweissGuest> searchAlgorithm,
      Func<GuestSearchModel, string> getStringSearchDetailedInfo)
    {
      return ModelFactory.GetStringSearch(itemsSource, searchAlgorithm, false, false, getStringSearchDetailedInfo, new Func<SearchMatches<EdelweissGuest>, 
          GuestSearchModel>(CreateSearchResult), EdelweissLocalResources.RequestGuest, null, 2, null, false, false, false, true, null);
    }

    protected override IDisposable GetSearchCustomButtonController(
      IPopup popup,
      IStringSearchModel<EdelweissGuest, GuestSearchModel> model,
      IUpdatableSearchController updatableController)
    {
      return null;
    }

    private static GuestSearchModel CreateSearchResult(SearchMatches<EdelweissGuest> matches) => new GuestSearchModel(matches);
  }
}
