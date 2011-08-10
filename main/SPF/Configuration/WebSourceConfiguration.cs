using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Caching;
using SPF.Web;

namespace SPF.Configuration
{
    /// <summary>
    /// WebSourcePrivoder section configuration definition
    /// </summary>
    public class WebSourceConfiguration : ConfigurationSection
    {
        public WebSourceConfiguration() { }

        [ConfigurationProperty("", IsDefaultCollection=true)]
        public WebSourceElementCollection WebSourceCollection
        {
            get
            {
                return (WebSourceElementCollection)base[""];
            }
        }
    }

    /// <summary>
    /// WebSource source element collection configuration definition
    /// </summary>
    public class WebSourceElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new WebSourceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WebSourceElement)element).Key;
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

        public WebSourceElement this[int index]
        {
            get
            {
                return (WebSourceElement)BaseGet(index);
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

        public new WebSourceElement this[string name]
        {
            get
            {
                return (WebSourceElement)BaseGet(name);
            }
        }


    }

    /// <summary>
    /// WebSource source element configuration definition
    /// Refer to source level element node
    /// </summary>
    public class WebSourceElement : ConfigurationElement
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

        [ConfigurationProperty("baseUrl", IsRequired = true)]
        public string BaseUrl
        {
            get
            {
                string baseUrl = base["baseUrl"] as string;
                if (baseUrl.Contains("http") || baseUrl.Contains("https"))
                {
                    return baseUrl;
                }
                else
                {
                    return String.Concat(AppDomain.CurrentDomain.BaseDirectory, baseUrl);
                }
            }
            set
            {
                base["baseUrl"] = value;
            }
        }

        [ConfigurationProperty("cache", IsRequired = true)]
        public WebSourceCacheElement WebSourceCache
        {
            get
            {
                return base["cache"] as WebSourceCacheElement;
            }
            set
            {
                base["cache"] = value;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection=true)]
        public WebSourceItemElementCollection WebSourceItems
        {
            get
            {
                return (WebSourceItemElementCollection)base[""];
            }
        }
    }

    /// <summary>
    /// WebSource source cache element definition
    /// </summary>
    public class WebSourceCacheElement : ConfigurationElement
    {
        [ConfigurationProperty("enable", IsRequired = true)]
        public bool CacheEnableTag
        {
            get
            {
                return Convert.ToBoolean(this["cacheDuration"]);
            }
            set
            {
                this["cacheDuration"] = value;
            }
        }

        [ConfigurationProperty("cacheDuration", IsRequired = true,DefaultValue=0)]
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
    /// WebSource item element collection configuration definition
    /// </summary>
    public class WebSourceItemElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new WebSourceItemElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WebSourceItemElement)element).Key;
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

        public WebSourceItemElement this[int index]
        {
            get
            {
                return (WebSourceItemElement)BaseGet(index);
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

        public new WebSourceItemElement this[string name]
        {
            get
            {
                return (WebSourceItemElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// WebSource item element configuration definition
    /// Refer to item level element node
    /// </summary>
    public class WebSourceItemElement : ConfigurationElement
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

        [ConfigurationProperty("contentType", IsRequired = false, DefaultValue = ContentType.HTML)]
        public ContentType ContentType
        {
            get
            {
                return (ContentType)base["contentType"];
            }
            set
            {
                base["contentType"] = value;
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

        [ConfigurationProperty("httpParam", IsRequired = false)]
        public WebSourceItemHttpVerbElement WebSourceHttpParam
        {
            get
            {
                return base["httpParam"] as WebSourceItemHttpVerbElement;
            }
            set
            {
                base["httpParam"] = value;
            }
        }

        [ConfigurationProperty("cache", IsRequired = true)]
        public WebSourceItemCacheElement WebSourceItemCache
        {
            get
            {
                return base["cache"] as WebSourceItemCacheElement;
            }
            set
            {
                base["cache"] = value;
            }
        }

        [ConfigurationProperty("proccessors", IsRequired = true)]
        public WebSourceItemProccessorElementCollection WebSourceItemProccessors
        {
            get
            {
                return (WebSourceItemProccessorElementCollection)base["proccessors"];
            }
        }
    }

    /// <summary>
    /// WebSourceItem HttpVerb element configuration definition
    /// </summary>
    public class WebSourceItemHttpVerbElement : ConfigurationElement
    {
        [ConfigurationProperty("verb", IsRequired = false, DefaultValue = HttpVerb.GET)]
        public HttpVerb HttpVerb
        {
            get
            {
                return (HttpVerb)this["verb"];
            }
            set
            {
                this["verb"] = value;
            }
        }
    }

    /// <summary>
    /// WebsourceItem cache definition
    /// </summary>
    public class WebSourceItemCacheElement : ConfigurationElement
    {
        [ConfigurationProperty("enable", IsRequired = true,DefaultValue=false)]
        public bool CacheEnableTag
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

        [ConfigurationProperty("cacheDuration", IsRequired = false,DefaultValue=0)]
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

        [ConfigurationProperty("cacheItemPriority", IsRequired = false, DefaultValue = CacheItemPriority.Normal)]
        public CacheItemPriority CacheItemPriority
        {
            get
            {
                return (CacheItemPriority)this["cacheItemPriority"];
            }
            set
            {
                this["cacheItemPriority"] = value;
            }
        }

        [ConfigurationProperty("fallback", IsRequired = false, DefaultValue = false)]
        public bool FallBackTag
        {
            get
            {
                return Convert.ToBoolean(this["fallback"]);
            }
            set
            {
                this["fallback"] = value;
            }
        }
    }

    /// <summary>
    /// WebSourceItem proccessors collection definition
    /// </summary>
    public class WebSourceItemProccessorElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new WebSourceItemProccessorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WebSourceItemProccessorElement)element).Name;
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

        public WebSourceItemProccessorElement this[int index]
        {
            get
            {
                return (WebSourceItemProccessorElement)BaseGet(index);
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

        public new WebSourceItemProccessorElement this[string name]
        {
            get
            {
                return (WebSourceItemProccessorElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// WebSource Proccessor Element definition
    /// </summary>
    public class WebSourceItemProccessorElement : ConfigurationElement
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

        [ConfigurationProperty("type", IsRequired = true)]
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

        [ConfigurationProperty("xsltPath", IsRequired = false)]
        public WebSourceItemProccessorXsltPathElement XsltPathElement
        {
            get
            {
                return this["xsltPath"] as WebSourceItemProccessorXsltPathElement;
            }
            set
            {
                this["xsltPath"] = value;
            }
        }

        [ConfigurationProperty("xsltArgs", IsRequired = false)]
        public WebSourceItemProccessorXsltArgsCollection WebsourceItemXsltArgs
        {
            get
            {
                return (WebSourceItemProccessorXsltArgsCollection)base["xsltArgs"];
            }
        }

        [ConfigurationProperty("resources", IsRequired = false)]
        public WebSourceItemProccessorResourceElementCollection WebSourceItemProccessorResources
        {
            get
            {
                return (WebSourceItemProccessorResourceElementCollection)base["resources"];
            }
        }
    }

    /// <summary>
    /// WebSourceXsltProccessor xsltPath element definition
    /// </summary>
    public class WebSourceItemProccessorXsltPathElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                string baseUrl = base["url"] as string;
                if (baseUrl.Contains("http") || baseUrl.Contains("https"))
                {
                    return baseUrl;
                }
                else
                {
                    return String.Concat(AppDomain.CurrentDomain.BaseDirectory, baseUrl);
                }
            }
            set
            {
                this["url"] = value;
            }
        }

        [ConfigurationProperty("enableScript", IsRequired = false, DefaultValue = false)]
        public bool EnableXsltScriptTag
        {
            get
            {
                return Convert.ToBoolean(this["enableScript"]);
            }
            set
            {
                this["enableScript"] = value;
            }
        }
    }

    /// <summary>
    /// WebSourceItem XsltProccessor xslt args element collection definition
    /// </summary>
    public class WebSourceItemProccessorXsltArgsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WebSourceItemProccessorXsltArgsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WebSourceItemProccessorXsltArgsElement)element).Key;
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

        public WebSourceItemProccessorXsltArgsElement this[int index]
        {
            get
            {
                return (WebSourceItemProccessorXsltArgsElement)BaseGet(index);
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

        public new WebSourceItemProccessorXsltArgsElement this[string name]
        {
            get
            {
                return (WebSourceItemProccessorXsltArgsElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// WebSourceItem XsltProccessor xslt args element definition
    /// </summary>
    public class WebSourceItemProccessorXsltArgsElement : ConfigurationElement
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

    /// <summary>
    /// WebSourceItem ContentLoader proccsessor element collection definition
    /// </summary>
    public class WebSourceItemProccessorResourceElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WebSourceItemProccessorResourceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WebSourceItemProccessorResourceElement)element).Key;
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

        public WebSourceItemProccessorResourceElement this[int index]
        {
            get
            {
                return (WebSourceItemProccessorResourceElement)BaseGet(index);
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

        public new WebSourceItemProccessorResourceElement this[string name]
        {
            get
            {
                return (WebSourceItemProccessorResourceElement)BaseGet(name);
            }
        }
    }

    /// <summary>
    /// WebSourceItem ContentLoader proccsessor element definition
    /// </summary>
    public class WebSourceItemProccessorResourceElement : ConfigurationElement
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

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return this["url"] as string;
            }
            set
            {
                this["url"] = value;
            }
        }
    }

}
