using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using SPF.Sql;

namespace SPF.Configuration
{
    /// <summary>
    /// SqlSourceContent section definition
    /// </summary>
    public class SqlSourceConfiguration : ConfigurationSection
    {
        public SqlSourceConfiguration()
        {

        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public SqlSourceElmentCollection SqlSourceElements
        {
            get
            {
                return (SqlSourceElmentCollection)base[""];
            }
        }
    }

    /// <summary>
    /// SqlSource element collcetion definition
    /// </summary>
    public class SqlSourceElmentCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SqlSourceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SqlSourceElement)element).Key;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "source";
            }
        }

        public SqlSourceElement this[int index]
        {
            get
            {
                return (SqlSourceElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SqlSourceElement this[string name]
        {
            get
            {
                return (SqlSourceElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// SqlSource element definition
    /// </summary>
    public class SqlSourceElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return base["key"] as string;
            }
            set
            {
                base["key"] = value;
            }
        }

        [ConfigurationProperty("connectionPorfile", IsRequired = true)]
        public string ConnectionPorfile
        {
            get
            {
                return base["connectionPorfile"] as string;
            }
            set
            {
                base["connectionPorfile"] = value;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public SqlSourceItemElementCollection SqlSourceItems
        {
            get
            {
                return base[""] as SqlSourceItemElementCollection;
            }
        }
    }

    /// <summary>
    /// SqlSourceItem element collection definition
    /// </summary>
    public class SqlSourceItemElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SqlSourceItemElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SqlSourceItemElement)element).Key;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "item";
            }
        }

        public SqlSourceItemElement this[int index]
        {
            get
            {
                return (SqlSourceItemElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SqlSourceItemElement this[string name]
        {
            get
            {
                return (SqlSourceItemElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// SqlSourceItem element definition
    /// </summary>
    public class SqlSourceItemElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return base["key"] as string;
            }
            set
            {
                base["key"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = false)]
        public string Type
        {
            get
            {
                return base["type"] as string;
            }
            set
            {
                base["type"] = value;
            }
        }

        [ConfigurationProperty("proccessors", IsRequired = true, IsDefaultCollection = true)]
        public SqlSourceItemProccessorElementCollection SqlSourceProccessors
        {
            get
            {
                return (SqlSourceItemProccessorElementCollection)base["proccessors"];
            }
        }

        [ConfigurationProperty("cache", IsRequired = false)]
        public SqlSourceItemCacheElement SqlSourceItemCacheElement
        {
            get
            {
                return base["cache"] as SqlSourceItemCacheElement;
            }
            set
            {
                base["cache"] = value;
            }
        }
    }

    /// <summary>
    /// SqlSourceItem cache element definition
    /// </summary>
    public class SqlSourceItemCacheElement : ConfigurationElement
    {
        [ConfigurationProperty("enable", IsRequired = true, DefaultValue = false)]
        public bool EnableCacheTag
        {
            get
            {
                return Convert.ToBoolean(this["enable"]);
            }
            set
            {
                this["enable"] = value;
            }
        }

        [ConfigurationProperty("cacheDuration", IsRequired = true, DefaultValue = 0)]
        public int CacheDuration
        {
            get
            {
                return Convert.ToInt32(this["cacheDuration"]);
            }
            set
            {
                this["cacheDuration"] = value;
            }
        }
    }

    /// <summary>
    /// SqlSourceItem Processor element collection definition
    /// </summary>
    public class SqlSourceItemProccessorElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SqlSourceItemProccessorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SqlSourceItemProccessorElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "proccessor";
            }
        }

        public SqlSourceItemProccessorElement this[int index]
        {
            get
            {
                return (SqlSourceItemProccessorElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SqlSourceItemProccessorElement this[string name]
        {
            get
            {
                return (SqlSourceItemProccessorElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// SqlSourceItem Processor element definition
    /// </summary>
    public class SqlSourceItemProccessorElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
        public string Type
        {
            get
            {
                return this["type"] as string;
            }
            set
            {
                this["type"] = value;
            }
        }

        [ConfigurationProperty("mapping", IsRequired = false)]
        public SqlSourceItemMappingProccessorElementCollection SqlSourceItemMappingProcessorArgs
        {
            get
            {
                return (SqlSourceItemMappingProccessorElementCollection)base["mapping"];
            }
        }

        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public SqlSourceItemLoadCommandElementCollection SqlSourceItemLoadCommands
        {
            get
            {
                return (SqlSourceItemLoadCommandElementCollection)base[""];
            }
        }

        [ConfigurationProperty("sqlArgs", IsRequired = false)]
        public SqlSourceItemArgsElementCollection SqlSourceItemArgs
        {
            get
            {
                return (SqlSourceItemArgsElementCollection)base["sqlArgs"];
            }
        }
    }

    /// <summary>
    ///  SqlSourceItem sqlobjectmappingProcessor element collection definition
    /// </summary>
    public class SqlSourceItemMappingProccessorElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SqlSourceItemMappingProccessorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SqlSourceItemMappingProccessorElement)element).ObjectValue;
        }

        protected override string ElementName
        {
            get
            {
                return "add";
            }
        }

        public SqlSourceItemMappingProccessorElement this[int index]
        {
            get
            {
                return (SqlSourceItemMappingProccessorElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SqlSourceItemMappingProccessorElement this[string name]
        {
            get
            {
                return (SqlSourceItemMappingProccessorElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// SqlSourceItem sqlobjectmappingProcessor element definition
    /// </summary>
    public class SqlSourceItemMappingProccessorElement : ConfigurationElement
    {
        [ConfigurationProperty("objectValue", IsRequired = true, IsKey = true)]
        public string ObjectValue
        {
            get
            {
                return this["objectValue"] as string;
            }
            set
            {
                this["objectValue"] = value;
            }
        }

        [ConfigurationProperty("sqlValue", IsRequired = true)]
        public string SqlValue
        {
            get
            {
                return this["sqlValue"] as string;
            }
            set
            {
                this["sqlValue"] = value;
            }
        }
    }

    /// <summary>
    /// SqlSourceItem sqlcommand element collection definition
    /// </summary>
    public class SqlSourceItemLoadCommandElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new SqlSourceItemLoadCommandElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SqlSourceItemLoadCommandElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "sqlCommand";
            }
        }

        public SqlSourceItemLoadCommandElement this[int index]
        {
            get
            {
                return (SqlSourceItemLoadCommandElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SqlSourceItemLoadCommandElement this[string name]
        {
            get
            {
                return (SqlSourceItemLoadCommandElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// SqlSourceItem sqlcommand element definition
    /// </summary>
    public class SqlSourceItemLoadCommandElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return base["name"] as string;
            }
            set
            {
                base["name"] = value;
            }
        }

        [ConfigurationProperty("commandType", IsRequired = false, DefaultValue = CommandType.Text)]
        public CommandType CommandType
        {
            get
            {
                return (CommandType)base["commandType"];
            }
            set
            {
                base["commandType"] = value;
            }
        }

        [ConfigurationProperty("text", IsRequired = true)]
        public string CommandText
        {
            get
            {
                return base["text"] as string;
            }
            set
            {
                base["text"] = value;
            }
        }
    }

    /// <summary>
    /// SqlSourceItem sqlcommand args element collection definition
    /// </summary>
    public class SqlSourceItemArgsElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SqlSourceItemArgsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SqlSourceItemArgsElement)element).Key;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "add";
            }
        }

        public SqlSourceItemArgsElement this[int index]
        {
            get
            {
                return (SqlSourceItemArgsElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SqlSourceItemArgsElement this[string name]
        {
            get
            {
                return (SqlSourceItemArgsElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// SqlSourceItem sqlcommand args element definition
    /// </summary>
    public class SqlSourceItemArgsElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return this["key"] as string;
            }
            set
            {
                this["key"] = value;
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["value"] as string;
            }
            set
            {
                this["value"] = value;
            }
        }
    }

}
