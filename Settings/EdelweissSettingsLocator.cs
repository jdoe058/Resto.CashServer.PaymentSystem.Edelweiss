// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Settings.EdelweissSettingsLocator
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using log4net;
using Resto.CashServer.Agent;
using Resto.CashServer.Data;
using Resto.CashServer.Data.Entities.CashServerEntities;
using Resto.CashServer.PaymentSystem.Edelweiss.Exceptions;
using Resto.CashServer.Services;
using Resto.Data;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.Front.Localization.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using AgentDevice = Resto.CashServer.Data.AgentDevice;
using CardProcessingDevice = Resto.CashServer.Data.Entities.CashServerEntities.CardProcessingDevice;
using Terminal = Resto.CashServer.Data.Terminal;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Settings
{
  internal class EdelweissSettingsLocator
  {
    private static ILog Log => LogFactory.Instance.GetLogger(typeof (EdelweissSettingsLocator));

    private EdelweissDriver Driver { get; set; }

    public Resto.CashServer.Data.Agent Agent { get; private set; }

    public static EdelweissSettingsLocator Instance { get; } = new EdelweissSettingsLocator();

    public EdelweissDriverSettings Settings => this.Driver?.EdelweissDriverSettings;

    public bool IsPrimaryTerminal => ((Entity) this.Agent).Id == AgentWrapper.EmbeddedAgentId;

    public void UpdateSettings()
    {
      this.UpdateDriverSettings();
      this.UpdateAgentSettings();
    }

    private void UpdateDriverSettings()
    {
      IReadOnlyList<CardProcessingDevice> allNotDeleted = ServiceFactory.Instance.ServerEntities.GetAllNotDeleted<CardProcessingDevice>((Func<CardProcessingDevice, bool>) (device => device.Driver is EdelweissDriver));
      CardProcessingDevice processingDevice;
      try
      {
        processingDevice = allNotDeleted.SingleOrDefault<CardProcessingDevice>();
      }
      catch (InvalidOperationException ex)
      {
        throw new EdelweissAvailabilityException(EdelweissLocalResources.ErrorMultipleDriver, (Exception) ex);
      }
      this.Driver = (EdelweissDriver) processingDevice?.Driver;
      if (this.Driver == null)
        throw new EdelweissAvailabilityException(EdelweissLocalResources.ErrorNoDriver);
      if (this.Settings == null)
        throw new EdelweissAvailabilityException(EdelweissLocalResources.ErrorNoSettings);
      EdelweissSettingsLocator.Log.InfoFormat("Connection: '{0}'", (object) this.Settings.ToConnectionString());
    }

    private void UpdateAgentSettings()
    {
      Terminal localTerminal = (Terminal) ServiceFactory.Instance.LocalDevices.LocalTerminal;
      if (((ServerEntity) localTerminal).Deleted)
        throw new EdelweissAvailabilityException(EdelweissLocalResources.ErrorTerminalNotFound);
      this.Agent = ServiceFactory.Instance.ServerEntities.TryGet<Resto.CashServer.Data.Agent>(((AgentDevice) localTerminal).AgentId) ?? throw new EdelweissAvailabilityException(EdelweissLocalResources.ErrorAgentNotFound);
    }
  }
}
