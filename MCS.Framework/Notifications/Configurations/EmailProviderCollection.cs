using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class EmailProviderCollection : ConfigurationElementCollection, ICollection<EmailProvider>
    {
        /// <summary>
        ///  Gets the Collections of this type contain elements that can be merged across a hierarchy
        ///  of configuration files.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        /// <summary>
        /// Create instance of the EmailProvider class.
        /// </summary>
        /// <returns> Instance of EmailProvider class.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EmailProvider();
        }

        /// <summary>
        /// Override CreateNewElement in the Cofiguration Class.
        /// </summary>
        /// <param name="elementName">Configuration element to be created.</param>
        /// <returns>New object of EmailProvider conifguration element.</returns>
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new EmailProvider();
        }

        /// <summary>
        /// Override GetElementkey in the Configuration class.
        /// </summary>
        /// <param name="element">Configuration Element.</param>
        /// <returns> EmailProvider element as object.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmailProvider)element).Name;
        }

        /// <summary>
        ///   Gets or sets the name of the System.Configuration.ConfigurationElement to
        ///   associate with the add operation in the System.Configuration.ConfigurationElementCollection
        ///   when overridden in a derived class.
        /// </summary>
        public new string AddElementName
        {
            get
            {
                return base.AddElementName;
            }

            set
            {
                base.AddElementName = value;
            }
        }

        /// <summary>
        ///     Gets or sets the name for the System.Configuration.ConfigurationElement to
        ///     associate with the clear operation in the System.Configuration.ConfigurationElementCollection
        ///     when overridden in a derived class.
        /// </summary>
        public new string ClearElementName
        {
            get
            {
                return base.ClearElementName;
            }

            set
            {
                base.AddElementName = value;
            }
        }

        /// <summary>
        ///   Gets the name of the System.Configuration.ConfigurationElement to
        ///   associate with the remove operation in the System.Configuration.ConfigurationElementCollection
        ///   when overridden in a derived class.
        /// </summary>
        public new string RemoveElementName
        {
            get
            {
                return base.RemoveElementName;
            }
        }

        /// <summary>
        ///  Gets the number of elements in the collection.
        /// </summary>
        public new int Count
        {
            get { return base.Count; }
        }

        /// <summary>
        /// Gets the configuration element at the specified index location.
        /// </summary>
        /// <param name="index"> The index location of the System.Configuration.ConfigurationElement to return.</param>
        /// <returns> The System.Configuration.ConfigurationElement at the specified index.</returns>
        public EmailProvider this[int index]
        {
            get
            {
                return (EmailProvider)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }

                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Returns the configuration element with the specified key.
        /// </summary>
        /// <param name="name">  The key of the element to return.  </param>
        /// <returns>The System.Configuration.ConfigurationElement with the specified key; otherwise,  null.</returns>
        new public EmailProvider this[string name]
        {
            get
            {
                return (EmailProvider)BaseGet(name);
            }
        }

        /// <summary>
        /// The index of the specified System.Configuration.ConfigurationElement.
        /// </summary>
        /// <param name="item"> The System.Configuration.ConfigurationElement for the specified index location.</param>
        /// <returns> The index of the specified System.Configuration.ConfigurationElement; otherwise,    -1.</returns>
        public int IndexOf(EmailProvider item)
        {
            return BaseIndexOf(item);
        }

        /// <summary>
        /// Add element to the cofiguration file.
        /// </summary>
        /// <param name="item">EmailProvider element of the configuration file.</param>
        public void Add(EmailProvider item)
        {
            this.BaseAdd(item);
        }

        /// <summary>
        ///  Adds a configuration element to the configuration element collection.
        /// </summary>
        /// <param name="element"> The System.Configuration.ConfigurationElement to add.</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            this.BaseAdd(element, false);
        }

        /// <summary>
        ///    Removes the System.Configuration.ConfigurationElement at the specified index
        ///     location.
        /// </summary>
        /// <param name="index"> The index location of the System.Configuration.ConfigurationElement to remove.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Removes the System.Configuration.ConfigurationElement at the specified index location.
        /// </summary>
        /// <param name="name"> The index location of the System.Configuration.ConfigurationElement to remove.</param>
        public void Remove(string name)
        {
            BaseRemove(name);
        }

        /// <summary>
        /// Removes all configuration element objects from the collection.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Removes a System.Configuration.ConfigurationElement from the collection.
        /// </summary>
        /// <param name="item">  The key of the System.Configuration.ConfigurationElement to remove.</param>
        /// <returns>True if removed and false otherwize.</returns>
        public bool Remove(EmailProvider item)
        {
            if (BaseIndexOf(item) >= 0)
            {
                BaseRemove(item.Name);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Indictaes either the current collection  contain the item or not.
        /// </summary>
        /// <param name="item">An object of type EmailProvider. </param>
        /// <returns>An object of type bool.</returns>
        public bool Contains(EmailProvider item)
        {
            if (this[item.Name] == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Copies the contents of the System.Configuration.ConfigurationElementCollection
        ///  to an array. 
        /// </summary>
        /// <param name="array"> Array of typ EmailProvider. </param>
        /// <param name="arrayIndex">An object of type int.</param>
        public void CopyTo(EmailProvider[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets a value indicating whether current collection readonly.
        /// </summary>
        public new bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Represent the helper method for iteration.
        /// </summary>
        /// <returns>return IEnumerator object of type EmailProvider.</returns>
        public new IEnumerator<EmailProvider> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
