﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Xbim.Common;
using Xbim.Common.Metadata;
using Xbim.Common.Step21;
using Xbim.IO.Step21;

namespace Xbim.IO.Xml
{
    public class XbimXmlWriter4
    {
        #region Fields

        private const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
        private const string Xlink = "http://www.w3.org/1999/xlink";
        private readonly string _ns = "http://www.buildingsmart-tech.org/ifcXML/IFC4/final";
        private readonly string _nsLocation = "http://www.buildingsmart-tech.org/ifcXML/IFC4/final/ifcXML4.xsd";
        private readonly string _expressUri = "http://www.buildingsmart-tech.org/ifc/IFC4/final/IFC4.exp";
        private readonly string _configuration = "http://www.buildingsmart-tech.org/ifcXML/IFC4/final/config/ifcXML4_config.xml";
        private readonly string _rootElementName = "ifcXML";
        private HashSet<long> _written;

        public bool WriteInverses;

        #endregion

        #region Properties

        public string Name;
        public string TimeStamp;
        public string Author;
        public string Organization;
        public string PreprocessorVersion;
        public string OriginatingSystem;
        public string Authorization;
        public string Documentation;

        private IStepFileHeader _fileHeader; // removed the initialiser because it's assigned from the model on write.

        #endregion

        public XbimXmlWriter4(XbimXmlSettings settings = null)
        {
            var now = DateTime.Now;
            TimeStamp = string.Format("{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}", now.Year, now.Month, now.Day,
                                      now.Hour, now.Minute, now.Second);
            PreprocessorVersion = string.Format("Xbim File Processor version {0}",
                                                Assembly.GetAssembly(GetType()).GetName().Version);
            OriginatingSystem = string.Format("Xbim version {0}", Assembly.GetExecutingAssembly().GetName().Version);

            if (settings == null)
            return;

            _ns = settings.Namespace;
            _nsLocation = settings.NamespaceLocation;
            _expressUri = settings.ExpressUri;
            _configuration = settings.Configuration;
            _rootElementName = settings.RootName;
        }

        private ExpressMetaData _metadata;

        /// <summary>
        /// This function writes model entities to the defined XML output. 
        /// </summary>
        /// <param name="model">Model to be used for serialization. If no entities are specified IModel.Instances will be used as a 
        /// source of entities to be serialized.</param>
        /// <param name="output">Output XML</param>
        /// <param name="entities">Optional entities enumerable. If you define this enumerable it will be
        /// used instead of all entities from IModel.Instances. This allows to define different way of entities retrieval
        /// like volatile instances from persisted DB model</param>
        public void Write(IModel  model, XmlWriter output, IEnumerable<IPersistEntity> entities = null)
        {
            _metadata = model.Metadata;
            
            try
            {
                _written = new HashSet<long>();

                output.WriteStartDocument();
                output.WriteStartElement(_rootElementName, _ns);
                //xmlns declarations
                output.WriteAttributeString("xmlns", "xsi", null, Xsi);
                output.WriteAttributeString("xmlns", "xlink", null, Xlink);
                output.WriteAttributeString("xmlns", "stp", null, _ns);
                output.WriteAttributeString("schemaLocation", Xsi, string.Format("{0} {1}", _ns, _nsLocation));

                //attributes
                output.WriteAttributeString("id", "uos_1");
                output.WriteAttributeString("express", _expressUri);
                output.WriteAttributeString("configuration", _configuration);

                _fileHeader = model.Header;
                WriteHeader(output);
                

                //use specified entities enumeration or just all instances in the model
                if(entities != null)
                    foreach (var entity in entities)
                        Write(entity, output);
                else
                    foreach (var entity in model.Instances)
                        Write(entity, output);

                output.WriteEndElement(); //uos
                output.WriteEndDocument();
            }
            //catch (Exception e)
            //{
            //    throw new Exception("Failed to write IfcXml file", e);
            //}
            finally
            {
                _written = null;
            }
        }

