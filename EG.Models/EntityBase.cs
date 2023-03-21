using System.ComponentModel.DataAnnotations;
using System.Reflection;
using EG.Models.MDM;
using Newtonsoft.Json;

namespace EG.Models
{
    public class EntityBase
    {
        [Display(Name = "Fecha de creación", Description = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación", Description = "Fecha de la última modificación")]
        public DateTime? FechaModificacion { get; set; }
       
        public TableSchema GetTableSchema()
        {
            Type type = this.GetType();
            var instance = Activator.CreateInstance(type);

            var attr = type.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.DisplayAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.DisplayAttribute;

            var displayName = attr.Name;

            TableSchema tableSchema = new TableSchema(type.Name, displayName);

            foreach (var prop in type.GetProperties())
            {

                //System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute
                var inversePropertyAttribute = prop.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute;
                var foreignKeyAttribute = prop.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;

                if (inversePropertyAttribute == null || foreignKeyAttribute != null)
                {

                    Type baseType = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;


                    if (foreignKeyAttribute == null)
                    {

                        //System.ComponentModel.DataAnnotations.RequiredAttribute
                        var isRequired = prop.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.RequiredAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.RequiredAttribute == null ? false : true;

                        //System.ComponentModel.DataAnnotations.KeyAttribute	
                        var isKey = prop.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.KeyAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.KeyAttribute == null ? false : true;

                        string propDisplayName = prop.Name;
                        string description = String.Empty;
                        string prompt = String.Empty;

                        var diplayAttribute = prop.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.DisplayAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.DisplayAttribute;

                        if (diplayAttribute != null)
                        {
                            propDisplayName = diplayAttribute.GetName();
                            description = diplayAttribute.GetDescription();
                            prompt = diplayAttribute.GetPrompt();
                        }

                        //System.ComponentModel.DataAnnotations.StringLengthAttribute
                        int length = 0;
                        var stringLengthAttribute = prop.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.StringLengthAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.StringLengthAttribute;
                        if (stringLengthAttribute != null)
                        {
                            length = stringLengthAttribute.MaximumLength;
                        }

                        //System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute
                        //var foreignKeyAttribute = prop.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;


                        if (isKey)
                        {
                            tableSchema.KeyDataType = baseType.Name;
                            tableSchema.KeyName = prop.Name;
                        }

                        bool isReadOnlyAttribute = false;
                        bool visibleInTable = true;

                        if (prop.Name == "CreadoPor" || prop.Name == "FechaCreacion" || prop.Name == "FechaModificacion" || prop.Name == "ModificadoPor")
                        {
                            isReadOnlyAttribute = true;
                            visibleInTable = false;
                        }

                        int? width = null;

                        if (prop.Name.ToLower() == "id" || prop.Name.ToLower() == "estado")
                        {
                            width = 100;
                        }

                        tableSchema.Fields.Add(new TableField()
                        {
                            Name = ToCamelCase(prop.Name),
                            FiledName = ToCamelCase(prop.Name),
                            Title = propDisplayName,
                            Description = description,
                            Prompt = prompt,
                            FiledLength = length,
                            IsRequired = isRequired,
                            Key = isKey,
                            BaseType = baseType.Name,
                            IsReadOnly = isReadOnlyAttribute,
                            IsForeignKey = false,
                            Visible = visibleInTable,
                            Width = width
                        });
                    }
                    else
                    {
                        var foreignKeyField = tableSchema.Fields.Where(f => f.Name == ToCamelCase(foreignKeyAttribute.Name)).FirstOrDefault();

                        if (foreignKeyField != null)
                        {
                            //TODO: Debemos buscar la fomra que el campo de descripción de la tabla foránea sea dinámico o colocarlo en un atributo (decorador)
                            //foreignKeyField.Name = $"{ToCamelCase(prop.Name)}.nombre"; Error en blazor
                            foreignKeyField.Name = $"{ToCamelCase(prop.Name)}";
                            foreignKeyField.IsForeignKey = true;
                            foreignKeyField.ForeigModelName = baseType.Name;
                            foreignKeyField.BaseType = "fk";
                            tableSchema.ReferentialProperties.Add(prop.Name);
                        }

                    }
                }

            }


            return tableSchema;
        }

        public static string ToCamelCase(string text)
        {
            return System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(text);
        }

        public static string ToPascalCase(string text)
        {
            var camelText = ToCamelCase(text);
            return $"{camelText.Substring(0, 1).ToUpper()}{camelText.Substring(1)}";
        }

        public static List<string> GetAllTables()
        {

            string nspace = "EG.Models.Entities";

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == nspace
                    select t.Name;
            return q.ToList();
        }

        public static Dictionary<string, object> ParseToKeyValueList(string tableName, object objectItem)
        {



            string typeName = $"EG.Models.Entities.{tableName},EG.Models";
            Type type = Type.GetType(typeName);

            dynamic rec = Activator.CreateInstance(type);

            //string objectJson = System.Text.Json.JsonSerializer.Serialize(objectItem);

            if (objectItem != null)
            {
                rec = JsonConvert.DeserializeObject(System.Text.Json.JsonSerializer.Serialize(objectItem), type);
            }

            //var rec2 = JsonConvert.DeserializeObject<List<object>>(objectJson);


            //PropertyInfo[] propertyInfos = type.GetProperties();
            Dictionary<string, object> propNames = new Dictionary<string, object>();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {

                var inversePropertyAttribute = propertyInfo.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute;
                var foreignKeyAttribute = propertyInfo.GetCustomAttribute(Type.GetType("System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute, System.ComponentModel.Annotations")) as System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;

                if (propertyInfo.CanRead && inversePropertyAttribute == null && foreignKeyAttribute == null)
                {
                    var pName = propertyInfo.Name;
                    if (objectItem != null)
                    {
                        var pValue = propertyInfo.GetValue(rec, null);
                        if (pValue != null)
                        {
                            propNames.Add(ToCamelCase(pName), pValue);
                        }
                        else
                        {
                            propNames.Add(ToCamelCase(pName), null);
                        }
                    }
                    else
                        propNames.Add(ToCamelCase(pName), null);
                }
            }

            return propNames;
        }

    }
}
