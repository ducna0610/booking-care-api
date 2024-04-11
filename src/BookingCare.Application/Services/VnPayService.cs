using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Text;
using BookingCare.Application.DTOs.Responses.Payment;
using BookingCare.Application.DTOs.Requests.Payment;
using BookingCare.Application.Repositories;
using BookingCare.Domain.Enums;
using BookingCare.Domain.Entities;
using System.Linq.Expressions;
using BookingCare.Application.Helpers;

namespace BookingCare.Application.Services
{
    public interface IVnPayService
    {
        Task<PaymentResponse> CreatePaymentUrl(PaymentRequest request, HttpContext context);
        PaymentReturnResponse PaymentExecuteReturn(VnPayRequest request);
        Task<VnpayPayIpnResponse> PaymentExecuteIpn(VnPayRequest request);
    }

    public class VnPayService : IVnPayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IBookingRepository _bookingRepository;

        public VnPayService
            (
                IUnitOfWork unitOfWork,
                IConfiguration configuration,
                IBookingRepository bookingRepository
            )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _bookingRepository = bookingRepository;
        }

        public async Task<PaymentResponse> CreatePaymentUrl(PaymentRequest request, HttpContext context)
        {
            Expression<Func<Booking, bool>> FilterById()
            {
                return x => x.Id == request.BookingId;
            }

            var filters = new List<Expression<Func<Booking, bool>>>();
            filters.Add(FilterById());

            Expression<Func<Booking, object>> schedule = x => x.Schedule;

            var includes = new List<Expression<Func<Booking, object>>>();
            includes.Add(schedule);

            var booking = await _bookingRepository.FirstOrDefaultAsync(filters, includes);
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var urlCallBack = _configuration["Payment:Vnpay:ReturnUrl"];
            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _configuration["Payment:Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Payment:Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Payment:Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)booking.Schedule.Price * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Payment:Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Payment:Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"THANH TOAN HOA DON DAT LICH KHAM");
            pay.AddRequestData("vnp_OrderType", "270001");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", booking.Id + "_" + tick);

            var paymentUrl = pay.CreateRequestUrl(_configuration["Payment:Vnpay:BaseUrl"], _configuration["Payment:Vnpay:HashSecret"]);
            return new PaymentResponse { Url = paymentUrl };
        }

        public PaymentReturnResponse PaymentExecuteReturn(VnPayRequest request)
        {
            var resultData = new PaymentReturnResponse();
            var isValidSignature = request.IsValidSignature(_configuration["Payment:Vnpay:HashSecret"]);

            if (!isValidSignature)
            {
                resultData.Success = false;
                return resultData;
            }

            resultData.OrderInfo = request.vnp_OrderInfo;
            resultData.OrderId = request.vnp_TxnRef;
            resultData.Amount = (int)request.vnp_Amount / 100;
            resultData.Success = true;
            return resultData;
        }

        public async Task<VnpayPayIpnResponse> PaymentExecuteIpn(VnPayRequest request)
        {
            var resultData = new VnpayPayIpnResponse();

            try
            {
                var isValidSignature = request.IsValidSignature(_configuration["Payment:Vnpay:HashSecret"]);

                if (isValidSignature)
                {
                    Expression<Func<Booking, bool>> FilterById()
                    {
                        var bookingId = Guid.Parse(request.vnp_TxnRef.Split("_")[0]);
                        return x => x.Id == bookingId;
                    }

                    var filters = new List<Expression<Func<Booking, bool>>>();
                    filters.Add(FilterById());

                    Expression<Func<Booking, object>> schedule = x => x.Schedule;

                    var includes = new List<Expression<Func<Booking, object>>>();
                    includes.Add(schedule);

                    var booking = await _bookingRepository.FirstOrDefaultAsync(filters, includes);

                    if (booking != null)
                    {
                        if (booking.Status != StatusEnum.COMPLETED)
                        {
                            if (booking.Schedule.Price == (request.vnp_Amount / 100))
                            {
                                booking.Status = StatusEnum.COMPLETED;
                                await _unitOfWork.ExecuteTransactionAsync(() =>
                                {
                                    _bookingRepository.Update(booking);
                                });

                                resultData.Set("00", "Confirm success");
                            }
                            else
                            {
                                resultData.Set("04", "Invalid amount");
                            }
                        }
                        else
                        {
                            resultData.Set("02", "Order already confirmed");
                        }
                    }
                    else
                    {
                        resultData.Set("01", "Order not found");
                    }
                }
                else
                {
                    resultData.Set("97", "Invalid signature");
                }
            }
            catch (Exception ex)
            {
                /// TODO: process when exception
                resultData.Set("99", "Input required data");
            }

            return resultData;
        }
    }

    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public string GetIpAddress(HttpContext context)
        {
            var ipAddress = "127.0.0.1";
            var remoteIpAddress = context.Connection.RemoteIpAddress;

            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                        .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                }

                if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();
            }

            return ipAddress;

        }
        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString();

            baseUrl += "?" + querystring;
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }

            var vnpSecureHash = HashHelper.HmacSHA512(vnpHashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;

            return baseUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var myChecksum = HashHelper.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }

            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }

            return data.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
