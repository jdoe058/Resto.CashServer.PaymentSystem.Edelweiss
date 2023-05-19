// Decompiled with JetBrains decompiler
// Type: Resto.CashServer.PaymentSystem.Edelweiss.Service.EdelweissService
// Assembly: Resto.CashServer.PaymentSystem.Edelweiss, Version=8.4.6018.0, Culture=neutral, PublicKeyToken=null
// MVID: F6FED1E7-EF0D-433E-B7CE-44E6A876C850
// Assembly location: C:\Users\Zheka\Desktop\Plugin.Front.Edelweiss\Resto.CashServer.PaymentSystem.Edelweiss.dll

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql.SqlProvider;
using log4net;
using Resto.CashServer.PaymentSystem.Edelweiss.Data;
using Resto.CashServer.PaymentSystem.Edelweiss.Exceptions;
using Resto.CashServer.PaymentSystem.Edelweiss.Settings;
using Resto.Data;
using Resto.Framework.Common;
using Resto.Front.Localization.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

using DataException = BLToolkit.Data.DataException;

namespace Resto.CashServer.PaymentSystem.Edelweiss.Service
{
  public sealed class EdelweissService : IEdelweissService
  {
    private static readonly string ErrorPay = EdelweissLocalResources.PayError;
    private static readonly string ErrorGetGuests = EdelweissLocalResources.GetGuestsError;
    private static readonly string[] WrongDepartCodeExceptionSymptoms = new string[2]
    {
      "[23000]",
      "ArtikelID"
    };
    private static readonly Regex ReNiceOdbcError = new Regex("\\[Sybase\\]\\[ODBC Driver\\]\\[Adaptive Server Anywhere\\](.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private readonly object sync = new object();

    private static EdelweissSettingsLocator EdelweissSettings => EdelweissSettingsLocator.Instance;

    private static ILog Log => LogFactory.Instance.GetLogger(typeof (EdelweissService));

    public List<EdelweissGuest> GetGuests() => this.ExecuteAndHandleExceptions(ErrorGetGuests, new Func<List<EdelweissGuest>>(GetGuestsInternal), new int?());

    private static List<EdelweissGuest> GetGuestsInternal()
    {
      using (EdelweissDbManager edelweissDbManager = new EdelweissDbManager(EdelweissSettings.Settings))
      {
        Log.InfoFormat("Connection state {0}", edelweissDbManager.Connection.State);
        return edelweissDbManager.SetCommand("SELECT GuestAccountID, RoomID, RoomNumber, GuestName, CardNumber, CreditLimit, BeginDate, EndDate, Type\r\n                                  FROM Admin.TillyPad_Guests").ExecuteList<EdelweissGuest>();
      }
    }

    public int Pay(
      Guid requestId,
      EdelweissGuest guest,
      Decimal sum,
      string cashier,
      int departmentCode)
    {
      return ExecuteAndHandleExceptions(ErrorPay, (() => PayInternal(requestId, guest, sum, cashier, departmentCode)), new int?(departmentCode));
    }

    private static int PayInternal(
      Guid requestId,
      EdelweissGuest guest,
      Decimal sum,
      string cashier,
      int departmentCode)
    {
      using (EdelweissDbManager edelweissDbManager = new EdelweissDbManager(EdelweissSettings.Settings))
      {
        Log.InfoFormat("Connection state {0}", edelweissDbManager.Connection.State);
        Log.InfoFormat("INSERT INTO Admin.TillyPad_Import (AccountID, OrderTime, OrderSum, Note, WaiterName, DepartCode, OrigOrderSum, TPadGUID) \r\n                                  VALUES ({0}, GETDATE(), {1}, {2}, {3}, {4}, {5}, {6})", new object[7]
        {
          guest.AccountId,
          sum,
          EdelweissLocalResources.IikoRMSPayment,
          cashier,
          departmentCode,
          sum,
          GuidToString(requestId)
        });
        edelweissDbManager.SetCommand("INSERT INTO Admin.TillyPad_Import (AccountID, OrderTime, OrderSum, Note, WaiterName, DepartCode, OrigOrderSum, TPadGUID) \r\n                                  VALUES (?, GETDATE(), ?, ?, ?, ?, ?, ?)", new IDbDataParameter[7]
        {
          edelweissDbManager.Parameter("@AccountID", guest.AccountId),
          edelweissDbManager.Parameter("@OrderSum", sum),
          edelweissDbManager.Parameter("@Note", EdelweissLocalResources.IikoRMSPayment),
          edelweissDbManager.Parameter("@WaiterName", cashier),
          edelweissDbManager.Parameter("@DepartCode", departmentCode),
          edelweissDbManager.Parameter("@OrigOrderSum", sum),
          edelweissDbManager.Parameter("@Guid", GuidToString(requestId))
        }).ExecuteNonQuery();
        edelweissDbManager.SetCommand("SELECT Id, ResCode FROM Admin.TillyPad_Import WHERE TPadGUID = ?", new IDbDataParameter[1]
        {
          edelweissDbManager.Parameter("@Guid", GuidToString(requestId))
        });
        using (IDataReader dataReader = edelweissDbManager.ExecuteReader(CommandBehavior.SingleRow))
        {
          dataReader.Read();
          int int32_1 = dataReader.GetInt32(dataReader.GetOrdinal("Id"));
          int int32_2 = dataReader.GetInt32(dataReader.GetOrdinal("ResCode"));
          if (int32_2 != 0)
            throw new EdelweissPayException(int32_2);
          return int32_1;
        }
      }
    }

    private T ExecuteAndHandleExceptions<T>(
      string errorLog,
      Func<T> actionProcess,
      int? departmentCode)
    {
      try
      {
        lock (sync)
        {
          EdelweissSettings.UpdateSettings();
          return actionProcess();
        }
      }
      catch (EdelweissAvailabilityException ex)
      {
        throw GetFaultException(errorLog, ex, new EdelweissFault()
        {
          Message = ex.Message
        });
      }
      catch (EdelweissPayException ex)
      {
        throw GetFaultException(errorLog, ex, new EdelweissFault()
        {
          Message = ex.Message,
          ResultCode = ex.ResultCode
        });
      }
      catch (DataException ex)
      {
        if (ex.InnerException is OdbcException)
        {
          string str = WrongDepartCodeExceptionSymptoms.All(new Func<string, bool>(ex.Message.Contains)) ? string.Format(EdelweissLocalResources.ErrorWrongDepartCode, departmentCode.GetValueOrDefault()) : string.Format(EdelweissLocalResources.ErrorDatabaseCommand, EdelweissDbManager.FormatOdbcException(ex));
          throw GetFaultException(errorLog + " " + str, (Exception) ex, new EdelweissFault()
          {
            Message = str
          });
        }
        throw GetFaultException(errorLog, ex, new EdelweissFault()
        {
          Message = EdelweissLocalResources.ErrorUnknown
        });
      }
      catch (Exception ex)
      {
        throw GetFaultException(errorLog, ex, new EdelweissFault()
        {
          Message = EdelweissLocalResources.ErrorUnknown
        });
      }
    }

    private static FaultException<EdelweissFault> GetFaultException(
      string message,
      Exception exception,
      EdelweissFault fault)
    {
      Log.Error((object) message, exception);
      return new FaultException<EdelweissFault>(fault);
    }

    private static string GuidToString(Guid guid) => guid.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture);

