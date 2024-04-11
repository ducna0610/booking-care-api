using BookingCare.Application.DTOs.Requests.Payment;
using BookingCare.Application.DTOs.Responses.Payment;
using BookingCare.Application.Helpers;
using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace BookingCare.Application.Services
{
    public interface IMomoService
    {
        Task<PaymentResponse> CreatePaymentUrl(PaymentRequest request);
        PaymentReturnResponse PaymentExecuteReturn(MomoRequest request);
        Task PaymentExecuteIpn(MomoRequest request);
    }

    public class MomoService : IMomoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IBookingRepository _bookingRepository;

        public MomoService
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

        public async Task<PaymentResponse> CreatePaymentUrl(PaymentRequest request)
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
            var tick = DateTime.Now.Ticks.ToString();

            string endpoint = _configuration["Payment:Momo:PaymentUrl"];
            string partnerCode = _configuration["Payment:Momo:PartnerCode"];
            string accessKey = _configuration["Payment:Momo:AccessKey"];
            string serectkey = _configuration["Payment:Momo:SecretKey"];
            string orderInfo = "THANH TOAN HOA DON DAT LICH KHAM";
            string redirectUrl = _configuration["Payment:Momo:ReturnUrl"];
            string ipnUrl = _configuration["Payment:Momo:IpnUrl"];
            string requestType = "captureWallet";

            string amount = ((int)booking.Schedule.Price).ToString();
            string orderId = booking.Id + "_" + tick;
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType;

            // sign signature SHA256
            string signature = HashHelper.HmacSHA256(rawHash, serectkey);

            // build body json request
            JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "partnerName", "Test" },
                    { "storeId", "MomoTestStore" },
                    { "requestId", requestId },
                    { "amount", amount },
                    { "orderId", orderId },
                    { "orderInfo", orderInfo },
                    { "redirectUrl", redirectUrl },
                    { "ipnUrl", ipnUrl },
                    { "lang", "en" },
                    { "extraData", extraData },
                    { "requestType", requestType },
                    { "signature", signature }
                };

            string responseFromMomo = SendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);

            // return response
            return new PaymentResponse { Url = jmessage.GetValue("payUrl").ToString() };
        }

        public PaymentReturnResponse PaymentExecuteReturn(MomoRequest request)
        {
            var resultData = new PaymentReturnResponse();
            var isValidSignature = request.IsValidSignature(_configuration["Payment:Momo:AccessKey"], _configuration["Payment:Momo:SecretKey"]);


            if (!isValidSignature)
            {
                resultData.Success = false;
                return resultData;
            }

            resultData.OrderInfo = request.OrderInfo;
            resultData.OrderId = request.OrderId;
            resultData.Amount = (int)request.Amount;
            resultData.Success = true;

            return resultData;
        }

        public async Task PaymentExecuteIpn(MomoRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(Guid.Parse(request.OrderId.Split("_")[0]));
            booking.Status = StatusEnum.COMPLETED;

            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _bookingRepository.Update(booking);
            });
        }

        private string SendPaymentRequest(string endpoint, string postJsonString)
        {
            try
            {
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                var postData = postJsonString;

                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";
                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    string temp = null!;
                    while ((temp = reader.ReadLine()!) != null)
                    {
                        jsonresponse += temp;
                    }
                }

                //todo parse it
                return jsonresponse;
                //return new MomoResponse(mtid, jsonresponse);
            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
}
