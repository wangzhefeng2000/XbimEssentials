// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool Xbim.CodeGeneration 
//  
//     Changes to this file may cause incorrect behaviour and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using Xbim.Ifc4.Interfaces;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Xbim.Ifc2x3.ProductExtension
{
	public partial class @IfcSpaceType : IIfcSpaceType
	{
		Ifc4.Interfaces.IfcSpaceTypeEnum IIfcSpaceType.PredefinedType 
		{ 
			get
			{
				switch (PredefinedType)
				{
					case IfcSpaceTypeEnum.USERDEFINED:
						//## Optional custom handling of PredefinedType == .USERDEFINED. 
						//##
						return Ifc4.Interfaces.IfcSpaceTypeEnum.USERDEFINED;
					
					case IfcSpaceTypeEnum.NOTDEFINED:
						return Ifc4.Interfaces.IfcSpaceTypeEnum.NOTDEFINED;
					
					
					default:
						throw new System.ArgumentOutOfRangeException();
				}
			} 
			set
			{
				switch (value)
				{
					case Ifc4.Interfaces.IfcSpaceTypeEnum.SPACE:
						//## Handle setting of SPACE member from IfcSpaceTypeEnum in property PredefinedType
						//TODO: Handle setting of SPACE member from IfcSpaceTypeEnum in property PredefinedType
						throw new System.NotImplementedException();
						//##
										
					case Ifc4.Interfaces.IfcSpaceTypeEnum.PARKING:
						//## Handle setting of PARKING member from IfcSpaceTypeEnum in property PredefinedType
						//TODO: Handle setting of PARKING member from IfcSpaceTypeEnum in property PredefinedType
						throw new System.NotImplementedException();
						//##
										
					case Ifc4.Interfaces.IfcSpaceTypeEnum.GFA:
						//## Handle setting of GFA member from IfcSpaceTypeEnum in property PredefinedType
						//TODO: Handle setting of GFA member from IfcSpaceTypeEnum in property PredefinedType
						throw new System.NotImplementedException();
						//##
										
					case Ifc4.Interfaces.IfcSpaceTypeEnum.INTERNAL:
						//## Handle setting of INTERNAL member from IfcSpaceTypeEnum in property PredefinedType
						//TODO: Handle setting of INTERNAL member from IfcSpaceTypeEnum in property PredefinedType
						throw new System.NotImplementedException();
						//##
										
					case Ifc4.Interfaces.IfcSpaceTypeEnum.EXTERNAL:
						//## Handle setting of EXTERNAL member from IfcSpaceTypeEnum in property PredefinedType
						//TODO: Handle setting of EXTERNAL member from IfcSpaceTypeEnum in property PredefinedType
						throw new System.NotImplementedException();
						//##
										
					case Ifc4.Interfaces.IfcSpaceTypeEnum.USERDEFINED:
						PredefinedType = IfcSpaceTypeEnum.USERDEFINED;
						return;
					
					case Ifc4.Interfaces.IfcSpaceTypeEnum.NOTDEFINED:
						PredefinedType = IfcSpaceTypeEnum.NOTDEFINED;
						return;
					
					
					default:
						throw new System.ArgumentOutOfRangeException();
				}
				
			}
		}

		private  Ifc4.MeasureResource.IfcLabel? _longName;

		Ifc4.MeasureResource.IfcLabel? IIfcSpaceType.LongName 
		{ 
			get
			{
				return _longName;
			} 
			set
			{
				SetValue(v => _longName = v, _longName, value, "LongName", byte.MaxValue);
				
			}
		}
		Ifc4.MeasureResource.IfcLabel? IIfcSpatialElementType.ElementType 
		{ 
			get
			{
				if (!ElementType.HasValue) return null;
				return new Ifc4.MeasureResource.IfcLabel(ElementType.Value);
			} 
			set
			{
				ElementType = value.HasValue ? 
					new MeasureResource.IfcLabel(value.Value) :  
					 new MeasureResource.IfcLabel?() ;
				
			}
		}
	//## Custom code
	//##
	}
}