    private sealed class EdelweissDbDataProvider : OdbcDataProvider
    {
      public override ISqlProvider CreateSqlProvider() => (ISqlProvider) new SybaseSqlProvider();
    }

    private sealed class EdelweissDbManager : DbManager
    {
      public EdelweissDbManager(EdelweissDriverSettings edelweissDriverSettings)
        : base(new EdelweissDbDataProvider(), GetConnectionString(edelweissDriverSettings))
      {
      }

      protected override IDbCommand OnInitCommand(IDbCommand command)
      {
        IDbCommand dbCommand = base.OnInitCommand(command);
        dbCommand.CommandTimeout = 0;
        return dbCommand;
      }

      protected override void OnOperationException(OperationType op, DataException ex)
      {
        if (op == null)
        {
          Log.ErrorFormat(FormatOdbcException(ex), Array.Empty<object>());
          throw new EdelweissAvailabilityException(EdelweissLocalResources.ErrorDatabaseConnect, ex);
        }
        base.OnOperationException(op, ex);
      }

      private static string GetConnectionString(EdelweissDriverSettings settings) => 
                string.Format("Driver={{Adaptive Server Anywhere 9.0}};Eng={0};Links=tcpip(Host={1});Uid={2};Pwd={3}", 
                    settings.EdelDb, settings.EdelServer, settings.Login, settings.Password);

      internal static string FormatOdbcException(DataException dataException)
      {
        StringBuilder messageBuilder = new StringBuilder();
        OdbcException innerException = (OdbcException) ((Exception) dataException).InnerException;
        EnumerableEx.ForEach(EnumerableEx.Distinct(innerException.Errors.Cast<OdbcError>(), (error => error.Message)).Select(error =>
        {
            Match match = ReNiceOdbcError.Match(error.Message);
            return !match.Success ? error.Message : match.Groups[1].Value;
        }).ToArray(), e => messageBuilder.AppendLine(e));
        messageBuilder.AppendFormat("Source: {0}", innerException.Source).AppendLine().AppendFormat("Initial ConnectionString: {0}", GetConnectionString(EdelweissSettings.Settings)).AppendLine();
        return messageBuilder.ToString();
      }
    }
  }
}
