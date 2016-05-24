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
namespace Xbim.Ifc2x3.SharedFacilitiesElements
{
	public partial class @IfcInventory : IIfcInventory
	{
		Ifc4.Interfaces.IfcInventoryTypeEnum? IIfcInventory.PredefinedType 
		{ 
			get
			{
				switch (InventoryType)
				{
					case IfcInventoryTypeEnum.ASSETINVENTORY:
						return Ifc4.Interfaces.IfcInventoryTypeEnum.ASSETINVENTORY;
					
					case IfcInventoryTypeEnum.SPACEINVENTORY:
						return Ifc4.Interfaces.IfcInventoryTypeEnum.SPACEINVENTORY;
					
					case IfcInventoryTypeEnum.FURNITUREINVENTORY:
						return Ifc4.Interfaces.IfcInventoryTypeEnum.FURNITUREINVENTORY;
					
					case IfcInventoryTypeEnum.USERDEFINED:
						//## Optional custom handling of PredefinedType == .USERDEFINED. 
						//##
						return Ifc4.Interfaces.IfcInventoryTypeEnum.USERDEFINED;
					
					case IfcInventoryTypeEnum.NOTDEFINED:
						return Ifc4.Interfaces.IfcInventoryTypeEnum.NOTDEFINED;
					
					
					default:
						throw new System.ArgumentOutOfRangeException();
				}
			} 
			set
			{
				switch (value)
				{
					case Ifc4.Interfaces.IfcInventoryTypeEnum.ASSETINVENTORY:
						InventoryType = IfcInventoryTypeEnum.ASSETINVENTORY;
						return;
					
					case Ifc4.Interfaces.IfcInventoryTypeEnum.SPACEINVENTORY:
						InventoryType = IfcInventoryTypeEnum.SPACEINVENTORY;
						return;
					
					case Ifc4.Interfaces.IfcInventoryTypeEnum.FURNITUREINVENTORY:
						InventoryType = IfcInventoryTypeEnum.FURNITUREINVENTORY;
						return;
					
					case Ifc4.Interfaces.IfcInventoryTypeEnum.USERDEFINED:
						InventoryType = IfcInventoryTypeEnum.USERDEFINED;
						return;
					
					case Ifc4.Interfaces.IfcInventoryTypeEnum.NOTDEFINED:
						InventoryType = IfcInventoryTypeEnum.NOTDEFINED;
						return;
					
					
					default:
						throw new System.ArgumentOutOfRangeException();
				}
				
			}
		}
		IIfcActorSelect IIfcInventory.Jurisdiction 
		{ 
			get
			{
				if (Jurisdiction == null) return null;
				var ifcorganization = Jurisdiction as ActorResource.IfcOrganization;
				if (ifcorganization != null) 
					return ifcorganization;
				var ifcperson = Jurisdiction as ActorResource.IfcPerson;
				if (ifcperson != null) 
					return ifcperson;
				var ifcpersonandorganization = Jurisdiction as ActorResource.IfcPersonAndOrganization;
				if (ifcpersonandorganization != null) 
					return ifcpersonandorganization;
				return null;
			} 
			set
			{
				if (value == null)
				{
					Jurisdiction = null;
					return;
				}	
				var ifcorganization = value as ActorResource.IfcOrganization;
				if (ifcorganization != null) 
				{
					Jurisdiction = ifcorganization;
					return;
				}
				var ifcperson = value as ActorResource.IfcPerson;
				if (ifcperson != null) 
				{
					Jurisdiction = ifcperson;
					return;
				}
				var ifcpersonandorganization = value as ActorResource.IfcPersonAndOrganization;
				if (ifcpersonandorganization != null) 
				{
					Jurisdiction = ifcpersonandorganization;
					return;
				}
				
			}
		}
		IEnumerable<IIfcPerson> IIfcInventory.ResponsiblePersons 
		{ 
			get
			{
				foreach (var member in ResponsiblePersons)
				{
					yield return member as IIfcPerson;
				}
			} 
		}
		Ifc4.DateTimeResource.IfcDate? IIfcInventory.LastUpdateDate 
		{ 
			get
			{
				//## Handle return of LastUpdateDate for which no match was found
			    return LastUpdateDate != null
			        ? new Ifc4.DateTimeResource.IfcDate(LastUpdateDate.ToISODateTimeString())
			        : null;
				//##
			} 
			set
			{
				//## Handle setting of LastUpdateDate for which no match was found
                if (!value.HasValue)
                {
                    LastUpdateDate = null;
                    return;
                }
                System.DateTime date = value.Value;
                LastUpdateDate = Model.Instances.New<DateTimeResource.IfcCalendarDate>(d =>
                {
                    d.YearComponent = date.Year;
                    d.MonthComponent = date.Month;
                    d.DayComponent = date.Day;
                });
				//##
				
			}
		}
		IIfcCostValue IIfcInventory.CurrentValue 
		{ 
			get
			{
				return CurrentValue;
			} 
			set
			{
				CurrentValue = value as CostResource.IfcCostValue;
				
			}
		}
		IIfcCostValue IIfcInventory.OriginalValue 
		{ 
			get
			{
				return OriginalValue;
			} 
			set
			{
				OriginalValue = value as CostResource.IfcCostValue;
				
			}
		}
	//## Custom code
	//##
	}
}