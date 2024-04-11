using BookingCare.Application.Utils;
using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace BookingCare.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IConfiguration _configuration;

        public ExcelService
            (
                IConfiguration configuration
            )
        {
            _configuration = configuration;
        }

        public async Task<string> ExportAsync<T>(IEnumerable<T> data, string sheetName = "Sheet1")
        {
            var fileName = "";
            using (IXLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(sheetName).FirstCell().InsertTable<T>(data);

                fileName = string.Concat("export_", typeof(T).Name, "_", Guid.NewGuid().ToString(), ".xlsx");
                var filePath = Path.Combine(_configuration["Folder:Upload"], fileName);
                workbook.SaveAs(filePath);
            }


            var url = _configuration["ApiUrl"] + '/' + "files" + '/' + fileName;
            return url;
        }

        public async Task<IEnumerable<T>> ImportAsync<T>(string fileName, string sheetName = "Sheet1")
        {
            List<T> list = new List<T>();
            Type typeOfObject = typeof(T);

            var filePath = Path.Combine(_configuration["Folder:Upload"], fileName);

            using (IXLWorkbook workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheets.Where(w => w.Name == sheetName).First();
                var props = typeOfObject.GetProperties();

                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                {
                    T obj = (T)Activator.CreateInstance(typeOfObject);
                    for (int i = 0; i < props.Length; i++)
                    {
                        var val = row.Cell(i + 1).Value;
                        var type = props[i].PropertyType;
                        props[i].SetValue(obj, TypeDescriptor.GetConverter(type).ConvertFromInvariantString(val.ToString()));
                    }

                    list.Add(obj);
                }
            }

            return list;
        }
    }
}