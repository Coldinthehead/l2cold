using Core.Login;
using Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Services
{
    public struct LSAccountDetails
    {
        public string Id;

        public SessionKeys Skeys;

        public LSAccountDetails(string id, SessionKeys Skeys)
        {
            this.Id = id;
            this.Skeys = Skeys;
        }
    }

    public class LoginServerService
    {
        private readonly LoginServer _loginServer;

        public LoginServerService(LoginServer loginServer)
        {
            _loginServer = loginServer;
        }
        public bool IsAccountLoggedIn(LSAccountDetails accDetails)
        {
            return true;
        }
    }
}
