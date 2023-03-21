using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Models.MDM
{
    public class TableSchema
    {

        public TableSchema(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
            //Description = description;
            KeyDataType = typeof(System.String).Name;
            Fields = new List<TableField>();
            ReferentialProperties = new List<string>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        //public string Description {  get; set; }
        public string KeyDataType { get; set; }
        public string KeyName { get; set; }
        public List<TableField> Fields { get; set; }
        public List<string> ReferentialProperties { get; set; }

    }
}
