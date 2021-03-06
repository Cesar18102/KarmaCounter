﻿using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using KarmaCounter.Models;
using KarmaCounter.ModelMapping.ModelMappingAttributes;

namespace KarmaCounter.ModelMapping
{
    public class ModelMapper
    {
        public async Task<string> ExtractJsonQueryBody<T, R>(T item) where T : IModelElement
                                                                     where R : ModelMappringAttribute => 
            WrapBody(await ExtractBody<T, R>(item));

        public async Task<string> ExtractJsonQueryBody<T, Q, R>(T item1, Q item2) where T : IModelElement
                                                                                  where Q : IModelElement
                                                                                  where R : ModelMappringAttribute =>
            WrapBody(await ExtractBody<T, R>(item1) + await ExtractBody<Q, R>(item2));

        public async Task<string> ExtractJsonQueryBody<T, Q, P, R>(T item1, Q item2, P item3) where T : IModelElement
                                                                                              where Q : IModelElement
                                                                                              where P : IModelElement
                                                                                              where R : ModelMappringAttribute =>
            WrapBody(await ExtractBody<T, R>(item1) + await ExtractBody<Q, R>(item2) + await ExtractBody<P, R>(item3));

        private async Task<string> ExtractBody<T, R>(T item) where T : IModelElement
                                                             where R : ModelMappringAttribute
        {
            return await Task.Run(() =>
            {
                string body = "";
                Type type = item.GetType();
                foreach (PropertyInfo PI in type.GetProperties().Union(type.GetRuntimeProperties()))
                {
                    R attribute = PI.GetCustomAttribute<R>();
                    if (attribute != null)
                    {
                        string propertyName = attribute.Name == null || attribute.Name == "" ? PI.Name : attribute.Name;
                        string propertyValue = JsonConvert.SerializeObject(PI.GetValue(item));
                        body += '"' + propertyName + '"' + " : " + propertyValue + ", ";
                    }
                }
                return body;
            });
        }

        private string WrapBody(string body) =>
            "{" + (body.Length <= 2 ? "" : body.Substring(0, body.Length - 2)) + " }";



        public async Task<IDictionary<string, string>> ExtractQueryParameters<T, R>(T item) where T : IModelElement
                                                                                            where R : ModelMappringAttribute =>
            await ExtractParameters<T, R>(item);

        public async Task<IDictionary<string, string>> ExtractQueryParameters<T, Q, R>(T item1, Q item2) where T : IModelElement
                                                                                                         where Q : IModelElement
                                                                                                         where R : ModelMappringAttribute
        {
            IDictionary<string, string> params1 = await ExtractParameters<T, R>(item1);
            IDictionary<string, string> params2 = await ExtractParameters<Q, R>(item2);

            return params1.Concat(params2).ToDictionary(P => P.Key, P => P.Value);
        }

        private async Task<IDictionary<string, string>> ExtractParameters<T, R>(T item) where T : IModelElement
                                                                                        where R : ModelMappringAttribute
        {
            IDictionary<string, string> data = new Dictionary<string, string>();

            await Task.Run(() =>
            {
                Type type = item.GetType();
                foreach (PropertyInfo PI in type.GetProperties().Union(type.GetRuntimeProperties()))
                {
                    R attribute = PI.GetCustomAttribute<R>();
                    if (attribute != null)
                    {
                        string propertyName = attribute.Name == null || attribute.Name == "" ? PI.Name : attribute.Name;
                        string propertyValue = JsonConvert.SerializeObject(PI.GetValue(item)).Trim('\"');
                        data.Add(propertyName, propertyValue);
                    }
                }
            });

            return data;
        }
    }
}
