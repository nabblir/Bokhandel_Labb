using Bokhandel_Labb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bokhandel_Labb.Commands
{
    public class Logger
        {
        private readonly BokhandelContext _context;

        public Logger(BokhandelContext context)
            {
            _context = context;
            }

        public static bool LoggaHändelse(BokhandelContext context, string user, string butiksnamn, int butikId, string händelse, string logTyp)
            {
            try
                {
                var logg = new LoggHistorik
                    {
                    //LoggId = context.LoggHistorik.Max(l => l.LoggId) + 1,
                    User = user,
                    Butiksnamn = butiksnamn,
                    ButikId = butikId,
                    Datum = DateTime.Now,
                    Händelse = händelse,
                    LogTyp = logTyp
                    };

                context.LoggHistorik.Add(logg);
                context.SaveChanges();
                return true;
                }
            catch
                {
                return false;
                }
            }
        }
    public class LoggTextConverter : IMultiValueConverter
        {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
            if (values[0] is string logTyp && values[1] is string händelse)
                {
                // Begränsa händelse till max 60 tecken
                string kortHändelse = händelse.Length > 60
                    ? händelse.Substring(0, 60) + "..."
                    : händelse;

                return $"{logTyp} {kortHändelse}";
                }

            return string.Empty;
            }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
            throw new NotImplementedException();
            }
        }
    }
