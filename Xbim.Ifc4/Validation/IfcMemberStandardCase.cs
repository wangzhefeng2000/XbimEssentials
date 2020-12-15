using System;
using Microsoft.Extensions.Logging;
using Xbim.Common;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Xbim.Common.Enumerations;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc4.Interfaces;
// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
namespace Xbim.Ifc4.SharedBldgElements
{
	public partial class IfcMemberStandardCase : IExpressValidatable
	{
		public enum IfcMemberStandardCaseClause
		{
			HasMaterialProfileSetUsage,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcMemberStandardCaseClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcMemberStandardCaseClause.HasMaterialProfileSetUsage:
						retVal = Functions.SIZEOF(Functions.USEDIN(this, "IFC4.IFCRELASSOCIATES.RELATEDOBJECTS").Where(temp => (Functions.TYPEOF(temp).Contains("IFC4.IFCRELASSOCIATESMATERIAL")) && (Functions.TYPEOF(temp.AsIfcRelAssociatesMaterial().RelatingMaterial).Contains("IFC4.IFCMATERIALPROFILESETUSAGE")))) == 1;
						break;
				}
			} catch (Exception  ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc4.SharedBldgElements.IfcMemberStandardCase>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcMemberStandardCase.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public override IEnumerable<ValidationResult> Validate()
		{
			foreach (var value in base.Validate())
			{
				yield return value;
			}
			if (!ValidateClause(IfcMemberStandardCaseClause.HasMaterialProfileSetUsage))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcMemberStandardCase.HasMaterialProfileSetUsage", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
