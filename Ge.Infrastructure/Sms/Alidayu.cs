using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Sms
{
    /// <summary>
    /// 阿里大鱼短信接口
    /// </summary>
    public class Alidayu
    {
        private static readonly string accessKeyId = AppSettingHelper.Get("AlidayuKey");
        private static readonly string accessKeySecret = AppSettingHelper.Get("AlidayuSecret");
        private static readonly string signName = "江苏工程技术文献信息中心";
        public static string SendSms(string phoneNumbers, string templateParam, string outId,string templateCode)
        {
            String product = "Dysmsapi";//短信API产品名称
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            //String accessKeyId = "LTAIs1a9VuXYV2sz";//你的accessKeyId
            //String accessKeySecret = "5CmterlF4kldJQpurNEuye2Esi9Nl6 ";//你的accessKeySecret

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();

            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = phoneNumbers;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = signName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = templateCode;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = templateParam;
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = outId;
                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);

                return response.Message;

            }
            catch (ServerException e)
            {
                return "server_error";
            }
            catch (ClientException e)
            {
                return "client_error";
            }
        }
    }
}
