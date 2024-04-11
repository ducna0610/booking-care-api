namespace BookingCare.Application.Utils;

public interface IExcelService
{
    Task<string> ExportAsync<T>(IEnumerable<T> data, string sheetName = "Sheet1");

    Task<IEnumerable<T>> ImportAsync<T>(string fileName, string sheetName = "Sheet1");
}