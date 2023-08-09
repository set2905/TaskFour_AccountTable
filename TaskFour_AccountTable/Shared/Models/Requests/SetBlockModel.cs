using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFour_AccountTable.Shared.Models.Requests
{
    public class SetBlockModel
    {
        public SetBlockModel(string[] userIds, bool blockValue)
        {
            this.userIds=userIds;
            this.blockValue=blockValue;
        }

        public string[] userIds { get; set; }
        public bool blockValue { get; set; }
    }
}
