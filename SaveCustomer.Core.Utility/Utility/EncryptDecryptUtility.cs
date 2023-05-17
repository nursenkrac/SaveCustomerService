using SaveCustomerService.Core.Infrastructure;
using Core.Operation.Derived.ValidationOperation;
using Core.Operation.Infrastructure;
using Core.Model.Infrastructure.Results;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using SaveCustomerService.Encryption.Derived;

namespace SaveCustomerService.Core.Utility.Utility
{
    public class EncryptDecryptUtility<T> where T : class,new()
    {
        private readonly string _encryptionKey;
        private readonly SHA256Encryption _sHA256Encryption;
        public EncryptDecryptUtility()
        {
            IConfigurationRoot _configurationRoot = IocManager.Resolve<IConfigurationRoot>();
            _sHA256Encryption = new SHA256Encryption();
            _encryptionKey = _configurationRoot.GetSection("EncryptionSettings:EncryptionKey").Value;
        }
        public T Encrypt(T model)
        {
            var encryptedModel = new T();
            Type myType = model.GetType();
            foreach (PropertyInfo property in myType.GetProperties())
            {
                var propertyType = property.PropertyType;
                if (propertyType.Name == TypeCode.String.ToString())
                {
                    var propertyValue = property.GetValue(model, null).ToString();
                    property.SetValue(encryptedModel, Encrypt(propertyValue));
                }
                else
                {
                    var propertyValue = property.GetValue(model, null);
                    property.SetValue(encryptedModel, propertyValue);
                }
            }
            return encryptedModel;
        }
        public T Decrypt(T model)
        {
            var decryptedModel = new T();
            Type myType = model.GetType();
            foreach (PropertyInfo property in myType.GetProperties())
            {
                var propertyType = property.PropertyType;
                
                if (propertyType.Name == TypeCode.String.ToString())
                {
                    var propertyValue = property.GetValue(model, null).ToString();
                    property.SetValue(decryptedModel, Decrypt(propertyValue));
                }
                else
                {
                    var propertyValue = property.GetValue(model, null);
                    property.SetValue(decryptedModel,propertyValue);
                }
            }
            return decryptedModel;
        }
        public string Encrypt(string text) => _sHA256Encryption.Encrypt(text, _encryptionKey);
        public string Decrypt(string text) => _sHA256Encryption.Decrypt(text, _encryptionKey);
    }
}
