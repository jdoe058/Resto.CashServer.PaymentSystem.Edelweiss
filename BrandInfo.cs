// Decompiled with JetBrains decompiler
// Type: BrandInfo
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using System.Collections.Generic;

internal static class BrandInfo
{
  public const string ServerInstance = "iikoServer";
  public const string OfficeAppWin = "iikoOffice";
  public const string Brand = "iiko";
  public const string ChainAppWin = "iikoChain";
  public const string OfficeAppWeb = "iikoWeb";
  public const string LoyaltyApp = "iikoCard";
  public const string Card5 = "iikoCard (old)";
  public const string LoyaltyAppCallCenter = "iikoCard5CallCenter";
  public const string CloudApi = "iikoTransport";
  public const string DriverApp = "iikoDeliveryMan";
  public const string WaiterApp = "iikoWaiter";
  public const string PosApp = "iikoFront";
  public const string DeliveryModule = "iikoDelivery";
  public const string FzModule = "iikoFranchise";
  public const string PosAgent = "iiko Agent";
  public const string CallCenterAppWin = "iikoCallCenter";
  public const string Plazius = "iiko.net";
  public const string DjAppWin = "iikoDJ";
  public const string CloudApiLegacy = "iiko.biz";
  public const string CloudApiLegacyUrl = "https://iiko.biz";
  public const string ExpertModule = "iikoExpert";
  public const string DeploymentCloud = "iikoCloud";
  public const string CallCenterAppWeb = "iikoCallCenter";
  public const string DocflowModule = "iikoDocFlow";
  public const string TroubleshooterModule = "iikoTroubleshooter";
  public const string TariffNano = "iikoNano";
  public const string TableService = "iikoTableService";
  public const string PosPbx = "iikoFrontPBX";
  public const string CheckOut = "iikoCheckOut";
  public const string Logistic = "iikoLogistic";
  public const string Evotor = "iikoEvotor";
  public const string ChainOperations = "iikoChainOperations";
  public const string Chain = "Chain";
  public const string Rms = "RMS";
  public const string MobileApp = "iikoTeam";
  public const string Crm = "iikoCRM";
  public const string MonitoringModule = "iikoWatchDog";
  public const string CopyrightFormat = "© 2005-{0} Resto software, inc.";
  public const string BrandWebsite = "https://iiko.ru";
  public const string BrandFzWebsite = "https://franchise.iiko.ru/";
  public const string PartnersUrl = "partners.iiko.ru";
  public const string TechSupportUrlFormat = "https://{0}/service.html?crmId={1}&salt={2}";
  public const string OfficeRssUrl = "https://integration.iiko.ru/v1/news/get";
  public const string LoyaltyAppPos = "iikoCard5 POS";
  public const bool ShowRealeaseNotesInOffice = true;
  public const bool ShowDocumentationInOffice = true;
  public const bool ShowChequeTemplateGallery = true;
  public static readonly Dictionary<string, string> ReplacementDict = new Dictionary<string, string>()
  {
    {
      "${SERVER_INSTANCE}",
      "iikoServer"
    },
    {
      "${OFFICE_APP_WIN}",
      "iikoOffice"
    },
    {
      "${BRAND}",
      "iiko"
    },
    {
      "${CHAIN_APP_WIN}",
      "iikoChain"
    },
    {
      "${OFFICE_APP_WEB}",
      "iikoWeb"
    },
    {
      "${LOYALTY_APP}",
      "iikoCard"
    },
    {
      "${CARD_5}",
      "iikoCard (old)"
    },
    {
      "${LOYALTY_APP_CALL_CENTER}",
      "iikoCard5CallCenter"
    },
    {
      "${CLOUD_API}",
      "iikoTransport"
    },
    {
      "${DRIVER_APP}",
      "iikoDeliveryMan"
    },
    {
      "${WAITER_APP}",
      "iikoWaiter"
    },
    {
      "${POS_APP}",
      "iikoFront"
    },
    {
      "${DELIVERY_MODULE}",
      "iikoDelivery"
    },
    {
      "${FZ_MODULE}",
      "iikoFranchise"
    },
    {
      "${POS_AGENT}",
      "iiko Agent"
    },
    {
      "${CALL_CENTER_APP_WIN}",
      "iikoCallCenter"
    },
    {
      "${PLAZIUS}",
      "iiko.net"
    },
    {
      "${DJ_APP_WIN}",
      "iikoDJ"
    },
    {
      "${CLOUD_API_LEGACY}",
      "iiko.biz"
    },
    {
      "${CLOUD_API_LEGACY_URL}",
      "https://iiko.biz"
    },
    {
      "${EXPERT_MODULE}",
      "iikoExpert"
    },
    {
      "${DEPLOYMENT_CLOUD}",
      "iikoCloud"
    },
    {
      "${CALL_CENTER_APP_WEB}",
      "iikoCallCenter"
    },
    {
      "${DOCFLOW_MODULE}",
      "iikoDocFlow"
    },
    {
      "${TROUBLESHOOTER_MODULE}",
      "iikoTroubleshooter"
    },
    {
      "${TARIFF_NANO}",
      "iikoNano"
    },
    {
      "${TABLE_SERVICE}",
      "iikoTableService"
    },
    {
      "${POS_PBX}",
      "iikoFrontPBX"
    },
    {
      "${CHECK_OUT}",
      "iikoCheckOut"
    },
    {
      "${LOGISTIC}",
      "iikoLogistic"
    },
    {
      "${EVOTOR}",
      "iikoEvotor"
    },
    {
      "${CHAIN_OPERATIONS}",
      "iikoChainOperations"
    },
    {
      "${CHAIN}",
      nameof (Chain)
    },
    {
      "${RMS}",
      "RMS"
    },
    {
      "${MOBILE_APP}",
      "iikoTeam"
    },
    {
      "${CRM}",
      "iikoCRM"
    },
    {
      "${MONITORING_MODULE}",
      "iikoWatchDog"
    },
    {
      "${COPYRIGHT_FORMAT}",
      "© 2005-{0} Resto software, inc."
    },
    {
      "${BRAND_WEBSITE}",
      "https://iiko.ru"
    },
    {
      "${BRAND_FZ_WEBSITE}",
      "https://franchise.iiko.ru/"
    },
    {
      "${PARTNERS_URL}",
      "partners.iiko.ru"
    },
    {
      "${TECH_SUPPORT_URL_FORMAT}",
      "https://{0}/service.html?crmId={1}&salt={2}"
    },
    {
      "${OFFICE_RSS_URL}",
      "https://integration.iiko.ru/v1/news/get"
    },
    {
      "${LOYALTY_APP_POS}",
      "iikoCard5 POS"
    }
  };
}
