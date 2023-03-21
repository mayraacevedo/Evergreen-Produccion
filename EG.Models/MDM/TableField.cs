using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.Models.MDM
{
    public class TableField
    {
        public bool Key { get; set; }
        public string Title { get; set; }
        public string Prompt { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string FiledName { get; set; }
        public string Type
        {
            get
            {
                string typeName = "text";
                switch (BaseType.ToLower())
                {
                    case "decimal":
                        {
                            typeName = "number";
                            break;
                        }
                    case "int32":
                        {
                            typeName = "number";
                            break;
                        }
                    case "datetime":
                        {
                            typeName = "date";
                            break;
                        }
                    case "boolean":
                        {
                            if (FiledName.ToLower() == "estado")
                                typeName = "checkbox";
                            else
                                typeName = "radio";
                            break;
                        }
                    case "fk":
                        {
                            typeName = "dropdown";
                            break;
                        }
                    case "money":
                        {
                            typeName = "money";
                            break;
                        }
                    case "moment":
                        {
                            typeName = "moment";
                            break;
                        }
                    case "quantity":
                        {
                            typeName = "quantity";
                            break;
                        }
                    default:
                        {
                            typeName = "text";
                            break;
                        }
                }
                return typeName;
            }
        }
        public bool IsRequired { get; set; }
        public string BaseType { get; set; }
        //public string ErrorDescription {  get; set; }
        public int FiledLength { get; set; }
        public bool IsReadOnly { get; set; }

        public bool IsForeignKey { get; set; }

        public string ForeigModelName { get; set; }

        public bool Visible { get; set; }

        public int? Width { get; set; }
        public bool Sorting { get; set; } = true;

    }
}