        private void WriteHeader(XmlWriter output)
        {
            output.WriteStartElement("header");
            output.WriteElementString("name", _fileHeader.FileName.Name);
            output.WriteElementString("time_stamp", _fileHeader.FileName.TimeStamp);

            if (_fileHeader.FileName.AuthorName.Count > 0)
                output.WriteElementString("author", string.Join(", ", _fileHeader.FileName.AuthorName));
            if (_fileHeader.FileName.Organization.Any())
                output.WriteElementString("organization", string.Join(", ", _fileHeader.FileName.Organization));
            output.WriteElementString("preprocessor_version", _fileHeader.FileName.PreprocessorVersion);
            output.WriteElementString("originating_system", _fileHeader.FileName.OriginatingSystem);
            output.WriteElementString("authorization", _fileHeader.FileName.AuthorizationName);
            if (_fileHeader.FileDescription.Description.Any())
                output.WriteElementString("documentation", string.Join(", ", _fileHeader.FileDescription.Description));
            output.WriteEndElement(); //end iso_10303_28_header
        }

        private void Write(IPersistEntity entity, XmlWriter output, int pos = -1)
        {

            if (_written.Contains(entity.EntityLabel)) //we have already done it
                return;
            _written.Add(entity.EntityLabel);

            var expressType = _metadata.ExpressType(entity);

            output.WriteStartElement(expressType.Type.Name);

            output.WriteAttributeString("id", string.Format("i{0}", entity.EntityLabel));
            if (pos > -1) //we are writing out a list element
                output.WriteAttributeString("pos", pos.ToString());
            
            WriteProperties(entity, output, expressType);
            output.WriteEndElement();
        }

        private void WriteProperties(IPersistEntity entity, XmlWriter output, ExpressType expressType)
        {
            IEnumerable<ExpressMetaProperty> toWrite;
            if (WriteInverses)
            {
                var l = new List<ExpressMetaProperty>(expressType.Properties.Values);
                l.AddRange(expressType.Inverses);
                toWrite = l;
            }
            else
            {
                toWrite = expressType.Properties.Values;
            }

            //sort toWrite properties so that value types go first. They will be written as attributes and that has to
            //happen before any nested elements are written
            toWrite = toWrite.OrderByDescending(p => IsAttributeValue(p.PropertyInfo.PropertyType));

            foreach (var ifcProperty in toWrite) //only write out persistent attributes, ignore inverses
            {
                if (ifcProperty.EntityAttribute.IsDerived) continue;

                var propType = ifcProperty.PropertyInfo.PropertyType;
                var propVal = ifcProperty.PropertyInfo.GetValue(entity, null);

                WriteProperty(ifcProperty.PropertyInfo.Name, propType, propVal, entity, output, -1,
                    ifcProperty.EntityAttribute);
            }
        }

        private bool IsAttributeValue(Type type)
        {
            if (type.IsValueType) return true;
            if (type == typeof (string)) return true;

            var generic = type.GetGenericArguments().FirstOrDefault();
            if (generic == null) return false;
            return generic.IsValueType;
        }

