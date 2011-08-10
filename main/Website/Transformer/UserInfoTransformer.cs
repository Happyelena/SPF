using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.Specialized;
using SPF;

namespace Website
{
    public class UserInfoTransformer:ITransformProccessor<UserInfo,DataTable>
    {
        public UserInfo Transform(System.Configuration.ConfigurationElement configElement, DataTable rawData)
        {
            return Transform(configElement, rawData, null);
        }

        public UserInfo Transform(System.Configuration.ConfigurationElement configElement, DataTable rawData, NameValueCollection paras)
        {
            var userInfoRow = rawData.Rows[0];
            UserInfo userInfo = new UserInfo()
            {
                UID = userInfoRow["uid"].ToString(),
                UName = userInfoRow["uname"].ToString(),
                UPass = userInfoRow["upass"].ToString(),
                UPassQuiz = userInfoRow["upass_quiz"].ToString(),
                UPassAnswer = userInfoRow["upass_answer"].ToString(),
                RoleName = userInfoRow["roleName"].ToString(),
            };
            return userInfo;
        }
    }
}