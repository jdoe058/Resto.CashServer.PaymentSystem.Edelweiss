// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.EdelweissPluginContext
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using log4net;
using Microsoft.Practices.Unity;
using Resto.CashServer.PaymentSystem.Edelweiss.Cards;
using Resto.CashServer.PaymentSystem.Edelweiss.Service;
using Resto.CashServer.Plugins;
using Resto.CashServer.Services;
using Resto.Framework.Common;
using Resto.Framework.Data;
using System;

namespace Resto.CashServer.PaymentSystem.Edelweiss
{
  public sealed class EdelweissPluginContext : ICashServerPluginContext, IDisposable
  {
    public const string EdelweissPaymentSystemName = "edelweiss";

    private static ILog Log => LogFactory.Instance.GetLogger(typeof (EdelweissPluginContext));

    public static EdelweissServiceClient ServiceClient { get; private set; }

    void ICashServerPluginContext.InitContext(PluginEnvironment pluginEnvironment)
    {
      EntitiesRegistryBase.AddTypeWithFullName<EdelweissPaymentItem>();
      EntitiesRegistryBase.AddTypeWithFullName<EdelweissCard>();
      EntitiesRegistryBase.AddTypeWithFullName<EdelweissCardTransaction>();
      if (Environment.Is64BitProcess)
      {
        EdelweissPluginContext.Log.Error("Edelweiss plugin can be run from 32-bit process only.");
      }
      else
      {
        if (PluginEnvironmentHelper.IsCashServerExists(pluginEnvironment))
        {
          UnityContainerExtensions.RegisterInstance(UnityHelper.GetFactoryContainer(), typeof (IEdelweissService),new EdelweissService());
          ServiceFactory.Instance.PluginsConfig.RegisterService(typeof (IEdelweissService));
          ServiceClient = new EdelweissServiceClient();
        }
        NonCashPaymentType.RegisterExternalPaymentType("edelweiss", new EdelweissPaymentType());
      }
    }

    void ICashServerPluginContext.AfterServerStarted() {}

    public void AfterModelsCreated(IServiceProvider serviceProvider)    {  }

    void IDisposable.Dispose()
    {
      if (ServiceClient == null)
        return;
      ServiceClient.Dispose();
    }
  }
}