        private void WriteProperty(string propName, Type propType, object propVal, IPersistEntity entity, XmlWriter output,
                                   int pos, EntityAttributeAttribute attr, bool wrap = false)
        {
            var optSet = propVal as IOptionalItemSet;
            if (optSet != null && !optSet.Initialized)
            {
                //don't write anything if this is uninitialized item set
                return;
            }
            if (propVal == null)
            //null or a value type that maybe null, need to write out sets and lists if they are mandatroy but empty
            {
                if (typeof(IExpressEnumerable).IsAssignableFrom(propType) && attr.State == EntityAttributeState.Mandatory)
                //special case as these two classes are optimised
                {
                    output.WriteStartElement(propName);
                    output.WriteAttributeString("cType", attr.ListType);
                    output.WriteEndElement();
                }
                return;
            }
            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                (propVal is IExpressValueType)) //deal with undefined types (nullables)
            {
                var cpl = propVal as IExpressComplexType;
                if (cpl == null)
                {
                    output.WriteAttributeString(propName, propVal.ToString());
                }
                else
                {
                    var complexProps = string.Join(" ", cpl.Properties);
                    output.WriteAttributeString(propName, complexProps);
                }
                return;
            }
            if (typeof(IExpressValueType).IsAssignableFrom(propType))
            {
                var realType = propVal.GetType();

                if (realType != propType)
                    //we have a type but it is a select type use the actual value but write out explicitly
                {
                    output.WriteStartElement(propName);
                    output.WriteElementString(realType.Name + "-wrapper", propVal.ToString());
                    output.WriteEndElement();
                    return;
                }

                if (pos > -1)
                {
                    output.WriteStartElement(propName + (wrap ? "-wrapper" : ""));
                    output.WriteAttributeString("pos", pos.ToString());
                    output.WriteValue(propVal.ToString());
                    output.WriteEndElement();
                }
                else
                    output.WriteAttributeString(propName, propVal.ToString());
                return;
            }
            if (typeof(IExpressEnumerable).IsAssignableFrom(propType))
            {
                var generic = propType.GetGenericArguments().FirstOrDefault();
                if(generic != null && generic.IsValueType)
                //if ((propName == "Coordinates" && entity.GetType().Name == "IfcCartesianPoint")
                //    || (propName == "DirectionRatios" && entity.GetType().Name == "IfcDirection")
                //    || (propName == "RelatingPriorities" && entity.GetType().Name == "IfcRelConnectsPathElements")
                //    || (propName == "RelatedPriorities" && entity.GetType().Name == "IfcRelConnectsPathElements")
                //    )
                {
                    output.WriteAttributeString(propName, string.Join(" ", ((IEnumerable)propVal).Cast<object>()));
                    return;
                }

                //special case for IfcRelDefinesByProperties
                if (propName == "RelatedObjects" && entity.ExpressType.Name == "IfcRelDefinesByProperties")
                {
                    propVal = ((IEnumerable)propVal).Cast<object>().FirstOrDefault();
                    var relEntity = propVal as IPersistEntity;
                    if (relEntity == null) return;

                    output.WriteStartElement(propName);
                    output.WriteAttributeString("ref", string.Format("i{0}", relEntity.EntityLabel));
                    output.WriteAttributeString("nil", Xsi, "true");
                    output.WriteAttributeString("type", Xsi, relEntity.GetType().Name);
                    output.WriteEndElement();
                    return;
                }

                output.WriteStartElement(propName);
                output.WriteAttributeString("stp", "cType", _ns, attr.ListType);
                var i = 0;
                var isSelect = typeof(IExpressSelectType).IsAssignableFrom(propType.GetGenericArguments().FirstOrDefault());
                foreach (var item in ((IExpressEnumerable)propVal))
                {
                    WriteProperty(item.GetType().Name, item.GetType(), item, entity, output, i, attr, isSelect);
                    i++;
                }
                output.WriteEndElement();
                return;
            }
            if (typeof(IPersistEntity).IsAssignableFrom(propType))
                //all writable entities must support this interface and ExpressType have been handled so only entities left
            {
                var persistEntity = (IPersistEntity)propVal;

                if (pos > -1) //we are writing out a list element
                {
                    output.WriteStartElement(propVal.GetType().Name);
                    output.WriteAttributeString("ref", string.Format("i{0}", persistEntity.EntityLabel));
                    output.WriteAttributeString("nil", Xsi, "true");
                    //output.WriteAttributeString("type", Xsi, propVal.GetType().Name);
                    output.WriteAttributeString("pos", pos.ToString());
                    output.WriteEndElement();
                    return;
                }

                if (propType.IsInterface)
                {
                    output.WriteStartElement(propName);
                    output.WriteStartElement(propVal.GetType().Name);
                    output.WriteAttributeString("ref", string.Format("i{0}", persistEntity.EntityLabel));
                    output.WriteAttributeString("nil", Xsi, "true");
                    output.WriteEndElement();
                    output.WriteEndElement();
                    return;
                }

                output.WriteStartElement(propName);
                output.WriteAttributeString("ref", string.Format("i{0}", persistEntity.EntityLabel));
                output.WriteAttributeString("nil", Xsi, "true");
                if (propType.IsAbstract)
                    output.WriteAttributeString("type", Xsi, persistEntity.GetType().Name);
                output.WriteEndElement();
                return;

            }
            if (typeof(IExpressComplexType).IsAssignableFrom(propType)) //it is a complex value tpye
            {
                var properties = ((IExpressComplexType)propVal).Properties;
                output.WriteAttributeString(propName, string.Join(" ", properties));
                return;
            }
            if (propType.IsValueType || typeof(string) == propType) //it might be an in-built value type double, string etc
            {
                var pInfoType = propVal.GetType();

                if (pos < 0)
                    output.WriteStartAttribute(propName);

                if (pInfoType.IsEnum) //convert enum
                {
                    if (pos > -1)
                    {
                        output.WriteStartElement(propName);
                        output.WriteAttributeString("pos", pos.ToString());
                    }
                    output.WriteValue(propVal.ToString().ToLower());
                }
                else if (pInfoType.UnderlyingSystemType == typeof(Boolean))
                {
                    if (pos > -1)
                    {
                        output.WriteStartElement("boolean-wrapper");
                        output.WriteAttributeString("pos", pos.ToString());
                    }
                    output.WriteValue((bool)propVal ? "true" : "false");
                }
                else if (pInfoType.UnderlyingSystemType == typeof(Double))
                {
                    if (pos > -1)
                    {
                        output.WriteStartElement("double-wrapper");
                        output.WriteAttributeString("pos", pos.ToString());
                    }
                    output.WriteValue(string.Format(new Part21Formatter(), "{0:R}", propVal));
                }
                else if (pInfoType.UnderlyingSystemType == typeof(Int16))
                {
                    if (pos > -1)
                    {
                        output.WriteStartElement("integer-wrapper");
                        output.WriteAttributeString("pos", pos.ToString());
                    }
                    output.WriteValue(propVal.ToString());
                }
                else if (pInfoType.UnderlyingSystemType == typeof(Int32) ||
                         pInfoType.UnderlyingSystemType == typeof(Int64))
                {
                    if (pos > -1)
                    {
                        output.WriteStartElement("long-wrapper");
                        output.WriteAttributeString("pos", pos.ToString());
                    }
                    output.WriteValue(propVal.ToString());
                }
                else if (pInfoType.UnderlyingSystemType == typeof(String)) //convert  string
                {
                    if (pos > -1)
                    {
                        output.WriteStartElement("string-wrapper");
                        output.WriteAttributeString("pos", pos.ToString());
                    }
                    output.WriteValue(string.Format(new Part21Formatter(), "{0}", propVal));
                }

                else
                    throw new NotSupportedException(string.Format("Invalid Value Type {0}", pInfoType.Name));

                if (pos > -1)
                    output.WriteEndElement();
                else
                    output.WriteEndAttribute();

                return;
            }
            if (typeof(IExpressSelectType).IsAssignableFrom(propType))
            // a select type get the type of the actual value
            {
                var realType = propVal.GetType();
                output.WriteStartElement(propName);
                if (typeof(IExpressValueType).IsAssignableFrom(realType))
                {
                    output.WriteElementString(realType.Name + "-wrapper", propVal.ToString());
                }
                else
                {
                    WriteProperty(realType.Name, realType, propVal, entity, output, -2, attr);
                }
                output.WriteEndElement();
                return;
            }
#if DEBUG
            throw new Exception(string.Format("Entity of type {0} has illegal property {1} of type {2}", entity.GetType(), propType.Name, propType.Name));
#endif
        }
    }
}