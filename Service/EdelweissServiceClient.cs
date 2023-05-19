// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Service.EdelweissServiceClient
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using log4net;
using Microsoft.Practices.Unity;
using Resto.CashServer.PaymentSystem.Edelweiss.Data;
using Resto.CashServer.PaymentSystem.Edelweiss.Exceptions;
using Resto.CashServer.PaymentSystem.Edelweiss.Settings;
using Resto.CashServer.Services;
using Resto.CashServer.Services.Data;
using Resto.CashServer.Sync;
using Resto.Framework.Common;
using Resto.Front.Localization.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Service
{
  public class EdelweissServiceClient : IDisposable
  {
    private static readonly string ErrorServiceFailedFormat = EdelweissLocalResources.ServiceFailedErrorTemplate;
    private readonly object sync = new object();

    private static IEdelweissService GetService()
    {
      EdelweissSettingsLocator instance = EdelweissSettingsLocator.Instance;
      instance.UpdateSettings();
      return instance.IsPrimaryTerminal ? UnityContainerExtensions.Resolve<IEdelweissService>((IUnityContainer) UnityHelper.GetFactoryContainer(), Array.Empty<ResolverOverride>()) : ServiceCaller.CreateChannel<IEdelweissService>(ServiceFactory.Instance.Networking.GetAgentAddress((IAgent) instance.Agent) ?? instance.Agent.Addresses.First<string>());
    }

    private static ILog Log => LogFactory.Instance.GetLogger(typeof (EdelweissServiceClient));

    public IEnumerable<EdelweissGuest> GetGuests()
    {
      IEnumerable<EdelweissGuest> guests = (IEnumerable<EdelweissGuest>) null;
      this.ExecuteServiceAction((Action) (() => guests = (IEnumerable<EdelweissGuest>) EdelweissServiceClient.GetService().GetGuests()), (Action<string>) (errorString =>
      {
        throw new EdelweissAvailabilityException(errorString);
      }));
      return guests;
    }

    public void Pay(
      Guid requestId,
      EdelweissGuest guest,
      Decimal sum,
      EdelweissCardTransaction transaction,
      int deparmentCode)
    {
      this.ExecuteServiceAction((Action) (() =>
      {
        string name = ServiceFactory.Instance.Users.ActiveSession.User.Name;
        transaction.SetCompleted(DateTime.Now, new int?(EdelweissServiceClient.GetService().Pay(requestId, guest, sum, name, deparmentCode)));
      }), (Action<string>) (errorString =>
      {
        throw new RestoException(errorString);
      }));
    }

    public void VoidTransaction(EdelweissCardTransaction transaction) => transaction.SetCompleted(DateTime.Now, new int?());

    private void ExecuteServiceAction(Action actionProcess, Action<string> actionFailed)
    {
      string error = "";
      try
      {
        this.ExecuteAndHandleExceptions(actionProcess, ref error);
      }
      catch (Exception ex)
      {
        EdelweissServiceClient.Log.Error((object) string.Format(EdelweissServiceClient.ErrorServiceFailedFormat, (object) error), ex);
        if (actionFailed == null)
          return;
        actionFailed(error);
      }
    }

    private void ExecuteAndHandleExceptions(Action actionProcess, ref string error)
    {
      try
      {
        lock (this.sync)
          actionProcess();
      }
      catch (EdelweissAvailabilityException ex)
      {
        error = ex.Message;
        throw;
      }
      catch (FaultException<EdelweissFault> ex)
      {
        error = ex.Detail.Message;
        throw;
      }
      catch (FaultException ex)
      {
        error = EdelweissLocalResources.ErrorServiceFailedUnknown;
        throw;
      }
      catch (CommunicationException ex)
      {
        error = string.Format(EdelweissLocalResources.ErrorServiceConnection, (object) ex.Message);
        throw;
      }
      catch (TimeoutException ex)
      {
        error = EdelweissLocalResources.ErrorServiceTimeout;
        throw;
      }
    }

    public void Dispose()
    {
    }
  }
}
